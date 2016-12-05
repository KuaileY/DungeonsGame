using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Entitas;
using UnityEngine;

public sealed class FovSystem:ISetPools,IReactiveSystem
{
    public TriggerOnEvent trigger { get {return InputMatcher.Fov.OnEntityAdded(); } }
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }

    public void Execute(List<Entity> entities)
    {
        var playerX = (int)_pools.core.controlableEntity.position.value.x;
        var playerY = (int)_pools.core.controlableEntity.position.value.y;

        var MapString = _pools.input.fileList.fileDic[Res.cache.fovData.ToString()].Element("fov").Value;
        var lines = MapString.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        var map = new char[Res.columns, Res.rows];
        for (int y = 0; y < Res.columns; y++)
            for (int x = 0; x < Res.rows; x++)
                map[x,y]= lines[Res.rows - y - 1][x];


        var str=RenderToString(playerX, playerY, map);
        var xdoc = new XDocument();
        xdoc.Add(new XElement("data", str));
        _pools.input.CreateEntity().AddSave("guagua", xdoc);
        foreach (var entity in entities )
        {
            _pools.input.DestroyEntity(entity);
        }
    }

    private string RenderToString(int playerX,int playerY,char[,] map)
    {
        bool[,] lit = new bool[Res.columns, Res.rows];
        int radius = 9;
        ShadowCaster.ComputeFieldOfViewWithShadowCasting(
            playerX, playerY, radius,
            (x1, y1) => map[x1, y1] == Res.TileTypeChar[(int)TileType.obstacle],
            (x2, y2) => { lit[x2, y2] = true; });
        var sb = new StringBuilder();
        for (int y = Res.rows - 1; y >= 0; --y)
        {
            for (int x = 0; x < Res.columns; ++x)
            {
                if (lit[x, y])
                {
                    if (x == playerX && y == playerY)
                        sb.Append("P");
                    else
                        sb.Append(map[x, y]);
                    if (_pools.input.bGHolder.goArray[x, y] != null)
                        _pools.input.bGHolder.goArray[x, y].GetComponent<Renderer>().enabled = true;

                }
                else
                    sb.Append(' ');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }


}

