using UnityEngine;
using UnityEngine.AI;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject generator;
    [SerializeField] private NavMeshSurface2d navMesh;
    void Start()
    {
        generator.GetComponent<RoomFirstDungeonGenerator>().GenerateDungeon();
        navMesh.BuildNavMesh();
    }

}
