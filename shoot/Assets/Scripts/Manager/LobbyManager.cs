using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }

    public void GameStart()
    {
        SceneManager.LoadScene(2);
    }
}
