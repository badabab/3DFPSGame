using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ItemState
{
    Idle,   // 대기상태
    Trace     // 날아오는 상태
}

public class ItemObject : MonoBehaviour
{
    public ItemType ItemType;

    // Todo 1. 아이템 프리팹을 3개(Health, Stamina, Bullet) 만든다. (도형이나 색깔 다르게해서 구별되게)
    // Todo 2. 플레이어와 일정 거리가 되면 아이템이 먹어지고(AddItem) 사라진다(Destroy). (collider)
    // 몬스터가 죽으면 아이템이 드랍(Health: 20%, Stamina: 20%, Bullet: 10%)
    // 일정거리가 되면 아이템이 베지어 곡선으로 날아오게 하기(중간점을 플레이어와 아이템 사이 랜덤 지정)

    private Transform _player;    // 플레이어, _itemEndPosition
    public float ItemDistance = 5;    // 아이템 인식 거리
    private Vector3 _itemStartPosition;
    public float ItemDuration = 0.6f;
    private float _itemProgress = 0f;

    private ItemState _itemState = ItemState.Idle;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _itemStartPosition = transform.position;
    }
    private void Update()
    {
         switch (_itemState)
         {
            case ItemState.Idle:
                Idle(); break;
                
            case ItemState.Trace:
                Trace(); break;
         }
    }

    private void Idle()
    {
        float distance = Vector3.Distance(_player.position, transform.position);
        if (distance <= ItemDistance)
        {
            _itemState = ItemState.Trace;
        }
    }
    private void Trace()
    {
        // 거리가 가까워지면 Slerp로 날아온다
        _itemProgress += Time.deltaTime / ItemDuration;
        transform.position = Vector3.Slerp(_itemStartPosition, _player.position, _itemProgress);
        if (_itemProgress > 0.6)
        {
            ItemManager.Instance.AddItem(ItemType);
            ItemManager.Instance.RefreshUI();
            gameObject.SetActive(false);
        }
    }

    /*private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            // 플레이어와 나의 거리를 알고 싶다
            //float distance = Vector3.Distance(collider.transform.position, transform.position);

            // 아이템 매니저(인벤토리)에 추가
            ItemManager.Instance.AddItem(ItemType);
            ItemManager.Instance.RefreshUI();
            gameObject.SetActive(false);
        }
    }*/
}
