using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public MapManager mapManager;

    [HideInInspector] public Vector2Int coord;
    [HideInInspector] public Vector2Int moveAttempt;
    [HideInInspector] public int turnsToWait;

    private void Awake()
    {
        //mapManager = GameObject.Find("GameManager").GetComponent<MapManager>();
        turnsToWait = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            if (Input.GetKey(KeyCode.LeftArrow))
                moveAttempt = coord + Vector2Int.up + Vector2Int.left;
            else if (Input.GetKey(KeyCode.RightArrow))
                moveAttempt = coord + Vector2Int.up + Vector2Int.right;
            else
                moveAttempt = coord + Vector2Int.up;

        else if (Input.GetKeyDown(KeyCode.DownArrow))
            if (Input.GetKey(KeyCode.LeftArrow))
                moveAttempt = coord + Vector2Int.down + Vector2Int.left;
            else if (Input.GetKey(KeyCode.RightArrow))
                moveAttempt = coord + Vector2Int.down + Vector2Int.right;
            else
            moveAttempt = coord + Vector2Int.down;

        else if (Input.GetKeyDown(KeyCode.RightArrow))
            if (Input.GetKey(KeyCode.UpArrow))
                moveAttempt = coord + Vector2Int.right + Vector2Int.up;
            else if (Input.GetKey(KeyCode.DownArrow))
                moveAttempt = coord + Vector2Int.right + Vector2Int.down;
            else
                moveAttempt = coord + Vector2Int.right;

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            if (Input.GetKey(KeyCode.UpArrow))
                moveAttempt = coord + Vector2Int.left + Vector2Int.up;
            else if (Input.GetKey(KeyCode.DownArrow))
                moveAttempt = coord + Vector2Int.left + Vector2Int.down;
            else
                moveAttempt = coord + Vector2Int.left;
    }
}
