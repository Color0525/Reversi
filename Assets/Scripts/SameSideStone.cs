using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameSideStone : Stone
{
    [SerializeField] GameObject _normalObject = null;
    [SerializeField] GameObject _sameSideObject = null;
    [SerializeField] Material _black = null;
    [SerializeField] Material _white = null;
    
    //static int _count = 3;
    //public int Count
    //{
    //    get { return _count; }
    //    set {_count = value;}
    //}

    bool _first = true;

    public override bool IsWhite
    {
        get { return _isWhite; }
        set
        {
            if (_first)
            {
                _first = false;
                _isWhite = value;
                OnColorChanged();
            }
            else
            {
                _normalObject.SetActive(false);
                _sameSideObject.SetActive(true);
                _sameSideObject.GetComponent<Renderer>().material = !_isWhite ? _black : _white;
            }
        }
    }
}
