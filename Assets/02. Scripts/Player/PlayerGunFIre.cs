using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFIre : MonoBehaviour
{
    // 목표: 마우스 왼쪽 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
    // 필요 속성:
    // - 총알 튀는 이펙트 프리팹
    public ParticleSystem HitEffect;

    private int _gunCount = 0; // 남은 총알
    public int MaxGunCount = 30; // 최대 총알 개수
    public float GunLoadTime = 1.5f;

    private float _gunTimer = 0f;
    public float GunCoolTime = 0.2f;

    [Header("총알 개수 UI")]
    public Text GunCountUI;

    private Coroutine _loadGunCoroutine;
    private bool _isLoading = false;
    public Text GunLoadingUI;

    private void Awake()
    {   
        _gunTimer = 0;
        _gunCount = MaxGunCount;
        RefreshUI();
    }
    private void Update()
    {
        // 1. 만약에 마우스 왼쪽 버튼을 누르면
        if (Input.GetMouseButton(0) && _gunCount > 0) // 왼쪽0, 오른쪽1, 휠2
        {
            _gunTimer -= Time.deltaTime;
            // 2. 레이(광선)를 생성하고, 위치와 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 3. 레이를 발사한다.
            // 4. 레이가 부딪힌 대상의 정보를 받아온다.
            RaycastHit hitInfo;
            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit && _gunTimer <= 0)
            {
                // 5. 부딪힌 위치에 (총알이 튀는) 이펙트를 생성한다.
                //Debug.Log(hitInfo.point);
                HitEffect.gameObject.transform.position = hitInfo.point;
                HitEffect.gameObject.transform.forward = hitInfo.normal; // 법선 벡터
                HitEffect.Play();
                Shooting();
            } 
            else if (!IsHit && _gunTimer <= 0)
            {
                Shooting();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _gunTimer = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            _loadGunCoroutine = StartCoroutine(LoadGun_Coroutine(GunLoadTime));
            RefreshUI();
        } 
        if (_isLoading)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StopCoroutine(_loadGunCoroutine);  
                _isLoading = false;
                RefreshUI();
            }          
        }
    }

    private void RefreshUI()
    {
        GunCountUI.text = $"Bullet {_gunCount} / {MaxGunCount}";
        if (_isLoading )
        {
            GunLoadingUI.text = $"재장전 중...";
        }
        else
        {
            GunLoadingUI.text = $"";
        }     
    }
    private void Shooting()
    {
        _gunCount--;
        _gunTimer = GunCoolTime;
        RefreshUI();
    }
    private IEnumerator LoadGun_Coroutine(float load_time)
    {
        _isLoading = true;
        yield return new WaitForSeconds(load_time);
        _isLoading = false;
        _gunCount = MaxGunCount;
        RefreshUI();
    }
}
