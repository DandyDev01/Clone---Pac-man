using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Grid;

public class GridXYTests
{
	int _columns = 27, _rows = 27;
	float _cellSize = 1;

    // A Test behaves as an ordinary method
    [Test]
    public void GridXYTestsSimplePasses()
    {
        // Use the Assert class to test conditions
    }

	[Test]
	public void WorldToCellPositionTest()
	{
		var oragin = new Vector2(0, 0);
		var worldPosition = new Vector2(1.23543f, 4.99876f);
		var grid = new GridXY<bool>(oragin, _columns, _rows, _cellSize);

		Vector3 cell = grid.GetCellPosition(worldPosition);

		Assert.AreEqual(cell.x, 1);
		Assert.AreEqual(cell.y, 4);
	}

	[Test]
	public void CellToWorldPosition()
	{
		var oragin = new Vector2(0, 0);
		var cell = new Vector2(11, 19);
		var grid = new GridXY<bool>(oragin, _columns, _rows, _cellSize);

		Vector3 worldPosition = grid.GetWorldPosition((int)cell.x, (int)cell.y);

		Assert.AreEqual(worldPosition.x, (cell.x * _cellSize + _cellSize / 2) + oragin.x);
		Assert.AreEqual(worldPosition.y, (cell.y * _cellSize + _cellSize / 2) + oragin.y);
	}

	[Test]
	public void EdgeCellToWorldPosition()
	{
		var oragin = new Vector2(0, 0);
		var cell = new Vector2(26, 26);
		var grid = new GridXY<bool>(oragin, _columns, _rows, _cellSize);

		Vector3 worldPosition = grid.GetWorldPosition((int)cell.x, (int)cell.y);

		Assert.AreEqual(worldPosition.x, (cell.x * _cellSize + _cellSize / 2) + oragin.x);
		Assert.AreEqual(worldPosition.y, (cell.y * _cellSize + _cellSize / 2) + oragin.y);
	}

	[Test]
	public void WorldPositionIsInRange()
	{
		var oragin = new Vector2(0, 0);
		var cell = new Vector2(26, 27);
		var grid = new GridXY<bool>(oragin, _columns, _rows, _cellSize);

		Vector3 worldPosition = grid.GetWorldPosition((int)cell.x, (int)cell.y);
		Assert.IsFalse(grid.IsInRange(worldPosition));

		cell.y = -1;
		worldPosition = grid.GetWorldPosition((int)cell.x, (int)cell.y);
		Assert.IsFalse(grid.IsInRange(worldPosition));

		cell.x = 27;
		cell.y = 0;
		worldPosition = grid.GetWorldPosition((int)cell.x, (int)cell.y);
		Assert.IsFalse(grid.IsInRange(worldPosition));

		cell.x = 12;
		cell.y = 5;
		worldPosition = grid.GetWorldPosition((int)cell.x, (int)cell.y);
		Assert.IsTrue(grid.IsInRange(worldPosition));
	}

	[Test]
	public void CellIsInRange()
	{
		var oragin = new Vector2(0, 0);
		var cell = new Vector2(26, 27);
		var grid = new GridXY<bool>(oragin, _columns, _rows, _cellSize);

		Assert.IsFalse(grid.IsInRange((int)cell.x, (int)cell.y));

		cell.y = -1;
		Assert.IsFalse(grid.IsInRange((int)cell.x, (int)cell.y));

		cell.x = 27;
		cell.y = 0;
		Assert.IsFalse(grid.IsInRange((int)cell.x, (int)cell.y));

		cell.x = 12;
		cell.y = 5;
		Assert.IsTrue(grid.IsInRange((int)cell.x, (int)cell.y));
	}


	[Test]
	public void IsInRange_WithValidPosition_ReturnsTrue()
	{
		var grid = new GridXY<bool>(Vector3.zero, 8, 8, 1f);
		Vector3 position = new Vector3(2f, 3f, 0f);
		bool result = grid.IsInRange(position);
		Assert.IsTrue(result);
	}

	[Test]
	public void IsInRange_WithInvalidPosition_ReturnsFalse()
	{
		var grid = new GridXY<bool>(Vector3.zero, 8, 8, 1f);
		Vector3 position = new Vector3(9f, 9f, 0f);
		bool result = grid.IsInRange(position);
		Assert.IsFalse(result);
	}

	[Test]
	public void GetNeighbourWorldPosition_NorthNeighbour_ReturnsCorrectPosition()
	{
		var grid = new GridXY<bool>(Vector3.zero, 8, 8, 1f);
		Vector3 position = new Vector3(2f, 2f, 0f);
		Vector3 result = grid.GetNeighbourWorldPosition(position, cellNeighbour.north);
		Assert.AreEqual(new Vector3(2.5f, 3.5f, 0f), result);
	}

	[Test]
	public void SetElement_WithValidPosition_SetsElementCorrectly()
	{
		var grid = new GridXY<bool>(Vector3.zero, 8, 8, 1f);
		bool element = true;
		bool result = grid.SetElement(2, 2, element);
		Assert.IsTrue(result);
		Assert.AreEqual(element, grid.GetElement(2, 2));
	}

	[Test]
	public void SetElement_WithInvalidPosition_ReturnsFalse()
	{
		var grid = new GridXY<bool>(Vector3.zero, 8, 8, 1f);
		bool element = false;
		bool result = grid.SetElement(10, 10, element);
		Assert.IsFalse(result);
	}
}
