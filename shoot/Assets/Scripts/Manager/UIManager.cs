using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider painBar;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text painText;
    [SerializeField] private Text weaponLevelText;

    private void Update()
    {
        var hp = GameManager.Instance.Hp;
        var pain = GameManager.Instance.Pain;
        var score = GameManager.Instance.Score;
        var power = GameManager.Instance.Power;

        hpBar.value = hp / 100f;
        painBar.value = pain / 100f;
        scoreText.text = score.ToString();
        hpText.text = $"{hp:F0}%";
        painText.text = $"{pain:F0}%";
        weaponLevelText.text = $"Weapon Level : {power}";
    }
}
