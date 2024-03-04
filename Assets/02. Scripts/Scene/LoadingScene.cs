using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public SceneNames NextScene;
    void Start()
    {
        LoadNextScene();
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene((int)NextScene);
    }
}
