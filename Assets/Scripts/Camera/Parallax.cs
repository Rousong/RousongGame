using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现视差效果
/// </summary>
public class Parallax : MonoBehaviour
{
    public Transform camera;
    public float moveRate;
    public bool lockY = false; //是否锁定Y轴跟随
    private Vector2 startPoint;

    private void Start()
    {
        startPoint = new Vector2(transform.position.x, transform.position.y);

    }

    private void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(startPoint.x + camera.position.x * moveRate, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(startPoint.x + camera.position.x * moveRate, startPoint.y);

        }

    }
}
