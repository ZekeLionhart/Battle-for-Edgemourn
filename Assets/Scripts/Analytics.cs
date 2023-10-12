using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics : MonoBehaviour
{
    private static float[] bowStats = { 0f, 0f, 0f, 0f };
    private static float[] volleyStats = { 0f, 0f, 0f, 0f };
    private static float[] fireballStats = { 0f, 0f, 0f, 0f };
    private static float[] lightningStats = { 0f, 0f, 0f, 0f };
    private static float[] stonewallStats = { 0f, 0f, 0f, 0f };
    private static float[] tarStats = { 0f, 0f, 0f, 0f };

    private void OnEnable()
    {
        HealthManager.OnZeroHealth += SaveData;
    }

    private void OnDisable()
    {
        HealthManager.OnZeroHealth -= SaveData;
    }

    public static void OnPowerUsed(PowerTypes power)
    {
        switch (power)
        {
            case PowerTypes.Warbow:
                bowStats[0]++;
                break;
            case PowerTypes.ArrowVolley:
                volleyStats[0]++;
                break;
            case PowerTypes.Fireball:
                fireballStats[0]++;
                break;
            case PowerTypes.LightningStrike:
                lightningStats[0]++;
                break;
            case PowerTypes.Stonewall:
                stonewallStats[0]++;
                break;
            case PowerTypes.BurningTar:
                tarStats[0]++;
                break;
            default:
                break;
        }
    }

    public static void OnPowerDamage(PowerTypes power, float damage) 
    { 
        switch (power)
        {
            case PowerTypes.Warbow:
                bowStats[1] += damage;
                break;
            case PowerTypes.ArrowVolley:
                volleyStats[1] += damage;
                break;
            case PowerTypes.Fireball:
                fireballStats[1] += damage;
                break;
            case PowerTypes.LightningStrike:
                lightningStats[1] += damage;
                break;
            case PowerTypes.Stonewall:
                stonewallStats[1] += damage;
                break;
            case PowerTypes.BurningTar:
                tarStats[1] += damage;
                break;
            default:
                break;
        }
    }

    public static void OnPowerKill(PowerTypes power, float credits) 
    {
        switch (power)
        {
            case PowerTypes.Warbow:
                bowStats[2]++;
                bowStats[3] += credits;
                break;
            case PowerTypes.ArrowVolley:
                volleyStats[2]++;
                volleyStats[3] += credits;
                break;
            case PowerTypes.Fireball:
                fireballStats[2]++;
                fireballStats[3] += credits;
                break;
            case PowerTypes.LightningStrike:
                lightningStats[2]++;
                lightningStats[3] += credits;
                break;
            case PowerTypes.Stonewall:
                stonewallStats[2]++;
                stonewallStats[3] += credits;
                break;
            case PowerTypes.BurningTar:
                tarStats[2]++;
                tarStats[3] += credits;
                break;
            default:
                break;
        }
    }

    private void SaveData()
    {
        for (int i = 0; i <= 3; i++)
        {
            PlayerPrefs.SetFloat("Bow" + i, bowStats[i]);
            PlayerPrefs.SetFloat("Volley" + i, volleyStats[i]);
            PlayerPrefs.SetFloat("Fireball" + i, fireballStats[i]);
            PlayerPrefs.SetFloat("Lightning" + i, lightningStats[i]);
            PlayerPrefs.SetFloat("Stonewall" + i, stonewallStats[i]);
            PlayerPrefs.SetFloat("Tar" + i, tarStats[i]);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            Debug.Log("Bow Uses: " + bowStats[0] + ", Damage: " + bowStats[1] + ", Kills: " + bowStats[2] + ", Credits: " + bowStats[3]);

        if (Input.GetKeyDown(KeyCode.I))
            Debug.Log("Volley Uses: " + volleyStats[0] + ", Damage: " + volleyStats[1] + ", Kills: " + volleyStats[2] + ", Credits: " + volleyStats[3]);

        if (Input.GetKeyDown(KeyCode.O))
            Debug.Log("Fireball Uses: " + fireballStats[0] + ", Damage: " + fireballStats[1] + ", Kills: " + fireballStats[2] + ", Credits: " + fireballStats[3]);

        if (Input.GetKeyDown(KeyCode.J))
            Debug.Log("Lightning Uses: " + lightningStats[0] + ", Damage: " + lightningStats[1] + ", Kills: " + lightningStats[2] + ", Credits: " + lightningStats[3]);

        if (Input.GetKeyDown(KeyCode.K))
            Debug.Log("Stonewall Uses: " + stonewallStats[0] + ", Damage: " + stonewallStats[1] + ", Kills: " + stonewallStats[2] + ", Credits: " + stonewallStats[3]);

        if (Input.GetKeyDown(KeyCode.L))
            Debug.Log("Tar Uses: " + tarStats[0] + ", Damage: " + tarStats[1] + ", Kills: " + tarStats[2] + ", Credits: " + tarStats[3]);
    }
#endif
}
