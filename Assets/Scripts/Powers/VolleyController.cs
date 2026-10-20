using System.Collections;
using UnityEngine;

public class VolleyController : BowController
{
    [SerializeField] private int numOfArrows;
    [SerializeField] private float spreadMultiplier;
    [SerializeField] private float arrowDelay;
    private int arrowCounter = 0;
    private int rand = 0;
    private WaitForSeconds delayWFS;
    private Quaternion arrowRotation;

    private void Awake()
    {
        delayWFS = new WaitForSeconds(arrowDelay);
    }

    protected override void Shoot(PowerController shootingPower, Vector3 vector, bool willShoot)
    {
        if (this != shootingPower || !isActive) return;

        ResetSprite();
        animator.SetTrigger(ParameterNames.Shoot);

        if (offCooldown && willShoot)
        {
            StartCooldown();
            Instantiate(bowShockwave, this.transform.position, this.transform.rotation);
            CallShotAnalytics();
            arrowCounter = 1;
            CreateArrow(vector, aimingPoint.position);
            arrowRotation = this.transform.rotation;
            NotifyPowerShot();
            CameraShake.Instance.Shake(shakeDuration, shakeIntensity);
            StartCoroutine(DelayShot(vector));
        }
    }

    private IEnumerator DelayShot(Vector3 vector)
    {
        yield return delayWFS;

        rand = Random.Range(1, numOfArrows);
        CreateArrow(vector, aimingPoint.position + rand * spreadMultiplier * Vector3.left);

        GameObject tempShockwave = Instantiate(bowShockwave, aimingPoint.position + rand * spreadMultiplier * Vector3.left, arrowRotation);
        tempShockwave.transform.localScale *= 0.5f;

        CameraShake.Instance.Shake(shakeDuration, shakeIntensity);

        arrowCounter++;

        if (arrowCounter < numOfArrows)
            StartCoroutine(DelayShot(vector));
    }
}