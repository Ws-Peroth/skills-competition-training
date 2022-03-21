using System;
using System.Collections;
using System.Collections.Generic;
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
}
public class PoolManager : MonoBehaviour
{
    #region SetPoolData
    [SerializeField] private GameObject playerBulletPrefab;
    [SerializeField] private GameObject bacteriaPrefab;
    [SerializeField] private GameObject virusPrefab;
    [SerializeField] private GameObject virusBulletPrefab;
    [SerializeField] private GameObject germPrefab;
    [SerializeField] private GameObject germBulletPrefab;
    [SerializeField] private GameObject cancerPrefab;
    [SerializeField] private GameObject cancerBulletPrefab;
    [SerializeField] private GameObject covidBossPrefab;
    [SerializeField] private GameObject covidBossBulletPrefab;
    [SerializeField] private GameObject upgradeCovidBossPrefab;
    [SerializeField] private GameObject upgradeCovidBossBulletPrefab;
    public static PoolManager Instance { get; set; }
    
    private readonly Queue<GameObject> _playerBulletPool = new Queue<GameObject>();
    private readonly Queue<GameObject> _bacteriaPool = new Queue<GameObject>();
    private readonly Queue<GameObject> _virusPool = new Queue<GameObject>();
    private readonly Queue<GameObject> _virusBulletPool = new Queue<GameObject>();
    private readonly Queue<GameObject> _germPool = new Queue<GameObject>();
    private readonly Queue<GameObject> _germBulletPool = new Queue<GameObject>();
    private readonly Queue<GameObject> _cancerPool = new Queue<GameObject>();
    private readonly Queue<GameObject> _cancerBulletPool = new Queue<GameObject>();
    private readonly Queue<GameObject> _covidBossPool = new Queue<GameObject>();
    private readonly Queue<GameObject> _covidBossBulletPool = new Queue<GameObject>();
    private readonly Queue<GameObject> _upgradeCovidBossPool = new Queue<GameObject>();
    private readonly Queue<GameObject> _upgradeCovidBossBulletPool = new Queue<GameObject>();

    private Queue<GameObject>[] _poolCombine;
    private GameObject[] _prefabCombine;

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
    };
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        _poolCombine = new[]
        {
            _playerBulletPool,              // 0
            _bacteriaPool,                  // 1
            _virusPool,                     // 2
            _virusBulletPool,               // 3
            _germPool,                      // 4
            _germBulletPool,                // 5
            _cancerPool,                    // 6
            _cancerBulletPool,              // 7
            _covidBossPool,                 // 8
            _covidBossBulletPool,           // 9
            _upgradeCovidBossPool,          // 10
            _upgradeCovidBossBulletPool,    // 11
        };
        _prefabCombine = new[]
        {
            playerBulletPrefab,
            bacteriaPrefab,
            virusPrefab,
            virusBulletPrefab,
            germPrefab,
            germBulletPrefab,
            cancerPrefab,
            cancerBulletPrefab,
            covidBossPrefab,
            covidBossBulletPrefab,
            upgradeCovidBossPrefab,
            upgradeCovidBossBulletPrefab,
        };
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
