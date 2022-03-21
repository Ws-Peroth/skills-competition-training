using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum DamageType
{
    Hp,
    Pain
}

public class GameManager : MonoBehaviour
{
    public float Pain { get; set; }
    public float Hp { get; set; }
    public static GameManager Instance { get; set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public const float BacteriaSpawnY = 5.5f; 


    private void Start()
    {
        Pain = 0;
        Pain = 100;
        StartCoroutine(BacteriaSpawnRoutine());
    }

    private IEnumerator BacteriaSpawnRoutine()
    {
        while (true)
        {
            var mob = PoolManager.Instance.CreatPrefab(PoolCode.Bacteria);
            mob.transform.position = new Vector3(Random.Range(-8, 8), BacteriaSpawnY, 0);
            ActiveMob(mob);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    private void ActiveMob(GameObject mob)
    {
        mob.SetActive(true);
        mob.GetComponent<Entity>().InitializeBaseData();
    }

    public void Damaged(float damage, DamageType damageType)
    {
        if (damageType == DamageType.Hp)
        {
            Hp -= damage;
            return;
        }

        Pain += damage;
    }
}
