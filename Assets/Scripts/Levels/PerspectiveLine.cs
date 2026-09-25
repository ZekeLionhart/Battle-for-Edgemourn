using UnityEngine;

[ExecuteAlways]
public class PerspectiveLine : MonoBehaviour
{
    [Header("---------Base---------")]
    [SerializeField] private Transform line;
    [SerializeField] private float lineXRotation;
    [SerializeField] private float lineYRotation;

    [Header("------Left Limit------")]
    [SerializeField] private Transform leftX;
    [SerializeField] private float leftRotation;
    [SerializeField] private float leftXScale;
    [SerializeField] private float leftYScale;

    [Header("------Right Limit------")]
    [SerializeField] private Transform rightX;
    [SerializeField] private float rightRotation;
    [SerializeField] private float rightXScale;
    [SerializeField] private float rightYScale;

    private void Update()
    {
        float t = Mathf.InverseLerp(leftX.position.x, rightX.position.x, transform.position.x);
        float rotation = Mathf.Lerp(leftRotation, rightRotation, t);
        float xScale = Mathf.Lerp(leftXScale, rightXScale, t);
        float yScale = Mathf.Lerp(leftYScale, rightYScale, t);

        line.localRotation = Quaternion.Euler(lineXRotation, lineYRotation, rotation);
        line.localScale = new Vector3(xScale, yScale);
    }
}