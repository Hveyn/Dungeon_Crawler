using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiInGame : MonoBehaviour
{
    [SerializeField] 
    private Sprite[] statesHealthBar;

    [SerializeField]
    private GameObject player;
    
    [SerializeField]
    private GameObject healthBar;
    
    private int _stateHealthBar;

    private void Update()
    {
        StateHealthBar();
    }

    private void StateHealthBar()
    {
        _stateHealthBar = !player.IsDestroyed() ? player.GetComponent<CounterHealth>().Health : 0;

        healthBar.GetComponent<Image>().sprite = statesHealthBar[_stateHealthBar];
    }
}
