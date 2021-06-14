using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStoneController : MonoBehaviour
{
    [SerializeField] int _blackCount = 3;
    [SerializeField] int _whiteCount = 3;
    [SerializeField] int _initialCount = 3;
    [SerializeField] Stone _selectStonePrefab = null;
    [SerializeField] Image _backImage;
    [SerializeField] Image _selectStoneImage = null;
    [SerializeField] Text _countText;

    bool _isSelected = false;
    Quaternion _selectStoneObjectInitialRot;
    Reversi _reversi;

    public int BlackCount { get { return _blackCount; } set{ _blackCount = value; } }
    public int WhiteCount { get { return _whiteCount; } set{ _whiteCount = value; } }
    public Stone SelectStonePrefab { get { return _selectStonePrefab; } }
    public bool IsSelected { get { return _isSelected; } set{ _isSelected = value; } }

    void Awake()
    {
        _selectStoneObjectInitialRot = _backImage.rectTransform.localRotation;
        _reversi = FindObjectOfType<Reversi>();
    }

    void Start()
    {
        _isSelected = false;
        _blackCount = _initialCount;
        _whiteCount = _initialCount;
    }

    void Update()
    {
        if (_isSelected)
        {
            Quaternion rot = Quaternion.AngleAxis(3f, Vector3.forward);
            Quaternion q = _backImage.rectTransform.rotation;
            _backImage.rectTransform.rotation = q * rot;
        }
        else if (_selectStoneImage.rectTransform.rotation != _selectStoneObjectInitialRot)
        {
            _backImage.rectTransform.rotation = _selectStoneObjectInitialRot;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        Start();
    }

    /// <summary>
    /// ターンを更新
    /// </summary>
    public void TurnUpdate()
    {
        _selectStoneImage.color = _reversi.IsBlackTurn ? Color.black : Color.white;
        _backImage.color = _reversi.IsBlackTurn ? Color.white : Color.black;
        TextUpdate();
    }

    /// <summary>
    /// 残り数を更新
    /// </summary>
    public void TextUpdate()
    {
        _countText.text = _reversi.GetControlNow() ? 
            _reversi.IsBlackTurn ? _blackCount.ToString() : _whiteCount.ToString() : 
            _countText.text = "?";
    }

    /// <summary>
    /// ポップアップ
    /// </summary>
    public void PopupStone()
    {
        if ((_reversi.IsBlackTurn ? _blackCount : _whiteCount) > 0 && _reversi.GetControlNow())
        {
            _selectStoneImage.rectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
    }

    /// <summary>
    /// ポップダウン
    /// </summary>
    public void PopdownStone()
    {
        _selectStoneImage.rectTransform.localScale = Vector3.one;
    }

    /// <summary>
    /// 石を選択
    /// </summary>
    public void SelectStone()
    {
        if ((_reversi.IsBlackTurn ? _blackCount : _whiteCount) > 0 && _reversi.GetControlNow())
        {
            _reversi.SetSelectStone(_selectStonePrefab, this);
        }
    }
}
