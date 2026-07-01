using UnityEngine;

public class CrosshairPower : PowerController
{
    [SerializeField] private float aimSpeed;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    private Vector2 startingPos;
    private Quaternion startingRot;
    private int directionMult = 1;

    protected override void Awake()
    {
        base.Awake();
        startingPos = new Vector2(aimingPoint.position.x, aimingPoint.position.y);
        startingRot = Quaternion.Euler(0f, 0f, aimingPoint.eulerAngles.z);
        aimingPoint.SetPositionAndRotation(startingPos, startingRot);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PowerManager.OnSwitchPowers += ResetAim;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PowerManager.OnSwitchPowers -= ResetAim;
    }

    protected virtual void Update()
    {
        HandleAimMovement();
    }

    protected virtual void HandleAimMovement()
    {
        if (Input.GetButton(KeyNames.Fire))
        {
            aimingPoint.position += aimSpeed * directionMult * Time.deltaTime * Vector3.right;

            if (aimingPoint.position.x >= rightLimit.position.x && directionMult > 0
                    || aimingPoint.position.x <= leftLimit.position.x && directionMult < 0)
                directionMult *= -1;
        }
    }

    protected virtual void HandleAimRotation()
    {
        if (Input.GetButton(KeyNames.Fire))
        {
            aimingPoint.Rotate(0, 0, directionMult * aimSpeed * Time.deltaTime);

            if (aimingPoint.eulerAngles.z >= rightLimit.eulerAngles.z && directionMult > 0
                    || aimingPoint.eulerAngles.z <= leftLimit.eulerAngles.z && directionMult < 0)
                directionMult *= -1;
        }
    }

    protected virtual void ResetAim(PowerController power)
    {
        aimingPoint.SetPositionAndRotation(startingPos, startingRot);
    }
}
