using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryGrid grid;
    public GameObject itemPrefab;
    public InventoryItemData testItemData;
    public InventoryItemData testItemData2;
    public InventoryUI inventoryUI;
    public GameObject inventoryPanel;

    // Cette liste sert maintenant de "file d'attente" ou de stockage temporaire
    public List<InventoryItemData> itemsToSpawn = new List<InventoryItemData>();

    public CharacterStatsUI displayStats;
    public PlayerHealth player;

    public bool _isOpen;

    private void Start()
    {
        // On initialise la UI une seule fois au début
        inventoryUI.CreateGrid();

        // Exemple : On ajoute quelques items de test
        for (int i = 0; i < 5; i++)
        {
            AddItem(testItemData);
            AddItem(testItemData2);
        }

        inventoryPanel.SetActive(false);
    }

    public void OpenInventory()
    {
        _isOpen = true;
        inventoryPanel.SetActive(true);

        displayStats.UpdateStats(
            player.currentHealth,
            player.maxHealth,
            player.attack,
            player.infection,
            player.fatigue
        );

        // On ne fait plus de "DisplayItems" ici car les objets 
        // sont déjà placés physiquement dans la grille.
    }

    public void CloseInventory()
    {
        _isOpen = false;
        inventoryPanel.SetActive(false);
    }

    // Tente d'ajouter un item à la première place disponible
    public void AddItem(InventoryItemData data)
    {
        // On cherche dans toute la grille un endroit où la FORME passe
        for (int y = 0; y < grid.height; y++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                Vector2Int potentialPos = new Vector2Int(x, y);
                if (grid.CanPlaceShape(data, potentialPos))
                {
                    SpawnItemInGrid(data, new Vector2Int(x, y));
                    return;
                }
            }
        }
        Debug.Log("Inventaire plein pour cet objet !");
    }

    // Crée physiquement l'objet dans la grille
    private void SpawnItemInGrid(InventoryItemData data, Vector2Int pos)
    {
        RectTransform cell = inventoryUI.cells[pos.x, pos.y];
        GameObject obj = Instantiate(itemPrefab, cell);

        InventoryItem item = obj.GetComponent<InventoryItem>();
        // On passe cellSize (64) pour que l'item ajuste sa taille visuelle
        item.SetData(data, inventoryUI.cellSize);

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;

        grid.PlaceItem(item, pos);
    }
}