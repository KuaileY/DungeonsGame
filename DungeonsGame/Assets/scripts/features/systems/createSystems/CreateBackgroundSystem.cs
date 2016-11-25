using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Entitas;


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
        var board = new XElement("floor_" + floor.ToString());
        var outData = new XElement[2] {board, interactive };
        //创建背景
        CreateBackgroundExtension.CreateBackground(_pools,floor,ref outData);
        //缓存数据到内存
        var interactiveData = _pools.input.fileList.fileDic[Res.cache.Interactive.ToString()];
        var bg = _pools.input.fileList.fileDic[Res.cache.background.ToString()];
        bg.Add(board);
        interactiveData.Add(interactive);
        //保存数据到文件
        _pools.input.CreateEntity().AddSave(Res.cache.background.ToString(), bg);
        _pools.input.CreateEntity().AddSave(Res.cache.Interactive.ToString(), interactiveData);
        //清除实体
        _pools.board.DestroyEntity(_pools.board.gameBoardEntity);
    }


}

