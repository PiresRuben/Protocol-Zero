using UnityEngine;

[CreateAssetMenu(fileName ="NewItem", menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;

    // Size of each Objectfs is set there, right now it's set at one
    public Vector2Int size = Vector2Int.one;
}
