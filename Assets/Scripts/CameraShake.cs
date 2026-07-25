using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool canShake = true;

    public static Action<float, float> CallShake;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        SettingsManager.UpdateSettings += ToggleShake;
        CallShake += Shake;
    }

    private void OnDisable()
    {
        SettingsManager.UpdateSettings -= ToggleShake;
        CallShake -= Shake;
    }

    private void Start()
    {
        SetUpShake(SettingsManager.Instance.CurrentSettings);
    }

    private void SetUpShake(SettingsData data)
    {
        canShake = data.screenShake;
    }

    private void Shake(float duration, float intensity)
    {
        StopAllCoroutines();
        if (canShake) StartCoroutine(ShakeCoroutine(duration, intensity));
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

    private void ToggleShake(SettingsData data)
    {
        canShake = data.screenShake;
    }
}