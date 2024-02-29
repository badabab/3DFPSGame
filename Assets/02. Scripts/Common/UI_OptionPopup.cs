using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    public void OnEndButtonClicked()
    {
        Debug.Log("게임종료");
    }
}
