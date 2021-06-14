using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Photon �p�̖��O��Ԃ��Q�Ƃ���
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
    /// �}�X��I��
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
    /// PlaceStone�C�x���g���N����
    /// </summary>
    public void RaisePlaceStone(BoardCube boardCube)
    {
        //�C�x���g�Ƃ��đ�����̂����
        byte eventCode = (int)NetworkGameManager.EventCode.PlaceStone;

        object[] parameters = new object[] { boardCube.Row, boardCube.Column };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.Receivers = ReceiverGroup.All;

        SendOptions sendOptions = new SendOptions(); // �I�v�V���������A���ɉ����w�肵�Ȃ�

        // �C�x���g���N����
        PhotonNetwork.RaiseEvent(eventCode, parameters, raiseEventOptions, sendOptions);
    }
}
