using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRend;
    private Vector3 startPos;
    private Vector3 endPos;

    public static Action<float, float> OnMouseUp;

    void Start()
    {
        lineRend.positionCount = 2;
    }

    void Update()
    {
        DrawAimingLine();
    }

    private void DrawAimingLine()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPos.z = 0f;
        }

        if (Input.GetButton("Fire1"))
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPos.z = 0f;

            lineRend.SetPosition(0, startPos);
            lineRend.SetPosition(1, endPos);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (endPos - startPos != Vector3.zero)
            {
                OnMouseUp((endPos - startPos).magnitude, CalculateAngle());

                lineRend.SetPosition(0, Vector3.zero);
                lineRend.SetPosition(1, Vector3.zero);
            }
        }
    }

    private float CalculateAngle()
    {
        float angle = Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x);
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);

        if (startPos.y < endPos.y && startPos.x > endPos.x)
            angle *= -1;

        if (endPos.y < startPos.y && endPos.x > startPos.x)
            angle = 90f;

        if (startPos.y < endPos.y && endPos.x > startPos.x)
            angle = -90f;

        return angle;
    }
}
