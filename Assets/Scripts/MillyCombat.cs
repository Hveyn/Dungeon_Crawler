using UnityEngine;

public class MillyCombat : MonoBehaviour
{
    [SerializeField] 
    private int damage = 1;
    private Animator _weaponAnimator;

    void Start()
    {
        _weaponAnimator = GetComponent<Animator>();
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
