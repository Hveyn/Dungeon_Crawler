using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public Vector2 RoomCenterPos { get; set; }
    public HashSet<Vector2Int> FloorTiles { get; private set; } = new HashSet<Vector2Int>();

    public HashSet<Vector2Int> NearWallTileUp { get; set; } = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> NearWallTileDown { get; set; } = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> NearWallTileLeft { get; set; } = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> NearWallTileRight { get; set; } = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> CornerTiles { get; set; } = new HashSet<Vector2Int>();
    
    public HashSet<Vector2Int> InnerTiles { get; set; } = new HashSet<Vector2Int>();
    
    public HashSet<Vector2Int> PropsPositions { get; set; } = new HashSet<Vector2Int>();
    public List<GameObject> PropGameObjectRaferences { get; set; } = new List<GameObject>();
    
    public List<Vector2Int> PositionsAccessibleFromPath { get; set; } = new List<Vector2Int>();

    public List<GameObject> EnemiesInTheRoom { get; set; } = new List<GameObject>();

    public Room(Vector2 roomCenterPos, HashSet<Vector2Int> floorTiles)
    {
        this.RoomCenterPos = roomCenterPos;
        this.FloorTiles = floorTiles;
    }


}
