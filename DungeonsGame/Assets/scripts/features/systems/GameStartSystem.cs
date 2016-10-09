using Entitas;
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
        //创建地图
        _pools.board.CreateEntity().AddGameBoard(1);
    }


}

