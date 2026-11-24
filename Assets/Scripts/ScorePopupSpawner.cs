using UnityEngine;

public class ScorePopupSpawner : MonoBehaviour
{
    [SerializeField] private ScoreVFX popupPrefab;
    [SerializeField] private PoolManager vfxPool;


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
        ScoreVFX popup = vfxPool.Get(popupPrefab);
        popup.transform.SetPositionAndRotation(coord.position, coord.rotation);
        popup.Initialize(score);
    }
}
