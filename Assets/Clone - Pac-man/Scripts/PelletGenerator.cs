using Grid;
using PlasticGui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PelletGenerator : MonoBehaviour
{
    [SerializeField] SampleGridXY _grid;
    [SerializeField] Tilemap _tilemap;
    [SerializeField] Pellet _pelletPrefab;

	private List<Pellet> _activePellets;
	private List<Pellet> _inactivePellets;

	public Action OnAllPelletPickedup;

	private void Awake()
	{
		_activePellets = new List<Pellet>();
		_inactivePellets = new List<Pellet>();
	}

	// Start is called before the first frame update
	private void Start()
    {
		_tilemap.CompressBounds();
		TileBase[] tiles = _tilemap.GetTilesBlock(_tilemap.cellBounds);
		GridLayout gridLayout = _tilemap.layoutGrid;

		for (int i = 0; i < _grid.Columns; i++)
		{
			for (int j = 0; j < _grid.Rows; j++)
			{
				Vector3 worldPos = _grid.WorldPosition(i, j);
				TileBase tile = _tilemap.GetTile(new Vector3Int((int)worldPos.x, (int)worldPos.y));
				if (tile is null)
				{
					worldPos = gridLayout.CellToWorld(new Vector3Int((int)worldPos.x, (int)worldPos.y));
					Pellet p = Instantiate(_pelletPrefab, worldPos + (Vector3.one / 2), Quaternion.identity);
					p.OnPickup += Deactiate;
					
					_activePellets.Add(p);
				}
			}
		}
	}

	private void Deactiate(Pellet pellet)
	{
		pellet.gameObject.SetActive(false);
		_activePellets.Remove(pellet);
		_inactivePellets.Add(pellet);

		if (_activePellets.Count <= 0)
			OnAllPelletPickedup?.Invoke();
	}

	internal void Reset()
	{
		_activePellets.AddRange(_inactivePellets);

		foreach (Pellet pellet in _inactivePellets)
		{
			pellet.gameObject.SetActive(true);
		}

		_inactivePellets.Clear();
	}
}
