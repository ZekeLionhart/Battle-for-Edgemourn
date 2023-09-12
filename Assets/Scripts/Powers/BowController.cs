using UnityEngine;

public class BowController : PowerController
{
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

    private void Update()
    {
        //if (Input.GetMouseButtonUp(0) && isActive)
            //ResetSprite();
    }

    protected override void AttemptShooting()
    {
        
    }

    private void Aim(float angle)
    {
        aimingPoint.rotation = Quaternion.identity;
        aimingPoint.Rotate(0, 0, angle);
    }

    protected virtual void Shoot(Vector3 vector, bool willShoot)
    {
        if (isActive)
        {
            ResetSprite();

            if (canShoot && willShoot)
            {
                CreateArrow(vector, shot, aimingPoint.position);
            }
        }
    }

    protected void ResetSprite()
    {
        animator.SetBool("PullWeak", false);
        animator.SetBool("PullMed", false);
        animator.SetBool("PullStrong", false);
    }

    protected void CreateArrow(Vector3 vector, Rigidbody2D arrow, Vector3 position)
    {
        Rigidbody2D shotRigid = Instantiate(arrow, position, aimingPoint.rotation);
        Vector3 force = -1f * arrowSpeed * vector;
        shotRigid.velocity = force;

        OnShotInstantiated(shotRigid.gameObject, damageType, damage, 0f);
        OnPowerShoot(this, cooldown);
        canShoot = false;
    }

    private void PullBow(int strength)
    {
        if (currentState < strength)
            onPullSfx.Play();

        currentState = strength;

        switch (strength)
        {
            case 1:
                animator.SetBool("PullWeak", true);
                animator.SetBool("PullMed", false);
                animator.SetBool("PullStrong", false);
                break;
            case 2:
                animator.SetBool("PullWeak", false);
                animator.SetBool("PullMed", true);
                animator.SetBool("PullStrong", false);
                break;
            case 3:
                animator.SetBool("PullWeak", false);
                animator.SetBool("PullMed", false);
                animator.SetBool("PullStrong", true);
                break;
            default:
                break;
        }
    }
}
