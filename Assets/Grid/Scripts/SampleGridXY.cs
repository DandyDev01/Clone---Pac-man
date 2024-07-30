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

		/// <summary>
		/// Find a path from the start location to the target location.
		/// </summary>
		/// <param name="target">Where the goal is.</param>
		/// <param name="start">Where the starting localtion is (initial state.)</param>
		/// <returns>A path from the start location to target location</returns>
		public List<Node> CalculatePath(Vector2 target, Vector2 start, Vector2 direction)
		{
			DeletePathMarkers();

			// normalize the start location to grid coords (alignment)
			start = grid.GetCellPosition(start);
			start = grid.GetWorldPosition((int)start.x, (int)start.y);

			// normalize the target location to the grid coords (alignment)
			target = grid.GetCellPosition(target);
			target = grid.GetWorldPosition((int)target.x, (int)target.y);

			Node root = CreateRoot(start, direction);
			List<Node> nodes = new();
			bool goalFound = false;

			nodes.Add(root);

			int index = 0;
			Node current = root;
			while (goalFound == false && index < 1300)
			{
				// node is not within the bounds of the search space.
				if (grid.IsInRange(current._worldPosition) == false)
				{
					continue;
				}

				// exit condition, path to goal is found.
				if (Vector3.Distance(current._worldPosition, target) < 0.1f)
				{
					goalFound = true;
					break;
				}

				Vector3[] neighborsWorldPosition = grid.GetNeighboursWorldPositions(current._worldPosition).ToArray();

				// get the neighboring cells of current that are traversable and not already marked.
				foreach (Vector3 neightbor in neighborsWorldPosition)
				{
					Vector3 cell = grid.GetCellPosition(neightbor);
					bool value = grid.GetElement((int)cell.x, (int)cell.y);
					if (value == false)
						continue;

					var traversableNeighbor = new Node(neightbor, current);
					if (nodes.Contains(x => x._worldPosition.Approx(traversableNeighbor._worldPosition)) == false)
					{
						nodes.Add(traversableNeighbor);
						GameObject g = Instantiate(_marker, traversableNeighbor._worldPosition, Quaternion.identity);
						_markers.Add(g);
					}
				}

				index += 1;
				// could not find a path
				if (index >= nodes.Count)
				{
					Debug.Log("Issue. Cannot get to target: " + target);
					EditorApplication.isPaused = true;
					return new List<Node>();
				}

				current = nodes[index];
			}

			DeletePathMarkers();

			List<Node> path = BuildPath(root, current);

			// draw path
			foreach (var item in path)
			{
				GameObject g = Instantiate(_marker, item._worldPosition, Quaternion.identity);
				_markers.Add(g);
			}

			return path;
		}

		private void DeletePathMarkers()
		{
			foreach (var item in _markers)
			{
				Destroy(item);
			}
			_markers.Clear();
		}

		/// <summary>
		/// build a path from and end node to start node
		/// </summary>
		/// <param name="start">the start of the path</param>
		/// <param name="end">The end of the path</param>
		/// <returns>path from start to end.</returns>
		private static List<Node> BuildPath(Node start, Node end)
		{
			List<Node> path = new();

			int i = 0;
			while (end._worldPosition.Approx(start._worldPosition) == false && i < 1300)
			{
				path.Add(end);
				end = end._parent;
				i++;
			}

			path.Reverse();

			return path;
		}

		/// <summary>
		/// Helper method to setup the root node.
		/// </summary>
		/// <param name="start">Location of the root node.</param>
		/// <returns>The root node.</returns>
		private Node CreateRoot(Vector2 start, Vector2 direction)
		{
			Vector3[] neighborsWorldPosition = grid.GetNeighboursWorldPositions(start).ToArray();

			List<Vector3> traversable = new();

			foreach (Vector3 neightbor in neighborsWorldPosition)
			{
				Vector3 cell = grid.GetCellPosition(neightbor);
				bool value = grid.GetElement((int)cell.x, (int)cell.y);
				if (value && neightbor != (Vector3)(start + (direction * -1)))
					traversable.Add(neightbor);
				
			}

			Node root = new Node(start, traversable.ToArray());

			foreach (var item in root._children)
			{
				item._parent = root;
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

		public static bool Contains<T>(this List<T> collection, Func<T, bool> p)
		{
			return collection.Where(p).Any();
		}
	}
}
