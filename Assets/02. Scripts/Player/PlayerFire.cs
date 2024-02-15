using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    // 목표: 마우스 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶다.
    // 필요 속성
    // - 수류탄 프리팹
    public GameObject BombPrefab;
    // - 수류탄 던지는 위치
    public Transform FirePosition;
    // - 수류탄 던지는 파워
    public float ThrowPower = 15f;

    private int _bombCount = 0; // 남은 수류탄
    public int MaxBomb = 3; // 최대폭탄 개수

    [Header("수류탄 개수 UI")]
    public Text BombCountUI;

    // 수류탄 오브젝트 풀링
    public int PoolSize = 5;
    private List<GameObject> _bombPool = null;

    private void Awake()
    {
        _bombCount = MaxBomb;
        
        _bombPool = new List<GameObject>();
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject bomb = Instantiate(BombPrefab);
            bomb.gameObject.SetActive(false);
            _bombPool.Add(bomb);
        }
    }

    private void Update()
    {
        /* 수류탄 투척 */
        // 1. 마우스 오른쪽 버튼을 감지
        if (Input.GetMouseButtonDown(1) && _bombCount > 0) // 우측버튼 1번
        {
            // 2. 수류탄 던지는 위치에다가 수류탄 생성
            //GameObject bomb = Instantiate(BombPrefab);
            _bombCount--;
            GameObject bomb = null;
            foreach (GameObject b in _bombPool)
            {
                if (b.gameObject.activeInHierarchy == false)
                {
                    bomb = b;
                    break;
                }
            }
            bomb.transform.position = FirePosition.position;
            bomb.gameObject.SetActive(true);           
            
            // 3. 시선이 바라보는 방향(카메라가 바라보는 방향 = 카메라의 전방)으로 수류탄 투척
            Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
            rigidbody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);

        }
        BombCountUI.text = $"{_bombCount} / {MaxBomb}";
    }
}
