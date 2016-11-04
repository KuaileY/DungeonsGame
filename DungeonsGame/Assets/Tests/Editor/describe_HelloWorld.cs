using NSpec;
class describe_ObjectPool:nspec
{
    void when_object_pooling()
    {

        it["hello world shoule be hello world"] = () => "hello".should_be("hello");

    }
}

