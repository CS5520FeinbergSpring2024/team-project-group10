using System;
using System.Collections;
using OpenCover.Framework.Model;
using UnityEngine;

/// <summary>
/// Contains much of the business logic for Pollen and Nectar ResourceProviders.
/// </summary>
public class FlowerResourceProvider : MonoBehaviour, IResourceProvider
{
    // To detect collisions with the bee.
    private protected static readonly string _beeObjectTag = "Bee";
    // For logging.
    private protected string TAG;

    // Set in the Unity editor.
    [SerializeField]
    private protected InventoryScriptableObject UserInventory;

    // Paired with Properties.
    private protected ResourceType _resourceType;
    private protected float _collectionAmountPerSecond;
    private protected float _secondsToCollectTotal;
    private protected int _totalCollectableAmount;
    private protected float _amountRemaining;
    private protected float _regenerationTimeSeconds;
    private protected float _regeneratedAmountPerSecond;
    private protected float _totalRegenerationCycles;
    private protected float _remainingRegenerableUnits;

    // Used only for internal logic.
    private protected float _partialUnitsForCollection;
    private protected bool _currentlyCollecting;
    private protected bool _currentlyRegenerating;
    private protected ParticleSystem _particleSystem;

    /// <summary>
    /// The IResourceAmountToEmissionRateConverter to use to convert this object's
    /// amount of collectable resources to a particle emission rate.
    /// </summary>
    public IResourceAmountToEmissionRateConverter EmissionRateConverter
    {
        get; set;
    }

    /// <summary>
    /// The type of resource that this produces. Either Pollen or Nectar.
    /// </summary>
    public ResourceType ResourceType
    {
        get { return _resourceType; }
        set
        {
            if (value == ResourceType.Pollen || value == ResourceType.Nectar)
            {
                _resourceType = value;
            }
        }
    }

    /// <summary>
    /// The number of units of the resource that can be 
    /// collected in one second.
    /// 
    /// Must be a positive value.
    /// 
    /// This value is bound to SecondsToCollectTotal. Changing this
    /// will change that to match and vice versa.
    /// </summary>
    public float CollectionAmountPerSecond
    {
        get { return _collectionAmountPerSecond; }
        set
        {
            if (value <= 0)
            {
                return;
            }
            if (float.IsInfinity(value))
            {
                _secondsToCollectTotal = 0;
            }
            else
            {
                _secondsToCollectTotal = TotalCollectableAmount / value;
            }
            _collectionAmountPerSecond = value;
        }
    }

    /// <summary>
    /// The number of seconds to collect the entire stock.
    /// 
    /// Cannot be infinite or negative.
    /// 
    /// This value is bound to CollectionAmountPerSecond. Changing this
    /// will change that to match and vice versa.
    /// </summary>
    public float SecondsToCollectTotal
    {
        get { return _secondsToCollectTotal; }
        set
        {
            if (float.IsInfinity(value) || value < 0)
            {
                return;
            }
            if (value == 0)
            {
                _collectionAmountPerSecond = TotalCollectableAmount;
            }
            else
            {
                _collectionAmountPerSecond = TotalCollectableAmount / value;
            }
            _secondsToCollectTotal = value;
        }
    }

    /// <summary>
    /// Amount of the resource that can be collected. Must be positive.
    /// 
    /// Note: If the resource is currently being collected, changes to
    /// this value will not take effect until the current collection is finished.
    /// </summary>
    public int TotalCollectableAmount
    {
        get { return _totalCollectableAmount; }
        set
        {
            if (value > 0)
            {
                _totalCollectableAmount = value;
                // This implementation allows resources to essentially be regenerated even
                // if IsRegenerable() is false or the _remainingRegenerableUnits is less
                // than the TotalCollectableAmount.
                if (!_currentlyCollecting && !_currentlyRegenerating)
                {
                    _amountRemaining = TotalCollectableAmount;
                }
            }
        }
    }

    /// <summary>
    /// Amount of the resource remaining to be colleced before 
    /// triggering a recharge.
    /// </summary>
    public int AmountRemaining
    {
        get { return (int)_amountRemaining; }
    }

    /// <summary>
    /// The number of seconds that a full recharge/regeneration takes.
    /// Cannot be infinite or negative.
    /// 
    /// This value is bound to RegeneratedAmountPerSecond. Changing this will change
    /// that to match and vice versa.
    /// </summary>
    public float RegenerationTimeSeconds
    {
        get { return _regenerationTimeSeconds; }
        set
        {
            if (float.IsInfinity(value) || value < 0)
            {
                return;
            }
            if (value == 0)
            {
                _regeneratedAmountPerSecond = TotalCollectableAmount;
            }
            else
            {
                _regeneratedAmountPerSecond = TotalCollectableAmount / value;
            }
            _regenerationTimeSeconds = value;
        }
    }

    /// <summary>
    /// The number of units of the resource that can be regenerated in one second.
    /// Must be a positive value. To prevent regeneration, set 
    /// TotalRegenerationCycles to 0.
    /// 
    /// This value is bound to RegenerationTimeSeconds. Changing this will change
    /// that to match and vice versa.
    /// </summary>
    public float RegeneratedAmountPerSecond
    {
        get { return _regeneratedAmountPerSecond; }
        set
        {
            if (value <= 0)
            {
                return;
            }
            if (float.IsInfinity(value))
            {
                _regenerationTimeSeconds = 0;
            }
            else
            {
                _regenerationTimeSeconds = TotalCollectableAmount / value;
            }
            _regeneratedAmountPerSecond = value;
        }
    }

    /// <summary>
    /// Number of times that this provider's resources may fully regenerate.
    /// Must be non-negative. May be infinite.
    /// </summary>
    public float TotalRegenerationCycles
    {
        get { return _totalRegenerationCycles; }
        set
        {
            if (value < 0)
            {
                return;
            }
            _totalRegenerationCycles = value;
            _remainingRegenerableUnits = value * TotalCollectableAmount;
        }
    }

    // Methods

    /// <summary>
    /// Set values for ResourceType, TotalCollectableAmount, SecondsToCollectTotal
    /// and RegenerationTimeSeconds.
    /// </summary>
    /// <param name="resourceType">Value to set for ResourceType.
    /// Defaults to ResourceType.Pollen</param>
    /// <param name="totalCollectableAmount">New value for TotalCollectableAmount.
    /// Defaults to 10.</param>
    /// <param name="secondsToCollectTotal">New value for SecondsToCollectTotal.
    /// Defaults to 0.</param>
    /// <param name="regenerationTimeSeconds">New value for RegenerationTimeSeconds.
    /// Defaults to 10.</param>
    public void SetValues(ResourceType resourceType = ResourceType.Pollen,
                          int totalCollectableAmount = 10,
                          float secondsToCollectTotal = 0,
                          float regenerationTimeSeconds = 10)
    {
        ResourceType = resourceType;
        TotalCollectableAmount = totalCollectableAmount;
        SecondsToCollectTotal = secondsToCollectTotal;
        RegenerationTimeSeconds = regenerationTimeSeconds;
    }


    /// <summary>
    /// Whether or not this resource can be collected at the moment.
    /// </summary>
    /// <returns>True if the resource can be collected. False otherwise.</returns>
    public bool IsCollectable()
    {
        // This option won't allow collection unless it's fully charged.
        // return AmountRemaining >= TotalCollectableAmount;
        return AmountRemaining > 0;
    }

    /// <summary>
    /// Whether or not the resource can be regenerated.
    /// </summary>
    /// <returns>True if the resource can be regenerated. False otherwise.</returns>
    public bool IsRegenerable()
    {
        return _remainingRegenerableUnits > 0;
    }

    /// <summary>
    /// Adds the fraction of the resource that was collected during deltaTime to the
    /// UserInventory.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the UserInventory is null.
    /// The UserInventory may be assigned in the Unity editor.</exception>
    public void Collect()
    {
        const string funcTag = "Collect()";
        if (IsCollectable())
        {
            _partialUnitsForCollection += CollectionAmountPerSecond * Time.deltaTime;
            try
            {
                if (_partialUnitsForCollection > 1)
                {
                    UserInventory.UpdateInventory(ResourceType,
                        (int)_partialUnitsForCollection);
                    _amountRemaining -= (int)_partialUnitsForCollection;
                    _partialUnitsForCollection %= 1;
                }
            }
            catch (NullReferenceException e)
            {
                throw new InvalidOperationException(
                    FormatLogMessage(funcTag, "UserInventory is null."), e);
            }
        }
        else
        {
            _currentlyCollecting = false;
        }
    }

    /// <summary>
    /// Adds the fraction of the resource that was regenerated during deltaTime to
    /// the amount of the resource that's available for collection.
    /// </summary>
    public void Regenerate()
    {
        if (IsRegenerable() && _amountRemaining < TotalCollectableAmount)
        {
            float iterAmount = RegeneratedAmountPerSecond * Time.deltaTime;
            _amountRemaining = Math.Min(_amountRemaining + iterAmount,
                                        TotalCollectableAmount);
            _remainingRegenerableUnits -= iterAmount;
        }
        else
        {
            _currentlyRegenerating = false;
        }
    }

    // For IResourceProvider.
    /// <summary>
    /// Start collecting the resource.
    /// </summary>
    public void CollectResource()
    {
        _currentlyCollecting = true;
        _currentlyRegenerating = false;
    }

    /// <summary>
    /// Set the emission rate over time for this flower's particles to match
    /// this flower's amount of collectable resources.
    /// </summary>
    public void UpdateParticleRate()
    {
        const string funcTag = "UpdateParticleRate";
        if (EmissionRateConverter != null)
        {
            float rate = EmissionRateConverter.EmissionRateFromResourceAmount(AmountRemaining, GetType());
            if (_particleSystem != null)
            {
                // This seems to be the only way to get it to access emission.rateOverTime.
                var e = _particleSystem.emission;
                e.rateOverTime = rate;
            }
            else
            {
                {
                    Debug.Log(FormatLogMessage(
                        funcTag,
                        "Cannot set particle emission rate. _particleSystem is null."));
                }
            }
        }
        else
        {
            Debug.Log(FormatLogMessage(
                funcTag,
                "Cannot convert amount to particle emission rate. EmissionRateConverter is null."));
        }
    }

    // Unity lifecycle methods.

    private protected void Awake()
    {
        TAG = GetType().ToString();
        if (UserInventory == null)
        {
            Debug.Log(FormatLogMessage("Awake()", "UserInventory is null."));
        }
        // Set default starting values.
        SetValues();
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private protected void OnTriggerEnter(Collider other)
    {
        // Start collecting whenever the bee bumps into the flower.
        if (other.gameObject.CompareTag(_beeObjectTag))
        {
            _currentlyCollecting = true;
            _currentlyRegenerating = false;
        }
    }

    private protected void OnTriggerExit(Collider other)
    {
        // Stop collecting and start regenerating 
        // whenever the bee leaves the flower.
        if (other.gameObject.CompareTag(_beeObjectTag))
        {
            _currentlyRegenerating = true;
            _currentlyCollecting = false;
        }
    }

    private protected void Update()
    {
        if (_currentlyCollecting)
        {
            Collect();
        }
        else if (_currentlyRegenerating)
        {
            Regenerate();
        }
        UpdateParticleRate();
    }

    // Utility Methods

    /// <summary>
    /// Utility method. Return the given message formatted with the class and given
    /// tags for logging.
    /// </summary>
    /// <param name="tag">Tag to add onto the message.</param>
    /// <param name="message">The main message</param>
    /// <returns></returns>
    private protected string FormatLogMessage(string tag, string message)
    {
        return $"[{TAG}: {tag}]: {message}";
    }

    // For debugging. Set up `StartCoroutine(DebugLogFieldValues());` in Awake() or Start()
    // to log updates every second.
    private IEnumerator DebugLogFieldValues()
    {
        while (true)
        {
            Debug.Log(FormatLogMessage(
                    "", "\tIn inventory = " + UserInventory.GetResourceCount(ResourceType)));
            Debug.Log(FormatLogMessage(
                "", "\t_amountRemaining = " + _amountRemaining));
            Debug.Log(FormatLogMessage(
                        "", "\t_remainingRegenerableUnits = " + _remainingRegenerableUnits));
            yield return new WaitForSeconds(1);
        }
    }
}
