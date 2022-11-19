using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class AgentPlacer : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab, playerPrefab;

    [SerializeField] private int playerRoomIndex;
    [SerializeField] private CinemachineVirtualCamera vCamer;

    [SerializeField] private int maxCountEnemiesInRoom;

    [SerializeField, Range(0, 1)] private float chanceSpawnEnemyInRoom = 0.8f;
    [SerializeField] private GameObject generator;
    private DungeonData _dungeonData;

    public UnityEvent OnFinished;

    private int _countEnemyInRoom;

    [SerializeField] private bool showGizmo = false;

    private void Awake()
    {
        _dungeonData = generator.GetComponent<DungeonData>();
    }

    public void PlaceAgents()
    {
        Debug.Log(_dungeonData.Rooms.Count);
        for (int i = 0; i < _dungeonData.Rooms.Count; i++)
        {
            if (i == playerRoomIndex)
            {
                /* GameObject player = Instantiate(playerPrefab);
                // player.transform.localPosition = _dungeonData.Rooms[i].RoomCenterPos + Vector2.one * 0.5f;
 
                 vCamer.Follow = player.transform;
                 vCamer.LookAt = player.transform;
                 _dungeonData.PlayerRefence = player;
                 */
                playerPrefab.transform.position = _dungeonData.Rooms[i].RoomCenterPos + Vector2.one * 0.5f;
            }

            Room room = _dungeonData.Rooms[i];
            RoomGraph roomGraph = new RoomGraph(room.FloorTiles);

            HashSet<Vector2Int> roomFloor = new HashSet<Vector2Int>(room.FloorTiles);

            roomFloor.IntersectWith(_dungeonData.Path);

            Dictionary<Vector2Int, Vector2Int> roomMap
                = roomGraph.RunBFS(roomFloor.First(), room.PropsPositions);

            room.PositionsAccessibleFromPath = roomMap.Keys.OrderBy(x => Guid.NewGuid()).ToList();

            if (UnityEngine.Random.value < chanceSpawnEnemyInRoom)
            {
                _countEnemyInRoom = UnityEngine.Random.Range(0, maxCountEnemiesInRoom+1);

                if (_countEnemyInRoom > 0)
                {
                    Debug.Log("spawn enemy");
                    PlaceEnemies(room, _countEnemyInRoom);
                }
            }
        }

        OnFinished?.Invoke();
    }

    private void PlaceEnemies(Room room, int enemyCount)
    {
        for (int k = 0; k < enemyCount; k++)
        {
            if (room.PositionsAccessibleFromPath.Count <= k)
            {
                return;
            }
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.localPosition = (Vector2)room.PositionsAccessibleFromPath[k] + Vector2.one * 0.5f;
            room.EnemiesInTheRoom.Add(enemy);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_dungeonData == null || showGizmo == false)
            return;
        foreach (Room room in _dungeonData.Rooms)
        {
            Color color = Color.green;
            color.a = 0.3f;
            Gizmos.color = color;

            foreach (Vector2Int pos in room.PositionsAccessibleFromPath)
            {
                Gizmos.DrawCube((Vector2)pos + Vector2.one * 0.5f, Vector2.one);
            }
        }
    }
}

public class RoomGraph
    {
        public static List<Vector2Int> fourDirections = new()
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left
        };

        private Dictionary<Vector2Int, List<Vector2Int>> graph = new Dictionary<Vector2Int, List<Vector2Int>>();

        public RoomGraph(HashSet<Vector2Int> roomFloor)
        {
            foreach (Vector2Int pos in roomFloor)
            {
                List<Vector2Int> neighbours = new List<Vector2Int>();
                foreach (Vector2Int direction in fourDirections)
                {
                    Vector2Int newPos = pos + direction;
                    if (roomFloor.Contains(newPos))
                    {
                        neighbours.Add(newPos);
                    }
                }
                graph.Add(pos,neighbours);
            }
        }


        public Dictionary<Vector2Int, Vector2Int> RunBFS(Vector2Int startPos, HashSet<Vector2Int> occupiedNodes)
        {
            Queue<Vector2Int> nodesToVisit = new Queue<Vector2Int>();
            nodesToVisit.Enqueue(startPos);

            HashSet<Vector2Int> visitedNodes = new HashSet<Vector2Int>();
            visitedNodes.Add(startPos);

            Dictionary<Vector2Int, Vector2Int> map = new Dictionary<Vector2Int, Vector2Int>();
            map.Add(startPos,startPos);

            while (nodesToVisit.Count>0)
            {
                Vector2Int node = nodesToVisit.Dequeue();
                List<Vector2Int> neighbours = graph[node];

                foreach (Vector2Int neighbourPosition in neighbours)
                {
                    if (visitedNodes.Contains(neighbourPosition) == false &&
                        occupiedNodes.Contains(neighbourPosition) == false)
                    {
                        nodesToVisit.Enqueue(neighbourPosition);
                        visitedNodes.Add(neighbourPosition);
                        map[neighbourPosition] = node;
                    }
                }
            }

            return map;
        }
    }

