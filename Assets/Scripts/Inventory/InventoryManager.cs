using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Pop-Up UI")]
    public GameObject PopUpUI;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI amountNewItem;
    [SerializeField] private GameObject itemSprite;

    // Référence à la coroutine en cours (si présente) pour pouvoir l'arrêter
    private Coroutine popupCoroutine;

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

    }

    public void CloseInventory()
    {
        _isOpen = false;
        inventoryPanel.SetActive(false);
    }

    public void AddItem(InventoryItemData data)
    {
        // Mettre à jour et afficher le pop-up UI pendant 2 secondes
        itemName.text = data.itemName;
        var itemSpritelog = itemSprite.GetComponent<UnityEngine.UI.RawImage>();
        itemSpritelog.texture = data.itemIcon.texture;
        amountNewItem.text = "1";

        // Activer le pop-up
        PopUpUI.SetActive(true);

        // Si une coroutine est déjà en cours pour cacher le pop-up, l'arrêter
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
            popupCoroutine = null;
        }

        // Démarrer une nouvelle coroutine pour cacher le pop-up après 2 secondes
        popupCoroutine = StartCoroutine(HidePopupAfterSeconds(2f));

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

    private IEnumerator HidePopupAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (PopUpUI != null)
        {
            PopUpUI.SetActive(false);
        }
        popupCoroutine = null;
    }

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