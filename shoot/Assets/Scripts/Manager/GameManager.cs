using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;

public enum DamageType
{
    Hp,
    Pain
}

public class GameManager : MonoBehaviour
{
    [field:SerializeField]
    public float Pain { get; set; }
    [field:SerializeField]
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
    public const float VirusSpawnY = 5.5f; 


    private void Start()
    {
        Pain = 0;
        Hp = 100;
        
        StartCoroutine(BacteriaSpawnRoutine());
        StartCoroutine(VirusSpawnRoutine());
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
    
    private IEnumerator VirusSpawnRoutine()
    {
        while (true)
        {
            Debug.Log($"[Virus] Spawn");
            var mob = PoolManager.Instance.CreatPrefab(PoolCode.Virus);
            mob.transform.position = new Vector3(Random.Range(-8, 8), VirusSpawnY, 0);
            ActiveMob(mob);
            
            var randomDelay = Random.Range(3f, 6f);
            Debug.Log($"[Virus] Delay : {randomDelay.ToString()}");
            yield return new WaitForSeconds(randomDelay);
        }
    }
    

    private static void ActiveMob(GameObject mob)
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
