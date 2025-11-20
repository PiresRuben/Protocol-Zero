using UnityEngine;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    public InventoryGrid grid;
    public GameObject itemPrefab;

    public InventoryItemData testItemData;



    private void Start()
    {
        AddItem(testItemData);
        AddItem(testItemData);
        AddItem(testItemData);
        AddItem(testItemData);
        AddItem(testItemData);
        AddItem(testItemData);
        AddItem(testItemData);
        AddItem(testItemData);
        AddItem(testItemData);
        AddItem(testItemData);
        AddItem(testItemData);
        AddItem(testItemData);

    }
    public void AddItem(InventoryItemData data)
    {
        for (int y = 0; y < grid.height; y++)
        {
            for (int x = 0; x < grid.widht; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                if (grid.IsCellFree(pos))
                {
                    GameObject obj = Instantiate(itemPrefab, grid.transform);
                    InventoryItem item = obj.GetComponent<InventoryItem>();
                    item.SetData(data); // assigne le sprite

                    RectTransform rect = obj.GetComponent<RectTransform>();

                    rect.sizeDelta = new Vector2(64, 64);
                    float spacing = 2f;
                    rect.anchoredPosition = new Vector2(
                        pos.x * (64 + spacing),
                        -pos.y * (64 + spacing)
                    );

                    grid.PlaceItem(item, pos);
                    return;
                }
            }
        }

        Debug.Log("Inventaire plein !");
    }
}