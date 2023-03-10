using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonData : MonoBehaviour
{
    public List<Room> Rooms { get; set; } = new List<Room>();
    public HashSet<Vector2Int> Path { get; set; } = new HashSet<Vector2Int>();

    [HideInInspector]
    public float avgCountFloorTilesInRooms;
    [HideInInspector]
    public int countChests;

    public int countEnemy;
    public GameObject PlayerRefence { get; set; }
    public void Reset()
    {
        foreach (var room in Rooms)
        {
            foreach (GameObject item in room.PropGameObjectRaferences)
            {
                Destroy(item);
            }

            foreach (GameObject item in room.EnemiesInTheRoom)
            {
                Destroy(item);
            }
        }
        
        countChests = 0;
        countEnemy = 0;
        Rooms = new();
        Path = new();
        Destroy(PlayerRefence);
    }

}
