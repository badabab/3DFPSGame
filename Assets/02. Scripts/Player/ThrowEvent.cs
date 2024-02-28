using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowEvent : MonoBehaviour
{
    private PlayerBombFireAbility _player;
    private void Start()
    {
        _player = GetComponentInParent<PlayerBombFireAbility>();
    }
    public void AttackEvent()
    {
        _player.BombThrow();
    }
}
