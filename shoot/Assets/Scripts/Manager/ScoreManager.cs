using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public const int LastRank = 5;
    public List<(string name, int score)> ScoreList = new List<(string name, int score)>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int GetLastScoreIndex() => ScoreList.Count > LastRank ? LastRank - 1 : ScoreList.Count - 1;
    public int GetLastScore() => ScoreList[GetLastScoreIndex()].score;

    public void AddScore(string userName, int score)
    {
        ScoreList.Add((userName, score));
        ScoreList.Sort((a, b) => a.score.CompareTo(b.score));
        if (ScoreList.Count > LastRank)
        {
            ScoreList.RemoveAt(LastRank);
        }
    }
}
