using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurvivalGameOver : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("SurvivalMinesweeper");
    }
}
