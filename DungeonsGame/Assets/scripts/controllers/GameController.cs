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
            //create
            .Add(pools.CreateSystem(new UISystem()))
            .Add(pools.board.CreateSystem(new GameBoardSystem()))
            .Add(pools.CreateSystem(new GameStartSystem()))
            ;
    }
}
