using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    public int width = 5;
    public int height = 4;
    private InventoryItem[,] grid;

    private void Awake() => grid = new InventoryItem[width, height];

    public bool CanPlaceShape(InventoryItemData data, Vector2Int pos)
    {
        foreach (var offset in data.shape)
        {
            int checkX = pos.x + offset.x;
            int checkY = pos.y + offset.y;

            if (checkX < 0 || checkX >= width || checkY < 0 || checkY >= height) return false;
            if (grid[checkX, checkY] != null) return false;
        }
        return true;
    }

    public void PlaceItem(InventoryItem item, Vector2Int pos)
    {
        InventoryUI ui = Object.FindObjectOfType<InventoryUI>();

        // 1. On remplit la grille logique
        foreach (var offset in item.data.shape)
        {
            grid[pos.x + offset.x, pos.y + offset.y] = item;
        }

        // 2. On ne change la priorité que de la cellule PARENTE
        if (ui != null && ui.cells[pos.x, pos.y] != null)
        {
            // On monte le SortingOrder de la cellule qui porte l'item
            SetCellPriority(ui.cells[pos.x, pos.y].gameObject, true);
        }

        item.SetGridPosition(pos);
    }

    public void RemoveItem(InventoryItem item)
    {
        Vector2Int pos = item.GetGridPosition();
        InventoryUI ui = Object.FindObjectOfType<InventoryUI>();

        foreach (var offset in item.data.shape)
        {
            int targetX = pos.x + offset.x;
            int targetY = pos.y + offset.y;

            grid[targetX, targetY] = null;

            // On remet la priorité à 0 quand l'objet part
            if (ui != null && ui.cells[targetX, targetY] != null)
            {
                SetCellPriority(ui.cells[targetX, targetY].gameObject, false);
            }
        }
    }

    public void SetCellPriority(GameObject cell, bool isOccupied)
    {
        Canvas cellCanvas = cell.GetComponent<Canvas>();
        if (cellCanvas != null)
        {
            // 10 = Devant les cases vides, 0 = Normal
            cellCanvas.sortingOrder = isOccupied ? 10 : 0;
        }
    }

    public InventoryItem GetItemAt(int x, int y) => grid[x, y];
    public void ClearGrid() => System.Array.Clear(grid, 0, grid.Length);
}