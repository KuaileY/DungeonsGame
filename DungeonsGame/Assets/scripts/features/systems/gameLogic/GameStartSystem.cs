using System;
using Entitas;
using System.Collections.Generic;
using System.Linq;
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
        BuildFileList();
        //加载配置文件
        loadConfig();
        //创建保持对象
        _pools.input.CreateEntity().AddHolder(new Dictionary<Res.InPools, Transform>());
        //创建背景持有对象
        _pools.input.CreateEntity().AddBGHolder(new GameObject[Res.columns, Res.rows]);
        //创建对象池
        _pools.input.CreateEntity().AddViewObjectPool(CreateObjectPoolExtension.createObjectPool());
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
            _pools.board.CreateEntity().AddGameBoard(1).AddPool(Res.InPools.Board);
            //创建交互对象
            _pools.core.CreateEntity().AddDungeon(1);
        }
        entities.SingleEntity().IsDestroy(true);

        //检查数据正确否
        _pools.input.CreateEntity().IsWatch(true);
    }

    void loadConfig()
    {
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

    void BuildFileList()
    {
        var fileList = new Dictionary<string, XDocument>();
        //当前层背景数据
        fileList.Add(Res.cache.background.ToString(), new XDocument());
        //当前层交互对象数据
        fileList.Add(Res.cache.Interactive.ToString(), new XDocument());
        //当前层玩家视野数据
        fileList.Add(Res.cache.fovData.ToString(), new XDocument());
        //加载文件管理
        _pools.input.CreateEntity().AddFileList(fileList);
    }
}

