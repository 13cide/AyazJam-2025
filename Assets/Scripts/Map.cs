using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] List<Transform> route;

    internal Vector2 GetNextPosFrom(int posIndex)
    {
        if (posIndex-1 == route.Count) return Vector2.negativeInfinity;
       return route[posIndex+1].position;
    }
}
