using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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
    // 开始新游戏
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
    }
    //重新加载当前场景
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       // PlayerPrefs.DeleteKey("playerHealth");
    }

    //暂停游戏
    public void PauseGame()
    {
        Time.timeScale = 0;

    }
    // 回到游戏
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    //返回主菜单
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
