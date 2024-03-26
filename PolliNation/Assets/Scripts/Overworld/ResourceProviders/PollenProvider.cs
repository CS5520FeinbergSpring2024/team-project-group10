
/// <summary>
/// Provides pollen. Extends FlowerResourceProvider.
/// </summary>
public class PollenProvider : FlowerResourceProvider
{
    new void Awake() {
        base.Awake();
        // Not really necessary since it defaults to pollen.
        SetValues(ResourceType.Pollen, secondsToCollectTotal: 5);
        TotalRegenerationCycles = 3;
    }
}
