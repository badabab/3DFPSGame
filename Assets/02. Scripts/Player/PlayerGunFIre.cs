using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunFIre : MonoBehaviour
{
    // 목표: 마우스 왼쪽 버튼을 누르면 시선이 바라보는 방향으로 총을 발사하고 싶다.
    // 필요 속성:
    // - 총알 튀는 이펙트 프리팹
    public ParticleSystem HitEffect;
    // 구현 순서:
    
    private void Update()
    {
        // 1. 만약에 마우스 왼쪽 버튼을 누르면
        if (Input.GetMouseButtonDown(0)) // 왼쪽0, 오른쪽1, 휠2
        {
            // 2. 레이(광선)를 생성하고, 위치와 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 3. 레이를 발사한다.
            // 4. 레이가 부딪힌 대상의 정보를 받아온다.
            RaycastHit hitInfo;
            bool IsHit = Physics.Raycast(ray, out hitInfo);
            if (IsHit)
            {
                // 5. 부딪힌 위치에 (총알이 튀는) 이펙트를 생성한다.
                //Debug.Log(hitInfo.point);
                HitEffect.gameObject.transform.position = hitInfo.point;
                HitEffect.gameObject.transform.forward = hitInfo.normal; // 법선 벡터
                HitEffect.Play();
            }           
        }
    }
}
