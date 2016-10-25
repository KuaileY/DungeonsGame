
using System;
using Entitas;

public static class PoolExtension
{
    public static Pool GetPool(Res.InPools name)
    {
        foreach (var pool in Pools.sharedInstance.allPools)
        {
            if (pool.ToString() == name.ToString())
                return pool;
        }
        throw new Exception("Pool is null!");
    }
}
