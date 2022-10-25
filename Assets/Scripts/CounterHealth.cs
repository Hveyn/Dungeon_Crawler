using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterHealth : MonoBehaviour
{
   [SerializeField] private int health=5;

   public int Health => health;
   
   public void TakeDamage(int damage)
   {
      health -= damage;
      if (health <= 0) Destroy(gameObject);
      Debug.Log(health);
   }
}
