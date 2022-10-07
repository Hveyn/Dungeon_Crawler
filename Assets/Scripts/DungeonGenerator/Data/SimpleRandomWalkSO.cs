using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParametrs_",menuName = "PCG/SimpleRandomWalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
   public int iteretions = 10, walkLenght = 10;
   public bool startRandomlyEachIteration = true;
}
