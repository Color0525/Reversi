using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    /// <summary>
    /// 指定したシーンをロード
    /// </summary>
    /// <param name="sceneName"></param>
    public void CollLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
