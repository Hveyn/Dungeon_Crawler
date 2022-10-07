using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGeneratop
{
    [SerializeField]
    private int corridorLenght = 14, corridorCount = 5;
    [SerializeField] 
    [Range(0.1f, 1f)] 
    private float roomPecent = 0.8f;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstDungeonGeneration();
    }

    private void CorridorFirstDungeonGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);
        
        floorPositions.UnionWith(roomPositions);
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPecent);

        List<Vector2Int> roomsToCreate =
            potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParametrs, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPostion = startPosition;
        potentialRoomPositions.Add(currentPostion);
        
        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPostion, corridorLenght);
            currentPostion = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPostion);
            floorPositions.UnionWith(corridor);
        }
    }
}
