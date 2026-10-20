using System;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRend;
    [SerializeField] private PowerManager powerManager;
    [SerializeField] private float maxLength;
    [SerializeField] private Transform origin;
    [SerializeField] private Transform back;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 prevStartPos;
    private Vector3 prevEndPos;
    private int currentPull;
    private bool wasFirstClickInside = false;

    public static event Action<PowerController, Vector3, bool> OnMouseUp;
    public static event Action<float> OnAimUpdate;
    public static event Action<int> OnBowPull;

    private void Awake()
    {
        lineRend.positionCount = 2;
    }

    private void OnEnable()
    {
        PowerController.OnPowerShoot += SavePreviousShot;
        PowerShooter.OnAimStarted += StartAim;
        PauseManager.OnPause += CancelAim;
        PowerManager.OnSwitchPowers += CancelAimOnPowerSwitch;
    }

    private void OnDisable()
    {
        PowerController.OnPowerShoot -= SavePreviousShot;
        PowerShooter.OnAimStarted -= StartAim;
        PauseManager.OnPause -= CancelAim;
        PowerManager.OnSwitchPowers -= CancelAimOnPowerSwitch;

        CancelAim();
    }

    void Update()
    {
        if (!PauseManager.isPaused)
            DrawAimingLine();
    }

    private void StartAim(Vector2 screenPosition)
    {
        if (PauseManager.isPaused || wasFirstClickInside)
            return;

        startPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0f));
        startPos.z = 0f;

        endPos = startPos;
        currentPull = 1;
        wasFirstClickInside = true;

        OnBowPull(currentPull);
        ShowPreviousShot();
    }

    private void DrawAimingLine()
    {
        if (Input.GetButton(KeyNames.Fire) && wasFirstClickInside)
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPos.z = 0f;

            endPos = startPos + Vector3.ClampMagnitude(endPos - startPos, maxLength);

            lineRend.SetPosition(0, startPos);
            lineRend.SetPosition(1, endPos);

            if (startPos != endPos)
                OnAimUpdate(CalculateAngle());

            if ((endPos - startPos).magnitude > maxLength / 3 * 2 && currentPull != 3)
            {
                currentPull = 3;
                OnBowPull(currentPull);
            }
            else if ((endPos - startPos).magnitude > maxLength / 3 &&
                (endPos - startPos).magnitude <= maxLength / 3 * 2 && currentPull != 2)
            {
                currentPull = 2;
                OnBowPull(currentPull);
            }
            else if ((endPos - startPos).magnitude <= maxLength / 3 && currentPull != 1)
            {
                currentPull = 1;
                OnBowPull(currentPull);
            }

        }

        if (Input.GetButtonUp(KeyNames.Fire) && wasFirstClickInside)
        {
            bool willShoot = false;

            if (endPos - startPos != Vector3.zero)
            {
                willShoot = true;

                lineRend.SetPosition(0, Vector3.zero);
                lineRend.SetPosition(1, Vector3.zero);
            }

            wasFirstClickInside = false;
            HidePreviousShot();
            PowerController shootingPower = powerManager.ActivePower;
            OnMouseUp(shootingPower, endPos - startPos, willShoot);
        }
    }

    private float CalculateAngle()
    {
        float angle = Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x);
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);

        if (startPos.y < endPos.y && startPos.x > endPos.x)
            angle = 360f - angle;

        if (startPos.y >= endPos.y && startPos.x < endPos.x)
            angle = 180f - angle;

        if (startPos.y < endPos.y && startPos.x <= endPos.x)
            angle = (180f + angle);

        return angle;
    }
    private void CancelAim()
    {
        if (!wasFirstClickInside)
            return;

        wasFirstClickInside = false;
        currentPull = 0;
        startPos = Vector3.zero;
        endPos = Vector3.zero;

        lineRend.SetPosition(0, Vector3.zero);
        lineRend.SetPosition(1, Vector3.zero);
        HidePreviousShot();

        OnBowPull?.Invoke(0);
    }

    private void CancelAimOnPowerSwitch(PowerController power)
    {
        CancelAim();
    }

    private void ShowPreviousShot()
    {
        if (prevStartPos != Vector3.zero && prevEndPos != Vector3.zero)
        {
            origin.position = startPos;
            origin.gameObject.SetActive(true);
            back.position = prevEndPos + (startPos - prevStartPos);
            back.gameObject.SetActive(true);
        }
    }

    private void SavePreviousShot(PowerController power, float cooldown)
    {
        if (power is not BowController) return;

        float angle = CalculateAngle();

        origin.rotation = Quaternion.identity;
        origin.Rotate(0, 0, angle);

        back.rotation = Quaternion.identity;
        back.Rotate(0, 0, angle);

        prevStartPos = startPos;
        prevEndPos = endPos;
    }

    private void HidePreviousShot()
    {
        origin.gameObject.SetActive(false);
        back.gameObject.SetActive(false);
    }
}
