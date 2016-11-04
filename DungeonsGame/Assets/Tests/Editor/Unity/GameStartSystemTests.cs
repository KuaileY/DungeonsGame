using Entitas;
using NUnit.Framework;
using UnityEngine;
class GameStartSystemTests
{
    [Test]
    public void game_init()
    {
        //given
        var pools = Pools.sharedInstance;
        TestsHelper.setPools(pools);

        var system = (ReactiveSystem)pools.input.CreateSystem(new GameStartSystem());
        //when

        (system.subsystem as IInitializeSystem).Initialize();
        //then
        Assert.IsNotNull(pools.input.holder);
    }
}

