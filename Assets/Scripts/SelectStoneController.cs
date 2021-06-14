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

    public bool IsSelected { get { return _isSelected; } set{ _isSelected = value; } }

    void Awake()
    {
        _selectStoneObjectInitialRot = _selectStoneImage.rectTransform.localRotation;
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
            Quaternion rot = Quaternion.AngleAxis(1f, Vector3.forward);
            Quaternion q = _selectStoneImage.rectTransform.rotation;
            _selectStoneImage.rectTransform.rotation = q * rot;
        }
        else if (_selectStoneImage.rectTransform.rotation != _selectStoneObjectInitialRot)
        {
            _selectStoneImage.rectTransform.rotation = _selectStoneObjectInitialRot;
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
    /// カウントの増減
    /// </summary>
    /// <param name="value"></param>
    public void Count(int value)
    {

        if (_reversi.IsBlackTurn)
        {
            _blackCount += value;
        }
        else
        {
            _whiteCount += value;
        }
        TextUpdate();
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
    void TextUpdate()
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
        _selectStoneImage.rectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
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
        Reversi reversi = FindObjectOfType<Reversi>();
        if ((reversi.IsBlackTurn ? _blackCount : _whiteCount) > 0)
        {
            reversi.SetSelectStone(_selectStonePrefab, this);
        }
    }
}
