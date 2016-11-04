using Entitas;
public static class TestsHelper
{
    public static Pool CreateCorePool()
    {
        return new Pool(CoreComponentIds.TotalComponents, 0, new PoolMetaData("Core", CoreComponentIds.componentNames, CoreComponentIds.componentTypes));
    }

    public static Pool CreateInputPool()
    {
        return new Pool(InputComponentIds.TotalComponents, 0, new PoolMetaData("Input", InputComponentIds.componentNames, InputComponentIds.componentTypes));
    }

    public static Pool CreateBoardPool()
    {
        return new Pool(BoardComponentIds.TotalComponents, 0, new PoolMetaData("Board", BoardComponentIds.componentNames, BoardComponentIds.componentTypes));
    }

    public static void setPools(Pools pools)
    {
        pools.board = CreateBoardPool();
        pools.core = CreateCorePool();
        pools.input = CreateInputPool();
    }
}

