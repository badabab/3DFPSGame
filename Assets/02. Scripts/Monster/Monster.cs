using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour, IHitable
{
    [Range(0, 100)]
    public int Health = 100;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;

    

    void Start()
    {
        Init();
    }

    public void Init()
    {
        Health = MaxHealth;
    }

    private void Update()
    {
        HealthSliderUI.value = (float)Health / (float)MaxHealth;
    }

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }
    // 몬스터 죽을 때 아이템 드랍
    public void Die()
    {
        ItemObjectFactory.Instance.MakePercent(transform.position);

        Destroy(gameObject);
    }
}
