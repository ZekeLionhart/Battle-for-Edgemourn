using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraRatio : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector2 targetAspect = new(16, 9);
    [SerializeField] private float referenceSize = 5f;
    [SerializeField] private bool limitGameplayRatio;
    [SerializeField] private bool allowRatioCorrection;

    private int previousWidth;
    private int previousHeight;

    void Start()
    {
        RefreshFraming();
    }

    private void Update()
    {
        if (Screen.width != previousWidth || Screen.height != previousHeight)
            RefreshFraming();
    }

    private void UpdateCrop()
    {
        float screenRatio = Screen.width / (float)Screen.height;
        float targetRatio = targetAspect.x / targetAspect.y;

        if (Mathf.Approximately(screenRatio, targetRatio))
        {
            mainCamera.rect = new Rect(0, 0, 1, 1);
        }
        else if (screenRatio > targetRatio)
        {
            float normalizedWidth = targetRatio / screenRatio;
            float barThickness = (1f - normalizedWidth) / 2f;
            mainCamera.rect = new Rect(barThickness, 0, normalizedWidth, 1);
        }
        else
        {
            float normalizedHeight = screenRatio / targetRatio;
            float barThickness = (1f - normalizedHeight) / 2f;
            mainCamera.rect = new Rect(0, barThickness, 1, normalizedHeight);
        }
    }

    private void RefreshFraming()
    {
        previousWidth = Screen.width;
        previousHeight = Screen.height;

        if (allowRatioCorrection)
            FixRatioAdaptation();
        else if (limitGameplayRatio)
        {
            mainCamera.orthographicSize = referenceSize;
            UpdateCrop();
        }
    }

    private void FixRatioAdaptation()
    {
        mainCamera.rect = new Rect(0f, 0f, 1f, 1f);

        float referenceAspect = targetAspect.x / targetAspect.y;
        float currentAspect = Screen.width / (float)Screen.height;

        mainCamera.orthographicSize = referenceSize * referenceAspect / currentAspect;
    }
}