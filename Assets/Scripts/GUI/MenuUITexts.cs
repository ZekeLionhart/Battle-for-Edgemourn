using TMPro;
using UnityEngine;

public class MenuUITexts : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    [SerializeField] private UITextKey uiText;

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();

        textComponent.text = TextDB.GetTextByKey(uiText);
    }
}
