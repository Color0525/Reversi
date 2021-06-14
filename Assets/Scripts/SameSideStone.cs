using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameSideStone : Stone
{
    [SerializeField] GameObject _dummyObject = null;
    [SerializeField] GameObject _sameSideObject = null;
    [SerializeField] Material _black = null;
    [SerializeField] Material _white = null;
    
    bool _first = true;

    public override bool IsBlack
    {
        get { return _isBlack; }
        set
        {
            if (_first)
            {
                _first = false;
                _isBlack = value;
                OnColorChanged();
            }
            else
            {
                _dummyObject.SetActive(false);
                _sameSideObject.SetActive(true);
                _sameSideObject.GetComponent<Renderer>().material = _isBlack ? _black : _white;
            }
        }
    }
}
