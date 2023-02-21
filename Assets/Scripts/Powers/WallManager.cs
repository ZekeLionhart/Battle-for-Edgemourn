using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    private Vector2 floor;
    private int hitpoints = 10;

    private void Awake()
    {
        floor = new Vector2(0f, transform.position.y);
    }

    private void OnEnable()
    {
        Enemy.OnDamageDealt += TakeDamage;
    }

    private void OnDisable()
    {
        Enemy.OnDamageDealt -= TakeDamage;
    }

    private void Update()
    {
        if (transform.position.y < floor.y + transform.localScale.y)
            transform.position += new Vector3(0f, 15f * Time.deltaTime);
        else
            transform.position = new Vector2(transform.position.x, floor.y + transform.localScale.y);
    }

    private void TakeDamage(GameObject target, int damage)
    {
        if (target == gameObject)
        {
            hitpoints -= damage;

            if (hitpoints <= 0)
                Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
