using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Reversi : MonoBehaviour
{
    [SerializeField] bool _blackControl = false;
    [SerializeField] bool _whiteControl = true;
    [SerializeField] float _autoSelectRate = 0.1f;
    [SerializeField] float _delayTime = 0.2f;
    [SerializeField] bool _noAnime = false;
    [SerializeField] bool _isPlaying = false;
    [SerializeField] bool _isBlackTurn = false;
    [SerializeField] Stone _selectStone = null;
    [SerializeField] BoardCube _boardCubePrefab = null;
    [SerializeField] Stone _normalStonePrefab = null;
    [SerializeField] float _stoneOffset = 0.5f;
    [SerializeField] SelectStoneController[] _selectStoneControllers = null;
    [SerializeField] RectTransform _turnBackGround = null;
    [SerializeField] Vector3 _turnBackGroundStartOffset = new Vector3(0, 100, 0);
    [SerializeField] Vector3 _turnBackGroundEndOffset = new Vector3(0, 1100, 0);
    [SerializeField] float _turnBackGroundTime = 0.5f;
    [SerializeField] Text _blackTotal = null;
    [SerializeField] Text _whiteTotal = null;
    [SerializeField] Text _yourBlack = null;
    [SerializeField] Text _yourWhite = null;
    [SerializeField] Image _gameStatePanel = null;
    [SerializeField] Text _gameStateText = null;

    float _delayCount = 0;
    bool _endAnime = false;
    int _rows = 8;
    int _columns = 8;
    BoardCube[,] _boardCubes;
    SelectStoneController _selectStoneController = null;
    bool _endTurn = false;
    int _passCount = 0;

    public bool IsBlackTurn { get { return _isBlackTurn; } }
    public bool EndAnime { get { return _endAnime; } set { _endAnime = value; } }

    void Awake()
    {
        _boardCubes = new BoardCube[_rows, _columns];    
    }

    void Start()
    {
        //�{�[�h�𐶐��A�z�u
        for (int r = 0; r < _rows; r++)
        {
            for (int c = 0; c < _columns; c++)
            {
                _boardCubes[r, c] = Instantiate(_boardCubePrefab);
                _boardCubes[r, c].transform.position = new Vector3(c - 3.5f, 0, -r + 3.5f);
                _boardCubes[r, c].Row = r;
                _boardCubes[r, c].Column = c;
            }
        }
        //�����΂�u��
        _selectStone = _normalStonePrefab;
        Placement(_boardCubes[3, 3], true);
        Placement(_boardCubes[3, 4], false);
        Placement(_boardCubes[4, 3], false);
        Placement(_boardCubes[4, 4], true);
        //�Q�[���J�n
        _isPlaying = true;
        _gameStatePanel.gameObject.SetActive(false);
        bool myColorIsBlack = Random.Range(0, 2) == 0;
        _blackControl = myColorIsBlack;
        _whiteControl = !myColorIsBlack;
        _yourBlack.gameObject.SetActive(myColorIsBlack);
        _yourWhite.gameObject.SetActive(!myColorIsBlack);
        foreach (var ss in _selectStoneControllers)
        {
            ss.Initialize();
        }
        TurnUpdate(true);
        BoardUpdate();
    }

    void Update()
    {
        if (!_isPlaying) return;
        //�^�[���I�����Ă����玟�̃^�[����
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
            //�I�[�g���̓���
            if (!_blackControl && _isBlackTurn || !_whiteControl && !_isBlackTurn)
            {
                _delayCount += Time.deltaTime;
                if (_delayCount > _delayTime)
                {
                    _delayCount = 0;
                    //�m���œ���΂��g�p
                    if (Random.value < _autoSelectRate)
                    {
                        _selectStoneControllers[Random.Range(0, _selectStoneControllers.Length)].SelectStone();
                    }
                    //�����_���Ȓu����}�X�ɒu��
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
    /// ������
    /// </summary>
    public void Initialize()
    {
        foreach (var bc in _boardCubes)
        {
            Destroy(bc.gameObject);
        }
        Start();
    }

    /// <summary>
    /// �΂�u��
    /// </summary>
    /// <param name="boardCube"></param>
    public void PlaceStone(BoardCube boardCube)
    {
        Placement(boardCube, _isBlackTurn);
        TurnOver(boardCube);
        _endTurn = true;
    }

    /// <summary>
    /// �΂�z�u
    /// </summary>
    /// <param name="boardCube"></param>
    /// <param name="isBlack"></param>
    void Placement(BoardCube boardCube, bool isBlack)
    {
        //�΂𐶐��A�z�u
        Stone stone = Instantiate(_selectStone, boardCube.transform);
        Vector3 offset = new Vector3(0, _stoneOffset, 0);
        stone.transform.position = boardCube.transform.position + offset;
        stone.IsBlack = isBlack ? true : false;
        boardCube.PlacedStone = stone;
        //����΂��g�p���Ă����Ȃ琔�����炵�A���ʐ΂ɖ߂�
        if (_selectStone != _normalStonePrefab)
        {
            _selectStoneController.Count(-1);
            SetSelectStone(_selectStone, _selectStoneController);
        }
        //�u������
        if (_isPlaying && !_noAnime)
        {
            stone.PlaceMovement();
        }
    }

    /// <summary>
    /// �΂��Ђ�����Ԃ�
    /// </summary>
    /// <param name="boardCube"></param>
    void TurnOver(BoardCube boardCube)
    {
        foreach (var stone in GetTurnOverWhenPlaced(boardCube))
        {
            stone.IsBlack = _isBlackTurn ? true : false;
            //�Ђ�����Ԃ�����
            if (!_noAnime)
            {
                stone.TurnOverMovement();
            }
        }
    }

    /// <summary>
    /// ���̃^�[����
    /// </summary>
    void NextTurn()
    {
        TurnUpdate(!_isBlackTurn);
        BoardUpdate();
    }

    /// <summary>
    /// �^�[�����X�V
    /// </summary>
    void TurnUpdate(bool nextTurnIsBlack)
    {
        _isBlackTurn = nextTurnIsBlack;//�t�F�̃^�[���ɂ���
        //�^�[���؂�ւ��p�w�i�𓮂���
        _turnBackGround.DOAnchorPos3D(_isBlackTurn ? _turnBackGroundStartOffset : _turnBackGroundEndOffset, _turnBackGroundTime)
            .SetEase(Ease.OutQuart);
        //����΂̃^�[�����X�V
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
        //���ꂼ��̍��v�����X�V
        int black = 0;
        int white = 0;
        foreach (var bc in _boardCubes)
        {
            if (bc.PlacedStone)
            {
                if (bc.PlacedStone.IsBlack)
                {
                    black++;
                }
                else
                {
                    white++;
                }
            }
        }
        _blackTotal.text = black.ToString();
        _whiteTotal.text = white.ToString();
        //�u����}�X���X�V
        int canCount = 0;
        foreach (var bc in _boardCubes)
        {
            if (bc.PlacedStone == null && GetTurnOverWhenPlaced(bc).Length > 0)
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

    /// <summary>
    /// ����΂�I����ԂɁA�������͉�������
    /// </summary>
    /// <param name="stone"></param>
    /// <param name="controller"></param>
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
    }

    /// <summary>
    /// �w��}�X�ɒu���ꂽ�Ƃ��ɗ��Ԃ��
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
                int dirR = boardCube.Row + i;
                int dirC = boardCube.Column + k;
                while (true)
                {
                    if (0 > dirR || dirR >= _rows || 0 > dirC || dirC >= _columns) break;
                    if (_boardCubes[dirR, dirC] == _boardCubes[boardCube.Row, boardCube.Column]) break;
                    if (!_boardCubes[dirR, dirC].PlacedStone) break;
                    //����ɓ��F�΂�����Ζ߂�悤�ɂ��t�F�΂��擾
                    if (_boardCubes[dirR, dirC].PlacedStone.IsBlack == _isBlackTurn)
                    {
                        List<Stone> lineTurnOverStones = new List<Stone>();
                        int r = dirR - i;
                        int c = dirC - k;
                        while (true)
                        {
                            if (r == boardCube.Row && c == boardCube.Column)
                            {
                                if (lineTurnOverStones.Count > 0)
                                {
                                    turnOverStones.AddRange(lineTurnOverStones);
                                }
                                break;
                            }
                            if (_boardCubes[r, c].PlacedStone.IsBlack != _isBlackTurn)
                            {
                                lineTurnOverStones.Add(_boardCubes[r, c].PlacedStone);
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

    /// <summary>
    /// ���݂̃^�[�������������삵�Ă��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    public bool GetControlNow()
    {
        return _blackControl && _isBlackTurn || _whiteControl && !_isBlackTurn ? true : false;
    }
}
