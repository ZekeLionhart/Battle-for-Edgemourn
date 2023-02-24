using UnityEngine;

public class CrosshairPower : PowerController
{
    [SerializeField] private float aimSpeed;
    private Vector2 startingPos;
    private int directionMult = 1;

    protected override void Awake()
    {
        base.Awake();
        startingPos = new Vector2(aimingPoint.transform.position.x, aimingPoint.transform.position.y);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        aimingPoint.transform.position = startingPos;
    }

    protected virtual void Update()
    {
        HandleAimMovement();

        if (Input.GetButtonDown("Fire1") && canShoot)
            Shoot();
    }

    protected virtual void HandleAimMovement()
    {
        aimingPoint.transform.position += new Vector3(directionMult * aimSpeed * Time.deltaTime, 0f);

        if (aimingPoint.transform.position.x >= 5f || aimingPoint.transform.position.x <= -6f)
            directionMult *= -1;
    }

    protected virtual void HandleAimRotation()
    {
        aimingPoint.transform.Rotate(0, 0, directionMult * aimSpeed * Time.deltaTime);

        if (aimingPoint.transform.rotation.z >= -0.1f || aimingPoint.transform.rotation.z <= -0.5f)
            directionMult *= -1;
    }
}
