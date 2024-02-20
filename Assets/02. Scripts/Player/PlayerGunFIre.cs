using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGunFire : MonoBehaviour
{
    public Gun CurrentGun;    // 현재 들고있는 총
    public Gun[] GunInventory;

    private float _timer;

    // 목표: 마우스 왼쪽 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
    // 필요 속성
    // - 총알 튀는 이펙트 프리팹
    public ParticleSystem HitEffect;


    // - 총알 개수 텍스트 UI
    public Text BulletTextUI;
    public Image ProfileImageUI;

    private bool _isReloading = false;      // 재장전 중이냐?
    public GameObject ReloadTextObject;

    private void Start()
    {
        RefreshUI();
        RefreshGun();
    }

    private void RefreshUI()
    {
        BulletTextUI.text = $"{CurrentGun.BulletRemainCount:d2}/{CurrentGun.BulletMaxCount}";
        ProfileImageUI.sprite = CurrentGun.ProfileImage;
    }
    private IEnumerator Reload_Coroutine()
    {
        _isReloading = true;

        // R키 누르면 1.5초 후 재장전, (중간에 총 쏘는 행위를 하면 재장전 취소)
        yield return new WaitForSeconds(CurrentGun.ReloadTime);
        CurrentGun.BulletRemainCount = CurrentGun.BulletMaxCount;
        RefreshUI();

        _isReloading = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && CurrentGun.BulletRemainCount < CurrentGun.BulletMaxCount)
        {
            if (!_isReloading)
            {
                StartCoroutine(Reload_Coroutine());
            }
        }

        ReloadTextObject.SetActive(_isReloading);

        _timer += Time.deltaTime;

        // 1. 만약에 마우스 왼쪽 버튼을 누른 상태 && 쿨타임이 다 지난 상태 && 총알 개수 > 0
        if (Input.GetMouseButton(0) && _timer >= CurrentGun.FireCooltime && CurrentGun.BulletRemainCount > 0)
        {
            // 재장전 취소
            if (_isReloading)
            {
                StopAllCoroutines();
                _isReloading = false;
            }

            CurrentGun.BulletRemainCount -= 1;
            RefreshUI();

            _timer = 0;

            // 2. 레이(광선)을 생성하고, 위치와 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 3. 레이를 발사한다.
            // 4. 레이가 부딛힌 대상의 정보를 받아온다.
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                //실습 과제 18. 레이저를 몬스터에게 맞출 시 몬스터 체력 닳는 기능 구현
                IHitable hitObject = hitInfo.collider.GetComponent<IHitable>();
                if (hitObject != null)  // 때릴 수 있는 친구인가요?
                {
                    hitObject.Hit(CurrentGun.Damage);
                }

                // 5. 부딛힌 위치에 (총알이 튀는)이펙트를 위치한다.
                HitEffect.gameObject.transform.position = hitInfo.point;
                // 6. 이펙트가 쳐다보는 방향을 부딛힌 위치의 법선 벡터로 한다.
                HitEffect.gameObject.transform.forward = hitInfo.normal;
                HitEffect.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentGun = GunInventory[0];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentGun = GunInventory[1];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentGun = GunInventory[2];
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            int currentIndex = Array.IndexOf(GunInventory, CurrentGun);
            int previousIndex = (currentIndex - 1 + GunInventory.Length) % GunInventory.Length;
            CurrentGun = GunInventory[previousIndex];
        }
        else if (Input.GetKeyDown (KeyCode.RightBracket))
        {
            int currentIndex = Array.IndexOf(GunInventory, CurrentGun);
            int nextIndex = (currentIndex + 1) % GunInventory.Length;
            CurrentGun = GunInventory[nextIndex];
        }
        RefreshGun();
        RefreshUI();
    }

    private void RefreshGun()
    {
        foreach (Gun gun in GunInventory)
        {
            /*if (gun == CurrentGun)
            {
                gun.gameObject.SetActive(true);
            }
            else
            {
                gun.gameObject.SetActive(false);
            }*/
            gun.gameObject.SetActive(gun == CurrentGun);
        }
    }
}