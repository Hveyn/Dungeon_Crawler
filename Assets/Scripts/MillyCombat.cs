using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MillyCombat : MonoBehaviour
{
    [SerializeField] 
    private int damage = 1;
    private Animator _weaponAnimator;
    private BoxCollider2D _collider;
    
    void Start()
    {
        _weaponAnimator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _weaponAnimator.SetTrigger("Sword_attact");
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject target = col.gameObject;

        if (target.GetComponent<CounterHealth>())
        {
            target.GetComponent<CounterHealth>().TakeDamage(damage);
            Debug.Log(target.name);
        }
        
    }
}
