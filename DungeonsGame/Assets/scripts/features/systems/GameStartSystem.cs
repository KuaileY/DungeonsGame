using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class GameStartSystem:IInitializeSystem,ISetPools
{
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }
    public void Initialize()
    {
        //一些初始化的东西
        //加载文件管理
        _pools.input.CreateEntity().AddFileList(new Dictionary<string, System.Xml.XmlDocument>());
        //加载房间配置
        _pools.input.CreateEntity().AddXML(Res.RoomsXml);
        //创建保持对象
        _pools.input.CreateEntity().AddHolder(new Dictionary<Res.InPools, UnityEngine.Transform>());
        //创建地图
        _pools.board.CreateEntity().AddGameBoard(1).AddPool(Res.InPools.Board);
        initBoard();
    }

    void initBoard()
    {
        GridComponent.Tile[,] grids = new GridComponent.Tile[Res.columns, Res.rows];
        for (int x = 0; x < Res.columns; x++)
        {
            for (int y = 0; y < Res.rows; y++)
            {
                grids[x, y].type = TileType.empty;
            }
        }
        _pools.board.CreateEntity().AddGrid(new List<SingleRoom>(),grids,Res.maps[0],Res.columns,Res.rows);
    }
}

