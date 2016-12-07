using Entitas;
using Entitas.Unity.Serialization.Blueprints;
using UnityEngine;

public class GameController : MonoBehaviour
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
            .Add(pools.input.CreateSystem(new InputSystem()))
            .Add(pools.input.CreateSystem(new ProcessInputSystem()))
            .Add(pools.input.CreateSystem(new IncrementTickSystem()))

            .Add(pools.input.CreateSystem(new LoadSpritesSystem()))
            .Add(pools.input.CreateSystem(new GameLoadSystem()))
            .Add(pools.input.CreateSystem(new GameSaveSystem()))
            //create
            //.Add(pools.CreateSystem(new UISystem()))
            .Add(pools.board.CreateSystem(new CreateBackgroundSystem()))
            .Add(pools.CreateSystem(new DungeonItemsCacheSystem()))
            //.Add(pools.board.CreateSystem(new LoadBoardSystem()))
            .Add(pools.core.CreateSystem(new CreateInteractiveObjSystem()))
            //behavior
            .Add(pools.input.CreateSystem(new TurnSystem()))
            .Add(pools.core.CreateSystem(new MoveSystem()))
            //game logic
            .Add(pools.input.CreateSystem(new GameStartSystem()))
            //common
            .Add(pools.CreateSystem(new AddViewSystem()))
            .Add(pools.core.CreateSystem(new AddViewFromObjectPoolSystem()))
            .Add(pools.CreateSystem(new RenderPositionSystem()))
            .Add(pools.input.CreateSystem(new FovSystem()))
            .Add(pools.core.CreateSystem(new MainLightSystem()))
            // Destroy
            .Add(pools.CreateSystem(new DestroyEntitySystem()))

            //Test
            .Add(pools.CreateSystem(new TestSystem()))
            .Add(pools.input.CreateSystem(new WatchDataSystem()))
            ;
    }
}
