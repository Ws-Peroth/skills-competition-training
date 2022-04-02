using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider painBar;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text painText;
    [SerializeField] private Text weaponLevelText;
    [SerializeField] private Text setPlayerHpText;
    [SerializeField] private Text setPlayerPainText;
    [SerializeField] private Slider setPlayerHpSlider;
    [SerializeField] private Slider setPlayerPainSlider;
    
    private void Update()
    {
        var hp = GameManager.Instance.Hp;
        var pain = GameManager.Instance.Pain;
        var score = GameManager.Instance.Score;
        var power = GameManager.Instance.Power;
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
    }

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
}
