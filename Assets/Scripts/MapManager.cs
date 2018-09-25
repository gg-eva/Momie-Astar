using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Astar;

public class MapManager : MonoBehaviour
{

    public int labSize = 5;     //Number of tiles of one side
    public Trace traceRef;      //Trace the shortest path
    public Tile wallRef;        //Wall
    public Tile safeRef;        //Empty tile
    public Tile[] tilesPrefab;  //Each possible tile from assets
    public Player playerRef;    //Player
    public Enemy enemyRef;      //Enemy
    public Skull gameOverSkull; //GameOver display

    public Text playerTurnsText;
    public Text enemyTurnsText;

    //Size info
    float wallSize;
    float playerScale;
    float enemyScale;

    //Instantiated tile map
    TileMap tileMap;

    //Instantiate
    Player player;
    Enemy enemy;
    Trace trace;
    Skull gameOver;

    //For gameManagement
    [HideInInspector] public bool isGameOver = true;

    //For movement
    TileMapNav tileMapNavEnemy;

    //Before any start
    private void Awake()
    {
        //Initialize useful variables
        wallSize = wallRef.GetComponent<SpriteRenderer>().bounds.size.x / labSize;

        playerScale = playerRef.GetComponent<Transform>().localScale.x / labSize;
        enemyScale = enemyRef.GetComponent<Transform>().localScale.x / labSize;

        playerTurnsText.text = "";
        enemyTurnsText.text = "";
    }

    public void CleanMap()
    {
        //Instantiated tile map
        //Destroy objects
        foreach (Tile tile in tileMap.map)
            Destroy(tile.gameObject);

        //Empty list
        tileMap.map.Clear();

        //Instantiated player, enemy and trace
        if (player != null)
            Destroy(player.gameObject);
        if (enemy != null)
            Destroy(enemy.gameObject);
        if (trace != null)
            Destroy(trace.gameObject);
        if (gameOver != null)
            Destroy(gameOver.gameObject);
    }

    //Initialize a random map
    public void InitializeRandomMap()
    {
        //Player
        Vector2Int playerCoord = new Vector2Int(1, 1);
        Vector3 playerPos = GetPosFromCoordinates(playerCoord);
        player = Instantiate(playerRef, playerPos, Quaternion.identity);
        player.GetComponent<Transform>().localScale = new Vector3(playerScale, playerScale);
        player.coord = playerCoord;
        player.moveAttempt = playerCoord;

        //Enemy
        Vector2Int enemyCoord = new Vector2Int(labSize - 2, labSize - 2);
        Vector3 enemyPos = GetPosFromCoordinates(enemyCoord);
        enemy = Instantiate(enemyRef, enemyPos, Quaternion.identity);
        enemy.GetComponent<Transform>().localScale = new Vector3(enemyScale, enemyScale);
        enemy.coord = enemyCoord;

        List<Tile> tiles = new List<Tile>();
        //Choosing each tile type for each place in the map
        for (int y = 0; y < labSize; ++y)
        {
            for (int x = 0; x < labSize; ++x)
            {
                Tile tileRef;
                Vector2Int tileCoord = new Vector2Int(x,y);

                if ((x == 0) || (x == labSize - 1) || (y == 0) || (y == labSize - 1))
                    tileRef = wallRef;
                else if (tileCoord == enemyCoord || tileCoord == playerCoord)
                    tileRef = safeRef;
                else
                    tileRef = FindRandomTile();

                //Instantiating and resizing
                //Tile
                Vector3 pos = GetPosFromCoordinates(tileCoord);
                Tile tile = Instantiate(tileRef, pos, Quaternion.identity);

                Transform tileTransform = tile.GetComponent<Transform>();
                tileTransform.localScale = tileTransform.localScale / labSize;

                //Add to list
                tiles.Add(tile);
            }
        }

        tileMap = new TileMap(tiles, labSize, labSize);

        //Nav info for the enemy
        tileMapNavEnemy = new TileMapNav(tileMap, tileMap.TileFromCoordinates(enemy.coord), tileMap.TileFromCoordinates(playerCoord));
        tileMapNavEnemy.InitAstar();

        //Trace
        trace = Instantiate(traceRef);
        UpdateTrace();

        //Game Over info
        isGameOver = false;
    }


    public void UpdateNavEnemy()
    {
        //TEMP
        tileMapNavEnemy.start = tileMap.TileFromCoordinates(enemy.coord);
        tileMapNavEnemy.target = tileMap.TileFromCoordinates(player.coord);
        tileMapNavEnemy.InitAstar();

        //Update trace
        UpdateTrace();
    }

    public void UpdateTrace()
    {
        List<Vector3> pathPos = new List<Vector3>();
        foreach (Vector2Int coord in tileMapNavEnemy.PathCoord())
            pathPos.Add(GetPosFromCoordinates(coord));
        trace.SetPath(pathPos);
    }


    Vector3 GetPosFromCoordinates(Vector2Int coord)
    {
        float xpos = (-labSize / 2 + coord.x) * wallSize;
        float ypos = (-labSize / 2 + coord.y) * wallSize;

        return new Vector3(xpos, ypos, 0);
    }

    Tile FindRandomTile()
    {
        int tileChoice = Random.Range(0, tilesPrefab.Length);
        Tile tile = tilesPrefab[tileChoice];
        return tile;
    }

    private bool TryMovePlayer()
    {
        bool moved = false;

        if (player.moveAttempt != player.coord 
            && player.moveAttempt.x >= 0 
            && player.moveAttempt.x < labSize
            && player.moveAttempt.y >= 0
            && player.moveAttempt.y < labSize)
        {
            Tile playerTile = tileMap.TileFromCoordinates(player.coord);
            Tile attemptTile = tileMap.TileFromCoordinates(player.moveAttempt);

            if(!attemptTile.obstacle && tileMap.SurroundingTiles(playerTile).Contains(attemptTile))
            {
                player.coord = player.moveAttempt;
                player.turnsToWait = attemptTile.tileCost + 1;

                player.transform.position = GetPosFromCoordinates(player.coord);
                moved = true;
            }
        }

        return moved;
    }

    private bool TryMoveEnemy()
    {
        bool moved = false;
        if (tileMapNavEnemy.PathFound())
        {
            Tile enemyTile = tileMap.TileFromCoordinates(enemy.coord);
            Tile attemptTile = tileMapNavEnemy.NextCoord();

            if(enemyTile != attemptTile)
            { 
                enemy.coord = tileMap.CoordinatesFromTile(attemptTile);
                enemy.turnsToWait = attemptTile.tileCost + 1;

                enemy.transform.position = GetPosFromCoordinates(enemy.coord);
                moved = true;
            }
        }
        return moved;
    }

    public void NewTurn()
    {
        if (player.turnsToWait <= 0)
        {
            if (TryMovePlayer())
            {
                UpdateNavEnemy();
            }
        }
        else
        {
            player.turnsToWait--;
            enemy.turnsToWait--;
            if (enemy.turnsToWait <= 0)
            {
                TryMoveEnemy();
                UpdateNavEnemy();
            }
        }

        playerTurnsText.text = "Player : " + player.turnsToWait;
        enemyTurnsText.text = "Enemy : " + enemy.turnsToWait;

        if (enemy.coord == player.coord)
            GameOver();
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOver = Instantiate(gameOverSkull);

        playerTurnsText.text = "";
        enemyTurnsText.text = "";
    }
}