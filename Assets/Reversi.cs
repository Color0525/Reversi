using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reversi : MonoBehaviour
{
    [SerializeField] bool isWhiteTurn = true;
    [SerializeField] BoardCube _boardCubePrefab = null;
    [SerializeField] Stone _stonePrefab = null;
    [SerializeField] float _stoneOffset = 0.2f;
    int _rows = 8;
    int _columns = 8;

    void Start()
    {
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                BoardCube bc = Instantiate(_boardCubePrefab);
                bc.transform.position = new Vector3(c - 3.5f, 0, -r + 3.5f);
                bc.SetIndex(r, c);
            }
        }
    }

    public void PlaceStone(BoardCube boardCube)
    {
        Stone stone = Instantiate(_stonePrefab);
        Vector3 pos = new Vector3(boardCube.transform.position.x, boardCube.transform.position.y + _stoneOffset, boardCube.transform.position.z);
        stone.transform.position = pos;
        stone.transform.SetParent(boardCube.transform);
    }
}
