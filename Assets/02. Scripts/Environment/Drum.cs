using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour, IHitable
{
    private int _hitCount = 0;
    public int Count = 3;

    public void Hit(int damage)
    {
        _hitCount++;
        if (_hitCount >= Count)
        {
            Destroy(gameObject);
        }
    }
}
