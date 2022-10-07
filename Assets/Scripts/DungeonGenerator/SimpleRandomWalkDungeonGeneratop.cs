using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGeneratop : AbstractDungeonGenerator
{
    [SerializeField] 
    protected SimpleRandomWalkSO randomWalkParametrs;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPosition = RunRandomWalk(randomWalkParametrs, startPosition);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPosition);
        WallGenerator.CreateWalls(floorPosition, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
    {
        var currentPostion = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iteretions; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPostion, parameters.walkLenght);
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
                currentPostion = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }

        return floorPositions;
    }
}
