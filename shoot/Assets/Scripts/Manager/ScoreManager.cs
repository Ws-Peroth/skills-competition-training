using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private const int LastRank = 5;
    public readonly List<(string name, int score)> ScoreList = new List<(string name, int score)>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int GetLastScoreIndex()
    {
        if (ScoreList.Count > LastRank)
        {
            return LastRank - 1;
        }

        return ScoreList.Count - 1;
    }

    public int GetLastScore()
    {
        var lastIndex = GetLastScoreIndex();
        if (lastIndex < 0)
        {
            return -1;
        }
        return ScoreList[lastIndex].score;
    }

    public void AddScore(string userName, int score)
    {
        ScoreList.Add((userName, score));
        ScoreList.Sort((a, b) => a.score.CompareTo(b.score));
        ScoreList.Reverse();
        if (ScoreList.Count > LastRank)
        {
            ScoreList.RemoveAt(LastRank);
        }
    }
}
