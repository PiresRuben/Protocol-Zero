using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;

    // Liste des positions relatives occupées (0,0 est le pivot)
    // Exemple pour un 'L' : (0,0), (0,1), (0,2), (1,0)
    public List<Vector2Int> shape = new List<Vector2Int> { new Vector2Int(0, 0) };

    public Vector2Int GetBounds()
    {
        if (shape.Count == 0) return Vector2Int.one;

        int maxX = 0;
        int maxY = 0;
        // On cherche le X et Y le plus loin du pivot (0,0)
        foreach (var pos in shape)
        {
            if (pos.x > maxX) maxX = pos.x;
            if (pos.y > maxY) maxY = pos.y;
        }
        // Si la case max est 1, la taille est 2 (car ça commence à 0)
        return new Vector2Int(maxX + 1, maxY + 1);
    }
}