using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 역할: 아이템들을 관리해주는 관리자
// 데이터 관리 -> 데이터를 생성, 수정, 삭제, 조회(검색)
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    public Text HealthItemCountTextUI;
    public Text StaminaItemCountTextUI;
    public Text BulletItemCountTextUI;

    public GameObject HealthItemPrefab;
    public GameObject StaminaItemPrefab;
    public GameObject BulletItemPrefab;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }        
    }

    public List<Item> ItemList = new List<Item>();
    private void Start()
    {
        ItemList.Add(new Item(ItemType.Health, 3));     // 0: Health
        ItemList.Add(new Item(ItemType.Stamina, 5));    // 1: Stamina
        ItemList.Add(new Item(ItemType.Bullet, 7));     // 2: Bullet

        RefreshUI();
    }

    // 1. 아이템 추가(생성)
    public void AddItem(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                ItemList[i].Count++;
                break;
            }
        }
    }
    // 2. 아이템 조회
    public int GetItemCount(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                return ItemList[i].Count;   
            }
        }
        return 0;
    }
    // 3. 아이템 사용
    public bool TryUseItem(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                return ItemList[i].TryUse();
            }
        }
        return false;
    }
    // UI 새로고침
    public void RefreshUI()
    {
        HealthItemCountTextUI.text = $"x{GetItemCount(ItemType.Health)}";
        StaminaItemCountTextUI.text = $"x{GetItemCount(ItemType.Stamina)}";
        BulletItemCountTextUI.text = $"x{GetItemCount(ItemType.Bullet)}";
    }
    // 몬스터 죽을 때 아이템 드랍
    public void DropItem(ItemType itemType)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].ItemType == itemType)
            {
                // 아이템 생성

                break;
            }
        }
    }
}
