using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class VizorEnemy : MonoBehaviour
{
    [SerializeField] 
    private GameObject target;
    
    [SerializeField]
    private GameObject enemy;

    private bool _isVisibleTarget = false;

    private NavMeshAgent _agent;
    void Start()
    {
        _agent = enemy.GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isVisibleTarget) _agent.SetDestination(target.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject colGameObject = col.gameObject;

        if (colGameObject == target)
        {
            _isVisibleTarget = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject otherGameObject = other.gameObject;

        if (otherGameObject == target)
        {
            _isVisibleTarget = false;
        }
    }
}
