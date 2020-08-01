using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Elements")]
    public GameObject pauseMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PauseGameAndShowMenu() //button method
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        
    }

    public void ResumeGameAndCloseMenu() //button method
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(false);

    }
}
