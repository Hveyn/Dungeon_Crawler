using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDamage : MonoBehaviour
{
    [SerializeField] 
    private GameObject player;
    [SerializeField]
    private int touchDamage = 1;
    
    private Collider2D _collider2D;

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject target = col.gameObject;

        if (target == player)
        {
            player.GetComponent<CounterHealth>().TakeDamage(touchDamage);
        }
    }
}
