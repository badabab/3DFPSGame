using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject BombEffectPrefab;

    private void OnCollisionEnter(Collision other)
    {       
        //Destroy(gameObject);
        gameObject.SetActive(false);
        GameObject effect = Instantiate(BombEffectPrefab);
        effect.transform.position = this.transform.position;
    }
}
