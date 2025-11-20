using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public InventoryItemData data;
    private Vector2Int gridposition;

    private Image iconImage;

    private void Awake()
    {
        // Récupère l'Image sur le prefab (l'enfant ou le même GameObject)
        iconImage = GetComponent<Image>();
    }

    public void SetData(InventoryItemData newData)
    {
        data = newData;

        if (iconImage != null && data != null && data.itemIcon != null)
        {
            iconImage.sprite = data.itemIcon; // assign the sprite
            iconImage.enabled = true;     // in case he is disabled
        }
    }


    public void SetGridPosition(Vector2Int pos)
    {
        gridposition = pos;
    }

    public Vector2Int GetGridPosition()
    {
        return gridposition;
    }


}
