using Entitas;
using UnityEngine;
public abstract class BaseView:MonoBehaviour,IBaseView
{
    public void Link(Entity entity, Pool pool)
    {
        gameObject.Link(entity, pool);
    }

    public virtual void OnPause(IBaseContext curContext)
    {
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public virtual void OnEnter(IBaseContext nextContext)
    {
        gameObject.SetActive(true);
    }

    public virtual void OnExit(IBaseContext curContext)
    {
        gameObject.SetActive(false);
    }

    public virtual void OnResume(IBaseContext lastContext)
    {
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}

