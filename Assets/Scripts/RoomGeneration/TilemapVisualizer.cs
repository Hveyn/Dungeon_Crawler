using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap, wallsLeftRightBottomTilemap, fullWallsTilemap, coliderTilemap;
    [SerializeField]
    private TileBase floorTile,pathFloorTile, wallTop, wallOnTop, wallSideRight, wallSideLeft, wallBottom, wallFull,
        wallInnerCornerDownLeft, wallInnerCornerDownRight, wallInnerCornerUpLeft, wallInnerCornerUpRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft,
        empyWallTile, empyFloorTile, nextLevelTile;

    [SerializeField]
    bool showPath = false;
    
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }
    
    public void PaintTransferTile(Vector2Int transferNextLevelPosition )
    {
        PaintTile(floorTilemap, nextLevelTile,transferNextLevelPosition);
    }
    
    public void PaintcoridorFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        if (showPath) PaintTiles(floorPositions, floorTilemap, pathFloorTile);
        else PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    public void PaintColiderFloorTiles(HashSet<Vector2Int> coliderMapPositions)
    {
        foreach (var position in coliderMapPositions)
        {
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
            coliderTilemap.SetTile(tilePosition, empyFloorTile);
        }
    }
    
    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintTile(tilemap, tile, position);
        }
    }
    
    private void PaintTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
        if (tilemap != wallTilemap)
        {
            wallTilemap.SetTile(tilePosition,empyWallTile);
            if (tile != wallFull)
            {
                tilePosition = new Vector3Int(tilePosition.x, tilePosition.y - 1, tilePosition.z);
                fullWallsTilemap.SetTile(tilePosition,empyWallTile);
            }
        }
    }
    private void PaintTile(Tilemap tilemap, TileBase botTile,TileBase topTile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, botTile);
        tilePosition.y += 1;
        tilemap.SetTile(tilePosition, topTile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        wallsLeftRightBottomTilemap.ClearAllTiles();
        fullWallsTilemap.ClearAllTiles();
        coliderTilemap.ClearAllTiles();
    }

    public void PaintSingeBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        
        

        if (tile != null)
        {
            if (tile == wallTop)
            {
                PaintTile(wallTilemap, tile, wallOnTop, position);
            }
            else if (tile == wallFull)
            {
                PaintTile(fullWallsTilemap, tile, position);
            }
            else
            {
                Vector2Int pos = position;
                pos.y += 1;

                PaintTile(wallsLeftRightBottomTilemap, tile, pos);

            }
        }
    }

    public void PaintSingeCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType,2);
        TileBase tile = null;

        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if (WallTypesHelper.wallInnerCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerUpLeft;
        }
        else if (WallTypesHelper.wallInnerCornerUpRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerUpRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }


        if (tile != null)
        {
            if (tile == wallFull)
            {
                PaintTile(wallsLeftRightBottomTilemap,tile,position);
            }
            else
            {
                Vector2Int pos = position;
                pos.y += 1;
                PaintTile(wallsLeftRightBottomTilemap,tile,pos);
            }
            
        }
    }
}

