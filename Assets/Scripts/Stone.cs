using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stone : MonoBehaviour
{
    [SerializeField] protected bool _isBlack = true;
    [SerializeField] float _animeTime = 0.5f;

    public virtual bool IsBlack
    {
        get { return _isBlack; }
        set
        {
            _isBlack = value;
            OnColorChanged();
            
        }
    }

    private void OnValidate()
    {
        OnColorChanged();
    }

    /// <summary>
    /// êFÇîΩì](âÒì])
    /// </summary>
    protected void OnColorChanged()
    {
        transform.rotation = _isBlack ? Quaternion.identity : Quaternion.AngleAxis(180f, Vector3.forward);
    }

    /// <summary>
    /// íuÇ≠ìÆÇ´
    /// </summary>
    public void PlaceMovement()
    {
        transform.position = transform.position + new Vector3(0, 1, 0);
        transform.DOLocalMoveY(transform.position.y - 1, _animeTime);
    }

    /// <summary>
    /// Ç–Ç¡Ç≠ÇËï‘Ç∑ìÆÇ´
    /// </summary>
    public void TurnOverMovement()
    {
        if (_isBlack)
        {
            transform.rotation = Quaternion.AngleAxis(180f, Vector3.forward);
            transform.DOLocalRotateQuaternion(Quaternion.identity, _animeTime).OnComplete(() =>
            {
                FindObjectOfType<Reversi>().EndAnime = true;
            });
        }
        else
        {
            transform.rotation = Quaternion.identity;
            transform.DOLocalRotateQuaternion(Quaternion.AngleAxis(180f, Vector3.forward), _animeTime).OnComplete(() =>
            {
                FindObjectOfType<Reversi>().EndAnime = true;
            });
        }
    }
}
