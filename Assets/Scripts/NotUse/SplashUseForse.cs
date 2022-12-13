using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class SplashUseForse : MonoBehaviour
{
    [SerializeField] 
    private float radios;
    [SerializeField]
    private float force;

    [SerializeField] 
    private bool active;

    private void Update()
    {
        if (active)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider2D[] overlapperColliders = Physics2D.OverlapCircleAll(transform.position, radios);

        foreach (Collider2D col in overlapperColliders)
        {
            Rigidbody2D rigidbody = col.attachedRigidbody;
            if (rigidbody)
            {
                rigidbody.AddExplosionForce(force,transform.position,radios);
            }
        }
    }
}
