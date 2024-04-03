using System;


/// <summary>
/// Interface that guarantees the ability to convert from an amount of a resource
/// available to collect to a corresponding particle emission rate.
/// </summary>
public interface IResourceAmountToEmissionRateConverter {

  /// <summary>
  /// Convert a value on the object's totalCollectableAmount scale to its equivalent on the
  /// object's particleEmissionRate scale.
  /// </summary>
  /// <param name="amount">Amount of the resource available to collect.</param>
  /// <param name="settingsKey">The key to use to look up production value settings. 
  /// Typically the type of the object calling this method.</param>
  /// <returns>The equivalent of the given amount on the particle emmision rate scale.</returns>
  float EmissionRateFromResourceAmount(float amount, Type settingsKey);
}