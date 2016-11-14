using System;
using Entitas;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using SQLite4Unity3d;

public class GameStartSystem:IInitializeSystem,ISetPools,IReactiveSystem
{
    public TriggerOnEvent trigger { get { return Matcher.AnyOf(InputMatcher.NewGame,InputMatcher.LoadGame).OnEntityAdded(); } }
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }
    public void Initialize()
    {
        //一些初始化的东西
        //加载配置文件
        loadConfig();
        //创建保持对象
        _pools.input.CreateEntity().AddHolder(new Dictionary<Res.InPools, UnityEngine.Transform>());
        //建立摄像机
        var camera = GameObject.FindObjectOfType<Camera>();
        _pools.core.CreateEntity()
            .IsCamera(true)
            .AddView(camera.GetComponent<ViewController>());

    }

    public void Execute(List<Entity> entities)
    {
        if (entities.SingleEntity().isLoadGame)
        {
            Debug.Log("load game");
            _pools.input.CreateEntity().IsLoad(true);
        }
        if (entities.SingleEntity().isNewGame)
        {
            Debug.Log("new game");
            //创建地图
            _pools.board.CreateEntity().AddGameBoard(3).AddPool(Res.InPools.Board);
            //_pools.core.CreateEntity().AddDungeon(1);
        }
        entities.SingleEntity().IsDestroy(true);

        //检查数据正确否
        _pools.input.CreateEntity().IsWatch(true);
    }

    void loadConfig()
    {
        //加载文件管理
        _pools.input.CreateEntity().AddFileList(new Dictionary<string, XDocument>());
        //创建连接
        var dbPath = Res.PathURL + Res.dbName;
        var connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        //添加连接
        _pools.input.CreateEntity().AddRuntimeData(connection);
        //把配置放入数据库
        foreach (var item in Enum.GetValues(typeof(Res.configs)))
        {
            string path = Res.configPath + item + Res.xlsxExtension;
            ExcelExtension.readExcel(path,_pools.input, connection);
        }


    }
}

