using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] MeshRenderer _upRenderer = null;
    [SerializeField] MeshRenderer _downRenderer = null;
    [SerializeField] bool _isWhite = true;
    public bool IsWhite
    {
        get { return _isWhite; }
        set
        {
            _isWhite = value;
            OnColorChanged();
        }
    }

    private void OnValidate()
    {
        OnColorChanged();
    }

    private void OnColorChanged()
    {
        if (_isWhite)
        {
            _upRenderer.sharedMaterial.color = Color.white;
            _downRenderer.sharedMaterial.color = Color.black;
        }
        else
        {
            _upRenderer.sharedMaterial.color = Color.black;
            _downRenderer.sharedMaterial.color = Color.white;
        }

    }

    //public enum ColorState
    //{
    //    White,
    //    Black,
    //}

    //[SerializeField] ColorState _onState = ColorState.None;
}
