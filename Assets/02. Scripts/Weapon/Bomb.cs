using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 목표: 수류탄 폭발 범위 데미지 기능 구현
    // 필요 속성:
    // - 범위
    public float ExplosionRadius = 3; // 폭발 범위   
    public int Damage = 60;
    public GameObject BombEffectPrefab;

    private void OnCollisionEnter(Collision other)          // 1. 터질 때
    {       
        gameObject.SetActive(false);            //Destroy(gameObject);
              
        GameObject effect = Instantiate(BombEffectPrefab);
        effect.transform.position = this.gameObject.transform.position;

        // 2. 범위안에 있는 모든 콜라이더를 찾는다.
        // -> 피직스, 오버랩 함수는 특정 영역(Radius) 안에 있는 특정 레이어들의 게임 오브젝트의 
        //    콜라이더 컴포넌트들을 모두 찾아 배열로 반환하는 함수
        // 영역의 형태: 스피어, 큐브, 캡슐
        int layer = LayerMask.GetMask("Monster") | LayerMask.GetMask("Player");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, layer);
        // | : 비트 합 연산자

        // 3. 찾은 콜라이더 중에서 타격 가능한(IHitable) 오브젝트를 찾아서 Hit()한다.
        foreach (Collider collider in colliders)
        {
            IHitable hitable = collider.gameObject.GetComponent<IHitable>();
            if (hitable != null)
            {
                hitable.Hit(Damage);
            }
        }
    }
}
