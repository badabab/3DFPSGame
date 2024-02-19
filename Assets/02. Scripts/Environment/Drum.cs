using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour, IHitable
{
    private int _hitCount = 0;      // 맞은 횟수
    public int Count = 3;           // 터지기까지 횟수
    public float UpForce = 50f;   // 위로 올라가는 힘 값
    public float DestroyDelay = 3f; // 사라지는 시간

    public GameObject DrumEffectPrefab;
    private Rigidbody _rigidbody;

    public float ExplosionRadius = 5;
    public int Damage = 70;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Hit(int damage)
    {
        _hitCount++;
        if (_hitCount >= Count)
        {
            Kill();         
        }
    }

    private void Kill()
    {
        GameObject explosion = Instantiate(DrumEffectPrefab);
        explosion.transform.position = this.transform.position;

        _rigidbody.AddForce(Vector3.up * UpForce, ForceMode.Impulse);
        _rigidbody.AddTorque(new Vector3(1, 0, 1) * UpForce / 2f);  // 회전

        int findLayer = LayerMask.GetMask("Monster") | LayerMask.GetMask("Player") | LayerMask.GetMask("Env");
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, findLayer);
      
        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Drum"))
            {
                Drum drum = c.GetComponent<Drum>();
                if (drum != null)
                {
                    drum.Kill();
                }
            }
            else
            {
                IHitable hitable = c.gameObject.GetComponent<IHitable>();
                if (hitable != null)
                {
                    hitable.Hit(Damage);
                }
                /*IHitable hitable = null;
                if (c.TryGetComponent<IHitable>(out hitable))
                {
                    hitable.Hit(Damage);
                }*/
            }
        }

        StartCoroutine(DestroyDelay_Coroutine(DestroyDelay));
    }
    IEnumerator DestroyDelay_Coroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
