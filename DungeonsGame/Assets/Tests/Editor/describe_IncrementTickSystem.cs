using Entitas;
using NSpec;

class describe_IncrementTickSystem : nspec
{
    void when_executing()
    {
        Pools pools = Pools.sharedInstance;
        pools.input = TestsHelper.CreateInputPool();
        IncrementTickSystem system = null;


        before = () =>
        {
            system = (IncrementTickSystem)pools.input.CreateSystem(new IncrementTickSystem());
        };

        it["initialize tick value should be 0"] = () =>
        {
            //given
            //when
            system.Initialize();
            //then
            pools.input.tick.value.should_be(0);
        };

        it["execute tick value should be greater 0 "] = () =>
        {
            //given

            //when
            system.Execute();
            //then
            pools.input.tick.value.should_be_greater_than(0);
        };
    }
}

