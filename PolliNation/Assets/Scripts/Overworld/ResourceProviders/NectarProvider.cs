/// <summary>
/// Provides nectar. Extends FlowerResourceProvider.
/// </summary>
public class NectarProvider : FlowerResourceProvider
{
    new void Awake() {
        base.Awake();
        SetValues(ResourceType.Nectar);
        TotalRegenerationCycles = 3;
    }
}

