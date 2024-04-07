using UnityEngine;

/// <summary>
/// Provide:
/// 
/// public method OpenBuildMenu() for click button 
/// allows open buildMenu.
/// 
/// </summary>
public class Tile : MonoBehaviour
{
    private BuildMenuScript buildMenu;
    public Vector2 tileID;

    // Start is called before the first frame update
    void Start()
    {
        // Set tileID as this tile's x and z coordinates as a Vector2
        tileID = new Vector2(transform.position.x, transform.position.z);
        
    }

    public void OpenBuildMenu(Vector2 tileID) 
    {
        buildMenu = GameObject.FindObjectOfType<BuildMenuScript>(true);
        if (buildMenu != null) 
        {   
            //pass current tile to the buildMenu and open the buildMenu screen
            buildMenu.OpenMenuForTile(tileID);
        }
       
    }

    void OnMouseDown() {
        Debug.Log(tileID);
        OpenBuildMenu(tileID);
    }
}
