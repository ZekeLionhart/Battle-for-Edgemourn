using UnityEngine;

public class StarController : MonoBehaviour
{
    [SerializeField] private AudioSource starHitSFX;
    [SerializeField] private float startX;
    [SerializeField] private float endX;
    [SerializeField] private float position;

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(
            new Vector3(startX, transform.localPosition.y, 0),
            new Vector3(endX, transform.localPosition.y, 0),
            position
        );
    }

    private void PlayStarHitSFX()
    {
        starHitSFX.Play();
    }
}
