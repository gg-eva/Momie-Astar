using System.Collections.Generic;
using UnityEngine;

namespace Astar
{
   
    public class TileMap
    {
        //Origin at letf bottom (0,0)
        public readonly int X;
        public readonly int Y;
        public readonly int size;

        public List<Tile> map;

        public TileMap(List<Tile> map, int X, int Y)
        {
            if (map.Count != X * Y)
                Debug.LogWarning("Wrong parameter for tile map");

            this.X = X;
            this.Y = Y;
            this.size = X * Y;
            this.map = map;
        }

        public Tile TileFromIndex(int index)
        {
            return map[index];
        }

        public Tile TileFromCoordinates(Vector2Int coord)
        {
            return map[IndexFromCoordinates(coord)];
        }

        public Vector2Int CoordinatesFromTile(Tile tile)
        {
            int index = map.IndexOf(tile);
            return CoordinatesFromIndex(index);
        }

        public int IndexOfTile(Tile tile)
        {
            return map.IndexOf(tile);
        }

        public int IndexFromCoordinates(Vector2Int coord)
        {
            return coord.y * X + coord.x;
        }

        public Vector2Int CoordinatesFromIndex(int index)
        {
            int x = index % X;
            int y = (index - x) / Y;
            return new Vector2Int(x, y);
        }

        public List<Tile> SurroundingTiles(Tile tile)
        {
            //8 directions
            //Cannot go in diagonal if obstacle obstructing angle
            List<Tile> surroundingTiles = new List<Tile>();

            int index = map.IndexOf(tile);
            Vector2Int coord = CoordinatesFromIndex(index);

            bool left, right, down, up;

            up = coord.y > 0;
            if (up)
                surroundingTiles.Add(map[index - X]);

            down = coord.y < Y - 1;
            if (down)
                surroundingTiles.Add(map[index + X]);

            right = coord.x < X - 1;
            if (right)
                surroundingTiles.Add(map[index + 1]);

            left = coord.x > 0;
            if (left)
                surroundingTiles.Add(map[index - 1]);

            if (up && right)
                if (!map[index - X].obstacle && !map[index + 1].obstacle)
                    surroundingTiles.Add(map[index - X + 1]);

            if (up && left)
                if (!map[index - X].obstacle && !map[index - 1].obstacle)
                    surroundingTiles.Add(map[index - X - 1]);

            if (down && right)
                if (!map[index + X].obstacle && !map[index + 1].obstacle)
                    surroundingTiles.Add(map[index + X + 1]);

            if (down && left)
                if (!map[index + X].obstacle && !map[index - 1].obstacle)
                    surroundingTiles.Add(map[index + X - 1]);

            return surroundingTiles;
        }

        public int EstimateDistance(Tile start, Tile target)
        {
            Vector2Int coordStart = CoordinatesFromTile(start);
            Vector2Int coordTarget = CoordinatesFromTile(target);
            return Mathf.Max(Mathf.Abs(coordStart.x - coordTarget.x), Mathf.Abs(coordStart.y - coordTarget.y));
        }

        public override string ToString()
        {
            string res = "";
            for (int y = Y - 1; y >= 0; --y)
            {
                for (int x = 0; x < X; ++x)
                {
                    int index = y * X + x;
                    res += map[index] + " ";
                }
                res += "\n";
            }
            return res;
        }
    }

    public class TileMapNav
    {
        TileMap map;

        public Tile start;
        public Tile target;

        //Vector3Int x: estimated cost, y: distance to get there, z: index of previous
        public Dictionary<Tile, Vector3Int> open  = new Dictionary<Tile, Vector3Int>();
        public Dictionary<Tile, Vector3Int> close = new Dictionary<Tile, Vector3Int>();

        public List<Tile> path = new List<Tile>();

        public TileMapNav(TileMap map, Tile start, Tile target)
        {
            this.map = map;
            this.start = start;
            this.target = target;
        }

        public void InitAstar()
        {
            open = new Dictionary<Tile, Vector3Int>();
            close = new Dictionary<Tile, Vector3Int>();

            //Init - add start tile to open and set targetFound to false
            open.Add(start, new Vector3Int(map.EstimateDistance(start, target), 0, map.IndexOfTile(start)));
            bool targetFound = false;

            while (open.Count != 0 && !targetFound)
            {
                //Find the next tile to examine - with random selection of tile with min cost
                List<Tile> currentTiles = new List<Tile>();
                int currentCost = int.MaxValue;

                foreach (Tile tile in open.Keys)
                {
                    //Get tiles with minimal cost
                    if (open[tile].x < currentCost)
                    {
                        currentTiles.Clear();
                        currentTiles.Add(tile);
                        currentCost = open[tile].x;
                    }
                    else if (open[tile].x == currentCost)
                    {
                        currentTiles.Add(tile);
                    }
                }

                //Target found?
                targetFound = currentTiles.Contains(target);
                Tile currentTile;

                //Target found
                if (targetFound)
                    currentTile = target;
                //Target not found
                else
                {
                    //Random tile
                    currentTile = currentTiles[Random.Range(0, currentTiles.Count)];
                    int currentDist = open[currentTile].y;

                    //Get surrounding tiles
                    List <Tile> surroundingTiles = map.SurroundingTiles(currentTile);
                    foreach (Tile tile in surroundingTiles)
                    {
                        if (!tile.obstacle && !open.ContainsKey(tile) && !close.ContainsKey(tile))
                        {
                            int estimatedDistance = map.EstimateDistance(tile, target);

                            //Total
                            int tileDist = currentDist + tile.tileCost + 1; //+ 1 for one move
                            
                            //Total estimated cost
                            int tileCost = tileDist + estimatedDistance;

                            //Add to tiles to check
                            open.Add(tile, new Vector3Int(tileCost, tileDist, map.IndexOfTile(currentTile)));
                        }
                    }
                }

                //Current tile has been checked
                close.Add(currentTile, open[currentTile]);
                open.Remove(currentTile);
            }

            path = Path();
        }

        List<Tile> Path()
        {
            List<Tile> path = new List<Tile>();
            
            if(PathFound())
            {
                //Trace back path
                Tile tile = target;
                path.Add(tile);

                while (tile != start)
                {
                    tile = map.map[close[tile].z];
                    path.Add(tile);
                }

                //Putting in the right order
                path.Reverse();
            }

            return path;
        }

        public bool PathFound()
        {
            return close.ContainsKey(target);
        }

        public List<Vector2Int> PathCoord()
        {
            List<Vector2Int> pathCoord = new List<Vector2Int>();
            foreach (Tile tile in path)
                pathCoord.Add(map.CoordinatesFromTile(tile));
            return pathCoord;
        }

        public Tile NextCoord()
        {
            Tile next;
            if (path.Count >= 2)
                next = path[1];
            else
                next = start;
 
            return next;
        }

        public string PathToString()
        {
            string res = "";

            if (!PathFound())
                res = "No path found";
            else
            {
                res += map.IndexOfTile(target);
                Tile tile = target;

                while(tile != start)
                {
                    tile = map.map[close[tile].z];
                    res = map.IndexOfTile(tile) + " -> " + res;
                }
                res = "START: " + res + " :TARGET";
            }
            return res;
        }

        public string MapWithCostToString()
        {
            string res = "";
            for (int y = map.Y - 1; y >=0 ; --y) 
            {
                for (int x = 0; x < map.X; ++x)
                {
                    int index = y * map.X + x;
                    Tile tile = map.map[index];
                    if (close.ContainsKey(tile))
                        res += close[tile].x.ToString("D2");
                    else if (open.ContainsKey(tile))
                        res += open[tile].x.ToString("D2");
                    else
                    {
                        if(tile.obstacle)
                            res += tile;
                        else
                            res += " ?";
                    }
                        
                    res += " ";
                }
                res += "\n";
            }
            return res;
        }
    }
}