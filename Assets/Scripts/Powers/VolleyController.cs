using UnityEngine;

public class VolleyController : BowController
{
    [SerializeField] private int numOfArrows;
    [SerializeField] private float spreadMultiplier;
    private Rigidbody2D[] arrows;

    protected override void Awake()
    {
        base.Awake();
        arrows = new Rigidbody2D[numOfArrows];
        for (int i = 0; i < numOfArrows; i++)
            arrows[i] = shot;
    }

    protected override void Shoot(Vector3 vector)
    {
        if (canShoot)
            for (int i = 0; i < numOfArrows; i++)
                CreateArrow(vector, arrows[i], aimingPoint.position + i * spreadMultiplier * Vector3.left);
    }
}