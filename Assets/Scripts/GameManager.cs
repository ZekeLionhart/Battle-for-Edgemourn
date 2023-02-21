using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PowerController> powers = new();
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float enemyDelay;
    private List<bool> cooldowns = new();
    private PowerController activePower;
    private WaitForSeconds enemyDelayWFS;
    private bool canSpawnEnemy = true;

    public static Action<PowerController> OnSwitchPowers;

    private void Awake()
    {
        enemyDelayWFS = new WaitForSeconds(enemyDelay);
        activePower = powers[0];

        for (int i = 0; i < powers.Count; i++)
            cooldowns.Add(false);
    }

    private void OnEnable()
    {
        HealthManager.OnZeroHealth += FailGame;
        PowerController.OnPowerShoot += StartPowerCooldown;
        CooldownManager.OnCooldownEnded += EndPowerCooldown;
    }

    private void OnDisable()
    {
        HealthManager.OnZeroHealth -= FailGame;
        PowerController.OnPowerShoot -= StartPowerCooldown;
        CooldownManager.OnCooldownEnded -= EndPowerCooldown;
    }

    private void Update()
    {
        if (canSpawnEnemy)
            StartCoroutine(SpawnEnemy());

        if (Input.GetButtonDown("Power1"))
            SwitchPowers(0);
        if (Input.GetButtonDown("Power2"))
            SwitchPowers(1);
        if (Input.GetButtonDown("Power3"))
            SwitchPowers(2);
        if (Input.GetButtonDown("Power4"))
            SwitchPowers(3);
        if (Input.GetButtonDown("Power5"))
            SwitchPowers(4);
        if (Input.GetButtonDown("Power6"))
            SwitchPowers(5);
    }

    private void SwitchPowers(int index)
    {
        powers[powers.IndexOf(activePower)].gameObject.SetActive(false);

        powers[index].gameObject.SetActive(true);
        activePower = powers[index];

        OnSwitchPowers(activePower);

        if (!cooldowns[index])
            CooldownManager.OnCooldownEnded(powers[index]);
    }

    private void StartPowerCooldown(PowerController power, float cooldown)
    {
        for (int i = 0; i < powers.Count; i++)
        {
            if (power == powers[i])
            {
                cooldowns[i] = true;
                break;
            }
        }
    }

    private void EndPowerCooldown(PowerController power)
    {
        for (int i = 0; i < powers.Count; i++)
        {
            if (power == powers[i])
            {
                cooldowns[i] = false;
                break;
            }
        }
    }

    private void FailGame()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    private IEnumerator SpawnEnemy()
    {
        canSpawnEnemy = false;

        yield return enemyDelayWFS;

        Instantiate(enemy, enemySpawnPoint.position, Quaternion.identity);

        canSpawnEnemy = true;
    }
}
