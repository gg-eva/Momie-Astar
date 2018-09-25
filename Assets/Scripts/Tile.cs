using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour{

    public bool obstacle;
    public int tileCost;

    public Tile (bool obstacle, int tileCost)
    {
        this.tileCost = tileCost;
        this.obstacle = obstacle;
    }

    public override string ToString()
    {
        if (obstacle)
            return " X";
        else
            return "" + tileCost.ToString("D2");
    }
}
