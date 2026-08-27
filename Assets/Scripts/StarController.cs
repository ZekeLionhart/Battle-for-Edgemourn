using UnityEngine;

public class StarController : MonoBehaviour
{
    [SerializeField] private Transform star;
    [SerializeField] private AudioSource starHitSFX;
    [SerializeField] private float position;
    [SerializeField] private float startX;
    private readonly float endX = 0f;

    private void Update()
    {
        star.localPosition = Vector3.Lerp(
            new Vector3(startX, star.localPosition.y, 0),
            new Vector3(endX, star.localPosition.y, 0),
            position
        );
    }

    private void PlayStarHitSFX()
    {
        starHitSFX.Play();
    }
}
