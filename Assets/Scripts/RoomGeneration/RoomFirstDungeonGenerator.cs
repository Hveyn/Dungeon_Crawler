using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : AbstractDungeonGenerator
{
   [SerializeField]
   private int minRoomWidth = 4, minRoomHeight = 4;
   
   [SerializeField]
   private int dungeonWidth = 20, dungeonHeight = 20;
   
   [SerializeField]
   [Range(0,10)]
   private int offset = 1;

   [SerializeField]
   private int spanCorridorConnect;

   public UnityEvent onFinishedRoomGeneration;

   private DungeonData _dungeonData;
   protected override void RunProceduralGeneration()
   {
      _dungeonData = FindObjectOfType<DungeonData>();
      if (_dungeonData == null) _dungeonData = gameObject.AddComponent<DungeonData>();
      CreateRooms();
   }
   

   private void CreateRooms()
   {
      _dungeonData.Reset();
      
      var roomsList = ProceduralGenerationAlgorithm.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition,
         new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

      HashSet<Vector2Int> floor= new HashSet<Vector2Int>();
      List<Vector2Int> roomCenters = new List<Vector2Int>();
      foreach (var room in roomsList)
      {
         HashSet<Vector2Int> floorRoom = CreateSimpleRoom(room);
         roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));

         Room dataRoom = new Room((Vector2Int)Vector3Int.RoundToInt(room.center), floorRoom);
         _dungeonData.Rooms.Add(dataRoom);
         tilemapVisualizer.PaintFloorTiles(floorRoom);
         floor.UnionWith(floorRoom);
      }

      ConnectRooms(roomCenters);
      
      //floor.UnionWith(corridors);
      tilemapVisualizer.PaintcoridorFloorTiles(_dungeonData.Path);
      floor.UnionWith(_dungeonData.Path);
      WallGenerator.CreateWalls(floor, tilemapVisualizer);

      HashSet<Vector2Int> coliderFloor = GenColiderFloor(floor);

      tilemapVisualizer.PaintColiderFloorTiles(coliderFloor);

      onFinishedRoomGeneration?.Invoke();

      Vector2 transferNextLevelPos = _dungeonData.Rooms[_dungeonData.Rooms.Count - 1].RoomCenterPos;
      tilemapVisualizer.PaintTransferTile(new Vector2Int((int)transferNextLevelPos.x,(int)transferNextLevelPos.y));
   }

   private HashSet<Vector2Int> GenColiderFloor(HashSet<Vector2Int> floor)
   {
      BoundsInt coliderMap = new BoundsInt((Vector3Int)startPosition,
         new Vector3Int(dungeonWidth, dungeonHeight, 0));

      HashSet<Vector2Int> coliderFloor = new HashSet<Vector2Int>();
      for (int i = 0; i < coliderMap.size.x; i++)
      {
         for (int j = 0; j < coliderMap.size.y; j++)
         {
            Vector2Int pos =(Vector2Int)coliderMap.min + new Vector2Int(i, j);
            if (floor.Contains(pos) == false)
            {
               coliderFloor.Add(pos);
            }
         }
      }

      return coliderFloor;
   }


   private void ConnectRooms(List<Vector2Int> roomCenters)
   {
      var currentRoomCenter = roomCenters[Random.Range(0,roomCenters.Count)];
      roomCenters.Remove(currentRoomCenter);

      while (roomCenters.Count > 0)
      {
         Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
         roomCenters.Remove(closest);
         HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
         currentRoomCenter = closest;
         _dungeonData.Path.UnionWith(newCorridor);
      }
   }

   private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
   {
      HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
      var position = currentRoomCenter;
      
      Vector2Int parallelPosition;

      corridor.Add(position);

      while (position.y != destination.y)
      {
         if (destination.y > position.y)
         {
            position += Vector2Int.up;
         }
         else if (destination.y < position.y)
         {
            position += Vector2Int.down;
            
         }
         
         parallelPosition = position;
         parallelPosition += Vector2Int.right * spanCorridorConnect;
         
         corridor.Add(position);
         corridor.Add(parallelPosition);
      }
      

      while (position.x != destination.x)
      {
         if (destination.x > position.x)
         {
            position += Vector2Int.right;
         }
         else if (destination.x < position.x)
         {
            position += Vector2Int.left;
         }
         
         parallelPosition = position;
         parallelPosition += Vector2Int.up * spanCorridorConnect;
         
         corridor.Add(position);
         corridor.Add(parallelPosition);
      }

      return corridor;
   }

   private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
   {
      Vector2Int closest = Vector2Int.zero;
      float distance = float.MaxValue;
      foreach (var position in roomCenters)
      {
         float currentDistance = Vector2.Distance(position, currentRoomCenter);
         if (currentDistance < distance)
         {
            distance = currentDistance;
            closest = position;
         }
      }

      return closest;
   }

   private HashSet<Vector2Int> CreateSimpleRoom(BoundsInt room)
   {
      HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

      for (int col = offset; col < room.size.x - offset; col++)
      {
         for (int row = offset; row < room.size.y - offset; row++)
         {
            Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
            floor.Add(position);
         }
      }
      

      return floor;
   }
}
