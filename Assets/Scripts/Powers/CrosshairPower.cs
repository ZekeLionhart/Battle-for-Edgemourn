using UnityEngine;

public class CrosshairPower : PowerController
{
    [SerializeField] private float aimSpeed;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    private Vector2 startingPos;
    private Quaternion startingRot;
    private int directionMult = 1;
    private bool aimStyleManual;

    protected override void Awake()
    {
        base.Awake();
        startingPos = new Vector2(aimingPoint.position.x, aimingPoint.position.y);
        startingRot = Quaternion.Euler(0f, 0f, aimingPoint.eulerAngles.z);
        aimingPoint.SetPositionAndRotation(startingPos, startingRot);
        SetAimStyle();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PowerManager.OnSwitchPowers += ResetAim;
        PauseManager.OnPause += ResetAim;
        SettingsManager.UpdateSettings += SetAimStyle;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PowerManager.OnSwitchPowers -= ResetAim;
        PauseManager.OnPause -= ResetAim;
        SettingsManager.UpdateSettings -= SetAimStyle;
    }

    protected virtual void Update()
    {
        HandleAimMovement();
    }

    protected virtual void HandleAimMovement()
    {
        if (aimStyleManual && !Input.GetButton(KeyNames.Fire) || !canShoot)
            return;

        aimingPoint.position += aimSpeed * directionMult * Time.deltaTime * Vector3.right;

        if (aimingPoint.position.x >= rightLimit.position.x && directionMult > 0
                || aimingPoint.position.x <= leftLimit.position.x && directionMult < 0)
            directionMult *= -1;
    }

    protected virtual void HandleAimRotation()
    {
        if (aimStyleManual && !Input.GetButton(KeyNames.Fire) || !canShoot)
            return;

        aimingPoint.Rotate(0, 0, directionMult * aimSpeed * Time.deltaTime);

        if (aimingPoint.eulerAngles.z >= rightLimit.eulerAngles.z && directionMult > 0
                || aimingPoint.eulerAngles.z <= leftLimit.eulerAngles.z && directionMult < 0)
            directionMult *= -1;
    }

    protected virtual void ResetAim()
    {
        aimingPoint.SetPositionAndRotation(startingPos, startingRot);
    }

    protected virtual void ResetAim(PowerController power)
    {
        ResetAim();
    }

    protected override void Shoot()
    {
        base.Shoot();
        ResetAim(this);
    }

    private void SetAimStyle()
    {
        if (PlayerPrefs.GetInt(SettingNames.AimStyle) == 0)
            aimStyleManual = false;
        else aimStyleManual = true;
    }
}
