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
    private Coroutine[] _spawnRoutine;
    
    [field:SerializeField]
    public float Pain { get; set; }
    [field:SerializeField]
    public float Hp { get; set; }
    public static GameManager Instance { get; set; }

    #region MonsterSpawnInformation

    private class SpawnData
    {
        public float Y { get; set; }
        public (float start, float end) X { get; set; }
        public (float start, float end) Delay { get; set; }
    }

    private readonly Dictionary<PoolCode, SpawnData> _spawnerDictionary = new Dictionary<PoolCode, SpawnData>
            {
                // Bacteria
                {PoolCode.Bacteria, new SpawnData { Y = 5.5f, X = (-2.7f, 2.7f), Delay = (1f, 3f)}},
                // Germ
                {PoolCode.Germ,     new SpawnData { Y = 5.5f, X = (-2.7f, 2.7f), Delay = (3f, 5f)}},
                // Virus
                {PoolCode.Virus,    new SpawnData { Y = 5.5f, X = (-2.7f, 2.7f), Delay = (5f, 7f)}},
                // Cancer
                {PoolCode.Cancer,    new SpawnData { Y = 5.5f, X = (-2.7f, 2.7f), Delay = (5f, 7f)}},
            };
    
    #endregion
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        Pain = 0;
        Hp = 100;

        _spawnRoutine = new[]
        {
            StartCoroutine(BacteriaSpawnRoutine()),
            StartCoroutine(VirusSpawnRoutine()),
            StartCoroutine(GermSpawnRoutine()),
            StartCoroutine(CancerSpawnRoutine()),
        };
    }

    private IEnumerator BacteriaSpawnRoutine()
    {
        yield return new WaitForSeconds(1f);
        
        while (true)
        {
            var delay = Spawn(PoolCode.Bacteria);
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator VirusSpawnRoutine()
    {
        yield return new WaitForSeconds(8f);
        
        while (true)
        {
            var delay = Spawn(PoolCode.Virus);
            yield return new WaitForSeconds(delay);
        }
    }
    
    private IEnumerator GermSpawnRoutine()
    {
        yield return new WaitForSeconds(15f);
        
        while (true)
        {
            var delay = Spawn(PoolCode.Germ);
            yield return new WaitForSeconds(delay);
        }
    }
    
    private IEnumerator CancerSpawnRoutine()
    {
        yield return new WaitForSeconds(25f);
        
        while (true)
        {
            var delay = Spawn(PoolCode.Cancer);
            yield return new WaitForSeconds(delay);
        }
    }

    private static float RandNum((float start, float end) tuple) => Random.Range(tuple.start, tuple.end);
    
    private float Spawn(PoolCode monsterType)
    {
        var spawnData = _spawnerDictionary[monsterType];
        var mob = PoolManager.Instance.CreatPrefab(monsterType);
        mob.transform.position = new Vector3(RandNum(spawnData.X), spawnData.Y, 0);
        ActiveMob(mob);
        return RandNum(spawnData.Delay);
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
