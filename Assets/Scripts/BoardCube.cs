using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCube : MonoBehaviour
{
    [SerializeField] bool _canBePlaced = false;
    [SerializeField] Stone _placedStone = null;

    int _row;
    int _column;

    public bool CanBePlaced
    {
        get { return _canBePlaced; }    
        set 
        { 
            _canBePlaced = value;
            GetComponent<Renderer>().material.color = _canBePlaced ? new Color(2, 2, 2) : Color.white;
        }
    }
    public Stone PlacedStone { get { return _placedStone; } set { _placedStone = value; } }
    public int Row { get { return _row; } set { _row = value; } }
    public int Column { get { return _column; } set { _column = value; } }

    /// <summary>
    /// ƒ}ƒX‚ð‘I‘ð
    /// </summary>
    public void SelectBoard()
    {
        Reversi reversi = FindObjectOfType<Reversi>();
        if (_canBePlaced && reversi.GetControlNow())
        {
            reversi.PlaceStone(this);
        }
    }
}
