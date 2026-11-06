# Callback ownership audit

Original inventory: 2026-09-29. Implementation status updated 2026-10-20 below; inventory tables describe the original code. All 41 public static Action fields in Assets/Scripts were traced to callers and subscribers.

## Principle

A public method exposes an intentional operation; it does not require public state fields. Use a reference to ask one object to do something. Use events to tell interested observers something happened.

Obtain references from existing serialized fields, Instantiate results, and interaction/collision targets. Verify component placement before choosing GetComponent versus parent lookup. Do not add a universal event bus, service locator, or new singleton system just to replace callbacks.

An event carrying an object reference is not automatically bad. Which enemy died is useful notification data. Initializing one projectile by broadcasting to every projectile is a targeted command in disguise.

## Object-specific broadcasts to replace (13)

| Callback | Existing behavior/problem | Proposed direction |
| --- | --- | --- |
| PowerController.OnShotInstantiated | Bow/volley, fireball, explosions, lightning, and tar broadcast stats to all projectiles, which filter by object identity. | Initialize the newly instantiated projectile directly. |
| TrebuchetManager.OnBoulderInstantiated | All boulders receive one boulder's stats and filter by Rigidbody identity. | Initialize the returned boulder instance directly. |
| StoneWallController.OnWallSummon | Every enabled wall receives the stats without filtering; old walls reset health and schedule another expiry. | Capture the new wall and initialize only it. |
| ProjectileManager.OnEnemyHit | Tar, explosions, and walls broadcast damage to all enemies, which filter by target. | Call the struck enemy's damage method; preserve resistances, duplicate-hit behavior, analytics, and death ordering. |
| ArrowManager.OnEnemyHitWithArrow | Every enemy receives one arrow hit and filters by target. | Call a public virtual arrow-hit method on the target; preserve shielder overrides, embedding, and gore. |
| EnemyBase.OnDamageDealt | Melee/boulders broadcast to tower health and all walls; boulders also raise another class's callback. | Route to the actual target's existing HealthManager or WallManager. Verify collider placement first; avoid an unnecessary damage-interface hierarchy. |
| FireballManager.OnTargetHit | Every enabled FireballController creates an explosion for each impact. One controller conceals the cross-instance problem. | Pass explosion configuration to each fireball, or use a creator-bound instance impact event. Prefer self-contained impact behavior if it should survive creator disablement. |
| MenuClick.OnFadeAudio | A known AudioSource is broadcast to AudioManagers, which filter by source. | Call the associated audio component's fade method after verifying prefab placement. |
| ButtonHover.OnFlagHover | Flags filter a global hover command by chapter. | Bind the hover component to its flag. |
| LevelPack.SetFlagPattern | Flags filter a chapter's state update by chapter. | Update the associated flag directly; preserve progress calculation and startup ordering. |
| ResultViewer.CallStarDespawn | One results panel commands all enabled stars. | Address the stars owned by that panel, preserving timing and visibility rules. |
| ResultViewer.CallMusicStop | Results UI tells GameManager to stop GameManager's own music during its result notification. | Stop music in the match owner's result flow at the equivalent point. |
| PowerClick.OnPowerSelect | A button broadcasts a command to every PowerManager. | Bind to its intended manager; retain serialized UnityEvent entry points. |

The wall reinitialization is confirmed source behavior whenever multiple enabled walls overlap. The fireball duplication is a source-confirmed multi-controller case, not a reproduced current-scene bug.

## Ownership/API cases (4)

| Callback | Finding | Proposed direction |
| --- | --- | --- |
| CooldownManager.OnCooldownEnded | PowerManager raises someone else's callback to restore readiness; the timer lives in UI geometry. | Resolve through B02's gameplay-owned cooldown and explicit power operations. Do not introduce a second competing timer. |
| SettingsManager.UpdateSettings | GameManager loads settings and raises the settings owner's delegate. Consumers also read current settings in Start. | Let SettingsManager own publication. Inspect startup dependencies before removing redundant external publication. |
| CameraShake.CallShake | Public delegate doubles as an effects API and sends every request to all subscribed cameras. | Establish intended camera scope and encapsulate request ownership; preserve intentional multi-camera response. No new singleton merely for this change. |
| LevelPlayAdsManager.CallInterstitial | A global ad request reaches all subscribed managers; they also choose navigation. | Explicit ad recipient and completion/destination contract; coordinate navigation and SDK lifecycle work (B03/B12). |

## Notifications worth retaining (23)

These are appropriate concepts for events in this single-player project. Their static scope is not by itself a diagnosed bug. Prefer owner-raised C# events. Do not blindly add null-conditional invocation that silently hides missing required gameplay subscribers.

| Callback | Purpose and assessment |
| --- | --- |
| GameManager.OnScoreChanged | ScoreView observes the current score. Retain. |
| GameManager.OnScoreEarned | Popup spawner observes awarded score and position. Retain. |
| GameManager.OnScoreCalculated | Results UI observes score breakdown. Retain. |
| GameManager.OnResultCalculated | ProgressManager applies the match result. Retain and preserve required save/progression wiring. |
| GameManager.OnGameEnded | Enemies stop resuming attacks. Retain; defeat-only semantics are B14. |
| HealthManager.OnHealthChanged | Health display observes health. Retain. |
| HealthManager.OnZeroHealth | Match, powers, cooldowns, and analytics react to defeat. Retain. |
| PauseManager.OnPause | Crosshair powers reset aim. Retain. |
| PauseManager.SetPauseVolume | Audio categories react to pause/resume volume. Retain fan-out; notification-style naming would be clearer. |
| SettingsManager.OnSettingsOpen | PauseManager blocks resume while settings are open. Retain for current single pause/settings context. |
| SettingsManager.OnSettingsClose | PauseManager restores resume behavior. Retain. |
| StreakManager.OnStreakIncreased | Streak UI reacts. Retain. |
| StreakManager.OnStreakEnded | Streak UI clears. Retain. |
| EnemyBase.OnEnemyDeath | Match tracks the dead enemy and awards score. Retain; enemy identity is legitimate event data. |
| WaveSpawner.OnEnemySpawn | Match registers campaign enemies. Retain; do not merge endless/campaign spawners. |
| WaveSpawner.OnFinishedSpawning | Match evaluates victory. Retain. |
| LevelButton.OnLevelChosen | Selector, audio, popup, and animations react. Retain shared selection notification. |
| LineDrawer.OnMouseUp | Bow/volley receive drag release; shooting checks active state. Retain current shared input. |
| LineDrawer.OnAimUpdate | Bow/volley update aim. Retain provisionally; verify inactive-bow intent (B13). |
| LineDrawer.OnBowPull | Bow/volley update pull animations and sounds. Retain provisionally; verify inactive-bow intent (B13). |
| PowerController.OnPowerShoot | Cooldown display, selection, and previous-aim display react. Retain; correct previous-shot filtering separately (B13). Derived emitters need a protected owner raising method when this becomes an event. |
| PowerManager.OnSwitchPowers | Activation, aim reset, and borders react. Retain notification; direct activation through the existing powers array is possible, but preserve notification ordering. |
| PowerShooter.IsInsideArea | Powers and aiming share whether a press began in the shooting area. Retain for current player context. |

## Unused declaration (1)

PowerShooter.OnScreenClick has no producer or subscriber in project scripts. Remove during relevant input cleanup.

## Unity and SDK callbacks

Unity physics, pointer, and animation callbacks are appropriate integration points. Preserve animation-event method names while changing their internals. Source-only caller searches cannot prove an animation method is unused.

LevelButton pairs Button.onClick registration in Awake with removal in OnDestroy.

LevelPlay SDK notifications are appropriate asynchronous events. The component subscribes to static OnInitSuccess/OnInitFailed in Start without matching removal. Its ad-object subscriptions also need a coherent destruction policy. Inspect the installed SDK before implementing lifecycle changes. No runtime leak has been demonstrated.

## Proposed batches

### 1. Direct initialization of spawned objects

Remove only OnShotInstantiated, OnBoulderInstantiated, and OnWallSummon. Expose instance Initialize methods and call them on the Instantiate result. Preserve serialized field names/types and prefab references.

Ten scripts affected:
- Powers/PowerController.cs
- Powers/BowController.cs (also verify inherited VolleyController.CreateArrow usage)
- Powers/BurningTarController.cs
- Powers/FireballController.cs (both projectile and explosion initialization)
- Powers/LightningController.cs
- Powers/StoneWallController.cs
- Powers/Projectiles/ProjectileManager.cs
- Powers/Projectiles/WallManager.cs
- Enemies/TrebuchetManager.cs
- Enemies/BoulderManager.cs

Initialize immediately after Instantiate, before subsequent Update/physics work. Awake/OnEnable already run during active-object creation; verify they do not consume passed stats. Missing required prefab components should be diagnosed rather than silently skipped.

Expected behavioral correction: creating a new wall no longer reinitializes old walls. Preserve balance, input, and save formats. No prefab changes are expected, subject to verifying component placement and variants.

Validation: search all removed callbacks/callers; compile in Unity if available; verify relevant prefabs; exercise bow, volley, fireball/explosion, lightning, tar, concurrent walls, and boulders with different stats. Test instance isolation rather than merely mirroring implementation.

### 2. Targeted damage and fireball impact

Affected: EnemyBase, ShielderManager, HealthManager, BoulderManager, ProjectileManager, ArrowManager, BurningTarManager, ExplosionManager, WallManager, FireballManager, FireballController.

Resolve collider/component ownership, preserve virtual shield behavior, and pass explosion configuration per fireball. Verify two enemies, shielded arrows, tower versus wall hits, and separate fireball configurations/creators. Remove subscriptions only after every producer routes correctly.

### 3. Targeted UI commands

Affected: MenuClick, AudioManager, ButtonHover, FlagController, LevelPack, PowerClick, PowerManager, ResultViewer, StarController, GameManager, and relevant UI/HUD/flag prefabs or scene overrides where wiring is necessary.

Reuse current references first. Preserve UnityEvent and animation entry points. Verify chapter hover/state, fades, selection, result music, and star outro behavior.

### 4. Owner APIs and external lifecycle

Separate follow-ups for settings (SettingsManager/GameManager/consumers), cooldown ownership (B02), camera shake/camera wiring, and advertising/navigation (LevelPlayAdsManager/MenuClick/ResultViewer, B03/B12).

Check startup, pause/resume, scene re-entry, Android, and WebGL. Currency/save behavior waits for B04's intent. Merely replacing public Action with event would fail foreign callers or leave the ownership problem unresolved.

### 5. Protect retained notifications and check input filtering

Use owner-raised events, protected raisers for derived power classes, remove the dead declaration, and address B13 after confirming behavior. Distinguish optional observers from required gameplay dependencies. Validate progression and defeat as well as presentation.

## Status

Updated 2026-10-20. Work proceeds as guided changes applied by the user; the assistant edits tracking documentation only.

- [x] Batch 1: direct initialization complete. User reports all tested and validated.
- [x] Batch 2, fireball impact portion: fireballs now create/configure their own explosions.
- [x] Batch 2, damage routing implementation: OnEnemyHit, OnEnemyHitWithArrow, and OnDamageDealt removed. User validated the direct-call migrations and confirmed the final floor/boolean-arrow regression checks complete on 2026-10-15 (B01.2-check).
- [x] Batch 3: targeted UI commands complete; user tested.
- [x] Batch 4: settings, camera, cooldown, and ad request/lifecycle ownership complete; user reports tests passing.
- [x] Batch 5: retained notifications converted to events, inherited shot notification raised through owner helper, and input filtering complete; user tested.

Implementation differs from the original proposal: the user preferred typed component prefab references and projectile-owned launch velocity. Prefab Inspector references were therefore reassigned as part of the guided migration. Bow/volley share arrow creation, and the temporary initialization bridge and generic PowerController.shot field were retired. Fireball impact ownership was completed early.

A source search on 2026-10-15 found no remaining OnShotInstantiated, OnBoulderInstantiated, OnWallSummon, or OnTargetHit references under Assets/Scripts. Runtime validation is reported by the user; no independent Unity build or platform-specific validation was performed by the assistant.
2026-10-15 completion update: damage receivers now use IDamageable (tower/defenses) and IDamageableEnemy (player-power targets). Special arrow reactions use IArrowHitReceiver.PinArrow returning bool, with ArrowManager responsible for damage according to that outcome. The user moved HealthManager and HitSFX to the tower. Source search confirms the three damage broadcasts are gone. The user confirmed final floor and arrow/shield validation complete on 2026-10-15; see B01.2-check. Next guided topic: MenuClick.OnFadeAudio and AudioManager, then the remaining targeted UI commands.
2026-10-20 final completion: all five batches are complete. The user explicitly chose a single CameraShake instance and shared aim angles between warbow and volley. Only pull effects and shot ownership are filtered; release captures the selected power before callbacks can switch it. CooldownView reads gameplay-owned progress. Ads accept a caller-owned continuation and track SDK initialization across scenes while disposing scene-owned ads. Rewarded-ad/store design remains deferred. No public static Action fields remain under Assets/Scripts. Runtime checks are user-reported; no independent Unity build or Android validation was performed. Earlier inventory/proposals and dated updates remain as historical context.
