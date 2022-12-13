using UnityEngine;

[CreateAssetMenu]
public class ParametrsRoomGeneration : ScriptableObject
{
   [Header("DungenSize")] 
   public int dungeonWidth = 65;
   public int dungeonHeight = 65;

   [Space, Header("MinRoomSize")] 
   public int minRoomWidth = 10;
   public int minRoomHeight = 10;

   [Header("DistanceBetweenRooms"), Range(0, 10)]
   public int offset = 1;

   [Header("CorridorsWidth"), Range(0, 3)]
   public int corridorsWidth = 1;

}
