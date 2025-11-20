using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    public InventoryGrid grid;
    public GameObject itemPrefab;

    public InventoryItemData testItemData;
    public InventoryUI inventoryUI;
    public GameObject inventoryPanel;
    public List<InventoryItemData> items = new List<InventoryItemData>();

    public CharacterStatsUI displayStats;
    public PlayerHealth player;

    public bool _isOpen;

    private void Awake()
    {
        _isOpen = false;
    }

    private void Start()
    {
        for (int i = 0; i < 12; i++)
        {
            AddItem(testItemData);
        }
    }


    public void OpenInventory()
    {
        _isOpen = true;

        inventoryPanel.SetActive(true);
        Debug.Log("Inventory Displayed");

        inventoryUI.CreateGrid();
        Debug.Log("Create Grid");

        displayStats.UpdateStats(
            player.currentHealth,
            player.maxHealth,
            player.attack,
            player.infection,
            player.fatigue
        );

        Debug.Log("Display Item");
        DisplayItems();
    }


    public void CloseInventory()
    {
        _isOpen = false;
        inventoryPanel.SetActive(false);
    }


    public void AddItem(InventoryItemData data)
    {
        if (items.Count >= grid.width * grid.height)
        {
            Debug.Log("Inventaire plein !");
            return;
        }

        items.Add(data);
    }

    public void DisplayItems()
    {
        // On vide d'abord la grille de données
        grid.ClearGrid(); // je te donne la méthode juste après

        // On enlève les anciens visuels
        foreach (Transform cell in inventoryUI.transform)
        {
            if (cell.childCount > 0)
            {
                Destroy(cell.GetChild(0).gameObject);
            }
        }

        int index = 0;

        for (int y = 0; y < grid.height; y++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                if (index >= items.Count)
                    return;

                Vector2Int pos = new Vector2Int(x, y);

                if (grid.IsCellFree(pos))
                {
                    RectTransform cell = inventoryUI.cells[x, y];
                    GameObject obj = Instantiate(itemPrefab, cell);

                    InventoryItem item = obj.GetComponent<InventoryItem>();
                    item.SetData(items[index]);

                    RectTransform rect = obj.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(64, 64);
                    rect.anchoredPosition = Vector2.zero;

                    grid.PlaceItem(item, pos);

                    index++;
                }
            }
        }
    }

}