using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using static Codice.CM.Common.CmCallContext;

namespace Grid
{
	public class SampleGridXY : MonoBehaviour
	{
		[SerializeField] private Vector3 oragin = Vector3.zero;
		[SerializeField] private int columns = 1;
		[SerializeField] private int rows = 1;
		[SerializeField] private float cellSize = 1;
		[SerializeField] Tilemap _tilemap;

		[SerializeField] private GameObject _marker;
		private List<GameObject> _markers = new();

		private GridXY<bool> grid;

		public GameObject Marker => _marker;
		public int Columns { get { return columns; } }
		public int Rows { get { return rows; } }
		public float CellSize { get { return cellSize; } }
		public Vector3 Oragin { get { return oragin; } }

		public GridXY<bool> Grid => grid;

		public void Awake()
		{
			grid = new GridXY<bool>(oragin, columns, Rows, CellSize);
			SetTraversableValues();
		}

		public Vector3 WorldPosition(int row, int col)
		{
			return grid.GetWorldPosition(col, row);
		}

		/// <summary>
		/// Scan the grid for obsticals and mark their cells as not traversable.
		/// </summary>
		public void SetTraversableValues()
		{
			_tilemap.CompressBounds();
			TileBase[] tiles = _tilemap.GetTilesBlock(_tilemap.cellBounds);
			GridLayout gridLayout = _tilemap.layoutGrid;

			for (int i = 0; i < Columns; i++)
			{
				for (int j = 0; j < Rows; j++)
				{
					Vector3 worldPos = WorldPosition(i, j);
					TileBase tile = _tilemap.GetTile(new Vector3Int((int)worldPos.x, (int)worldPos.y));
					if (tile is null)
					{
						worldPos = gridLayout.CellToWorld(new Vector3Int((int)worldPos.x, (int)worldPos.y));

						if (Grid.IsInRange(worldPos) == false)
							continue;

						var cell = grid.GetCellPosition(worldPos);
						grid.SetElement((int)cell.x, (int)cell.y, true);
					}
				}
			}
		}

	}

	public static class Extensions
	{

		public static bool Approx(this Vector3 me, Vector3 other)
		{
			float x1 = me.x;
			float y1 = me.y;
			float z1 = me.z;
			float x2 = other.x;
			float y2 = other.y;
			float z2 = other.z;

			if (Mathf.Approximately(x1, x2) && Mathf.Approximately(y1, y2) && Mathf.Approximately(z1, z2))
				return true;

			return false;
		}

		public static T GetRandom<T>(this T[] array)
		{
			return array[UnityEngine.Random.Range(0, array.Length)];
		}

		public static bool Contains<T>(this T[] collection, Func<T, bool> p)
		{
			return collection.Where(p).Any();
		}

		public static bool Contains<T>(this List<T> collection, Func<T, bool> p)
		{
			return collection.Where(p).Any();
		}
	}
}
