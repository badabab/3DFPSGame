using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_OptionPopup : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void OnContinueButtonClicked()
    {
        Debug.Log("계속하기");
        GameManager.Instance.Continue();
        Close();
    }
    public void OnRestartButtonClicked()
    {
        Debug.Log("다시하기");
        // 씬매니저야, ()씬을 로드해라
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void OnEndButtonClicked()
    {
        Debug.Log("게임종료");

        // 빌드 후 실행됐을 경우 종료하는 방법
        Application.Quit();

#if UNITY_EDITOR
        // 유니티 에디터에서 실행했을 경우 종료하는 방법
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
