using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUseAbility : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance.State == GameState.Go)
        {
            if (Input.GetKeyDown(KeyCode.T))    // HealthItem
            {
                ItemManager.Instance.TryUseItem(ItemType.Health);
            }
            else if (Input.GetKeyDown(KeyCode.Y))   // StaminaItem
            {
                ItemManager.Instance.TryUseItem(ItemType.Stamina);
            }
            else if (Input.GetKeyDown(KeyCode.U))   // BulletItem
            {
                ItemManager.Instance.TryUseItem(ItemType.Bullet);
            }

            ItemManager.Instance.RefreshUI();
        }      
    }
}
