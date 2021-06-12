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
    //[SerializeField] bool _onStone = false;
    [SerializeField] Stone _placedStone = null;
    public Stone PlacedStone
    {
        get { return _placedStone; }
        set { _placedStone = value; }
    }

    //int _row;
    //int _column;
    
    ///// <summary>
    ///// マス情報をセット
    ///// </summary>
    ///// <param name="row"></param>
    ///// <param name="column"></param>
    //public void SetIndex(int row, int column)
    //{
    //    _row = row;
    //    _column = column;
    //}

    /// <summary>
    /// マスを選択
    /// </summary>
    public void Select()
    {
        if (!_placedStone)
        {
            FindObjectOfType<Reversi>().PlaceStone(this);
        }
    }

}
