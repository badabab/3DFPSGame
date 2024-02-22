using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemType ItemType;

    // Todo 1. 아이템 프리팹을 3개(Health, Stamina, Bullet) 만든다. (도형이나 색깔 다르게해서 구별되게)
    // Todo 2. 플레이어와 일정 거리가 되면 아이템이 먹어지고(AddItem) 사라진다(Destroy). (collider)
    // 몬스터가 죽으면 아이템이 드랍(Health: 20%, Stamina: 20%, Bullet: 10%)
    // 일정거리가 되면 아이템이 베지어 곡선으로 날아오게 하기(중간점을 플레이어와 아이템 사이 랜덤 지정)

    //public float ItemRadius = 3;    // 아이템 먹는 범위

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            // 플레이어와 나의 거리를 알고 싶다
            float distance = Vector3.Distance(collider.transform.position, transform.position);
            Debug.Log(distance);

            // 아이템 매니저(인벤토리)에 추가
            ItemManager.Instance.AddItem(ItemType);
            ItemManager.Instance.RefreshUI();
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
