using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCube : MonoBehaviour
{
    //public enum OnState
    //{
    //    None = 0,
    //    White = 1,
    //    Black = 2,
    //}

    //[SerializeField] OnState _onState = OnState.None;
    [SerializeField] bool _onStone = false; 
    int _row;
    int _column;
    

    public void SetIndex(int row, int column)
    {
        _row = row;
        _column = column;
    }

    public void Select()
    {
        if (!_onStone)
        {
            _onStone = true;
            FindObjectOfType<Reversi>().PlaceStone(this);
        }
    }

}
