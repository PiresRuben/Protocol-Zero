using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public InventoryItemData data;
    private Vector2Int gridPosition;
    private CanvasGroup canvasGroup;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        Image img = GetComponent<Image>();
        if (img == null) img = gameObject.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0); // Transparent
        img.raycastTarget = true;
    }

    public void SetData(InventoryItemData newData, int cellSize)
    {
        data = newData;
        rect = GetComponent<RectTransform>();

        Vector2Int bounds = data.GetBounds();
        rect.sizeDelta = new Vector2(bounds.x * cellSize, bounds.y * cellSize);

        Image parentImg = GetComponent<Image>();
        if (parentImg == null) parentImg = gameObject.AddComponent<Image>();
        parentImg.raycastTarget = true;

        foreach (Transform child in transform) { Destroy(child.gameObject); }

        foreach (Vector2Int offset in data.shape)
        {
            GameObject block = new GameObject("VisibleBlock", typeof(Image));
            block.transform.SetParent(this.transform);

            RectTransform rt = block.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            rt.sizeDelta = new Vector2(cellSize, cellSize);
            rt.pivot = new Vector2(0, 1);
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.anchoredPosition = new Vector2(offset.x * cellSize, -offset.y * cellSize);

            Image bImg = block.GetComponent<Image>();
            bImg.sprite = data.itemIcon; // On met le bandage sur CHAQUE bloc
            bImg.raycastTarget = false;

        }
    }


    // Drag & Drop handlers
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    private void UseItem()
    {
        if (data == null) return;

        Debug.Log("Utilisation de : " + data.itemName);

        // SWITCH basé sur le nom (ou mieux, sur un Enum dans votre ScriptableObject)
        switch (data.itemName)
        {
            case "Bandage":
                ApplyHealing(10);
                break;

            case "Seringue":
                ApplyInfection(20);
                break;

            default:
                Debug.Log("Cet objet n'a pas de comportement spécifique.");
                break;
        }
    }

    private void ApplyHealing(int amount)
    {
        // On accède au joueur via le Manager ou un Singleton
        PlayerHealth player = Object.FindObjectOfType<PlayerHealth>();
        if (player != null)
        {
            player.Heal(amount);
            Debug.Log("Soins appliqués !");
            Consume(); 
        }
    }

    private void ApplyInfection(int amount)
    {
        PlayerHealth player = Object.FindObjectOfType<PlayerHealth>();
        if (player != null)
        {
            player.CureInfection(amount);
            Debug.Log("Infection appliquée !");
            Consume(); 
        }
    }

    public void Consume()
    {
        InventoryGrid grid = Object.FindObjectOfType<InventoryGrid>();

        if (grid != null)
        {
            grid.RemoveItem(this);

            Debug.Log($"{data.itemName} a été consommé et retiré de l'inventaire.");

            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Impossible de trouver InventoryGrid pour supprimer l'objet !");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Début du Drag");
        Object.FindObjectOfType<InventoryGrid>().RemoveItem(this);

        transform.SetParent(transform.root.GetComponentInChildren<Canvas>().transform);

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // On suit la souris
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Fin du Drag");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        InventoryUI ui = Object.FindObjectOfType<InventoryUI>();
        InventoryGrid grid = Object.FindObjectOfType<InventoryGrid>();
        Vector2Int targetPos = ui.GetGridPositionFromMouse(eventData.position);

        if (targetPos.x != -1 && grid.CanPlaceShape(data, targetPos))
        {
            grid.PlaceItem(this, targetPos);
            transform.SetParent(ui.cells[targetPos.x, targetPos.y]);
            rect.anchoredPosition = Vector2.zero;
        }
        else
        {
            // Retour à l'envoyeur
            grid.PlaceItem(this, gridPosition);
            transform.SetParent(ui.cells[gridPosition.x, gridPosition.y]);
            rect.anchoredPosition = Vector2.zero;
        }
    }

    public void SetGridPosition(Vector2Int pos) => gridPosition = pos;
    public Vector2Int GetGridPosition() => gridPosition;
}