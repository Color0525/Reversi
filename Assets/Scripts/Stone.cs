using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stone : MonoBehaviour
{
    //public enum StoneType
    //{
    //    Normal,
    //    SameSide,
    //}
    //[SerializeField] StoneType _stoneType = StoneType.Normal; 
    //[SerializeField] MeshRenderer _upRenderer = null;
    //[SerializeField] MeshRenderer _downRenderer = null;

    [SerializeField] bool _isWhite = false;
    public bool IsWhite
    {
        get { return _isWhite; }
        set
        {
            _isWhite = value;
            OnColorChanged();
            //if (!_isWhite)
            //{
            //    this.gameObject.transform.DOLocalRotate(Vector3.zero, _turnOverTime);//rotation = Quaternion.Lerp(_white, _black, _turnOverTime);// AngleAxis(180, Vector3.forward);
            //                                                    //_upRenderer.sharedMaterial.color = Color.white;
            //                                                    //_downRenderer.sharedMaterial.color = Color.black;
            //}
            //else
            //{
            //    this.gameObject.transform.DORotate(new Vector3(0, 0, 180f), _turnOverTime);
            //    //_upRenderer.sharedMaterial.color = Color.black;
            //    //_downRenderer.sharedMaterial.color = Color.white;
            //}
            ////if (_isWhite != value)
            ////{
                
            ////}
        }
    }
    [SerializeField] float _moveTime = 0.5f;


    private void OnValidate()
    {
        OnColorChanged();
    }

    
    void OnColorChanged()
    {
        if (!_isWhite)
        {
            transform.rotation = Quaternion.identity;
            //transform.DOLocalRotate(Vector3.zero, _moveTime);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            //transform.DOLocalRotate(new Vector3(0, 0, 180f), _moveTime);
        }
    }

    public void PlaceMovement()
    {
        Vector3 offset = new Vector3(0, 1, 0);
        transform.position = transform.position + offset;
        transform.DOLocalMoveY(transform.position.y - 1, _moveTime);
    }

    public void TurnOverMovement()
    {
        if (!_isWhite)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            transform.DOLocalRotate(Vector3.zero, _moveTime);
        }
        else
        {
            transform.rotation = Quaternion.identity;
            transform.DOLocalRotate(new Vector3(0, 0, 180f), _moveTime);
        }
    }

    //public enum ColorState
    //{
    //    White,
    //    Black,
    //}

    //[SerializeField] ColorState _onState = ColorState.None;
}
