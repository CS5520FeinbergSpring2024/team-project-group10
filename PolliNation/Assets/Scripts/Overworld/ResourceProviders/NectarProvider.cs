/// <summary>
/// Provides nectar. Extends FlowerResourceProvider.
/// </summary>
public class NectarProvider : FlowerResourceProvider
{
    new void Awake() {
        base.Awake();
        // Not really necessary since it defaults to pollen.
        SetValues(ResourceType.Nectar);
        TotalRegenerationCycles = 3;
    }
}

