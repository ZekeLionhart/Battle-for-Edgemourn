using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] private float timer;

    private void Awake()
    {
        Destroy(gameObject, timer);
    }
}
