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
    

    [SerializeField] protected bool _isWhite = false;
    public virtual bool IsWhite
    {
        get { return _isWhite; }
        set
        {
            _isWhite = value;
            OnColorChanged();
            
        }
    }

    [SerializeField] float _moveTime = 0.5f;

    //[SerializeField] SelectStoneController _cotrollore = null;
    //public SelectStoneController Cotrollore { get { return _cotrollore; } }

    private void OnValidate()
    {
        OnColorChanged();
    }


    protected void OnColorChanged()
    {
        transform.rotation = !_isWhite ? Quaternion.identity : Quaternion.AngleAxis(180f, Vector3.forward);
        //if (!_isWhite)
        //{
        //    transform.rotation = Quaternion.identity;
        //    //transform.DOLocalRotate(Vector3.zero, _moveTime);
        //}
        //else
        //{
        //    transform.rotation = Quaternion.AngleAxis(180f, Vector3.forward);
        //    //transform.DOLocalRotate(new Vector3(0, 0, 180f), _moveTime);
        //}
    }

    public void PlaceMovement()
    {
        transform.position = transform.position + new Vector3(0, 1, 0);
        transform.DOLocalMoveY(transform.position.y - 1, _moveTime);
    }

    public void TurnOverMovement()
    {
        if (!_isWhite)
        {
            transform.rotation = Quaternion.AngleAxis(180f, Vector3.forward);
            transform.DOLocalRotateQuaternion(Quaternion.identity, _moveTime).OnComplete(() =>
            {
                FindObjectOfType<Reversi>().EndAnime = true;
            });
        }
        else
        {
            transform.rotation = Quaternion.identity;
            transform.DOLocalRotateQuaternion(Quaternion.AngleAxis(180f, Vector3.forward), _moveTime).OnComplete(() =>
            {
                FindObjectOfType<Reversi>().EndAnime = true;
            });
        }
    }

    
}
