using System.Collections;
using UnityEngine;

public class VolleyController : BowController
{
    [SerializeField] private int numOfArrows;
    [SerializeField] private float spreadMultiplier;
    [SerializeField] private float arrowDelay;
    private Rigidbody2D[] arrows;
    private int arrowCounter = 0;
    private int rand = 0;
    private WaitForSeconds delayWFS;
    private Quaternion arrowRotation;

    protected override void Awake()
    {
        base.Awake();
        arrows = new Rigidbody2D[numOfArrows];
        for (int i = 0; i < numOfArrows; i++)
            arrows[i] = shot;
        delayWFS = new WaitForSeconds(arrowDelay);
    }

    protected override void Shoot(Vector3 vector, bool willShoot)
    {
        if (!isActive) return;

        ResetSprite();
        animator.SetTrigger(ParameterNames.Shoot);

        if (offCooldown && willShoot)
        {
            Instantiate(bowShockwave, this.transform.position, this.transform.rotation);
            CallShotAnalytics();
            arrowCounter = 1;
            CreateArrow(vector, arrows[arrowCounter], aimingPoint.position);
            OnPowerShoot(this, cooldown);
            arrowRotation = this.transform.rotation;
            StartCoroutine(DelayShot(vector));
        }
    }

    private IEnumerator DelayShot(Vector3 vector)
    {
        yield return delayWFS;

        rand = Random.Range(1, numOfArrows);
        CreateArrow(vector, arrows[arrowCounter], aimingPoint.position + rand * spreadMultiplier * Vector3.left);

        GameObject tempShockwave = Instantiate(bowShockwave, aimingPoint.position + rand * spreadMultiplier * Vector3.left, arrowRotation);
        tempShockwave.transform.localScale *= 0.5f;

        arrowCounter++;

        if (arrowCounter < numOfArrows)
            StartCoroutine(DelayShot(vector));
    }
}