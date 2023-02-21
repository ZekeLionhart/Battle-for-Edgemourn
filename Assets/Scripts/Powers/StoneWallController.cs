using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StoneWallController : PowerController
{
    [SerializeField] private Transform floor;
    [SerializeField] private float aimSpeed;
    private Vector2 startingPos;
    private int directionMult = 1;

    protected override void Awake()
    {
        base.Awake();
        startingPos = new Vector2(transform.position.x, transform.position.y);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        transform.position = startingPos;
    }

    private void Update()
    {
        HandleAimMovement();

        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            Shoot();
        }
    }

    protected virtual void HandleAimMovement()
    {
        transform.position += new Vector3(directionMult * aimSpeed * Time.deltaTime, 0f);

        if (transform.position.x >= 5f || transform.position.x <= -6f)
            directionMult *= -1;
    }

    protected override void Shoot()
    {
        Instantiate(shot, new Vector2(transform.position.x
                , floor.position.y + floor.localScale.y / 2 - shot.transform.localScale.y / 2), shot.transform.rotation);

        OnPowerShoot(this, cooldown);
        canShoot = false;
    }
}
