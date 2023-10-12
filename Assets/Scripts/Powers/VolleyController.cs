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
        if (isActive)
        {
            ResetSprite();
            animator.SetTrigger(ParameterNames.Shoot);

            if (canShoot && willShoot)
            {
                CallShotAnalytics();
                arrowCounter = 0;
                CreateArrow(vector, arrows[arrowCounter], aimingPoint.position + rand * spreadMultiplier * Vector3.left);
                OnPowerShoot(this, cooldown);
                StartCoroutine(DelayShot(vector));
            }
        }
    }

    private IEnumerator DelayShot(Vector3 vector)
    {
        yield return delayWFS;

        rand = Random.Range(1, numOfArrows);
        CreateArrow(vector, arrows[arrowCounter], aimingPoint.position + rand * spreadMultiplier * Vector3.left);
        arrowCounter++;

        if (arrowCounter < numOfArrows)
            StartCoroutine(DelayShot(vector));
    }
}