using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Reversi : MonoBehaviour
{
    [SerializeField] bool _autoBlack = false;
    [SerializeField] bool _autoWhite = true;
    [SerializeField] float _autoSelectRate = 0.1f;
    [SerializeField] bool _noAnime = false;
    [SerializeField] float _delayTime = 0.2f;

    [SerializeField] bool _isPlaying = false;
    [SerializeField] bool _isWhiteTurn = false;
    public bool IsWhiteTurn { get { return _isWhiteTurn; } }

    [SerializeField] BoardCube _boardCubePrefab = null;
    [SerializeField] Stone _selectStone = null;
    [SerializeField] Stone _normalStonePrefab = null;
    [SerializeField] SelectStoneController[] _selectStoneControllers = null;

    //[SerializeField] Stone[] _specialStonePrefabs = null;
    //[SerializeField] int[] _specialStoneCount = null;

    [SerializeField] float _stoneOffset = 0.5f;

    [SerializeField] RectTransform _turnBackGround = null;
    [SerializeField] Vector3 _turnBackGroundOffset = new Vector3(0, 1200, 0);
    [SerializeField] float _turnBackGroundTime = 0.5f;
    //[SerializeField] Text _turnText = null;
    [SerializeField] Text _blackTotal = null;
    [SerializeField] Text _whiteTotal = null;
    [SerializeField] Text _YourBlack = null;
    [SerializeField] Text _YourWhite = null;
    [SerializeField] Image _gameStatePanel = null;
    [SerializeField] Text _gameStateText = null;

    int _rows = 8;
    int _columns = 8;
    BoardCube[,] _boardCubes;
    float _delayCount = 0;
    int _passCount = 0;
    SelectStoneController _selectStoneController = null;
    bool _endTurn = false;
    bool _endAnime = false;
    public bool EndAnime { get { return _endAnime; } set { _endAnime = value; } }

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

        _selectStone = _normalStonePrefab;
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
        _YourBlack.gameObject.SetActive(!_autoBlack);
        _YourWhite.gameObject.SetActive(!_autoWhite);
        foreach (var ss in _selectStoneControllers)
        {
            ss.Setup();
        }
        TurnUpdate(false);
        BoardUpdate();
    }

    void Update()
    {
        if (!_isPlaying) return;

        if (_endTurn)
        {
            if (_endAnime || _noAnime)
            {
                _endTurn = false;
                _endAnime = false;
                NextTurn();
            }
        }
        else
        {
            //�I�[�g�ł̓���
            if (_autoBlack && !_isWhiteTurn || _autoWhite && _isWhiteTurn)
            {
                _delayCount += Time.deltaTime;
                if (_delayCount > _delayTime)
                {
                    _delayCount = 0;

                    if (Random.value < _autoSelectRate)
                    {
                        //SelectStoneController ranStone = _selectStoneControllers[Random.Range(0, _selectStoneControllers.Length)];
                        _selectStoneControllers[Random.Range(0, _selectStoneControllers.Length)].SelectStone();
                        //if (!_isWhiteTurn)
                        //{
                        //    if (ranStone.BlackCount > 0)//(_isWhiteTurn ? BlackCount : WhiteCount) > 0)
                        //    {
                        //        SetSelectStone(ranStone._selectStonePrefab, this);
                        //    }
                        //}

                    }

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

        
    }


    /// <summary>
    /// �΂�u��
    /// </summary>
    /// <param name="boardCube"></param>
    public void PlaceStone(BoardCube boardCube)
    {
        oku(boardCube, _isWhiteTurn);
        kaesu(boardCube);

        _endTurn = true;
        //TurnUpdate(!_isWhiteTurn);
        //BoardUpdate();
    }

    void oku(BoardCube boardCube, bool isWhite)
    {
        Stone stone = Instantiate(_selectStone, boardCube.transform);
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

        if (_selectStone != _normalStonePrefab)
        {
            _selectStoneController.Count(-1);
            SetSelectStone(_selectStone, _selectStoneController);
        }
        //if (_selectStone != _normalStonePrefab)
        //{
        //    _selectStoneController.Count--;
        //    _selectStone = _normalStonePrefab;
        //    _selectStoneController.IsSelected = false;
        //    _selectStoneController = null;
        //}

        if (_isPlaying && !_noAnime)
        {
            stone.PlaceMovement();
        }
    }

    void kaesu(BoardCube boardCube)
    {
        foreach (var stone in GetTurnOverWhenPlaced(boardCube))
        {
            stone.IsWhite = _isWhiteTurn ? true : false;
            if (!_noAnime)
            {
                stone.TurnOverMovement();
            }
        }
    }

    void NextTurn()
    {
        TurnUpdate(!_isWhiteTurn);
        BoardUpdate();
    }

    /// <summary>
    /// �^�[�����X�V
    /// </summary>
    void TurnUpdate(bool nextTurnIsWhite)
    {
        _isWhiteTurn = nextTurnIsWhite;

        //_turnText.text = _isWhiteTurn ? "���̔�" : "���̔�";
        //_turnText.color = _isWhiteTurn ? Color.white : Color.black;
        //_turnBackGround.color = _isWhiteTurn ? Color.white : Color.black;
        if (!_noAnime)
        {
            _turnBackGround.DOAnchorPos3D(!_isWhiteTurn ? Vector3.zero : _turnBackGroundOffset, _turnBackGroundTime)
                .SetEase(Ease.OutQuart);
        }

        foreach (var ss in _selectStoneControllers)
        {
            ss.TurnUpdate();
        }
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
        _blackTotal.text = black.ToString();// $"�� �F{black}";
        _whiteTotal.text = white.ToString();// $"�� �F{white}";

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
                NextTurn();
            }
            else
            {
                _isPlaying = false;
                _gameStatePanel.gameObject.SetActive(true);
                if (black == white)
                {
                    _gameStateText.text = "Draw";
                    _gameStateText.color = Color.gray;
                    _gameStateText.gameObject.GetComponent<Outline>().effectColor = Color.black;
                }
                else if (black > white)
                {
                    _gameStateText.text = "Winner Black";
                    _gameStateText.color = Color.black;
                    _gameStateText.gameObject.GetComponent<Outline>().effectColor = Color.white;
                }
                else
                {
                    _gameStateText.text = "Winner White";
                    _gameStateText.color = Color.white;
                    _gameStateText.gameObject.GetComponent<Outline>().effectColor = Color.black;
                }
            }
        }
        else
        {
            _passCount = 0;
        }
    }

    public void SetSelectStone(Stone stone, SelectStoneController controller)
    {
        if (_selectStone != stone)
        {
            _selectStone = stone;
            _selectStoneController = controller;
            _selectStoneController.IsSelected = true;
        }
        else
        {
            _selectStone = _normalStonePrefab;
            _selectStoneController.IsSelected = false;
            _selectStoneController = null;
        }
        //_selectStone = _selectStone != stone ? stone : _normalStonePrefab;
        //if (_selectStone != stone)
        //{
        //    _selectStone = stone;
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
        return _autoBlack && !_isWhiteTurn || _autoWhite && _isWhiteTurn ? true : false;
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
