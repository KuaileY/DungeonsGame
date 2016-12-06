using Entitas;
using Entitas.Unity.Serialization.Blueprints;
using UnityEngine;

public class TestController : MonoBehaviour
{
    Systems _systems;
    void Start ()
	{
	    var pools = Pools.sharedInstance;
	    pools.SetAllPools();

	    _systems = createSystems(pools);
	    _systems.Initialize();
	}
	
	void Update ()
	{
	    _systems.Execute();
	    _systems.Cleanup();
	}

    void OnDestroy()
    {
        _systems.TearDown();
    }

    Systems createSystems(Pools pools)
    {
        return new Feature("Systems")
            //input
            .Add(pools.input.CreateSystem(new TestInputSystem()))
            .Add(pools.input.CreateSystem(new LoadSpritesSystem()))
            .Add(pools.CreateSystem(new TestSystem()))
            //create

            //behavior

            //game logic

            //common
            .Add(pools.CreateSystem(new AddViewSystem()))
            .Add(pools.CreateSystem(new RenderPositionSystem()))
            // Destroy
            .Add(pools.CreateSystem(new DestroyEntitySystem()))

            //Test
            .Add(pools.input.CreateSystem(new WatchDataSystem()))
            ;
    }
}
