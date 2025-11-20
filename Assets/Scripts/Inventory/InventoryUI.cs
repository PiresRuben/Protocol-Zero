using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventoryGrid grid;
    public GameObject cellPrefab;

    public int cellSize = 64;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (int y = 0; y < grid.height; y++)
        {
            for (int x = 0; x < grid.widht; x++)
            {
                GameObject cell = Instantiate(cellPrefab, transform);
                RectTransform rt = cell.GetComponent<RectTransform>();
                // place each cellslot (empty) in order, remove the "-" next to the y to make the inventory go up and not down
                float spacing = 2f; // 2 pixels between each cell
                rt.anchoredPosition = new Vector2(x * (cellSize + spacing), -y * (cellSize + spacing));
            }
        }
    }
}