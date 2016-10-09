using System.Collections.Generic;
using Entitas;

public class GameBoardSystem : IReactiveSystem, ISetPools
{
    public TriggerOnEvent trigger { get { return BoardMatcher.GameBoard.OnEntityAdded(); } }
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }

    public void Execute(List<Entity> entities)
    {
        TestLoadConfig.log.Trace("GameBoardSystem Execute");
        int floor = _pools.board.gameBoard.floor;
        _pools.board.DestroyEntity(_pools.board.gameBoardEntity);
        createBoard(floor);
    }

    void createBoard(int floor)
    {
        TestLoadConfig.log.Trace("gameBoard floor:" + floor);
    }
}

