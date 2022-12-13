using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiInGame : MonoBehaviour
{
    public static UiInGame Instance;
    [SerializeField] 
    private Sprite[] statesHealthBar;

    [SerializeField]
    private GameObject player;
    
    [SerializeField]
    private GameObject healthBar;

    [SerializeField] 
    private GameObject pausePanel;

    [SerializeField] 
    private GameObject deathPanel;
    
    [SerializeField] 
    private GameObject winPanel;
    private int _stateHealthBar;
    private void Awake()
    {
     
        if(deathPanel.activeSelf) pausePanel.SetActive(false);
        if(winPanel.activeSelf) pausePanel.SetActive(false);
        if(pausePanel.activeSelf) pausePanel.SetActive(false);
        Instance = this;
    }

    private void Update()
    {
        UiInGame[] copyUi = FindObjectsOfType<UiInGame>();
        foreach (UiInGame copy in copyUi)
        {
            Debug.Log("find");
            if (copy.gameObject != gameObject)
            {
                Debug.Log("not");
                Destroy(copy.gameObject);
            }
        }
        player = GameObject.FindGameObjectWithTag("Player");
        StateHealthBar();

        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf)
            {
                pausePanel.SetActive(false);
                Cursor.visible = false;
            }
            else
            {
                pausePanel.SetActive(true);
                Cursor.visible = true;
            }
        }
        

        if (player.IsDestroyed() || player == null)
        {
            deathPanel.SetActive(true);
        }
    }

    public void FinalGame()
    {
        winPanel.SetActive(true);
    }
    
    private void StateHealthBar()
    {
        if (player != null)
        {
            _stateHealthBar = player.GetComponent<CounterHealth>().Health;
        }
        else
        {
            _stateHealthBar = 0;
        }

        if (_stateHealthBar < 0) healthBar.GetComponent<Image>().sprite = statesHealthBar[0];
        else healthBar.GetComponent<Image>().sprite = statesHealthBar[_stateHealthBar];
        
    }
}
