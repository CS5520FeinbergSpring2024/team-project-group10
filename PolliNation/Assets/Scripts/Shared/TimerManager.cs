using System.Collections;
using UnityEngine;

public static class TimerManager
{
    /*//public static IEnumerator StartCountdown(float duration, System.Action<float> onUpdate, System.Action onFinished)
    public static IEnumerator UpdateInventoryEveryMinute(int produceRate, int storageLimit, ResourceType resourceType, WorkersMenuScript workersMenuScript, HiveScriptableObject hiveScriptableObject, int workersAssigned, System.Action<float> onUpdate, System.Action onFinished)
    {
        int resourceProduced = 0;
        int storageCapacity = hiveScriptableObject[resourceType].storageLevel * storageLimit;


        while (resourceProduced < storageCapacity)
        {
            float productionThisInterval = produceRate * Time.deltaTime * workersAssigned;
            resourceProduced += Mathf.FloorToInt(productionThisInterval);
            resourceProduced = Mathf.Min(resourceProduced, storageCapacity);
            onUpdate?.Invoke((float)resourceProduced / storageCapacity);
            workersMenuScript.saveChanges();
            yield return null;
        }

        onFinished?.Invoke();
    }

    public static string FormatGathering(int workersAssigned, ResourceType resourceType, float percentageToCapacity)
    {

        return string.Format("{workersAssigned} bees gathering {resourceType}, {percentageToCapacity}/100 to full", workersAssigned, percentageToCapacity);
    }


 

    }*/
}
