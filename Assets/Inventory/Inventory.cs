using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int flourLevel = 5;
    public int breadLevel = 0;
    public float drawOffSet = 0;
    public string InventoryName = string.Empty;

    private void OnGUI()
    {
        GUI.Box(new Rect(0, drawOffSet, 100, 100), InventoryName);
        GUI.Label(new Rect(10, 20 + drawOffSet, 100, 20), "Flour: " + flourLevel);
        GUI.Label(new Rect(10, 35 + drawOffSet, 100, 20), "Bread: " + breadLevel);
    }
}