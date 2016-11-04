using System.Collections.Generic;
using Entitas;
using NUnit.Framework;
using UnityEngine;

class AddViewSystemTests
{
    [Test]
    public void AddView()
    {
        //given
        var pools = Pools.sharedInstance;
        TestsHelper.setPools(pools);
        var system = (ReactiveSystem) pools.CreateSystem(new AddViewSystem());
        pools.input.CreateEntity().AddHolder(new Dictionary<Res.InPools, UnityEngine.Transform>());
        //when

        (system.subsystem as IInitializeSystem).Initialize();
        //then
        //Assert.IsTrue(pools.input.tick.value == 0);
    }

}

