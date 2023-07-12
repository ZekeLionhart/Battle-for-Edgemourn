using System.Collections;
using UnityEngine;

public class GeneralSelfDestroy : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private GameObject hissing;

    private void Start()
    {
        StartCoroutine(SelfDestroy());
    }

    private void OnParticleCollision(GameObject other)
    {
        hissing.SetActive(true);
    }

    private IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(timer);

        Destroy(gameObject);
    }
}
