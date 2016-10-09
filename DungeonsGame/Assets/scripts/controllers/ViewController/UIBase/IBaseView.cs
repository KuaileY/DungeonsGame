using Entitas;
using UnityEngine;

public interface IBaseView
{
    GameObject gameObject { get; }
    void Link(Entity entity, Pool pool);
    //载入
    void OnEnter(IBaseContext nextContext);
    //退出
    void OnExit(IBaseContext curContext);
    //暂停
    void OnPause(IBaseContext curContext);
    //恢复
    void OnResume(IBaseContext lastContext);
}

