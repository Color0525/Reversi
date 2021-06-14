using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStoneController : MonoBehaviour
{
    [SerializeField] Stone _selectStonePrefab = null;
    [SerializeField] int _initialCount = 3;
    [SerializeField] int _blackCount = 3;
    public int BlackCount { get { return _blackCount; } }

    [SerializeField] int _whiteCount = 3;
    public int WhiteCount { get { return _whiteCount; } }

    //[SerializeField] bool _isWhiteTurn = false;
    [SerializeField] Image _backImage;
    [SerializeField] Image _selectStoneImage = null;
    [SerializeField] Text _countText;
    //[SerializeField] Material _black = null;
    //[SerializeField] Material _white = null;
    Reversi _reversi;
    Quaternion _selectStoneObjectRot;
    bool _isSelected = false;
    public bool IsSelected
    { 
        get { return _isSelected; }
        set
        { 
            _isSelected = value;
            if (!_isSelected)
            {
                _selectStoneImage.rectTransform.rotation = _selectStoneObjectRot;
            }
        }
    } 
    //public int InitialCount
    //{
    //    get { return _initialCount; }
    //    set
    //    {
    //        _initialCount = value;
    //        TextUpdate();
    //    }
    //}

    void Awake()
    {
        _reversi = FindObjectOfType<Reversi>();
    }


   

    void Update()
    {
        if (_isSelected)
        {
            Quaternion rot = Quaternion.AngleAxis(0.5f, Vector3.forward);
            Quaternion q = _selectStoneImage.rectTransform.rotation;
            _selectStoneImage.transform.rotation = q * rot;
        }
    }


    public void Setup()
    {
        _selectStoneObjectRot = _selectStoneImage.rectTransform.localRotation;
        _blackCount = _initialCount;
        _whiteCount = _initialCount;
    }


    public void Count(int value)
    {
        if (!_reversi.IsWhiteTurn)
        {
            _blackCount += value;
        }
        else
        {
            _whiteCount += value;
        }
        TextUpdate();
    }

    public void TurnUpdate()
    {
        _selectStoneImage.color = !_reversi.IsWhiteTurn ? Color.black : Color.white;
        _backImage.color = !_reversi.IsWhiteTurn ? Color.white : Color.black;
        //_selectStoneObject.transform.rotation = !_reversi.IsWhiteTurn ? Quaternion.identity : Quaternion.AngleAxis(180f, Vector3.forward);
        TextUpdate();
    }

    void TextUpdate()
    {
        _countText.text = !_reversi.IsWhiteTurn ? _blackCount.ToString() : _whiteCount.ToString();
    }

    public void PopupStone()
    {
        _selectStoneImage.rectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }
    public void PopdownStone()
    {
        _selectStoneImage.rectTransform.localScale = Vector3.one;
    }

    /// <summary>
    /// êŒÇëIë
    /// </summary>
    public void SelectStone()
    {
        Reversi reversi = FindObjectOfType<Reversi>();
        if ((!reversi.IsWhiteTurn ? _blackCount : _whiteCount) > 0)
        {
            reversi.SetSelectStone(_selectStonePrefab, this);
        }
    }

}
