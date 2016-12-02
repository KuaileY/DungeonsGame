using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Entitas;
using UnityEngine;


public class CreateBackgroundSystem:IReactiveSystem,ISetPools
{
    Pools _pools;
    public TriggerOnEvent trigger { get {return  BoardMatcher.GameBoard.OnEntityAdded(); } }
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }
    public void Execute(List<Entity> entities)
    {
        int floor = _pools.board.gameBoard.floor;
        if (entities.Count > 1)
            throw new Exception("GameBoard greater than 1");
        //需要输出的数据
        var interactive = new XElement("interactive");
        var board = new XElement("floor_" + floor);
        var fov = new XElement("fov");
        var outData = new XElement[3] {board, interactive,fov };
        var backgroundHolder = new GameObject[Res.columns, Res.rows];
        //创建背景
        var watch = new SSTimer("a");
        CreateBackgroundExtension.CreateBackground(_pools,floor,ref outData,ref backgroundHolder);
        watch.Dispose();
        //缓存数据到内存
        var interactiveData = _pools.input.fileList.fileDic[Res.cache.Interactive.ToString()];
        var bg = _pools.input.fileList.fileDic[Res.cache.background.ToString()];
        var fovData = _pools.input.fileList.fileDic[Res.cache.fovData.ToString()];
        fovData.Add(fov);
        bg.Add(board);
        interactiveData.Add(interactive);
        _pools.input.bGHolder.goArray = backgroundHolder;
        //保存数据到文件
        _pools.input.CreateEntity().AddSave(Res.cache.background.ToString(), bg);
        _pools.input.CreateEntity().AddSave(Res.cache.Interactive.ToString(), interactiveData);
        _pools.input.CreateEntity().AddSave(Res.cache.fovData.ToString(), fovData);
        //清除实体
        _pools.board.DestroyEntity(_pools.board.gameBoardEntity);
    }


}

