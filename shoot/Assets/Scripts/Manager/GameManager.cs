using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum DamageType
{
    Hp,
    Pain
}

public class GameManager : MonoBehaviour
{
    private const float FadeEffectTime = 2f;
    private const float StageTimeA = 0;
    private const float StageTimeB = 0;
    private const float BossDelay = 5;
    private Coroutine[] _spawnRoutine;
    private Coroutine _gameRoutine;
    public int Stage { get; set; }
    [field: SerializeField] public float Pain { get; set; }
    [field: SerializeField] public float Hp { get; set; }
    [field: SerializeField] public int Score { get; set; }
    [field: SerializeField] public bool IsUnbreakable { get; set; }
    [field: SerializeField] public bool IsForcedUnbreakableChanged { get; set; }

    [SerializeField] private bool forcedUnbreakable;

    public bool ForcedUnbreakable
    {
        get => forcedUnbreakable;
        set
        {
            IsForcedUnbreakableChanged = true;
            forcedUnbreakable = value;
        }
    }

    [field: SerializeField] public int Power { get; set; }
    public static GameManager Instance { get; set; }
    private bool _isFinish;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

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
        {PoolCode.Bacteria, new SpawnData {Y = 5.5f, X = (-2.7f, 2.7f), Delay = (1f, 3f)}},
        // Germ
        {PoolCode.Germ, new SpawnData {Y = 5.5f, X = (-2.7f, 2.7f), Delay = (3f, 5f)}},
        // Virus
        {PoolCode.Virus, new SpawnData {Y = 5.5f, X = (-2.7f, 2.7f), Delay = (5f, 7f)}},
        // Cancer
        {PoolCode.Cancer, new SpawnData {Y = 5.5f, X = (-2.7f, 2.7f), Delay = (5f, 7f)}},
        // Erythrocyte ?????????
        {PoolCode.Erythrocyte, new SpawnData {Y = 5.5f, X = (-2.7f, 2.7f), Delay = (5f, 10f)}},
        // Leukocyte ?????????
        {PoolCode.Leukocyte, new SpawnData {Y = 5.5f, X = (-2.7f, 2.7f), Delay = (1f, 2f)}},
        // CovidBoss
        {PoolCode.CovidBoss, new SpawnData {Y = 6.2f, X = (0f, 0f), Delay = (0f, 0f)}},
        // UpgradeCovidBoss
        {PoolCode.UpgradeCovidBoss, new SpawnData {Y = 6.2f, X = (0f, 0f), Delay = (0f, 0f)}},
    };

    #endregion

    #region GameRoutine

    private void Start()
    {
        Score = 0;
        Pain = 0;
        Hp = 100;

        _gameRoutine = StartCoroutine(GameRoutine());
    }

    private IEnumerator GameRoutine()
    {
        Stage = 1;
        UIManager.Instance.FadeOutEffect(FadeEffectTime);
        yield return new WaitForSeconds(FadeEffectTime + 0.5f);
        UIManager.Instance.SetFadeImageActive(false);

        _spawnRoutine = new[]
        {
            StartCoroutine(MonsterSpawnRoutine(1, PoolCode.Bacteria)),
            StartCoroutine(MonsterSpawnRoutine(8, PoolCode.Germ)),
            StartCoroutine(MonsterSpawnRoutine(15, PoolCode.Virus)),
            StartCoroutine(MonsterSpawnRoutine(25, PoolCode.Cancer)),
            StartCoroutine(MonsterSpawnRoutine(10, PoolCode.Erythrocyte)),
            StartCoroutine(MonsterSpawnRoutine(1, PoolCode.Leukocyte)),
        };

        yield return new WaitForSeconds(StageTimeA);
        Debug.Log("Stop Spawn");
        foreach (var coroutine in _spawnRoutine)
        {
            StopCoroutine(coroutine);
        }

        yield return new WaitForSeconds(BossDelay);
        Spawn(PoolCode.CovidBoss);
    }

    public void StartStage2()
    {
        Stage = 2;
        StopAllCoroutines();

        foreach (var coroutine in _spawnRoutine)
        {
            StopCoroutine(coroutine);
        }

        StartCoroutine(Stage2Routine());
    }

    private IEnumerator Stage2Routine()
    {
        PoolManager.Instance.DestroyEntirePrefabs();
        yield return new WaitForSeconds(2f);

        UIManager.Instance.FadeInEffect(FadeEffectTime);
        yield return new WaitForSeconds(FadeEffectTime);

        yield return new WaitForSeconds(0.5f);

        UIManager.Instance.FadeOutEffect(FadeEffectTime);
        yield return new WaitForSeconds(FadeEffectTime);
        UIManager.Instance.SetFadeImageActive(false);

        // Routine Reset
        _spawnRoutine = new[]
        {
            StartCoroutine(MonsterSpawnRoutine(1, PoolCode.Bacteria)),
            StartCoroutine(MonsterSpawnRoutine(8, PoolCode.Germ)),
            StartCoroutine(MonsterSpawnRoutine(15, PoolCode.Virus)),
            StartCoroutine(MonsterSpawnRoutine(25, PoolCode.Cancer)),
            StartCoroutine(MonsterSpawnRoutine(10, PoolCode.Erythrocyte)),
            StartCoroutine(MonsterSpawnRoutine(1, PoolCode.Leukocyte)),
        };
        yield return new WaitForSeconds(StageTimeB);
        Debug.Log($"{_spawnRoutine.Length}");
        foreach (var coroutine in _spawnRoutine)
        {
            StopCoroutine(coroutine);
        }

        yield return new WaitForSeconds(BossDelay);
        Spawn(PoolCode.UpgradeCovidBoss);
    }

    public void Ending()
    {
        StartCoroutine(EndingEffect());
    }

    private IEnumerator EndingEffect()
    {
        PoolManager.Instance.DestroyEntirePrefabs();
        yield return new WaitForSeconds(5f);
        UIManager.Instance.FadeInEffect(FadeEffectTime);
        // If New Record
        var lastScore = ScoreManager.Instance.GetLastScore();
        if (ScoreManager.Instance.GetLastScoreIndex() < 5)
        {
            UIManager.Instance.OpenInputScoreScreen();
            yield break;
        }

        if (Score < lastScore)
        {
            SceneManager.LoadScene(1);
            yield break;
        }

        UIManager.Instance.OpenInputScoreScreen();
        // Show LeaderBoard
        // Else Goto Menu
    }

    private IEnumerator MonsterSpawnRoutine(float spawnTime, PoolCode monsterType)
    {
        yield return new WaitForSeconds(spawnTime);

        while (true)
        {
            var delay = Spawn(monsterType);
            yield return new WaitForSeconds(delay);
        }
    }

    private static float RandNum((float start, float end) tuple) => Random.Range(tuple.start, tuple.end);

    public float Spawn(PoolCode monsterType)
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

    #endregion

    public void SetPlayerHp(float hp)
    {
        Hp = hp;
        if (Hp <= 0)
        {
            GameOver();
        }
    }

    public void SetPlayerPain(float pain)
    {
        Pain = pain;
        if (Pain >= 100)
        {
            GameOver();
        }
    }

    public void Damaged(float damage, DamageType damageType)
    {
        if (damageType == DamageType.Hp)
        {
            Debug.Log($"Get Hp Damage {damage}");
            Hp = Hp - damage < 0 ? 0 : Hp - damage;
        }
        else
        {
            Debug.Log($"Get Pain Damage {damage}");
            Pain = Pain + damage > 100 ? 100 : Pain + damage;
        }

        if (Hp <= 0 || Pain >= 100)
        {
            GameOver();
        }
    }

    public void GetScore(int score)
    {
        Score += score;
    }

    private void GameOver()
    {
        if (_isFinish) return;
        _isFinish = true;
        var finishScore = Score;
        Debug.Log($"[GameOver] Score : {finishScore}");
    }
}