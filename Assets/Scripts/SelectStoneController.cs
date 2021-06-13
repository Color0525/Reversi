using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStoneController : MonoBehaviour
{
    [SerializeField] Stone _selectStonePrefab = null;
    [SerializeField] int _initialCount = 3;
    [SerializeField] int _count = 3;
    [SerializeField] GameObject _selectStoneObject = null;
    [SerializeField] Text _countText;
    Quaternion _selectStoneObjectRot;
    bool _isSelected = false;
    public int Count
    {
        get { return _count; }
        set
        {
            _count = value;
            TextUpdate();
        }
    }
    public bool IsSelected
    { 
        get { return _isSelected; }
        set
        { 
            _isSelected = value;
            if (!_isSelected)
            {
                _selectStoneObject.transform.rotation = _selectStoneObjectRot;
            }
        }
    } 

    private void OnValidate()
    {
        TextUpdate();
    }

    void TextUpdate()
    {
        _countText.text = _count.ToString();
    }

    /// <summary>
    /// êŒÇëIë
    /// </summary>
    public void SelectStone()
    {
        if (_count > 0)
        {
            Reversi reversi = FindObjectOfType<Reversi>();
            reversi.SetStone(_selectStonePrefab, this);
        }
    }

    public void Setup()
    {
        _selectStoneObjectRot = _selectStoneObject.transform.localRotation;
        Count = _initialCount;
    }

    void Update()
    {
        if (_isSelected)
        {
            Quaternion rot = Quaternion.AngleAxis(1f, Vector3.up);
            Quaternion q = _selectStoneObject.transform.rotation;
            _selectStoneObject.transform.rotation = q * rot;
        }
    }
}
