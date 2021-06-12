using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reversi : MonoBehaviour
{
    [SerializeField] bool isWhiteTurn = false;
    [SerializeField] Text _turnText = null;
    [SerializeField] BoardCube _boardCubePrefab = null;
    [SerializeField] Stone _stonePrefab = null;
    [SerializeField] float _stoneOffset = 0.2f;
    int _rows = 8;
    int _columns = 8;
    BoardCube[,] _boardCubes;

    void Start()
    {
        _boardCubes = new BoardCube[_rows, _columns];
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                _boardCubes[r, c] = Instantiate(_boardCubePrefab);
                _boardCubes[r, c].transform.position = new Vector3(c - 3.5f, 0, -r + 3.5f);
                //_boardCubes[r, c].SetIndex(r, c);
            }
        }
        PlaceStone(_boardCubes[3, 4]);
        PlaceStone(_boardCubes[3, 3]);
        PlaceStone(_boardCubes[4, 3]);
        PlaceStone(_boardCubes[4, 4]);

        TurnTextUpdate();
    }

    /// <summary>
    /// Î‚ğ’u‚­
    /// </summary>
    /// <param name="boardCube"></param>
    public void PlaceStone(BoardCube boardCube)
    {
        Stone stone = Instantiate(_stonePrefab);
        stone.IsWhite = isWhiteTurn ? true : false;
        Vector3 pos = new Vector3(boardCube.transform.position.x, boardCube.transform.position.y + _stoneOffset, boardCube.transform.position.z);
        stone.transform.position = pos;
        stone.transform.SetParent(boardCube.transform);
        boardCube.PlacedStone = stone;
        
        //syuuikakuninn 

        isWhiteTurn = isWhiteTurn ? false : true;
        TurnTextUpdate();
    }


    IEnumerable<BoardCube> GetNeighborBoardCubes(int row, int column)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int k = -1; k <= 1; k++)
            {
                if (0 <= row + i && row + i < _rows && 0 <= column + k && column + k < _columns
                    && _boardCubes[row + i, column + k] != _boardCubes[row, column])
                {
                    yield return _boardCubes[row + i, column + k];
                }
            }
        }
    }

    /// <summary>
    /// è”Ô‚ğXV
    /// </summary>
    void TurnTextUpdate()
    {
        _turnText.text = isWhiteTurn ? "”’‚Ì”Ô" : "•‚Ì”Ô";
        _turnText.color = isWhiteTurn ? Color.white : Color.black;
    }
}
