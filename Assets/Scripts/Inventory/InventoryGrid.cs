using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
	public int width = 5;
	public int height = 4;

	private InventoryItem[,] grid;

    private void Awake()
    {
		grid = new InventoryItem[width, height];
    }

	public bool IsCellFree(Vector2Int position)
	{
		if (position.x < 0 || position.y < 0 || position.x >= width || position.y >= height)
		{
			return false;
		}
		return grid[position.x, position.y] == null;
	}

	public bool PlaceItem(InventoryItem item, Vector2Int position)
	{
		if (!IsCellFree(position))
		{
			return false;
		}
		grid[position.x, position.y] = item;
		item.SetGridPosition(position);

		return true;
	}

	public void RemoveItem(Vector2Int position)
	{
		if (position.x < 0 || position.y < 0 || position.x >= width || position.y >= height)
		{
			return;
		}

		grid[position.x, position.y] = null;
	}

    public void ClearGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = null;
            }
        }
    }

}