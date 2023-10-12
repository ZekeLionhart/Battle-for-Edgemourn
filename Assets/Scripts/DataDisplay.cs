using TMPro;
using UnityEngine;

public class DataDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] bowData;
    [SerializeField] private TextMeshProUGUI[] volleyData;
    [SerializeField] private TextMeshProUGUI[] fireballData;
    [SerializeField] private TextMeshProUGUI[] lightningData;
    [SerializeField] private TextMeshProUGUI[] stonewallData;
    [SerializeField] private TextMeshProUGUI[] tarData;

    private void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        for (int i = 0; i < bowData.Length; i++)
        {
            bowData[i].text = PlayerPrefs.GetFloat("Bow" + i).ToString();
            volleyData[i].text = PlayerPrefs.GetFloat("Volley" + i).ToString();
            fireballData[i].text = PlayerPrefs.GetFloat("Fireball" + i).ToString();
            lightningData[i].text = PlayerPrefs.GetFloat("Lightning" + i).ToString();
            stonewallData[i].text = PlayerPrefs.GetFloat("Stonewall" + i).ToString();
            tarData[i].text = PlayerPrefs.GetFloat("Tar" + i).ToString();
        }
    }
}
