using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlasksEffects : MonoBehaviour
{
    public static FlasksEffects Instance;
    
    [SerializeField] private float speedBoost = 1.5f;
    [SerializeField] private int powerBoost = 2;
    [SerializeField] private int countHealingHp = 2;
    [SerializeField] private int timeEffect;

    [SerializeField]
    private GameObject player;

    private CounterHealth _health;
    private PlayerMovement _playerSpeed;
    private MillyCombat _weapon;

    
    private void Awake()
    {
        Instance = this;
        _health = player.GetComponent<CounterHealth>();
        _playerSpeed = player.GetComponent<PlayerMovement>();
    }

    public void UpdateSelectedWeapon(GameObject weapon)
    {
        _weapon = weapon.GetComponent<MillyCombat>();
    }
    
    public void SelectedEffect(SpetificationsItem item)
    {
        switch (item.Action)
        {
            case SpetificationsItem.ActionType.Heal:
                _health.Healing(countHealingHp);
                break;
            case SpetificationsItem.ActionType.Boost:
                StartCoroutine(AddEffect(_playerSpeed));
                break;
            case SpetificationsItem.ActionType.PowerBoost:
                StartCoroutine(AddEffect(_weapon));
                break;
            case SpetificationsItem.ActionType.Poison:
                int damagePoison = Random.Range(1, _health.MaxHealth - 1);
                Debug.Log("TakeDamage: "+damagePoison);
                _health.TakeDamage(damagePoison);
                break;
        }
    }

    private IEnumerator AddEffect(PlayerMovement playerSpeed)
    {
        playerSpeed.BoostSpeed(speedBoost);
        yield return new WaitForSeconds(timeEffect);
        
        playerSpeed.DownSpeed(speedBoost);
    }
    private IEnumerator AddEffect(MillyCombat weapon)
    {
        if (weapon == null) weapon = FindObjectOfType<MillyCombat>();
        Debug.Log(weapon);
        weapon.PowerBoost(powerBoost);
        yield return new WaitForSeconds(timeEffect);
        weapon.PowerDown(powerBoost);
    }
 
}
