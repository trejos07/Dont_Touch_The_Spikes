using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Wall : MonoBehaviour
{
    private new SpriteRenderer renderer;
    public static event Action OnPlayerHitWall;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Game.Instance.OnScoreChange += (int score) => { SetColor(); };
    }

    private void OnCollisionEnter2D(Collision2D collision)//si algo choca
    {
        if (collision.transform.tag == "Player")//y si su tag es igual a player
        {
            if (OnPlayerHitWall != null)
                OnPlayerHitWall();// comunico que me toco un player
        }
    }
    public void SetColor()
    {
        Color color_2 = Color.Lerp(Game.Instance.Color, Color.black, 0.3f);
        renderer.color = color_2;
    }
}
