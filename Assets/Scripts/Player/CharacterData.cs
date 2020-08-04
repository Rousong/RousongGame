using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CharacterData : MonoBehaviour
{
    public string characterName;

    // public StatSystem stats;

    [Header("动画状态机")]
    public int state;

    [Header("玩家属性")]
    public float health;
    public float energy;
    public float muscles;
    public float fat;
    public bool isDead;

    [Header("玩家物品")]
    private Inventory inventory;
    public GameObject myBag;
    bool isOpen;

    private void Awake()
    {
        inventory = new Inventory();
       
    }
    private void Start()
    {
        UIManager.instance.UpdateHeath(health);
    }
    private void Update()
    {
        OpenMyBag();
    }
    void OpenMyBag()
    {
        isOpen = myBag.activeSelf;
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            myBag.SetActive(isOpen);
        }
        if (isOpen) //如果打开背包菜单 那么游戏暂停
        {
            GameManager.instance.PauseGame();
        }
        else //如果关闭背包菜单 那么游戏继续
        {
            GameManager.instance.ResumeGame();
        }
    }
}
