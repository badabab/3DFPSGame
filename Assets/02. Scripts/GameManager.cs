using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 역할: 게임 관리자
// -> 게임 전체의 상태를 알리고 시작과 끝을 텍스트로 나타낸다.
public enum GameState
{
    Ready,  //준비
    Go,  //시작
    Over    //오버
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // 게임의 상태는 처음에 "준비" 상태
    public GameState State { get; private set; } = GameState.Ready;

    public Text StateTextUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
        private void Start()
    {
        StartCoroutine(Start_Coroutine());
    }
    private IEnumerator Start_Coroutine()
    {
        // 게임 상태
        // 1. 게임 준비 상태 (Ready...)
        State = GameState.Ready;
        StateTextUI.gameObject.SetActive(true);
        Refresh();

        // 2. 1.6초 후에 게임 시작 상태 (Start!)
        yield return new WaitForSeconds(1.6f);
        State = GameState.Go;
        Refresh();

        // 3. 0.4초 후에 텍스트 사라지고
        yield return new WaitForSeconds(0.4f);
        StateTextUI.gameObject.SetActive(false);   
    }
    // 4. 플레이를 하다가
    public void GameOver()
    {
        // 5. 플레이어 체력이 0이 되면 "게임오버" 상태
        State = GameState.Over;
        StateTextUI.gameObject.SetActive(true);
        Refresh();
    } 

    public void Refresh()
    {
        switch (State)
        {
            case GameState.Ready:
            {
                //StateTextUI.color = Color.cyan;
                StateTextUI.color = new Color(90f/255f, 45f/255f, 200f/255f, 1f);
                //StateTextUI.color = new Color32(90, 45, 200, 255);

                StateTextUI.text = "Ready...";
                break;
            }
            case GameState.Go:
            {
                StateTextUI.color = Color.blue;
                StateTextUI.text = "Start!";              
                break;
            }
            case GameState.Over:
            {
                StateTextUI.color = Color.green;
                StateTextUI.text = "Game Over";
                break;
            }
        }
    }
}
