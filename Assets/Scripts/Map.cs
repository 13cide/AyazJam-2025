using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] List<Transform> route;

    internal Vector2 GetNextPosFrom(int posIndex)
    {
        if (posIndex >= route.Count - 1) 
            return new Vector2(-1488, 0f);
        return route[posIndex + 1].position;
    }
}
