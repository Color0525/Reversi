using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reversi : MonoBehaviour
{
    [SerializeField] bool _blackAuto = false;
    [SerializeField] bool _whiteAuto = true;

    [SerializeField] bool _isPlaying = false;
    [SerializeField] bool _isWhiteTurn = false;

    [SerializeField] BoardCube _boardCubePrefab = null;
    [SerializeField] Stone _stonePrefab = null;
    [SerializeField] float _stoneOffset = 0.2f;

    [SerializeField] Text _turnText = null;
    [SerializeField] Text _blackText = null;
    [SerializeField] Text _whiteText = null;
    [SerializeField] Image _gameStatePanel = null;
    [SerializeField] Text _gameStateText = null;
    int _rows = 8;
    int _columns = 8;
    BoardCube[,] _boardCubes;
    float delayCount = 0;

    void Start()
    {
        _boardCubes = new BoardCube[_rows, _columns];
        Setup();
    }

    void Update()
    {
        if (!_isPlaying) return;
        if (_blackAuto && !_isWhiteTurn)
        {

        }    
    }

    /// <summary>
    /// 盤面をセットアップ
    /// </summary>
    public void Setup()/////////// ＜_boardCubesの要素をデストロイする
    {
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                _boardCubes[r, c] = Instantiate(_boardCubePrefab);
                _boardCubes[r, c].transform.position = new Vector3(c - 3.5f, 0, -r + 3.5f);
                _boardCubes[r, c]._row = r;
                _boardCubes[r, c]._column = c;
            }
        }
        PlaceStone(_boardCubes[3, 3]);
        _boardCubes[3, 3]._placedStone.IsWhite = true;
        PlaceStone(_boardCubes[3, 4]);
        _boardCubes[3, 4]._placedStone.IsWhite = false;
        PlaceStone(_boardCubes[4, 3]);
        _boardCubes[4, 3]._placedStone.IsWhite = false;
        PlaceStone(_boardCubes[4, 4]);
        _boardCubes[4, 4]._placedStone.IsWhite = true;

        TurnUpdate(_isWhiteTurn);
        BoardUpdate();
    }
    
    /// <summary>
    /// ターンを更新
    /// </summary>
    void TurnUpdate(bool nextTurnIsWhite)
    {
        _isWhiteTurn = nextTurnIsWhite;

        _turnText.text = _isWhiteTurn ? "白の番" : "黒の番";
        _turnText.color = _isWhiteTurn ? Color.white : Color.black;
    }

    /// <summary>
    /// 盤面やUIを更新
    /// </summary>
    void BoardUpdate()
    {
        int black = 0;
        int white = 0;
        foreach (var bc in _boardCubes)
        {
            if (bc._placedStone)
            {
                if (!bc._placedStone.IsWhite)
                {
                    black++;
                }
                else
                {
                    white++;
                }
            }
        }
        _blackText.text = $"黒 ： {black}";
        _whiteText.text = $"白 ： {white}";

        if (black + white != _rows * _columns)////////////////////置けないときパス、両者置けないとき終了
        {
            _isPlaying = true;
            _gameStatePanel.gameObject.SetActive(false);
            foreach (var bc in _boardCubes)
            {
                bc.CanBePlaced = bc._placedStone == null && GetTurnOverWhenPlaced(bc).Length > 0 ? true : false;
            }
        }
        else
        {
            _isPlaying = false;
            _gameStatePanel.gameObject.SetActive(true);
            _gameStateText.text = black == white ? "引き分け"
                : black > white ? "黒の勝ち"
                : "白の勝ち";
        }
    }


    /// <summary>
    /// 石を置く
    /// </summary>
    /// <param name="boardCube"></param>
    public void PlaceStone(BoardCube boardCube)
    {
        Stone stone = Instantiate(_stonePrefab);
        stone.IsWhite = _isWhiteTurn ? true : false;
        Vector3 pos = new Vector3(boardCube.transform.position.x, boardCube.transform.position.y + _stoneOffset, boardCube.transform.position.z);
        stone.transform.position = pos;
        stone.transform.SetParent(boardCube.transform);
        boardCube._placedStone = stone;

        if (!_isPlaying) return;

        //ひっくり返す
        foreach (var turnOver in GetTurnOverWhenPlaced(boardCube))
        {
            turnOver.IsWhite = _isWhiteTurn ? true : false;
        }

        //ターン切り替え
        TurnUpdate(!_isWhiteTurn);
        BoardUpdate();
    }

    /// <summary>
    /// 指定マスに置かれたときに裏返る石を返す
    /// </summary>
    /// <param name="boardCube"></param>
    /// <returns></returns>
    Stone[] GetTurnOverWhenPlaced(BoardCube boardCube)
    {
        List<Stone> turnOverStones = new List<Stone>();
        for (int i = -1; i <= 1; i++)
        {
            for (int k = -1; k <= 1; k++)
            {
                int dirR = boardCube._row + i;
                int dirC = boardCube._column + k;
                while (true)
                {
                    if (0 > dirR || dirR >= _rows || 0 > dirC || dirC >= _columns) break;
                    if (_boardCubes[dirR, dirC] == _boardCubes[boardCube._row, boardCube._column]) break;
                    if (!_boardCubes[dirR, dirC]._placedStone) break;

                    if (_boardCubes[dirR, dirC]._placedStone.IsWhite == _isWhiteTurn)
                    {
                        List<Stone> lineTurnOverStones = new List<Stone>();
                        int r = dirR - i;
                        int c = dirC - k;
                        while (true)
                        {
                            if (r == boardCube._row && c == boardCube._column)
                            {
                                if (lineTurnOverStones.Count > 0)
                                {
                                    turnOverStones.AddRange(lineTurnOverStones);
                                }
                                break;
                            }
                            if (_boardCubes[r, c]._placedStone.IsWhite != _isWhiteTurn)
                            {
                                lineTurnOverStones.Add(_boardCubes[r, c]._placedStone);
                            }
                            r -= i;
                            c -= k;
                        }
                        break;
                    }
                    dirR += i;
                    dirC += k;
                }
            }
        }
        return turnOverStones.ToArray();
    }

    //IEnumerable<BoardCube> GetNeighborBoardCubes(BoardCube boardCube)
    //{
    //    int row = boardCube._row;
    //    int column = boardCube._column;
    //    for (int i = -1; i <= 1; i++)
    //    {
    //        for (int k = -1; k <= 1; k++)
    //        {
    //            if (0 <= row + i && row + i < _rows && 0 <= column + k && column + k < _columns
    //                && _boardCubes[row + i, column + k] != _boardCubes[row, column])
    //            {
    //                yield return _boardCubes[row + i, column + k];
    //            }
    //        }
    //    }
    //}

}
