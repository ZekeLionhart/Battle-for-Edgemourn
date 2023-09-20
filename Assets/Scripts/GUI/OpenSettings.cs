using UnityEngine;

public class OpenSettings : MonoBehaviour
{
    [SerializeField] private GameObject settingsScreen;

    public void OnSettingsClick()
    {
        settingsScreen.SetActive(true);
    }
}
