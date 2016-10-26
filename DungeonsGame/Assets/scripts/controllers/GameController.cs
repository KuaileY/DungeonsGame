using Entitas;
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

            .Add(pools.input.CreateSystem(new LoadXMLSystem()))
            .Add(pools.input.CreateSystem(new LoadSpritesSystem()))
            .Add(pools.input.CreateSystem(new GameLoadSystem()))
            .Add(pools.input.CreateSystem(new GameSaveSystem()))
            //create
            //.Add(pools.CreateSystem(new UISystem()))
            .Add(pools.board.CreateSystem(new CreateBoardSystem()))
            .Add(pools.board.CreateSystem(new LoadBoardSystem()))
            //game logic
            .Add(pools.input.CreateSystem(new GameStartSystem()))
            //common
            .Add(pools.CreateSystem(new AddViewSystem()))
            .Add(pools.CreateSystem(new RenderPositionSystem()))
            // Destroy
            .Add(pools.CreateSystem(new DestroyEntitySystem()))
            ;
    }
}
