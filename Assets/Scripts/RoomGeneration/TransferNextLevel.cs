using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferNextLevel : MonoBehaviour
{
    [SerializeField] private GameObject transferPrefab;
    
    public void CreateTransfer(Vector2Int position)
    {
        GameObject copyTransfer = Instantiate(transferPrefab);
        copyTransfer.transform.position = new Vector3Int(position.x, position.y, 0);
    }
}
