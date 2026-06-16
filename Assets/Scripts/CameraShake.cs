using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;

    public static Action<float, float> CallShake;

    private void OnEnable()
    {
        CallShake += Shake;
    }

    private void OnDisable()
    {
        CallShake -= Shake;
    }

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    private void Shake(float duration, float intensity)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(duration, intensity));
    }

    private IEnumerator ShakeCoroutine(float duration, float intensity)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}