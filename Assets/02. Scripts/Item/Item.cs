using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Health,  // 체력이 꽉찬다.
    Stamina, // 스테미나 꽉찬다.
    Bullet   // 현재 들고있는 총의 총알이 꽉찬다.
}
public class Item
{
    public ItemType ItemType;
    public int Count;

    public Item(ItemType itemType, int count)
    {
        ItemType = itemType;
        Count = count;
    }

    public bool TryUse()
    {
        if (Count == 0)
        {
            return false;
        }

        Count -= 1;

        PlayerMoveAbility playerMoveAbility = GameObject.FindWithTag("Player").GetComponent<PlayerMoveAbility>();
        PlayerGunFireAbility playerGunFireAbility = GameObject.FindWithTag("Player").GetComponent<PlayerGunFireAbility>();
        switch (ItemType)
        {
            case ItemType.Health:
            {
                playerMoveAbility.Health = playerMoveAbility.MaxHealth;
                break;
            }
            case ItemType.Stamina:
            {
                playerMoveAbility.Stamina = PlayerMoveAbility.MaxStamina;
                break;
            }
            case ItemType.Bullet:
            {
                playerGunFireAbility.CurrentGun.BulletRemainCount = playerGunFireAbility.CurrentGun.BulletMaxCount;
                break;
            }
        }
        return true;
    }
}
