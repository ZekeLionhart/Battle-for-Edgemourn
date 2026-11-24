using UnityEngine;

public class BowShockwaveVFX : PoolableObject
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator.writeDefaultValuesOnDisable = true;
    }

    public void Initialize(Vector3 scale)
    {
        transform.localScale = scale;

        gameObject.SetActive(true);
        animator.Update(0f);
    }

    private void ReturnToPool()
    {
        ReleaseToPool();
    }
}
