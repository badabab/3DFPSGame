using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombFireAbility : MonoBehaviour
{
    private Animator _animator;

    // 목표: 마우스 오른쪽 버튼을 누르면 시선이 바라보는 방향으로 수류탄을 던지고 싶다.
    // 필요 속성
    // - 수류탄 프리팹
    public GameObject BombPrefab;
    public GameObject BombEffectPrefab;
    // - 수류탄 던지는 위치
    public Transform FirePosition;
    // - 수류탄 던지는 파워
    public float ThrowPower = 15f;

    private int _bombCount = 0; // 남은 수류탄
    public int MaxBombCount = 3; // 최대폭탄 개수

    [Header("수류탄 개수 UI")]
    public TextMeshProUGUI BombCountUI;

    // 수류탄 오브젝트 풀링
    public int PoolSize = 3;
    private List<GameObject> _bombPool = null;
    //private List<GameObject> _bombEffectPool = null;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _bombCount = MaxBombCount;
        RefreshUI();

        _bombPool = new List<GameObject>();
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject bomb = Instantiate(BombPrefab);  // 생성
            //GameObject bombEffect = Instantiate(BombEffectPrefab);
            bomb.gameObject.SetActive(false);           // 비활성화
            //bombEffect.SetActive(false);
            _bombPool.Add(bomb);                        // 창고에 넣기
            //_bombEffectPool.Add(bombEffect);
        }
    }

    private void Update()
    {
        if(GameManager.Instance.State == GameState.Go)
        {
            /* 수류탄 투척 */
            // 1. 마우스 오른쪽 버튼을 감지
            if (Input.GetMouseButtonDown(1) && _bombCount > 0) // 우측버튼 1번
            {
                _animator.SetTrigger("Throw");

                // 2. 수류탄 던지는 위치에다가 수류탄 생성
                //GameObject bomb = Instantiate(BombPrefab);
                _bombCount--;
                RefreshUI();
                GameObject bomb = null;
                foreach (GameObject b in _bombPool)
                {
                    if (b.gameObject.activeInHierarchy == false)
                    {
                        bomb = b;
                        bomb.gameObject.SetActive(true);
                        break;
                    }
                }
                bomb.transform.position = FirePosition.position;

                // 3. 시선이 바라보는 방향(카메라가 바라보는 방향 = 카메라의 전방)으로 수류탄 투척
                Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
                rigidbody.velocity = Vector3.zero;
                rigidbody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
            }
        }       
    }

    private void RefreshUI()
    {
        BombCountUI.text = $"{_bombCount} / {MaxBombCount}";
    }
}
