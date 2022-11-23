using UnityEngine;

public class TouchDamage : MonoBehaviour
{
    [SerializeField] 
    private GameObject player;
    [SerializeField]
    private int touchDamage = 1;
    
    /*private void OnTriggerStay2D(Collider2D col)
    {
        GameObject target = col.gameObject;

        if (target == player)
        {
            player.GetComponent<CounterHealth>().TakeDamage(touchDamage);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject target = col.gameObject;

        if (target.CompareTag("Player"))
        {
            target.GetComponent<CounterHealth>().TakeDamage(touchDamage);
        }
    }
}
