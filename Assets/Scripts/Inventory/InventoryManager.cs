using UnityEngine;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    public InventoryGrid grid;
    public GameObject itemPrefab;

    public InventoryItemData testItemData;
    public InventoryUI inventoryUI;


    private void Start()
    {
        inventoryUI.CreateGrid();

        for (int i = 0; i < 12; i++)
        {
            AddItem(testItemData);
        }
    }

    public void AddItem(InventoryItemData data)
    {
        for (int y = 0; y < grid.height; y++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                if (grid.IsCellFree(pos))
                {
                    RectTransform cell = inventoryUI.cells[x, y];
                    GameObject obj = Instantiate(itemPrefab, cell);
                    InventoryItem item = obj.GetComponent<InventoryItem>();
                    item.SetData(data); // assigne le sprite

                    RectTransform rect = obj.GetComponent<RectTransform>();

                    rect.sizeDelta = new Vector2(64, 64);
                    rect.anchoredPosition = Vector2.zero;

                    grid.PlaceItem(item, pos);
                    return;
                }
            }
        }

        Debug.Log("Inventaire plein !");
    }

}