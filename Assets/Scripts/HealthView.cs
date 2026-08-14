using TMPro;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthLabel;

    private void OnEnable()
    {
        HealthManager.OnHealthChanged += UpdateHealth;
    }
    private void OnDisable()
    {
        HealthManager.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(int newHP)
    {
        healthLabel.text = newHP.ToString();
    }
}
