using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CharacterData : MonoBehaviour
{
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
    }
}
