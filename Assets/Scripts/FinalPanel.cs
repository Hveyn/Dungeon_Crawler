using System;
using UnityEngine;
using UnityEngine.UI;

public class FinalPanel : MonoBehaviour
{
    [SerializeField]
    private Text countCoins;
    [SerializeField]
    private Text counterCoins;
    [SerializeField] 
    private Text counterKeys;
    [SerializeField]
    private Text countEnemy;
    
    private void OnEnable()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        int priceKeys = Int32.Parse(counterKeys.text) * 150;
        int totalSum = Int32.Parse(counterCoins.text)+priceKeys;
        countCoins.text = totalSum.ToString();
        countEnemy.text = DataHolder.KillsEnemy.ToString();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        countCoins.text = "0";
        countEnemy.text = "0";
    }
}

