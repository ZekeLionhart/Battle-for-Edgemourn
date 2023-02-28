using UnityEngine;

public class BoulderManager : MonoBehaviour
{
    private int damage;

    protected virtual void OnEnable()
    {
        TrebuchetManager.OnBoulderInstantiated += SetVariables;
    }

    protected virtual void OnDisable()
    {
        TrebuchetManager.OnBoulderInstantiated -= SetVariables;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Tower") || collision.collider.CompareTag("Player"))
        {
            EnemyBase.OnDamageDealt(collision.gameObject, damage);
            Destroy(gameObject);
        }
    }

    private void SetVariables(Rigidbody2D shot, int damage)
    {
        if (shot.gameObject == gameObject)
            this.damage = damage;
    }
}
