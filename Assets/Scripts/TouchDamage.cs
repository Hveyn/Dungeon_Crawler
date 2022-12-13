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

    private IEnumerable _takeDamage;

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject target = col.gameObject;

        if (target.CompareTag("Player"))
        {
            if (target != null)
            {
                _takeDamage = Damage(target);
                StartCoroutine(_takeDamage.GetEnumerator());
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        GameObject target = other.gameObject;

        if (target.CompareTag("Player"))
        {
            StopCoroutine(_takeDamage.GetEnumerator());
        }
    }

    private IEnumerable Damage(GameObject target)
    {
        while (true)
        {
            if(target.IsDestroyed() || target == null) break;
            target.GetComponent<CounterHealth>().TakeDamage(touchDamage);
            yield return new WaitForSeconds(1);
        }
    }
}
