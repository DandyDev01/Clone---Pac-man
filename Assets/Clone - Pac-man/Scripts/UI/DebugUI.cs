using Grid;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _value;
    [SerializeField] private TextMeshProUGUI _world;
    [SerializeField] private TextMeshProUGUI _cell;
    [SerializeField] private SampleGridXY _grid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_grid.Grid.IsInRange(mousePos) == false)
            return;

		_canvas.gameObject.transform.position = mousePos;
        _cell.text = "Cell: " + _grid.Grid.GetCellPosition(mousePos);
        _world.text = "World: " + (new Vector2(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y)) + (Vector2.one / 2));
        _value.text = "Value: " + _grid.Grid.GetElement((int)_grid.Grid.GetCellPosition(mousePos).x, (int)_grid.Grid.GetCellPosition(mousePos).y);
    }
}
