using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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
		[SerializeField] private GameObject _marker1;
		[SerializeField] private Transform s;
		[SerializeField] private Transform t;


		private List<GameObject> _markers;

		private GridXY<bool> grid;

		public int Columns { get { return columns; } }
		public int Rows { get { return rows; } }
		public float CellSize { get { return cellSize; } }
		public Vector3 Oragin { get { return oragin; } }

		public GridXY<bool> Grid => grid;

		public void Awake()
		{
			grid = new GridXY<bool>(oragin, columns, Rows, CellSize);
			_markers = new List<GameObject>();
			SetTraversableValues();
		}

		public Vector3 WorldPosition(int row, int col)
		{
			return grid.GetWorldPosition(col, row);
		}

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

		public List<Node> CalculatePath(Vector2 target, Vector2 start)
		{
			Node root = CreateRoot(start);
			List<Node> nodes = new();
			bool goalFound = false;

			//if (_markers.Count > 0)
			//{
			//	foreach (var marker in _markers)
			//	{
			//		Destroy(marker.gameObject);
			//	}

			//	_markers.Clear();
			//}

			nodes.Add(root);

			int index = 0;
			Node current = root;
			Node goal = null;
			while (goalFound == false && index < 1300)
			{
				if (grid.IsInRange(current._worldPosition) == false)
				{
					continue;
				}

				if (current._worldPosition.Approx((Vector3)target))
				{
					goalFound = true;
					goal = current;
				}

				Vector3[] neighborsWorldPosition = grid.GetNeighboursWorldPositions(current._worldPosition).ToArray();

				List<Vector3> traversable = new();

				foreach (Vector3 neightbor in neighborsWorldPosition)
				{
					Vector3 cell = grid.GetCellPosition(neightbor);
					bool value = grid.GetElement((int)cell.x, (int)cell.y);
					if (value)
						traversable.Add(neightbor);
				}

				foreach (var item in traversable)
				{
					var newNode = new Node(item, current);
					if (nodes.Where(x => x._worldPosition.Approx(newNode._worldPosition)).Any() == false)
					{
						nodes.Add(newNode);
					}
				}

				index += 1;
				if (index >= nodes.Count)
				{
					Debug.Log("issue");
					return new List<Node>();
				}

				current = nodes[index];

				//var g = Instantiate(_marker, current._worldPosition, Quaternion.identity);
				//_markers.Add(g);
			}

			List<Node> path = new();

			int i = 0;
			while (current._worldPosition.Approx(root._worldPosition) == false && i < 1300)
			{
				path.Add(current);
				current = current._parent;
				i++;
			}

			path.Reverse();

			index = 0;
			goalFound = false;

			return path;
		}

		private Node CreateRoot(Vector2 start)
		{
			Vector3[] neighborsWorldPosition = grid.GetNeighboursWorldPositions(start).ToArray();

			List<Vector3> traversable = new();

			foreach (Vector3 neightbor in neighborsWorldPosition)
			{
				Vector3 cell = grid.GetCellPosition(neightbor);
				bool value = grid.GetElement((int)cell.x, (int)cell.y);
				if (value)
					traversable.Add(neightbor);
				
			}

			Node root = new Node(start, traversable.ToArray());

			foreach (var item in root._children)
			{
				item._parent = root;
				Instantiate(_marker, item._worldPosition, Quaternion.identity);
			}

			return root;
		}
	}

	

	public class Node
	{
		public readonly List<Node> _children = new();
		public readonly Vector3 _worldPosition;
		public Node _parent;

		public Node(Vector3 item)
		{
			_worldPosition = item;
		}

		public Node(Vector3 item, Node parent)
		{
			_worldPosition = item;
			_parent = parent;
		}

		public Node(Vector2 start, Vector3[] traverableNeighbors)
		{
			_worldPosition = start;
			foreach (var item in traverableNeighbors)
			{
				_children.Add(new Node(item));
			};
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
	}

}
