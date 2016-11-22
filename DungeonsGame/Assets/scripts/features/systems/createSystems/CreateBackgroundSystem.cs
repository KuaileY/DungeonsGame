using System.Collections.Generic;
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
        _pools.board.DestroyEntity(_pools.board.gameBoardEntity);
        List<Tables.Background> backgroundData = CreateBackgroundExtension.Read(floor,_pools.input.runtimeData.db);
        CreateBackgroundExtension.CreateBackground(backgroundData,_pools,floor);
    }


}

