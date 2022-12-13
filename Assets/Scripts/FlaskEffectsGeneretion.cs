using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlaskEffectsGeneretion : MonoBehaviour
{
    [Header("FlasksEffects")]
    [SerializeField]
    private SpetificationsItem[] greenFlaskEffects;
    [SerializeField]
    private SpetificationsItem[] yellowFlaskEffects;
    [SerializeField]
    private SpetificationsItem[] redFlaskEffects;

    private SpetificationsItem _greenFlaskEffect;
    private SpetificationsItem _yellowFlaskEffect;
    private SpetificationsItem _redFlaskEffect;
    
    private void Awake()
    {
        int indexGreenFlaskEffect = Random.Range(0, greenFlaskEffects.Length);
        int indexYellowFlaskEffect = Random.Range(0, yellowFlaskEffects.Length);
        int indexRedFlaskEffect = Random.Range(0, redFlaskEffects.Length);
        

        
        _greenFlaskEffect = greenFlaskEffects[indexGreenFlaskEffect];

        
        while (indexYellowFlaskEffect == indexGreenFlaskEffect)
        {
            indexYellowFlaskEffect = Random.Range(0, yellowFlaskEffects.Length);
        }

        _yellowFlaskEffect = yellowFlaskEffects[indexYellowFlaskEffect];

        while (indexRedFlaskEffect == indexGreenFlaskEffect || indexRedFlaskEffect == indexYellowFlaskEffect)
        {
            indexRedFlaskEffect = Random.Range(0, redFlaskEffects.Length);
        }

        _redFlaskEffect = redFlaskEffects[indexRedFlaskEffect];

        DataHolder.GreenFlaskEffect = _greenFlaskEffect;
        DataHolder.YellowFlaskEffect = _yellowFlaskEffect;
        DataHolder.RedFlaskEffect = _redFlaskEffect;
    }
}
