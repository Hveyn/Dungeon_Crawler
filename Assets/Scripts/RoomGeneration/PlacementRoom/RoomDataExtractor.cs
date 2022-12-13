using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomDataExtractor : MonoBehaviour
{
    private DungeonData _dungeonData;

    [SerializeField] 
    private bool showGizmo;
    [SerializeField]
    private UnityEvent onFinishedRoomProcessing;
    
    
    private void Awake()
    {
        _dungeonData = GetComponent<DungeonData>();
    }

    public void ProcessRooms()
    {
        if (_dungeonData == null)
        {
            return;
        }

        int countAllFloorTiles = 0;

        foreach (Room room in _dungeonData.Rooms)
        {
            countAllFloorTiles += room.FloorTiles.Count;
            // Поиск угловых, около сен и внутренних тайлов
            foreach (Vector2Int tilePosition in room.FloorTiles)
            {
                int neighboursCount = 4;
                
                if (room.FloorTiles.Contains(tilePosition + Vector2Int.up) == false)
                {
                    room.NearWallTileUp.Add(tilePosition);
                    neighboursCount--;
                }
                if (room.FloorTiles.Contains(tilePosition + Vector2Int.down) == false)
                {
                    room.NearWallTileDown.Add(tilePosition);
                    neighboursCount--;
                }
                if (room.FloorTiles.Contains(tilePosition + Vector2Int.right) == false)
                {
                    room.NearWallTileRight.Add(tilePosition);
                    neighboursCount--;
                }
                if (room.FloorTiles.Contains(tilePosition + Vector2Int.left) == false)
                {
                    room.NearWallTileLeft.Add(tilePosition);
                    neighboursCount--;
                }
                
                //поиск угловых тайлов
                if (neighboursCount <= 2)
                    room.CornerTiles.Add(tilePosition);

                if (neighboursCount == 4)
                    room.InnerTiles.Add(tilePosition);
            }
            
            room.NearWallTileUp.ExceptWith(room.CornerTiles);
            room.NearWallTileDown.ExceptWith(room.CornerTiles);
            room.NearWallTileLeft.ExceptWith(room.CornerTiles);
            room.NearWallTileRight.ExceptWith(room.CornerTiles);
        }

        _dungeonData.avgCountFloorTilesInRooms = countAllFloorTiles / _dungeonData.Rooms.Count;
        Invoke("RunEvent", 1);
    }

    public void RunEvent()
    {
        onFinishedRoomProcessing?.Invoke();
    }
    
    private void OnDrawGizmosSelected()
    {
        if (_dungeonData == null || showGizmo == false)
            return;
        foreach (Room room in _dungeonData.Rooms)
        {
            //Обозначение внутренних тайлов
            Gizmos.color = Color.yellow;
            foreach (Vector2Int floorPosition in room.InnerTiles)
            {
                if (_dungeonData.Path.Contains(floorPosition))
                    continue;
                Gizmos.DrawCube( floorPosition + Vector2.one * 0.5f, Vector2.one);
            }
            //Обозначение тайлов у верхней стены
            Gizmos.color = Color.blue;
            foreach (Vector2Int floorPosition in room.NearWallTileUp)
            {
                if (_dungeonData.Path.Contains(floorPosition))
                    continue;
                Gizmos.DrawCube( floorPosition + Vector2.one * 0.5f, Vector2.one);
            }
            //Обозначение тайлов у нижней стены
            Gizmos.color = Color.green;
            foreach (Vector2Int floorPosition in room.NearWallTileDown)
            {
                if (_dungeonData.Path.Contains(floorPosition))
                    continue;
                Gizmos.DrawCube( floorPosition + Vector2.one * 0.5f, Vector2.one);
            }
            //Обозначение тайлов у правой стены
            Gizmos.color = Color.white;
            foreach (Vector2Int floorPosition in room.NearWallTileRight)
            {
                if (_dungeonData.Path.Contains(floorPosition))
                    continue;
                Gizmos.DrawCube( floorPosition + Vector2.one * 0.5f, Vector2.one);
            }
            //Обозначение тайлов у левой стены
            Gizmos.color = Color.cyan;
            foreach (Vector2Int floorPosition in room.NearWallTileLeft)
            {
                if (_dungeonData.Path.Contains(floorPosition))
                    continue;
                Gizmos.DrawCube( floorPosition + Vector2.one * 0.5f, Vector2.one); 
            }
            //Обозначение угловых тайлов 
            Gizmos.color = Color.magenta;
            foreach (Vector2Int floorPosition in room.CornerTiles)
            {
                if (_dungeonData.Path.Contains(floorPosition))
                    continue;
                Gizmos.DrawCube( floorPosition + Vector2.one * 0.5f, Vector2.one); 
            }
        }
    }
    
}
