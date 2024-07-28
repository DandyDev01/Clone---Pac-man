using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private SampleGridXY _grid;
	[SerializeField] private Transform _target;
	[SerializeField] private GameObject _marker;

	private void Start()
	{
		//List<Node> path = _grid.CalculatePath(_target.position, transform.position);

		//foreach (Node node in path)
		//{
		//	Instantiate(_marker, node._worldPosition, Quaternion.identity);
		//}
	}
}
