using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCannonTarget : MonoBehaviour
{
    public Texture2D cross;
    public Vector2 hotSpot = Vector2.zero;

    TerrainCollider terrainCollider;
    Vector3 worldPosition;
    // Start is called before the first frame update
    void Start()
    {
        terrainCollider = Terrain.activeTerrain.GetComponent<TerrainCollider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) GetMousePos();
    }

    private void OnEnable() {
        Cursor.SetCursor(cross, hotSpot, CursorMode.ForceSoftware);
    }

    private void OnDisable() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); 
    }
    public void GetMousePos()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if(terrainCollider.Raycast(ray, out hitData, 1000))
            {
                worldPosition = hitData.point;
            }
        foreach(CannonController c in FindObjectsOfType<CannonController>())
        {
            c.target = worldPosition;
        }
        UIManager.instance.ToggleCannonAbility();
    }
}
