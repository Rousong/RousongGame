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
    public GameObject healthBar;

    [Header("UI Elements")]
    public GameObject pauseMenu;

    [Header("Inventory")]
    public GameObject myBag;
    bool isOpen;
    public Button OpenInventoryButton;

    Sprite m_ClosedInventorySprite;
    Sprite m_OpenInventorySprite;

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

    private void Start()
    {
        m_ClosedInventorySprite = ((Image)OpenInventoryButton.targetGraphic).sprite;
        m_OpenInventorySprite = OpenInventoryButton.spriteState.pressedSprite;
    }
    public void UpdateHeath(float currentHealth)
    {
        switch (currentHealth)
        {
            case 3:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(true);
                healthBar.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 2:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(true);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 1:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 0:
                healthBar.transform.GetChild(0).gameObject.SetActive(false);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
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

    public void OpenMyBag()
    {
        isOpen = myBag.activeSelf;
        isOpen = !isOpen;
        myBag.SetActive(isOpen);

        if (isOpen) //如果打开背包菜单 那么游戏暂停
        {
            ((Image)OpenInventoryButton.targetGraphic).sprite = m_OpenInventorySprite;
            AudioManager.instance.PlayInventoryAudio(isOpen);
            GameManager.instance.PauseGame();
            
        }
        else //如果关闭背包菜单 那么游戏继续
        {
            ((Image)OpenInventoryButton.targetGraphic).sprite = m_ClosedInventorySprite;
            AudioManager.instance.PlayInventoryAudio(isOpen);
            GameManager.instance.ResumeGame();
        }
    }
}
