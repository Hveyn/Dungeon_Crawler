using UnityEngine;
using UnityEngine.AI;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject generator;
    [SerializeField] private NavMeshSurface2d navMesh;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            generator.GetComponent<RoomFirstDungeonGenerator>().GenerateDungeon();
        }
           
    }

}
