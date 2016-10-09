
public class GameStartContext : BaseContext
{
    public GameStartContext() : base(Res.GameStart)
    {
    }
}
public class GameStartView : BaseView
{
    public void newGameCallBack()
    {
        Singleton<ContextManager>.Instance.Pop();
    }
}

