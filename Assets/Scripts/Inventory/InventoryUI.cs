using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventoryGrid grid;
    public GameObject cellPrefab;

    public int cellSize = 64;


    [HideInInspector]
    public RectTransform[,] cells;

    private bool gridCreated = false;

    public void CreateGrid()
    {
        if (gridCreated) return;

        cells = new RectTransform[grid.width, grid.height];

        for (int y = 0; y < grid.height; y++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                GameObject cell = Instantiate(cellPrefab, transform);
                RectTransform rt = cell.GetComponent<RectTransform>();

                float spacing = 2f;
                rt.anchoredPosition = new Vector2(
                    x * (cellSize + spacing),
                    -y * (cellSize + spacing)
                );

                cells[x, y] = rt;
            }
        }

        gridCreated = true;
    }

    public Vector2Int GetGridPositionFromMouse(Vector2 mousePos)
    {
        for (int y = 0; y < grid.height; y++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(cells[x, y], mousePos))
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

}