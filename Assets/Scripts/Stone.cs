using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    //[SerializeField] MeshRenderer _upRenderer = null;
    //[SerializeField] MeshRenderer _downRenderer = null;
    [SerializeField] bool _isWhite = false;
    public bool IsWhite
    {
        get { return _isWhite; }
        set
        {
            _isWhite = value;
            //OnColorChanged();
            if (_isWhite)
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                //_upRenderer.sharedMaterial.color = Color.white;
                //_downRenderer.sharedMaterial.color = Color.black;
            }
            else
            {
                transform.rotation = Quaternion.identity;
                //_upRenderer.sharedMaterial.color = Color.black;
                //_downRenderer.sharedMaterial.color = Color.white;
            }
        }
    }

    //private void OnValidate()
    //{
    //    OnColorChanged();
    //}

    /// <summary>
    /// èÛë‘Ç…ÇÊÇ¡ÇƒîíçïïœÇ¶ÇÈ
    /// </summary>
    //private void OnColorChanged()
    //{
    //    if (_isWhite)
    //    {
    //        transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
    //        //_upRenderer.sharedMaterial.color = Color.white;
    //        //_downRenderer.sharedMaterial.color = Color.black;
    //    }
    //    else
    //    {
    //        transform.rotation = Quaternion.identity;
    //        //_upRenderer.sharedMaterial.color = Color.black;
    //        //_downRenderer.sharedMaterial.color = Color.white;
    //    }

    //}

    //public enum ColorState
    //{
    //    White,
    //    Black,
    //}

    //[SerializeField] ColorState _onState = ColorState.None;
}
