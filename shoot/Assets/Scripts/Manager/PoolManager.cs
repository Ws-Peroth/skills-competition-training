using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public enum PoolCode
{
    PlayerBullet,
    Bacteria,
    Virus,
    VirusBullet,
    Germ,
    GermBullet,
    Cancer,
    CancerBullet,
    CovidBoss,
    CovidBossBullet,
    UpgradeCovidBoss,
    UpgradeCovidBossBullet,
    Erythrocyte,
    Leukocyte,
    PowerUpItem,
    HpHealItem,
    PainDownItem,
    ScoreUpItem,
    UnbreakableItem,
    DamageUpItem,
    
    MaxCount
}

public class PoolManager : MonoBehaviour
{
    #region SetPoolData
    [Header("0. playerBulletPrefab")]
    [Header("1. bacteriaPrefab")]
    [Header("2, 3. virusPrefab, virusBulletPrefab")]
    [Header("4, 5. germPrefab, germBulletPrefab")]
    [Header("6, 7. cancerPrefab, cancerBulletPrefab")]
    [Header("8, 9. covidBossPrefab, covidBossBulletPrefab")]
    [Header("10, 11. upgradeCovidBossPrefab, upgradeCovidBossBulletPrefab")]
    [Header("12, 13. Erythrocyte, Leukocyte")]
    [Header("14, 15. PowerUpItem, HpHealItem")]
    [Header("16, 17. PainDownItem, ScoreUpItem")]
    [Header("18, 19. UnbreakableItem, DamageUpItem")]
    
    public GameObject[] _prefabCombine;
    public static PoolManager Instance { get; set; }
    private Queue<GameObject>[] _poolCombine = new Queue<GameObject>[(int)PoolCode.MaxCount];
    
    private readonly Dictionary<PoolCode, int> _poolCode = new Dictionary<PoolCode, int>()
    {
        {PoolCode.PlayerBullet, 0},
        {PoolCode.Bacteria, 1},
        {PoolCode.Virus, 2},
        {PoolCode.VirusBullet, 3},
        {PoolCode.Germ, 4},
        {PoolCode.GermBullet, 5},
        {PoolCode.Cancer, 6},
        {PoolCode.CancerBullet, 7},
        {PoolCode.CovidBoss, 8},
        {PoolCode.CovidBossBullet, 9},
        {PoolCode.UpgradeCovidBoss, 10},
        {PoolCode.UpgradeCovidBossBullet, 11},
        {PoolCode.Erythrocyte, 12},
        {PoolCode.Leukocyte, 13},
        {PoolCode.PowerUpItem, 14},
        {PoolCode.HpHealItem, 15},
        {PoolCode.PainDownItem, 16},
        {PoolCode.ScoreUpItem, 17},
        {PoolCode.UnbreakableItem, 18},
        {PoolCode.DamageUpItem, 19},
    };
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        _poolCombine = new Queue<GameObject>[(int)PoolCode.MaxCount];

        for (var i = 0; i < _poolCombine.Length; i++)
        {
            _poolCombine[i] = new Queue<GameObject>();
        }
    }

    #endregion
    private void Start()    
    {
        
    }

    public void DestroyPrefab(GameObject bullet, PoolCode poolIndex)
    {
        var pool = _poolCombine[_poolCode[poolIndex]];
        pool.Enqueue(bullet);
        bullet.SetActive(false);
    }

    public GameObject CreatPrefab(PoolCode poolIndex)
    {
        var index = _poolCode[poolIndex];
        var pool = _poolCombine[index];
        
        if (pool.Count != 0)
        {
            return pool.Dequeue();
        }

        var bullet = Instantiate(_prefabCombine[index], transform);
        bullet.SetActive(false);
        return bullet;
    }
    
    
}
