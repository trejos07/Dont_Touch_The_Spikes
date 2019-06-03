using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public static event Action OnDeathTrigger;

    private void OnCollisionEnter2D(Collision2D collision)// si algo me toca 
    {
        if (collision.transform.tag == "Player")//si lo que me toco tiene un tag: Player
        {
            if (OnDeathTrigger != null)
                OnDeathTrigger();//comunico que el player toco los Spikes
        }
    }
}
