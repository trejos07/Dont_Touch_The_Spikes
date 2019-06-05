using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private new SpriteRenderer renderer;
    public static event Action OnDeathTrigger;

    protected virtual void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Game.Instance.OnScoreChange += (int score) => { SetColor(); };
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)// si algo me toca 
    {
        if (collision.transform.tag == "Player")//si lo que me toco tiene un tag: Player
        {
            if (OnDeathTrigger != null)
                OnDeathTrigger();//comunico que el player toco los Spikes
        }
    }

    public void SetColor ()
    {
        Color color_2 = Color.Lerp(Game.Instance.Color, Color.black, 0.3f);
        renderer.color = color_2;
    }

}
