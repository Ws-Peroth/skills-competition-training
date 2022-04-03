using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; set; }
    [SerializeField] private Image fadeImage;

    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider painBar;
    [SerializeField] private Slider setPlayerHpSlider;
    [SerializeField] private Slider setPlayerPainSlider;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text painText;
    [SerializeField] private Text weaponLevelText;
    [SerializeField] private Text setPlayerHpText;
    [SerializeField] private Text setPlayerPainText;
    [SerializeField] private Text currentStageText;
    [SerializeField] private Text inputUserNameScoreText;
    [SerializeField] private Text scoreRankText;
    [SerializeField] private Text nameRankText;

    [SerializeField] private string UserName { get; set; }

    [SerializeField] private GameObject inputScoreScreen;
    [SerializeField] private GameObject leaderBoardScreen;

    private Coroutine _fadeRoutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        fadeImage.gameObject.SetActive(false);
        inputScoreScreen.gameObject.SetActive(false);
        leaderBoardScreen.gameObject.SetActive(false);
    }

    private void Update()
    {
        var hp = GameManager.Instance.Hp;
        var pain = GameManager.Instance.Pain;
        var score = GameManager.Instance.Score;
        var power = GameManager.Instance.Power;
        var stage = GameManager.Instance.Stage;
        var setHp = setPlayerHpSlider.value;
        var setPain = setPlayerPainSlider.value;

        hpBar.value = hp / 100f;
        painBar.value = pain / 100f;
        scoreText.text = $"Score : {score.ToString()}";
        hpText.text = $"{hp:F0}%";
        painText.text = $"{pain:F0}%";
        weaponLevelText.text = $"Weapon Level : {power}";
        setPlayerHpText.text = $"{setHp:F0}%";
        setPlayerPainText.text = $"{setPain:F0}%";
        currentStageText.text = $"Stage {stage}";
    }

    public void OpenInputScoreScreen()
    {
        if (fadeImage.gameObject.activeSelf)
        {
            fadeImage.gameObject.SetActive(false);
        }

        inputUserNameScoreText.text = $"Score : {GameManager.Instance.Score.ToString()}";
        inputScoreScreen.SetActive(true);
    }

    public void OpenLeaderBoardScreen()
    {
        scoreRankText.text = "";
        nameRankText.text = "";
        var leaderBoard = ScoreManager.Instance.ScoreList;

        for (var i = 0; i < leaderBoard.Count; i++)
        {
            scoreRankText.text += $"{leaderBoard[i].score.ToString()}\n";
            nameRankText.text += $"{(i + 1).ToString()}. {leaderBoard[i].name}\n";
        }

        leaderBoardScreen.SetActive(true);
    }

    public void OnInputName(Text inputName)
    {
        UserName = inputName.text;
    }

    public void FinishInputName()
    {
        ScoreManager.Instance.AddScore(UserName, GameManager.Instance.Score);
        OpenLeaderBoardScreen();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(1);
    }

    #region CheatUI

    public void SetStage(int stage)
    {
        PoolManager.Instance.DestroyEntirePrefabs();
        switch (stage)
        {
            case 1:
                SceneManager.LoadScene(2);
                break;
            case 2:
                GameManager.Instance.StartStage2();
                break;
        }
    }

    public void SetPlayerPower(int power)
    {
        GameManager.Instance.Power = power;
    }

    public void ForcedUnbreakableMode(bool isForced)
    {
        GameManager.Instance.ForcedUnbreakable = isForced;

    }

    public void KillAllMonster()
    {
        PoolManager.Instance.DestroyAllPrefabs(PoolCode.Bacteria);
        PoolManager.Instance.DestroyAllPrefabs(PoolCode.Germ);
        PoolManager.Instance.DestroyAllPrefabs(PoolCode.Virus);
        PoolManager.Instance.DestroyAllPrefabs(PoolCode.Cancer);
        PoolManager.Instance.DestroyAllPrefabs(PoolCode.CovidBoss);
        PoolManager.Instance.DestroyAllPrefabs(PoolCode.UpgradeCovidBoss);
    }

    public void SubmitPlayerHp()
    {
        GameManager.Instance.SetPlayerHp(setPlayerHpSlider.value);
    }

    public void SubmitPain()
    {
        GameManager.Instance.SetPlayerPain(setPlayerPainSlider.value);
    }

    public void SpawnWhiteCell()
    {
        GameManager.Instance.Spawn(PoolCode.Leukocyte);
    }

    public void SpawnRedCell()
    {
        GameManager.Instance.Spawn(PoolCode.Erythrocyte);
    }

    #endregion

    public void FadeOutEffect(float time)
    {
        EffectManager.Instance.FadeOutEffect(fadeImage, time);
    }

    public void FadeInEffect(float time)
    {
        EffectManager.Instance.FadeInEffect(fadeImage, time);
    }

    public void SetFadeImageActive(bool active)
    {
        fadeImage.gameObject.SetActive(active);
    }
}
