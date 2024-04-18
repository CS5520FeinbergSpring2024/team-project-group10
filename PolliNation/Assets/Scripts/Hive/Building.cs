using UnityEngine;

public class Building : MonoBehaviour {
    private Vector2 _tileId;
    internal Vector2 TileId {
        get { return _tileId; }
        set { _tileId = value; }
    }

    public void DestroyGameObject() {
        Destroy(gameObject);
    }

    // Programatically sets the building's resource display
    public void UpdateResourceDisplay(ResourceType resourceType) {
        Material resourceMaterial;
        switch (resourceType) {
            case ResourceType.Nectar:
                resourceMaterial = Resources.Load<Material>("ResourceImage-Nectar");
                break;
            case ResourceType.Pollen:
                resourceMaterial = Resources.Load<Material>("ResourceImage-Pollen");
                break;
            case ResourceType.Buds:
                resourceMaterial = Resources.Load<Material>("ResourceImage-Buds");
                break;
            case ResourceType.Water:
                resourceMaterial = Resources.Load<Material>("ResourceImage-Water");
                break;
            case ResourceType.Honey:
                resourceMaterial = Resources.Load<Material>("ResourceImage-Honey");
                break;
            case ResourceType.Propolis:
                resourceMaterial = Resources.Load<Material>("ResourceImage-Propolis");
                break;
            case ResourceType.RoyalJelly:
                resourceMaterial = Resources.Load<Material>("ResourceImage-RoyalJelly");
                break;
            default:
                Debug.Log("Invalid resource");
                resourceMaterial = null;
                break;
        }
        Renderer resourceDisplayRenderer1 = gameObject.transform.GetChild(0).GetChild(6).gameObject.GetComponent<Renderer>();
        Renderer resourceDisplayRenderer2 = gameObject.transform.GetChild(0).GetChild(7).gameObject.GetComponent<Renderer>();
        resourceDisplayRenderer1.material = resourceMaterial;
        resourceDisplayRenderer2.material = resourceMaterial;
    }
}