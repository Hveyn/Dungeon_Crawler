using System;
using UnityEngine;

public class CounterHealth : MonoBehaviour
{
   [SerializeField] 
   private int maxHealth = 5;

   private DungeonData _dungeonData;

 
   private int _health;
   
   public int MaxHealth => maxHealth;
   public int Health => _health;

   private void Awake()
   {
      _dungeonData = FindObjectOfType<DungeonData>();
      _health = maxHealth;
   }

   public void ResetHp()
   {
      _health = maxHealth;
   }

   public void Healing(int countHp)
   {
      if (_health + countHp > maxHealth)
      {
         _health = maxHealth;
      }
      else
      {
         _health += countHp;
      }
   }
   
   public void TakeDamage(int damage)
   {
      if (_health - damage <= 0) _health = 0;
      else _health -= damage;
      
      Debug.Log(_health);
      if (_health <= 0)
      {
         if (gameObject.CompareTag("Enemy"))
         {
            gameObject.GetComponent<DeathEnemy>().Death();
            _dungeonData.countEnemy -= 1;
            DataHolder.KillsEnemy += 1;
         }
         else
         {
            Destroy(gameObject);
            StopAllCoroutines();
         }
      }
   }
}
