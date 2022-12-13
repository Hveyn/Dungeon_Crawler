using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    [SerializeField] 
    private GameObject transferPrefab;

    
    [SerializeField]
    private RoomFirstDungeonGenerator generator;

    private DungeonData _dungeonData;

    private void Awake()
    {
        Instance = this;
        DataHolder.KillsEnemy = 0;
    }

    private void Start()
    {
        if (generator != null)
        {
            _dungeonData = generator.GetComponent<DungeonData>();
            generator.GenerateDungeon();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log(_dungeonData.countEnemy);
        }
    }

    public void LoadNextScene(int indexScene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(indexScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void CreateTransfer()
    {
        Vector2 transferNextLevelPos = _dungeonData.Rooms[_dungeonData.Rooms.Count - 1].RoomCenterPos;
        Vector2Int positionTransfer = new Vector2Int((int)transferNextLevelPos.x, (int)transferNextLevelPos.y);
        
        GameObject copyTransfer = Instantiate(transferPrefab);
        copyTransfer.transform.position = new Vector3(positionTransfer.x+0.5f, positionTransfer.y+0.5f, 0);
        if(_dungeonData == null) Debug.Log("dungeon null");
        _dungeonData.Rooms[_dungeonData.Rooms.Count-1].PropGameObjectRaferences.Add(copyTransfer);
    }
    
    
}
