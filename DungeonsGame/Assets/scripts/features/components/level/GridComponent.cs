using System.Collections.Generic;
using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Board,SingleEntity]
public class GridComponent:IComponent
{
    public List<SingleRoom> rooms;
    public Tile[,] grids;
    public string name;
    public int width;
    public int height;
    public struct Tile
    {
        public int roomID;
        public int roomX;
        public int roomY;
        public TileType type;
    }
}

