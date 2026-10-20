using UnityEngine;

public class BowController : PowerController
{
    [SerializeField] protected ArrowManager arrowPrefab;
    [SerializeField] protected GameObject bowShockwave;
    [SerializeField] private AudioSource onPullSfx;
    [SerializeField] protected float arrowSpeed;
    private int currentState = 0;

    protected override void OnEnable()
    {
        base.OnEnable();
        LineDrawer.OnMouseUp += Shoot;
        LineDrawer.OnAimUpdate += Aim;
        LineDrawer.OnBowPull += PullBow;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LineDrawer.OnMouseUp -= Shoot;
        LineDrawer.OnAimUpdate -= Aim;
        LineDrawer.OnBowPull -= PullBow;
    }

    private void Aim(float angle)
    {
        aimingPoint.rotation = Quaternion.identity;
        aimingPoint.Rotate(0, 0, angle);
    }

    protected virtual void Shoot(Vector3 vector, bool willShoot)
    {
        if (!isActive) return;

        ResetSprite();

        if (offCooldown && willShoot)
        {
            StartCooldown();
            Instantiate(bowShockwave, this.transform.position, this.transform.rotation);
            CallShotAnalytics();
            CreateArrow(vector, aimingPoint.position);
            CameraShake.Instance.Shake(shakeDuration, shakeIntensity);
            NotifyPowerShot();
        }
    }

    protected void ResetSprite()
    {
        animator.SetBool(ParameterNames.BowPullWeak, false);
        animator.SetBool(ParameterNames.BowPullMed, false);
        animator.SetBool(ParameterNames.BowPullStrong, false);
    }

    protected void CreateArrow(Vector3 vector, Vector3 position)
    {
        ArrowManager arrow = Instantiate(arrowPrefab, position, aimingPoint.rotation);

        arrow.Initialize(powerType, damageType, damage, 0f, shakeDuration, shakeIntensity);

        Vector3 velocity = -arrowSpeed * vector;
        arrow.Launch(velocity);
    }

    private void PullBow(int strength)
    {
        if (currentState < strength)
            onPullSfx.Play();

        currentState = strength;

        switch (strength)
        {
            case 0:
                ResetSprite();
                break;
            case 1:
                animator.SetBool(ParameterNames.BowPullWeak, true);
                animator.SetBool(ParameterNames.BowPullMed, false);
                animator.SetBool(ParameterNames.BowPullStrong, false);
                break;
            case 2:
                animator.SetBool(ParameterNames.BowPullWeak, false);
                animator.SetBool(ParameterNames.BowPullMed, true);
                animator.SetBool(ParameterNames.BowPullStrong, false);
                break;
            case 3:
                animator.SetBool(ParameterNames.BowPullWeak, false);
                animator.SetBool(ParameterNames.BowPullMed, false);
                animator.SetBool(ParameterNames.BowPullStrong, true);
                break;
            default:
                break;
        }
    }
}