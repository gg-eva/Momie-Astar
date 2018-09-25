using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Astar;

namespace AstarTest
{
    public class TestAstar : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            //TEST1
            int X = 4;
            int Y = 3;
            int start = 0;
            int target = X * Y - 1;

            List<Tile> tileList = new List<Tile>(new Tile[] {
                new Tile(false, 0), new Tile(false, 0), new Tile(false, 1), new Tile(false, 1),
                new Tile(false, 0), new Tile(true,  0), new Tile(true,  0), new Tile(false, 0),
                new Tile(false, 0), new Tile(false, 0), new Tile(true,  0), new Tile(false, 0)
            });

            TileMap tileMap = new TileMap(tileList, X, Y);
            TileMapNav tileMapNav = new TileMapNav(tileMap, tileList[start], tileList[target]);
            tileMapNav.InitAstar();

            Debug.Log("TEST 1");
            Debug.Log("MAP");
            Debug.Log(tileMap);
            Debug.Log("START : " + start);
            Debug.Log("TARGET : " + target);
            Debug.Log("PATH");
            Debug.Log(tileMapNav.PathToString());
            Debug.Log("DISTANCE MAP");
            Debug.Log(tileMapNav.MapWithCostToString());


            //TEST2
            X = 10;
            Y = 10;
            start = 5 * X + 4;
            target = X + 6;

            tileList = new List<Tile>(new Tile[] {
                new Tile(true,  0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0),
                new Tile(true,  0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0),
                new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0),
                new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(true,  0), new Tile(true,  0), new Tile(true,  0), new Tile(true,  0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0),
                new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0),
                new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0),
                new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0),
                new Tile(true,  0), new Tile(true,  0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0),
                new Tile(true,  0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0),
                new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(true,  0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0),
            });

            tileMap = new TileMap(tileList, X, Y);
            tileMapNav = new TileMapNav(tileMap, tileList[start], tileList[target]);
            tileMapNav.InitAstar();

            Debug.Log("TEST 2");
            Debug.Log("MAP");
            Debug.Log(tileMap);
            Debug.Log("START : " + start);
            Debug.Log("TARGET : " + target);
            Debug.Log("PATH");
            Debug.Log(tileMapNav.PathToString());
            Debug.Log("DISTANCE MAP");
            Debug.Log(tileMapNav.MapWithCostToString());


            //TEST3
            X = 10;
            Y = 10;
            start = 5 * X + 4;
            target = X + 6;

            tileList = new List<Tile>(new Tile[] {
                new Tile(true,  0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0),
                new Tile(true,  0), new Tile(false, 1), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0),
                new Tile(false, 1), new Tile(false, 1), new Tile(false, 1), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0),
                new Tile(false, 1), new Tile(false, 1), new Tile(false, 1), new Tile(true,  0), new Tile(true,  0), new Tile(true,  0), new Tile(true,  0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0),
                new Tile(false, 1), new Tile(false, 1), new Tile(false, 1), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0),
                new Tile(false, 1), new Tile(false, 1), new Tile(false, 1), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0),
                new Tile(false, 4), new Tile(false, 4), new Tile(false, 4), new Tile(false, 4), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0),
                new Tile(true,  0), new Tile(true,  0), new Tile(true,  0), new Tile(false, 4), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0),
                new Tile(true,  0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 4), new Tile(false, 4), new Tile(false, 4), new Tile(false, 4), new Tile(false, 4), new Tile(false, 4), new Tile(false, 4),
                new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(false, 0), new Tile(true,  0), new Tile(true,  0), new Tile(false, 0), new Tile(false, 0),
            });

            tileMap = new TileMap(tileList, X, Y);
            tileMapNav = new TileMapNav(tileMap, tileList[start], tileList[target]);
            tileMapNav.InitAstar();

            Debug.Log("TEST 3");
            Debug.Log("MAP");
            Debug.Log(tileMap);
            Debug.Log("START : " + start);
            Debug.Log("TARGET : " + target);
            Debug.Log("PATH");
            Debug.Log(tileMapNav.PathToString());
            Debug.Log("DISTANCE MAP");
            Debug.Log(tileMapNav.MapWithCostToString());
        }
    }
}
