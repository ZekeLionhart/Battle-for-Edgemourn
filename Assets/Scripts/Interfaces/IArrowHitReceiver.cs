using UnityEngine;

public interface IArrowHitReceiver
{
    bool PinArrow(Vector2 hitPoint, Quaternion rotation, Rigidbody2D arrow);
}