using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PropPlacementManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject generator;
    
    private DungeonData _dungeonData;
    [SerializeField]
    private List<Prop> propsToPlace;

    [SerializeField, Range(0,1)] 
    private float cornerPropPlacementChance = 0.7f;

    [SerializeField] 
    private GameObject propPrefab;
    
    [SerializeField] 
    private GameObject allPropPrefab;

    [SerializeField]
    private float sizeColliderProp = 0.8f;
    
    [SerializeField] 
    private NavMeshSurface2d navmesh;
    
    public UnityEvent OnFinished;
    
    private void Awake()
    {
        _dungeonData = generator.GetComponent<DungeonData>();
    }

    public void ProcessRooms()
    {
        if (_dungeonData == null)
            return;
        foreach (Room room in _dungeonData.Rooms)
        {
            
            //Расположение объектов в углах ~ Place props place propc in the corners
            List<Prop> cornerProps = propsToPlace.Where(x => x.Corner).ToList();
            PlaceCornerProps(room, cornerProps);
            
            //Расплоложение объектов у левой стены ~ Place props near LEFT wall
            List<Prop> leftWallProps = propsToPlace
                .Where(x => x.NearWallLeft)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y)
                .ToList();

            PlaceProps(room, leftWallProps, room.NearWallTileLeft, PlacementOriginCorner.BottomLeft);
            
            //Расплоложение объектов у правой стены ~ Place props near RIGHT wall
            List<Prop> rightWallProps = propsToPlace
                .Where(x => x.NearWallRight)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y)
                .ToList();

            PlaceProps(room, rightWallProps, room.NearWallTileRight, PlacementOriginCorner.TopRight);
            
            //Расплоложение объектов у верхней стены ~ Place props near UP wall
            List<Prop> topWallProps = propsToPlace
                .Where(x => x.NearWallUp)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y)
                .ToList();

            PlaceProps(room, topWallProps, room.NearWallTileUp, PlacementOriginCorner.TopLeft);
            
            //Расплоложение объектов у нижней стены ~ Place props near DOWN wall
            List<Prop> downWallProps = propsToPlace
                .Where(x => x.NearWallDown)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y)
                .ToList();

            PlaceProps(room, downWallProps, room.NearWallTileDown, PlacementOriginCorner.BottomLeft);
            
            //Расплоложение центральный объектов ~ Place inner props
            List<Prop> innerProps = propsToPlace
                .Where(x => x.Inner)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y)
                .ToList();

            PlaceProps(room, innerProps, room.InnerTiles, PlacementOriginCorner.BottomLeft);
        }
        navmesh.BuildNavMesh();
        OnFinished?.Invoke();
    }

    private void PlaceProps(Room room, List<Prop> wallProps, HashSet<Vector2Int> avaiableTiles, PlacementOriginCorner placement)
    {
        float chancePlaceProp;
        chancePlaceProp = room.FloorTiles.Count / _dungeonData.avgCountFloorTilesInRooms;
        /*Убераем позиции пути коридора из позможных мест для размещения объекта
         ~ Remove path position from the initial nearWallTiles to ensure the clean path to traverse dungeon*/
        HashSet<Vector2Int> tempPositions = new HashSet<Vector2Int>(avaiableTiles);
        tempPositions.ExceptWith(_dungeonData.Path);
        
        // Пытаемся расположить все объекты для каждого места ~ We will try place all the props
        foreach (Prop propToPlace in wallProps)
        {
            if (UnityEngine.Random.Range(0f, 1f) < chancePlaceProp)
            {
                // Расчитываем кол-во объектов одного типа ~ We will to place only certain quantuty of each prop
                int quantity = UnityEngine.Random.Range(propToPlace.PlacementQuantityMin,
                    propToPlace.PlacementQuantityMax + 1);

                for (int i = 0; i < quantity; i++)
                {
                    //удаляем занятые посиции ~ remove taken position
                    tempPositions.ExceptWith(room.PropsPositions);
                    //перемешиваем позиции ~ shiffle the positions
                    List<Vector2Int> avaiablePositions = tempPositions.OrderBy(x => Guid.NewGuid()).ToList();
                    /* Если в точке размещения нет объекта то пытаемся разместить объект ~
                       If placement has failed there is no point in trying to place the same prop again*/
                    if (TryPlacingPropBruteForce(room, propToPlace, avaiablePositions, placement) == false)
                        break;
                }
            }
        }
    }

    private bool TryPlacingPropBruteForce(
        Room room, Prop propToPlace, List<Vector2Int> avaiablePositions, PlacementOriginCorner placement)
    {
        for (int i = 0; i < avaiablePositions.Count; i++)
        {
            Vector2Int position = avaiablePositions[i];
            if(room.PropsPositions.Contains(position))
                continue;

            List<Vector2Int> freePositionsAround
                = TryToFitProp(propToPlace, avaiablePositions, position, placement);
            
            //Если хватает места для объекта ~ if we have enough place the prop
            if (freePositionsAround.Count == propToPlace.PropSize.x * propToPlace.PropSize.y)
            {
                PlacePropGameObjectAt(room, position, propToPlace);

                foreach (Vector2Int pos in freePositionsAround)
                {
                    room.PropsPositions.Add(pos);
                }


                if (propToPlace.PlaceAsGroup)
                {
                    placeGroupObject(room, position, propToPlace, 1);
                }

                return true;
            }
        }

        return false;


    }

    private List<Vector2Int> TryToFitProp(
        Prop prop, List<Vector2Int> avaiablePositions, Vector2Int originPosition, PlacementOriginCorner placement)
    {
        List<Vector2Int> freePositions = new();

        if (placement == PlacementOriginCorner.BottomLeft)
        {
            for (int xOffset = 0; xOffset < prop.PropSize.x; xOffset++)
            {
                for (int yOffset = 0; yOffset < prop.PropSize.y; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                    if(avaiablePositions.Contains(tempPos)) 
                        freePositions.Add(tempPos);
                }
            }
        }
        else if (placement == PlacementOriginCorner.BottomRight)
        {
            for (int xOffset = -prop.PropSize.x; xOffset < prop.PropSize.x; xOffset++)
            {
                for (int yOffset = 0; yOffset < prop.PropSize.y; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                    if(avaiablePositions.Contains(tempPos)) 
                        freePositions.Add(tempPos);
                }
            }
        }
        else if (placement == PlacementOriginCorner.TopLeft)
        {
            for (int xOffset = 0; xOffset < prop.PropSize.x; xOffset++)
            {
                for (int yOffset = -prop.PropSize.y; yOffset < prop.PropSize.y; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                    if(avaiablePositions.Contains(tempPos)) 
                        freePositions.Add(tempPos);
                }
            }
        }
        else // Top Right
        {
            for (int xOffset = -prop.PropSize.x; xOffset < prop.PropSize.x; xOffset++)
            {
                for (int yOffset = -prop.PropSize.y; yOffset < prop.PropSize.y; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);
                    if(avaiablePositions.Contains(tempPos)) 
                        freePositions.Add(tempPos);
                }
            }
        }

        return freePositions;
    }

    private void PlaceCornerProps(Room room, List<Prop> cornerProps)
    {
        float tempChance = cornerPropPlacementChance;

        foreach (Vector2Int cornerTile in room.CornerTiles)
        {
            if (UnityEngine.Random.value < tempChance)
            {
                Prop propToPlace
                    = cornerProps[UnityEngine.Random.Range(0, cornerProps.Count)];

                PlacePropGameObjectAt(room, cornerTile, propToPlace);
                if (propToPlace.PlaceAsGroup)
                {
                    placeGroupObject(room, cornerTile, propToPlace, 2);
                }
            }
            else
            {
                tempChance = Mathf.Clamp01(tempChance + 0.1f);
            }
        }
    }

    private void placeGroupObject(Room room, Vector2Int groupOriginPosition, Prop propToPlace, int searchOffset)
    {
        // Плохо работает с размещением больших объектов группами

        int count = UnityEngine.Random.Range(propToPlace.GroupMinCount, propToPlace.GroupMaxCount) - 1;
        count = Math.Clamp(count, 0, 8);

        /* Поиск свободного места вокруг центральной точки ~ find the avaiable spaces around the center point
        / searchOffset используться для ограничения дальности между точками и центрльной точкой 
        we use searchOffset to limit the distance between those points and the center point*/
        List<Vector2Int> avaiableSpaces = new List<Vector2Int>();
        for (int xOffset = -searchOffset; xOffset <= searchOffset; xOffset++)
        {
            for (int yOffset = -searchOffset; yOffset <= searchOffset; yOffset++)
            {
                Vector2Int tempPos = groupOriginPosition + new Vector2Int(xOffset, yOffset);
                if (room.FloorTiles.Contains(tempPos) &&
                    !_dungeonData.Path.Contains(tempPos)&&
                    !room.PropsPositions.Contains(tempPos))
                {
                    avaiableSpaces.Add(tempPos);
                }
            }
        }
        
        //Перемешивание списка ~ shiffle the list
        avaiableSpaces.OrderBy(x => Guid.NewGuid());
        
        /*Расположение объектов (Если есть свободное пространтсов мы дудем заполнять его
         ~ place the props (as many as we want or if there is less space fill all the avaiable spaces*/
        int tempCount = count < avaiableSpaces.Count ? count : avaiableSpaces.Count;
        for (int i = 0; i < tempCount; i++)
        {
            PlacePropGameObjectAt(room, avaiableSpaces[i], propToPlace);
        }
    }

    private GameObject PlacePropGameObjectAt(Room room, Vector2Int placementPosition, Prop propToPlace)
    {
        //Создание объекта на данной позиции ~ Instantiat the prop at this position
        // идея собрать все объекты в одну группу ввиде объекта родителя
        GameObject prop = Instantiate(propPrefab, allPropPrefab.transform);
        SpriteRenderer propSpriteRenderer = prop.GetComponentInChildren<SpriteRenderer>();
        
        //Установка спрайта ~ set the sprite
        propSpriteRenderer.sprite = propToPlace.PropSprite;
        
        //Добавление коллайдера ~ Add a collider
        CapsuleCollider2D collider
            = propSpriteRenderer.gameObject.AddComponent<CapsuleCollider2D>();

        NavMeshObstacle obstacle = propSpriteRenderer.gameObject.GetComponent<NavMeshObstacle>();
        collider.offset = Vector2.zero;
        

        if (propToPlace.PropSize.x > propToPlace.PropSize.y)
        {
            collider.direction = CapsuleDirection2D.Horizontal;
        }
        
        Vector2 size
            = new Vector2(propToPlace.PropSize.x * sizeColliderProp, propToPlace.PropSize.y * sizeColliderProp);
        collider.size = size;
        obstacle.size = size;
        
        prop.transform.localPosition = (Vector2)placementPosition;
        
        //регулировка позиции спрайта ~ abjust the position to the sprite
        propSpriteRenderer.transform.localPosition
            = (Vector2)propToPlace.PropSize * 0.5f;
        
        /* Сохранение обхекта в данных комнаты которая находится в данных подземелья ~
           Save the prop in the room data (so in the dungeon data) */
        room.PropsPositions.Add(placementPosition);
        room.PropGameObjectRaferences.Add(prop);
        
        return prop;
    }

    enum PlacementOriginCorner
    {
        BottomLeft,
        BottomRight,
        TopLeft,
        TopRight
    }
}
