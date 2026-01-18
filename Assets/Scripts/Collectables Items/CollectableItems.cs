using UnityEngine;

public class CollectableItems : MonoBehaviour
{
    [Header("Item Data")]
    public InventoryItemData linkedItem;
    [SerializeField] private Sprite ItemSprite;

    [Header("Collectable Parameters")]
    [SerializeField] private float rangeToCollect = 2.0f;
    [SerializeField] private bool collectable = false;

    public PlayerController player;
    public InventoryManager inventoryManager;

    [Header("UI Elements")]
    [SerializeField] private Vector3 uiOffset = new Vector3(0, 0.8f, 0);
    public Canvas CollectableUI;

    void Start()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerController>();
        }
        if (inventoryManager == null)
        {
            inventoryManager = FindFirstObjectByType<InventoryManager>();
        }
    }

    private void Update()
    {
        if (checkRange())
        {
            DisplayUI();
        }
        else 
        {
            CollectableUI.enabled = false;
        }
    }

    private bool checkRange()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= rangeToCollect)
        {
            collectable = true;
            return true;
        }

        collectable = false; // collectable sert juste à track l'état et pour empecher de chopper l'item hors de portée
        return false;

    }

    private void DisplayUI()
    {

        if (collectable)
        {
            CollectableUI.transform.position = transform.position + uiOffset;
            CollectableUI.enabled = true;
            if ( Input.GetKeyDown(KeyCode.V))
            {
                if (inventoryManager != null)
                {
                    inventoryManager.AddItem(linkedItem);
                    CollectableUI.enabled = false;  
                    Destroy(gameObject);
                }
            }
        }
                
    }
}
