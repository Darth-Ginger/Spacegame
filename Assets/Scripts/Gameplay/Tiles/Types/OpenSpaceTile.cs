using System;
using UnityEngine;
using static Board;

[System.Serializable]
public class OpenSpaceTile : Tile
{

    
    public OpenSpaceTile() : base(TileType.OpenSpace, null) 
    {
        this.CostModifier = 0.5;
    }

    public override bool CanWalk() => true;
    public override double GetCostModifier(){ return this.CostModifier; }


}
