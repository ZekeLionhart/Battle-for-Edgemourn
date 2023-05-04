using System;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRend;
    [SerializeField] private float maxLength;
    [SerializeField] private Transform origin;
    [SerializeField] private Transform back;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 prevStartPos;
    private Vector3 prevEndPos;
    private int currentPull;

    public static Action<Vector3, bool> OnMouseUp;
    public static Action<float> OnAimUpdate;
    public static Action<int> OnBowPull;

    private void Awake()
    {
        lineRend.positionCount = 2;
    }

    private void OnEnable()
    {
        PowerController.OnPowerShoot += SavePreviousShot;
    }

    private void OnDisable()
    {
        PowerController.OnPowerShoot -= SavePreviousShot;
    }

    void Update()
    {
        if (!PauseManager.isPaused)
            DrawAimingLine();
    }

    private void DrawAimingLine()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPos.z = 0f;
            currentPull = 1;
            OnBowPull(currentPull);

            ShowPreviousShot();
        }

        if (Input.GetButton("Fire1"))
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

        if (Input.GetButtonUp("Fire1"))
        {
            bool willShoot = false;

            if (endPos - startPos != Vector3.zero)
            {
                willShoot = true;

                lineRend.SetPosition(0, Vector3.zero);
                lineRend.SetPosition(1, Vector3.zero);
            }

            HidePreviousShot();
            OnMouseUp(endPos - startPos, willShoot);
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
