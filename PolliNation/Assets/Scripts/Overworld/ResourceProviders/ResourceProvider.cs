/// <summary>
/// Contract to ensure method consistency between all 
/// ResourceProvider classes. Any class operating as a
/// raw/collectable (as opposed to produced) resource should
/// implement this interface.
/// </summary>
public interface IResourceProvider
{
    /// <summary>
    /// The ResourceType that this ResourceProvider collects.
    /// </summary>
    ResourceType ResourceType
    {
        get;
    }

    /// <summary>
    /// Whether or not this ResourceProvider instance can be used
    /// to collect resources at the moment.
    /// </summary>
    /// <returns>
    /// true if the resource can be collected now. false otherwise.
    /// </returns>
    bool IsCollectable();

    /// <summary>
    /// Adjusts inventory levels to "collect" the resource that this
    /// instance provides.
    /// </summary>
    void CollectResource();
}
