#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Created at runtime so gameplay scenes and release builds need no debug UI setup.
public class DebugPanel : MonoBehaviour
{
    private const float MinimumSpeed = 0.125f;
    private const float MaximumSpeed = 16f;
    private readonly List<LineDrawer> suspendedAimControls = new();
    private GameObject overlay;
    private Text speedLabel;
    private Text launcherLabel;
    private Button dieButton;
    private Button refillHealthButton;
    private Button forceVictoryButton;
    private Button killAllButton;
    private Button fastCooldownButton;
    private Text fastCooldownLabel;
    private bool fastCooldownEnabled;
    private Button highDamageButton;
    private Text highDamageLabel;
    private bool highDamageEnabled;
    private Button noDamageButton;
    private Text noDamageLabel;
    private bool noDamageEnabled;
    [SerializeField] private EnemyBase[] enemyPrefabs;
    private WaveSpawner waveSpawner;
    private UnlimitedSpawner unlimitedSpawner;
    private Button enemySelectorButton;
    private Text enemySelectorLabel;
    private Button spawnEnemyButton;
    private int selectedEnemy;
    private Button slowerButton;
    private Button fasterButton;
    private Button normalSpeedButton;
    private HealthManager health;
    private GameManager gameManager;
    private Font font;
    private bool hasGameEnded;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void RegisterScenes()
    {
        SceneManager.sceneLoaded -= CreateForScene;
        SceneManager.sceneLoaded += CreateForScene;
    }

    private static void CreateForScene(Scene scene, LoadSceneMode mode)
    {
        HealthManager sceneHealth = FindObjectOfType<HealthManager>();
        if (sceneHealth == null || FindObjectOfType<DebugPanel>() != null)
            return;

        GameObject panelObject = new GameObject("Debug Panel (Development Only)");
        SceneManager.MoveGameObjectToScene(panelObject, sceneHealth.gameObject.scene);
        DebugPanel panel = panelObject.AddComponent<DebugPanel>();
        panel.health = sceneHealth;
    }

    private void OnEnable()
    {
        GameManager.OnGameEnded += HandleGameEnded;
    }

    private void OnDisable()
    {
        GameManager.OnGameEnded -= HandleGameEnded;
        RestoreAimControls();
        if (overlay != null)
            overlay.SetActive(false);
    }

    private void Start()
    {
        // Development builds receive this component and its prefab references at build time.
        health = FindObjectOfType<HealthManager>();
        gameManager = FindObjectOfType<GameManager>();
        waveSpawner = FindObjectOfType<WaveSpawner>();
        unlimitedSpawner = FindObjectOfType<UnlimitedSpawner>();
#if UNITY_EDITOR
        string[] enemyNames = { "Grunt", "Shielder", "Ram", "Trebuchet" };
        enemyPrefabs = new EnemyBase[enemyNames.Length];
        for (int i = 0; i < enemyNames.Length; i++)
        {
            string path = "Assets/Prefabs/Enemies/" + enemyNames[i] + ".prefab";
            GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
            enemyPrefabs[i] = prefab != null ? prefab.GetComponent<EnemyBase>() : null;
            if (enemyPrefabs[i] == null)
                Debug.LogError("Debug spawn enemy prefab is missing: " + path);
        }
#endif
        font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32760;
        CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        gameObject.AddComponent<GraphicRaycaster>();

        Button launcher = CreateButton(transform, "Debug", TogglePanel);
        RectTransform launcherRect = launcher.GetComponent<RectTransform>();
        launcherRect.anchorMin = launcherRect.anchorMax = new Vector2(1f, 1f);
        launcherRect.pivot = new Vector2(1f, 1f);
        launcherRect.anchoredPosition = new Vector2(-16f, -16f);
        launcherRect.sizeDelta = new Vector2(170f, 44f);
        launcherLabel = launcher.GetComponentInChildren<Text>();

        overlay = new GameObject("Input Blocker", typeof(RectTransform), typeof(Image));
        overlay.transform.SetParent(transform, false);
        Stretch(overlay.GetComponent<RectTransform>());
        overlay.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.65f);

        GameObject window = new GameObject("Cheat Panel", typeof(RectTransform), typeof(Image), typeof(VerticalLayoutGroup));
        window.transform.SetParent(overlay.transform, false);
        RectTransform windowRect = window.GetComponent<RectTransform>();
        windowRect.anchorMin = windowRect.anchorMax = new Vector2(0.5f, 0.5f);
        windowRect.sizeDelta = new Vector2(560f, 660f);
        window.GetComponent<Image>().color = new Color(0.07f, 0.09f, 0.13f, 1f);
        VerticalLayoutGroup layout = window.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(24, 24, 20, 20);
        layout.spacing = 8f;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        CreateLabel(window.transform, "DEBUG / CHEATS", 26, 34f);
        CreateLabel(window.transform, "Editor & Development Builds only  |  F2 to toggle", 16, 24f);
        speedLabel = CreateLabel(window.transform, "", 22, 32f);
        Transform speedRow = CreateRow(window.transform);
        slowerButton = CreateButton(speedRow, "Speed / 2", () => ChangeSpeed(0.5f));
        normalSpeedButton = CreateButton(speedRow, "Reset x1", () => SetSpeed(1f));
        fasterButton = CreateButton(speedRow, "Speed x2", () => ChangeSpeed(2f));
        CreateLabel(window.transform, "MATCH", 18, 26f);
        Transform matchRow = CreateRow(window.transform);
        dieButton = CreateButton(matchRow, "Die instantly", Die);
        refillHealthButton = CreateButton(matchRow, "Refill health", RefillHealth);
        Transform outcomeRow = CreateRow(window.transform);
        killAllButton = CreateButton(outcomeRow, "Kill all enemies", KillAllEnemies);
        forceVictoryButton = CreateButton(outcomeRow, "Force victory", ForceVictory);
        CreateLabel(window.transform, "OVERRIDES", 18, 26f);
        fastCooldownButton = CreateButton(window.transform, "0.5s cooldowns + refresh: OFF", ToggleFastCooldown);
        fastCooldownLabel = fastCooldownButton.GetComponentInChildren<Text>();
        Transform damageRow = CreateRow(window.transform);
        highDamageButton = CreateButton(damageRow, "Power damage 1000: OFF", ToggleHighDamage);
        highDamageLabel = highDamageButton.GetComponentInChildren<Text>();
        noDamageButton = CreateButton(damageRow, "Enemy damage 0: OFF", ToggleNoDamage);
        noDamageLabel = noDamageButton.GetComponentInChildren<Text>();
        Transform spawnRow = CreateRow(window.transform);
        enemySelectorButton = CreateButton(spawnRow, "", SelectNextEnemy);
        enemySelectorLabel = enemySelectorButton.GetComponentInChildren<Text>();
        spawnEnemyButton = CreateButton(spawnRow, "Spawn enemy", SpawnEnemy);
        UpdateEnemySelector();
        CreateLabel(window.transform, "Click the enemy name to cycle types.\nOverrides reset when the level reloads.", 16, 44f);
        CreateButton(window.transform, "Close", TogglePanel);
        overlay.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
            TogglePanel();

        float speed = Time.timeScale;
        string speedText = "x" + speed.ToString("0.###");
        launcherLabel.text = "Debug  |  " + speedText;
        speedLabel.text = "Current speed: " + speedText + (PauseManager.isPaused ? " (paused)" : "");
        bool canChangeSpeed = !PauseManager.isPaused && !hasGameEnded && speed > 0f;
        slowerButton.interactable = canChangeSpeed && speed > MinimumSpeed;
        fasterButton.interactable = canChangeSpeed && speed < MaximumSpeed;
        normalSpeedButton.interactable = canChangeSpeed;
        dieButton.interactable = !hasGameEnded && health != null && health.CurrentHealth > 0;
        refillHealthButton.interactable = !hasGameEnded && health != null
            && health.CurrentHealth > 0 && health.CurrentHealth < health.MaxHealth;
        forceVictoryButton.interactable = !hasGameEnded && gameManager != null;
        killAllButton.interactable = !hasGameEnded;
        fastCooldownButton.interactable = !hasGameEnded;
        highDamageButton.interactable = !hasGameEnded;
        noDamageButton.interactable = !hasGameEnded;
        bool hasEnemyPrefabs = enemyPrefabs != null && enemyPrefabs.Length > 0;
        enemySelectorButton.interactable = !hasGameEnded && hasEnemyPrefabs;
        spawnEnemyButton.interactable = !hasGameEnded && hasEnemyPrefabs
            && enemyPrefabs[selectedEnemy] != null
            && ((waveSpawner != null && waveSpawner.isActiveAndEnabled)
                || (unlimitedSpawner != null && unlimitedSpawner.isActiveAndEnabled));
    }

    private void LateUpdate()
    {
        // Also catches endless-mode spawns, which do not raise WaveSpawner.OnEnemySpawn.
        if (noDamageEnabled && !hasGameEnded)
            ApplyNoDamage();
    }

    private void TogglePanel()
    {
        bool open = !overlay.activeSelf;
        if (open)
        {
            // Cancels a bow drag already in progress, using LineDrawer's existing OnDisable.
            foreach (LineDrawer aimControl in FindObjectsOfType<LineDrawer>())
            {
                if (!aimControl.enabled)
                    continue;
                suspendedAimControls.Add(aimControl);
                aimControl.enabled = false;
            }
        }
        else
        {
            RestoreAimControls();
        }

        overlay.SetActive(open);
    }

    private void RestoreAimControls()
    {
        foreach (LineDrawer aimControl in suspendedAimControls)
        {
            if (aimControl != null && !hasGameEnded)
                aimControl.enabled = true;
        }
        suspendedAimControls.Clear();
    }

    private void Die()
    {
        if (hasGameEnded || health == null || health.CurrentHealth <= 0)
            return;

        TogglePanel();
        health.TakeDamage(health.CurrentHealth);
    }

    private void KillAllEnemies()
    {
        if (hasGameEnded)
            return;

        // Snapshot the scene enemies: deaths update GameManager's live enemy collection.
        foreach (EnemyBase enemy in FindObjectsOfType<EnemyBase>())
        {
            if (enemy != null)
                enemy.DebugDeath();
        }
    }

    private void ForceVictory()
    {
        if (hasGameEnded || gameManager == null)
            return;

        TogglePanel();
        gameManager.DebugForceVictory();
    }

    private void RefillHealth()
    {
        if (hasGameEnded || health == null || health.CurrentHealth <= 0)
            return;

        health.Heal(health.MaxHealth);
    }

    private void ToggleFastCooldown()
    {
        if (hasGameEnded)
            return;

        fastCooldownEnabled = !fastCooldownEnabled;
        foreach (PowerController power in FindObjectsOfType<PowerController>(true))
        {
            if (fastCooldownEnabled)
                power.DebugEnableFastCooldown();
            else
                power.DebugDisableFastCooldown();
        }

        fastCooldownLabel.text = "0.5s cooldowns + refresh: " + (fastCooldownEnabled ? "ON" : "OFF");
    }

    private void ToggleHighDamage()
    {
        if (hasGameEnded)
            return;

        highDamageEnabled = !highDamageEnabled;
        foreach (PowerController power in FindObjectsOfType<PowerController>(true))
        {
            if (highDamageEnabled)
                power.DebugEnableHighDamage();
            else
                power.DebugDisableHighDamage();
        }

        highDamageLabel.text = "Power damage 1000: " + (highDamageEnabled ? "ON" : "OFF");
    }

    private void ToggleNoDamage()
    {
        if (hasGameEnded)
            return;

        noDamageEnabled = !noDamageEnabled;
        ApplyNoDamage();
        noDamageLabel.text = "Enemy damage 0: " + (noDamageEnabled ? "ON" : "OFF");
    }

    private void ApplyNoDamage()
    {
        foreach (EnemyBase enemy in FindObjectsOfType<EnemyBase>(true))
        {
            if (noDamageEnabled)
                enemy.DebugEnableNoDamage();
            else
                enemy.DebugDisableNoDamage();
        }
    }

    private void SelectNextEnemy()
    {
        if (hasGameEnded || enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

        selectedEnemy = (selectedEnemy + 1) % enemyPrefabs.Length;
        UpdateEnemySelector();
    }

    private void UpdateEnemySelector()
    {
        bool hasPrefab = enemyPrefabs != null && enemyPrefabs.Length > 0
            && enemyPrefabs[selectedEnemy] != null;
        enemySelectorLabel.text = hasPrefab ? "Enemy: " + enemyPrefabs[selectedEnemy].name + " >" : "No enemy prefab";
    }

    private void SpawnEnemy()
    {
        if (hasGameEnded || enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

        EnemyBase prefab = enemyPrefabs[selectedEnemy];
        if (prefab == null)
            return;

        if (waveSpawner != null && waveSpawner.isActiveAndEnabled)
            waveSpawner.DebugSpawnEnemy(prefab);
        else if (unlimitedSpawner != null && unlimitedSpawner.isActiveAndEnabled)
            unlimitedSpawner.DebugSpawnEnemy(prefab);

        if (noDamageEnabled)
            ApplyNoDamage();
    }

    private void ChangeSpeed(float multiplier)
    {
        SetSpeed(Mathf.Clamp(Time.timeScale * multiplier, MinimumSpeed, MaximumSpeed));
    }

    private void SetSpeed(float speed)
    {
        // Only write on a button click. Pause and match results retain ownership of their speeds.
        if (!PauseManager.isPaused && !hasGameEnded && Time.timeScale > 0f)
            Time.timeScale = speed;
    }

    private void HandleGameEnded()
    {
        hasGameEnded = true;
        overlay.SetActive(false);
        suspendedAimControls.Clear();
    }

    private Transform CreateRow(Transform parent)
    {
        GameObject row = new GameObject("Button Row", typeof(RectTransform), typeof(HorizontalLayoutGroup), typeof(LayoutElement));
        row.transform.SetParent(parent, false);
        HorizontalLayoutGroup layout = row.GetComponent<HorizontalLayoutGroup>();
        layout.spacing = 8f;
        layout.childControlWidth = layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        row.GetComponent<LayoutElement>().preferredHeight = 44f;
        return row.transform;
    }

    private Button CreateButton(Transform parent, string title, UnityAction action)
    {
        GameObject buttonObject = new GameObject(title, typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
        buttonObject.transform.SetParent(parent, false);
        buttonObject.GetComponent<Image>().color = new Color(0.2f, 0.28f, 0.38f, 1f);
        buttonObject.GetComponent<LayoutElement>().preferredHeight = 44f;
        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = buttonObject.GetComponent<Image>();
        ColorBlock colors = button.colors;
        colors.disabledColor = new Color(0.4f, 0.4f, 0.4f, 0.5f);
        button.colors = colors;
        // Avoid retaining keyboard selection: Space is also a gameplay input.
        button.navigation = new Navigation { mode = Navigation.Mode.None };
        if (action != null)
            button.onClick.AddListener(action);
        Text label = CreateLabel(buttonObject.transform, title, 18, 0f);
        label.alignment = TextAnchor.MiddleCenter;
        Stretch(label.rectTransform);
        return button;
    }

    private Text CreateLabel(Transform parent, string title, int size, float height)
    {
        GameObject labelObject = new GameObject("Label", typeof(RectTransform), typeof(Text), typeof(LayoutElement));
        labelObject.transform.SetParent(parent, false);
        Text label = labelObject.GetComponent<Text>();
        label.font = font;
        label.fontSize = size;
        label.text = title;
        label.color = new Color(0.9f, 0.94f, 1f);
        label.alignment = TextAnchor.MiddleLeft;
        label.raycastTarget = false;
        labelObject.GetComponent<LayoutElement>().preferredHeight = height;
        return label;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
    }
}
#endif
