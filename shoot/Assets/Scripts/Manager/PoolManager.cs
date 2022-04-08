using System.Collections.Generic;
using TMPro;
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
    private readonly Queue<GameObject>[] _deactivatePrefabPoolCombine = new Queue<GameObject>[(int) PoolCode.MaxCount];
    private readonly List<GameObject>[] _activatePrefabPoolCombine = new List<GameObject>[(int)PoolCode.MaxCount];


    public static int PoolCodeIndex(PoolCode code) => (int) code;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        for (var i = 0; i < _deactivatePrefabPoolCombine.Length; i++)
        {
            _deactivatePrefabPoolCombine[i] = new Queue<GameObject>();
            _activatePrefabPoolCombine[i] = new List<GameObject>();
        }
    }

    #endregion
    
    private void Start()    
    {
        
    }

    public void DestroyPrefab(GameObject prefab, PoolCode poolIndex)
    {
        var pool = _deactivatePrefabPoolCombine[PoolCodeIndex(poolIndex)];
        pool.Enqueue(prefab);
        prefab.SetActive(false);
    }

    public GameObject CreatPrefab(PoolCode poolIndex)
    {
        var index = PoolCodeIndex(poolIndex);
        var pool = _deactivatePrefabPoolCombine[index];
        
        if (pool.Count != 0)
        {
            return pool.Dequeue();
        }

        var prefab = Instantiate(prefabCombine[index], transform);
        
        var activatePrefabList = _activatePrefabPoolCombine[index];
        activatePrefabList.Add(prefab);
        prefab.SetActive(false);
        return prefab;
    }

    public void DestroyAllPrefabs(PoolCode code)
    {
        var pool = _activatePrefabPoolCombine[PoolCodeIndex(code)];
        foreach (var prefab in pool)
        {
            prefab.GetComponent<Entity>().Killed();
        }
    }
    
    public void DestroyEntirePrefabs()
    {
        foreach (var pool in _activatePrefabPoolCombine)
        {
            foreach (var prefab in pool)
            {
                prefab.SetActive(false);
            }
        }
    }
}
