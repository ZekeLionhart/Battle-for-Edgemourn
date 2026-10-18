using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Vector3 originalPosition;
    private bool canShake = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Only one CameraShake component should exist.", this);

            enabled = false;
            return;
        }

        Instance = this;
        originalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        SettingsManager.UpdateSettings += ToggleShake;
    }

    private void OnDisable()
    {
        SettingsManager.UpdateSettings -= ToggleShake;
    }

    private void Start()
    {
        SetUpShake(SettingsManager.Instance.CurrentSettings);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void SetUpShake(SettingsData data)
    {
        canShake = data.screenShake;
    }

    public void Shake(float duration, float intensity)
    {
        if (!isActiveAndEnabled) return;

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