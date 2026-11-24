using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolableObject : MonoBehaviour
{
    private IObjectPool<PoolableObject> pool;
    public ulong UseId { get; private set; }

    internal void SetPool(IObjectPool<PoolableObject> pool)
    {
        this.pool = pool;
        UseId++;
    }

    protected void ReleaseToPool()
    {
        pool.Release(this);
    }
}
