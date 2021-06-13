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

    [SerializeField] float _stoneOffset = 0.5f;
    [SerializeField] float _delayTime = 0.2f;

    [SerializeField] Text _turnText = null;
    [SerializeField] Text _blackText = null;
    [SerializeField] Text _whiteText = null;
    [SerializeField] Image _gameStatePanel = null;
    [SerializeField] Text _gameStateText = null;
    int _rows = 8;
    int _columns = 8;
    BoardCube[,] _boardCubes;
    float _delayCount = 0;
    int _passCount = 0;

    void Start()
    {
        _boardCubes = new BoardCube[_rows, _columns];
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

        oku(_boardCubes[3, 3], true);
        oku(_boardCubes[3, 4], false);
        oku(_boardCubes[4, 3], false);
        oku(_boardCubes[4, 4], true);
        //_boardCubes[3, 3]._placedStone.IsWhite = true;
        //_boardCubes[3, 3]._placedStone.transform.rotation = Quaternion.Euler(0, 0, 180);
        //_boardCubes[4, 4]._placedStone.IsWhite = true;
        //_boardCubes[4, 4]._placedStone.transform.rotation = Quaternion.Euler(0, 0, 180);

        _isPlaying = true;
        _gameStatePanel.gameObject.SetActive(false);
        TurnUpdate(false);
        BoardUpdate();
    }

    void Update()
    {
        if (!_isPlaying) return;

        //�I�[�g�ł̓���
        if (_blackAuto && !_isWhiteTurn || _whiteAuto && _isWhiteTurn)
        {
            _delayCount += Time.deltaTime;
            if (_delayCount > _delayTime)
            {
                _delayCount = 0;

                List<BoardCube> canBePlacedBoard = new List<BoardCube>();
                foreach (var bc in _boardCubes)
                {
                    if (bc.CanBePlaced)// && !bc._placedStone)
                    {
                        canBePlacedBoard.Add(bc);
                    }
                }
                PlaceStone(canBePlacedBoard[Random.Range(0, canBePlacedBoard.Count)]);
            }
        }
    }


    /// <summary>
    /// �΂�u��
    /// </summary>
    /// <param name="boardCube"></param>
    public void PlaceStone(BoardCube boardCube)
    {
        oku(boardCube, _isWhiteTurn);
        //�u��
        //Stone stone = Instantiate(_stonePrefab, boardCube.transform);
        ////stone.transform.SetParent(boardCube.transform);
        //Vector3 offset = new Vector3(0, _stoneOffset, 0);
        //stone.transform.position = boardCube.transform.position + offset;
        //if (!_isWhiteTurn)
        //{
        //    stone.IsWhite = false;
        //    stone.transform.rotation = Quaternion.identity;
        //}
        //else
        //{
        //    stone.IsWhite = true;
        //    stone.transform.rotation = Quaternion.Euler(0, 0, 180);
        //}
        //boardCube._placedStone = stone;


        kaesu(boardCube);
        //�Ђ�����Ԃ�
        //foreach (var turnOver in GetTurnOverWhenPlaced(boardCube))
        //{
        //    turnOver.IsWhite = _isWhiteTurn ? true : false;
        //}

        TurnUpdate(!_isWhiteTurn);
        BoardUpdate();
    }

    void oku(BoardCube boardCube, bool isWhite)
    {
        Stone stone = Instantiate(_stonePrefab, boardCube.transform);
        //stone.transform.SetParent(boardCube.transform);
        Vector3 offset = new Vector3(0, _stoneOffset, 0);
        stone.transform.position = boardCube.transform.position + offset;
        stone.IsWhite = !isWhite ? false : true;
        //if (!isWhite)
        //{
        //    stone.IsWhite = false;
        //    //stone.transform.rotation = Quaternion.identity;
        //}
        //else
        //{
        //    stone.IsWhite = true;
        //    //stone.transform.rotation = Quaternion.Euler(0, 0, 180);
        //}
        boardCube._placedStone = stone;

        if (_isPlaying)
        {
            stone.PlaceMovement();
        }
    }

    void kaesu(BoardCube boardCube)
    {
        foreach (var stone in GetTurnOverWhenPlaced(boardCube))
        {
            stone.IsWhite = _isWhiteTurn ? true : false;
            stone.TurnOverMovement();
        }
    }

    /// <summary>
    /// �^�[�����X�V
    /// </summary>
    void TurnUpdate(bool nextTurnIsWhite)
    {
        _isWhiteTurn = nextTurnIsWhite;

        _turnText.text = _isWhiteTurn ? "���̔�" : "���̔�";
        _turnText.color = _isWhiteTurn ? Color.white : Color.black;
    }

    /// <summary>
    /// �Ֆʂ�UI���X�V
    /// </summary>
    void BoardUpdate()
    {
        //�R�}���J�E���g���X�V
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
        _blackText.text = $"�� �F{black}";
        _whiteText.text = $"�� �F{white}";

        //�u����}�X���X�V
        int canCount = 0;
        foreach (var bc in _boardCubes)
        {
            //bc.CanBePlaced = bc._placedStone == null && GetTurnOverWhenPlaced(bc).Length > 0 ? true : false;
            if (bc._placedStone == null && GetTurnOverWhenPlaced(bc).Length > 0)
            {
                bc.CanBePlaced = true;
                canCount++;
            }
            else
            {
                bc.CanBePlaced = false;
            }
        }
        //�p�X��2�A���ŋN����ƃQ�[���I��
        if (canCount == 0)
        {
            if (_passCount == 0)
            {
                _passCount++;
                TurnUpdate(!_isWhiteTurn);
                BoardUpdate();
            }
            else
            {
                _isPlaying = false;
                _gameStatePanel.gameObject.SetActive(true);
                if (black == white)
                {
                    _gameStateText.text = "��������";
                    _gameStateText.color = Color.gray;
                    _gameStateText.gameObject.GetComponent<Outline>().effectColor = Color.black;
                }
                else if (black > white)
                {
                    _gameStateText.text = "���̏���";
                    _gameStateText.color = Color.black;
                    _gameStateText.gameObject.GetComponent<Outline>().effectColor = Color.white;
                }
                else
                {
                    _gameStateText.text = "���̏���";
                    _gameStateText.color = Color.white;
                    _gameStateText.gameObject.GetComponent<Outline>().effectColor = Color.black;
                }
            }
        }
        else
        {
            _passCount = 0;
        }
        //if (black + white != _rows * _columns)////////////////////�u���Ȃ��Ƃ��p�X�A���Ғu���Ȃ��Ƃ��I��
        //{
            
        //}
        //else
        //{
        //    _isPlaying = false; 
        //    _gameStatePanel.gameObject.SetActive(true);
        //    if (black == white)
        //    {
        //        _gameStateText.text = "��������";
        //        _gameStateText.color = Color.gray;
        //    }
        //    else if (black > white)
        //    {
        //        _gameStateText.text = "���̏���";
        //        _gameStateText.color = Color.black;
        //    }
        //    else
        //    {
        //        _gameStateText.text = "���̏���";
        //        _gameStateText.color = Color.white;
        //    }
        //}
    }

    /// <summary>
    /// ���g���C
    /// </summary>
    public void Retry()
    {
        foreach (var bc in _boardCubes)
        {
            Destroy(bc.gameObject);
        }
        Start();
    }


    /// <summary>
    /// �w��}�X�ɒu���ꂽ�Ƃ��ɗ��Ԃ�΂�Ԃ�
    /// </summary>
    /// <param name="boardCube"></param>
    /// <returns></returns>
    Stone[] GetTurnOverWhenPlaced(BoardCube boardCube)
    {
        //8�������m�F����
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
                    //����ɓ��F�R�}������Ζ߂�悤�ɈِF�R�}���擾
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

    public bool GetNowAuto()
    {
        return _blackAuto && !_isWhiteTurn || _whiteAuto && _isWhiteTurn ? true : false;
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
