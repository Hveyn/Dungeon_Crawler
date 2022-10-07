using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPosition = FindWallsDirection(floorPositions, Direction2D.cardinalDiractionsList);
        foreach (var position in basicWallPosition)
        {
            tilemapVisualizer.PaintSingeBasicWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsDirection(HashSet<Vector2Int> floorPositions, List<Vector2Int> diractionsList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in diractionsList)
            {
                var neigbourPosition = position + direction;
                if (floorPositions.Contains(neigbourPosition) == false)
                    wallPositions.Add(neigbourPosition);
            }
        }

        return wallPositions;
    }
}
