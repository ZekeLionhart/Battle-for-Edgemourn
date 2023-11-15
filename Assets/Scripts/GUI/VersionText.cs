using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionTextBox;

    private void Start()
    {
        versionTextBox.text = "Version: " + Application.version;
    }
}
