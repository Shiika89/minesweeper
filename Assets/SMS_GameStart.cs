using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SMS_GameStart : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("SurvivalMinesweeper");
    }
}
