using Grid;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private SampleGridXY _grid;
	[SerializeField] private Transform _target;
	[SerializeField] private GameObject _marker;

	private List<Node> _path;
	private Node _currentTarget;
	private int _index = 0;

	private void Start()
	{

		StartCoroutine(PathUpdater());

		_currentTarget = _path.First();
	}

	private void Update()
	{
		if (_index >= _path.Count)
			return;

		transform.position = Vector2.MoveTowards(transform.position, _currentTarget._worldPosition, _speed * Time.deltaTime);

		if (transform.position == _currentTarget._worldPosition)
		{
			_index += 1;

			if (_index >= _path.Count)
				return;

			_currentTarget = _path[_index];
		}
	}

	private IEnumerator PathUpdater()
	{
		while (true)
		{
			_path = _grid.CalculatePath(_target.position, transform.position);

			foreach (Node node in _path)
			{
				Instantiate(_marker, node._worldPosition, Quaternion.identity);
			}

			_index = 0;
			Debug.Log("update");
			yield return new WaitForSeconds(1f);
		}
	}
}
