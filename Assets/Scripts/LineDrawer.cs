using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRend;
    private Vector2 startPos;
    private Vector2 endPos;

    public static Action<float, float> OnMouseUp;

    void Start()
    {
        lineRend.positionCount = 2;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButton("Fire1"))
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRend.SetPosition(0, new Vector3(startPos.x, startPos.y, 0f));
            lineRend.SetPosition(1, new Vector3(endPos.x, endPos.y, 0f));
        }

        if (Input.GetButtonUp("Fire1"))
        {
            OnMouseUp((endPos - startPos).magnitude, CalculateAngle());

            lineRend.SetPosition(0, Vector3.zero);
            lineRend.SetPosition(1, Vector3.zero);
        }
    }

    private float CalculateAngle()
    {
        float angle = (Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x));

        if (startPos.y < endPos.y && startPos.x > endPos.x)
        {
            angle *= -1;
        }

        if (endPos.y < startPos.y && endPos.x > startPos.x)
        {
            angle = 90f;
        }

        if (startPos.y < endPos.y && endPos.x > startPos.x)
        {
            angle = -90f;
        }

        angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        Debug.Log(angle);
        return angle;
    }
}
