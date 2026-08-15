using UnityEngine;

public class ScorePopupSpawner : MonoBehaviour
{
    [SerializeField] private ScoreVFX popupPrefab;

    private void OnEnable()
    {
        GameManager.OnScoreEarned += SpawnPopup;
    }

    private void OnDisable()
    {
        GameManager.OnScoreEarned -= SpawnPopup;
    }

    private void SpawnPopup(int score, Transform coord)
    {
        popupPrefab.SetScoreText(score);
        Instantiate(popupPrefab, coord.position, coord.rotation);
    }
}
