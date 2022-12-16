using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TouchDamage : MonoBehaviour
{
    [SerializeField] 
    private GameObject player;
    [SerializeField]
    private int touchDamage = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject target = col.gameObject;

        if (target.CompareTag("Player"))
        {
            if (target != null)
            {
                target.GetComponent<CounterHealth>().TakeDamage(touchDamage);
            }
        }
    }
}
