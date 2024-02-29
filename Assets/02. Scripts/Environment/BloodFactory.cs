using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodFactory : MonoBehaviour
{
    public static BloodFactory Instance { get; private set; }
    public GameObject BloodPrefab;

    private List<GameObject> _bloodPool;
    public int PoolSize = 5;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _bloodPool = new List<GameObject>();
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject blood = Instantiate(BloodPrefab);
            blood.transform.SetParent(this.transform);
            blood.gameObject.SetActive(false);
            _bloodPool.Add(blood);
        }
    }

    public void Make(Vector3 position, Vector3 normal)
    {
        GameObject bloodObject = null;
        foreach (GameObject b in _bloodPool)
        {
            if (b.gameObject.activeInHierarchy == false)
            {
                bloodObject = b;
                break;
            }
        }
        bloodObject.transform.position = position;
        bloodObject.transform.forward = normal;
        bloodObject.gameObject.SetActive(true);  
    }
}
