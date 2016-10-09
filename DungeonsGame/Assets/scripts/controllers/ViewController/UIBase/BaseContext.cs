
public class BaseContext:IBaseContext
{
    public UIType ViewType { get; private set; }

    public BaseContext(UIType viewType)
    {
        ViewType = viewType;
    }

}

