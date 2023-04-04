using UnityEngine;

public class CrosshairPower : PowerController
{
    [SerializeField] private float aimSpeed;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    private Vector2 startingPos;
    private int directionMult = 1;

    protected override void Awake()
    {
        base.Awake();
        startingPos = new Vector2(aimingPoint.position.x, aimingPoint.position.y);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        aimingPoint.position = startingPos;
    }

    protected virtual void Update()
    {
        HandleAimMovement();

        if (Input.GetButtonDown("Fire1") && canShoot)
            Shoot();
    }

    protected virtual void HandleAimMovement()
    {
        aimingPoint.position += new Vector3(directionMult * aimSpeed * Time.deltaTime, 0f);

        if (aimingPoint.position.x >= rightLimit.position.x && directionMult > 0
                || aimingPoint.position.x <= leftLimit.position.x && directionMult < 0)
            directionMult *= -1;
    }

    protected virtual void HandleAimRotation()
    {
        aimingPoint.Rotate(0, 0, directionMult * aimSpeed * Time.deltaTime);

        if (aimingPoint.rotation.z >= rightLimit.rotation.z && directionMult > 0
                || aimingPoint.rotation.z <= leftLimit.rotation.z && directionMult < 0)
            directionMult *= -1;
    }
}
