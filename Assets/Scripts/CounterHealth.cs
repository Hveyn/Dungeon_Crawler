using System;
using UnityEngine;

public class CounterHealth : MonoBehaviour
{
   [SerializeField] private int health=5;
   
   public int Health => health;
   private int _healthStart;

   private void Awake()
   {
      _healthStart = health;
   }

   public void ResetHP()
   {
      health = _healthStart;
   }
   public void TakeDamage(int damage)
   {
      health -= damage;
      if (health <= 0) Destroy(gameObject);
      Debug.Log(health);
   }
}
