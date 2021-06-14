using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Photon 用の名前空間を参照する
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

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
    /// マスを選択
    /// </summary>
    public void SelectBoard()
    {
        Reversi reversi = FindObjectOfType<Reversi>();
        if (_canBePlaced && reversi.GetControlNow())
        {
            if (!reversi.Network)
            {
                reversi.PlaceStone(this);
            }
            else
            {
                RaisePlaceStone(this);
            }
        }
    }

    /// <summary>
    /// PlaceStoneイベントを起こす
    /// </summary>
    public void RaisePlaceStone(BoardCube boardCube)
    {
        //イベントとして送るものを作る
        byte eventCode = (int)NetworkGameManager.EventCode.PlaceStone;

        object[] parameters = new object[] { boardCube.Row, boardCube.Column };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.Receivers = ReceiverGroup.All;

        SendOptions sendOptions = new SendOptions(); // オプションだが、特に何も指定しない

        // イベントを起こす
        PhotonNetwork.RaiseEvent(eventCode, parameters, raiseEventOptions, sendOptions);
    }
}
