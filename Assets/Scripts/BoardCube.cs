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
    [SerializeField] bool _canBePlaced = false;
    public bool CanBePlaced
    {
        get { return _canBePlaced; }    
        set 
        { 
            _canBePlaced = value;
            GetComponent<Renderer>().material.color = _canBePlaced ? new Color(2, 2, 2) : Color.white;
            //if (_canBePlaced)
            //{
            //    ren.material.color += Color.white;
            //}
            //else
            //{

            //}
        }
    }

    //[SerializeField] OnState _onState = OnState.None;
    //[SerializeField] bool _onStone = false;
    public Stone _placedStone = null;
    //public Stone PlacedStone
    //{
    //    get { return _placedStone; }
    //    set { _placedStone = value; }
    //}

    public int _row;
    public int _column;

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
        Reversi reversi = FindObjectOfType<Reversi>();
        if (_canBePlaced && !reversi.GetNowAuto())// && !_placedStone)
        {
            reversi.PlaceStone(this);
        }
    }

}
