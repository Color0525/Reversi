using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    /// <summary>
    /// �w�肵���V�[�������[�h
    /// </summary>
    /// <param name="sceneName"></param>
    public void CollLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
