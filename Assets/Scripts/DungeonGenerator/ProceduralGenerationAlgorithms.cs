using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{

    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLenght)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);
        var previousPositon = startPosition;

        for (int i = 0; i < walkLenght; i++)
        {
            var newPosition = previousPositon + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPositon = newPosition;
        }

        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridoreLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPositision = startPosition;
        corridor.Add(currentPositision);
        
        for (int i = 0; i < corridoreLength; i++)
        {
            currentPositision += direction;
            corridor.Add(currentPositision);
        }

        return corridor;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDiractionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1), // UP
        new Vector2Int(1, 0), //RIGHT
        new Vector2Int(0, -1), //DOWN
        new Vector2Int(-1, 0) //LEFT
    };

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDiractionsList[Random.Range(0, cardinalDiractionsList.Count)];
    }
}