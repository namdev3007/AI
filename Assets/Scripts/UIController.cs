using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject uIGamePlay;
    public GameObject uILevel;

    public void TurnOnUILevel()
    {
        uILevel.SetActive(true);
        gameManager.SetActive(false);
        uIGamePlay.SetActive(false);
    }

    public void TurnOnGamePlay()
    {
        uILevel.SetActive(false);
        gameManager.SetActive(true);
        uIGamePlay.SetActive(true);
    }    
}
