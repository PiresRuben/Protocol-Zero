using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
	public int widht = 5;
	public int height = 4;

	private InventoryItem[,] grid;

    private void Awake()
    {
		grid = new InventoryItem[widht, height];
    }

	public bool IsCellFree(Vector2Int position)
	{
		if (position.x < 0 || position.y < 0 || position.x >= widht || position.y >= height)
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
		if (position.x < 0 || position.y < 0 || position.x >= widht || position.y >= height)
		{
			return;
		}

		grid[position.x, position.y] = null;
	}
}