using System.Collections.Generic;
using UnityEngine;

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
    
    public GameObject[] prefabCombine;
    public static PoolManager Instance { get; set; }
    private Queue<GameObject>[] _poolCombine = new Queue<GameObject>[(int)PoolCode.MaxCount];

    public static int PoolCodeIndex(PoolCode code) => (int) code;
    
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

    public void DestroyPrefab(GameObject prefab, PoolCode poolIndex)
    {
        var pool = _poolCombine[PoolCodeIndex(poolIndex)];
        pool.Enqueue(prefab);
        prefab.SetActive(false);
    }

    public GameObject CreatPrefab(PoolCode poolIndex)
    {
        var index = PoolCodeIndex(poolIndex);
        var pool = _poolCombine[index];
        
        if (pool.Count != 0)
        {
            return pool.Dequeue();
        }

        var prefab = Instantiate(prefabCombine[index], transform);
        prefab.SetActive(false);
        return prefab;
    }
}
