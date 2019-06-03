using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Wall : MonoBehaviour
{
    public static event Action OnPlayerHitWall;

    private void OnCollisionEnter2D(Collision2D collision)//si algo choca
    {
        if (collision.transform.tag == "Player")//y si su tag es igual a player
        {
            if (OnPlayerHitWall != null)
                OnPlayerHitWall();// comunico que me toco un player
        }
    }
}
