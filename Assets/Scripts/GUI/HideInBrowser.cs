using UnityEngine;

public class HideInBrowser : MonoBehaviour
{
    [SerializeField] private Transform[] iconsToHideInBrowser;

    private void Awake()
    {
        HideIcons();
    }

    private void HideIcons()
    {
#if UNITY_WEBGL
        foreach (Transform icon in iconsToHideInBrowser)
            icon.gameObject.SetActive(false);
#endif
    }
}
