﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [HideInInspector] public Vector2Int coord;
    [HideInInspector] public int turnsToWait;

    private void Awake()
    {
        turnsToWait = 0;
    }
}
