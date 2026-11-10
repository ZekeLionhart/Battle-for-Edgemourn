using UnityEngine;
using UnityEngine.Pool;

public sealed class ProjectilePool : MonoBehaviour
{
    private const int MaxInactiveArrows = 50;

    [SerializeField] private ArrowManager arrowPrefab;

    private ObjectPool<ArrowManager> arrowPool;
    private Transform inactiveArrows;

    private void Awake()
    {
        GameObject container = new("Inactive Arrows"); // string
        container.SetActive(false);
        container.transform.SetParent(transform, false);
        inactiveArrows = container.transform;

        arrowPool = new ObjectPool<ArrowManager>(
            createFunc: CreateArrow,
            actionOnRelease: StoreArrow,
            actionOnDestroy: arrow => Destroy(arrow.gameObject),
            maxSize: MaxInactiveArrows);
    }

    public ArrowManager GetArrow()
    {
        ArrowManager arrow = arrowPool.Get();
        arrow.transform.SetParent(null, false);
        return arrow;
    }

    private ArrowManager CreateArrow()
    {
        ArrowManager arrow = Instantiate(arrowPrefab, inactiveArrows, false);
        arrow.gameObject.SetActive(false);
        arrow.SetPool(arrowPool);
        return arrow;
    }

    private void StoreArrow(ArrowManager arrow)
    {
        arrow.gameObject.SetActive(false);
        arrow.transform.SetParent(inactiveArrows, false);
    }

    private void OnDestroy()
    {
        arrowPool.Dispose();
    }

    /*//[ContextMenu("Log Arrow Pool Counts")]
    //private void LogArrowPoolCounts()
    private void Update()
    {
        if (arrowPool == null)
        {
            Debug.Log("The arrow pool is available in Play Mode.", this);
            return;
        }

        Debug.Log(
            $"Arrows: {arrowPool.CountInactive} inactive, " +
            $"{arrowPool.CountActive} checked out, {arrowPool.CountAll} total",
            this);
    }*/
}
