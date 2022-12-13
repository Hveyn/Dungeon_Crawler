using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RandomSplashItem : MonoBehaviour
{
    [Header("Random Splash"), SerializeField]
    private Transform objTransform;
    [SerializeField] 
    private float delay = 0;
    [SerializeField] 
    private float pastTime = 0;
    [SerializeField]
    private float movingTime = 1.0f;

    private Vector3 _randomDirection;
    private Rigidbody2D _rigidbody2D;
    
    private void Awake()
    {
        //Random x and y axis
        _randomDirection = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (movingTime >= delay)
        {
            pastTime = Time.deltaTime;
            //position of coin
            objTransform.position += _randomDirection * Time.deltaTime;
            delay += pastTime;
        }
    }
}
