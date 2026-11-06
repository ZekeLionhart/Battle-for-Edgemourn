# Project review backlog

Reviewed 2026-09-29. Source and serialized-asset review only; no Unity playtests or builds run.
Track fixes here and record validation before checking an item off. Preserve save compatibility and Inspector references.

## Current focus

- [x] B01 — Callback ownership complete (2026-10-20). All five audit batches completed through guided user edits and user-reported testing; see [CallbackAudit.md](CallbackAudit.md).

## Remaining findings from the project review

- [x] B02 — Cooldown ownership complete. PowerController owns timing/readiness; CooldownView displays progress; PowerManager no longer duplicates cooldown state. User validated pause, switching, volley, return-to-bow, and defeat behavior.
- [ ] B03 — Navigation ownership: ad manager no longer chooses destinations; ResultViewer owns its continuations. Remaining review: menus, pause, results, and transition destinations, coordinated with B05.
- [ ] B04 — DEFERRED by user: rewarded ads and PLAYER_COINS are tutorial leftovers. Revisit currency ownership when designing the in-game store; leave reward logic and save data unchanged for now.
- [ ] B05 — Retry code fixed: PauseManager and ResultViewer directly reload the current scene; obsolete destination storage and transition assets removed. User reports completion. Remaining cleanup: remove the still-enabled TransitionScene entry from Build Settings (source checked 2026-10-20).
- [ ] B06 — Unlock on defeat: ProgressManager calls UnlockNextLevel outside the victory condition. Confirm the intended rule before changing this confirmed behavior.
- [ ] B07 — Campaign/scene identity: entries 4–6 load scenes 1–3, whose GameManagers still reference entries 1–3. Identify placeholders; ensure playable entries save results against the selected level.
- [ ] B08 — Folder/naming housekeeping: 25 of 69 gameplay scripts sit directly in Assets/Scripts. Consider modest audio/persistence/scoring/UI grouping when those areas are touched. Clarify SampleScene, CanvasMain, and CanvasGame roles. Preserve metadata and serialized references on any moves.
- [ ] B09 — Project guide: expand README with startup scene, required components, adding levels/waves, platform differences, and animation-event dependencies.
- [ ] B10 — Regression coverage: no project-owned automated tests found under Assets. Add focused progression/retry checks when changing those systems and Unity playtests for prefab wiring and callback routing.

## Follow-ups found during inspection

- [ ] B11 — Persistence recovery: JSON reads/writes have no explicit malformed/interrupted-save recovery. WebGL writes call SyncFileSystem; deletes do not explicitly synchronize. Investigate platform behavior before changing either. No data-loss incident reproduced.
- [x] B12 — Advertising lifetime implementation complete. Scene listeners and ad objects cleaned up on destruction; static initialization tracking survives scene changes; uninitialized/unavailable ads allow navigation. Mobile-only tracking excluded from WebGL. User reports tests passing; no independent Android certification by assistant.
- [x] B13 — Aim filtering complete. Previous-shot updates accept bow powers only; release captures its intended power; pull sounds/animations are filtered. Shared bow aim is intentionally retained at user request so switching preserves visual direction. User validated behavior.
- [x] B14 — Match-end semantics corrected: GameManager emits OnGameEnded on both victory and defeat, guarded against repeated completion. Source verified 2026-10-20; user reports regression tests passing.
- [ ] B15 — Content status: SampleScene, named as endless mode in SceneNames, is disabled in build settings. The implementation of dialogue/story scenes mentioned in project instructions was not established in this review. Document playable versus experimental/planned content.

## Validation log

- 2026-09-29: all 41 public static Action fields in Assets/Scripts inventoried, callers/subscribers traced, representative prefab bindings inspected. No runtime code changes or Unity tests.

- [x] B01.1 — Direct initialization of boulders, walls, arrows/volley, fireballs/explosions, lightning, and burning tar. Completed through user-applied edits; user reports all tested and validated on 2026-10-15.
- [x] B01.2a — Fireball impact/explosion ownership moved to each fireball during initialization migration.
- 2026-10-15: Source search confirms OnShotInstantiated, OnBoulderInstantiated, OnWallSummon, and OnTargetHit are absent from Assets/Scripts. Playtest validation is user-reported; no independent build/platform certification performed. B01 remains open for damage, UI, ownership APIs, and retained-event protection.

- [x] B01.2 — Damage-delivery implementation complete (2026-10-15): direct/interface calls replace player-to-enemy and enemy-to-defense broadcasts. User reported successful tests during the migration. HealthManager/HitSFX were moved to the tower by the user. IDamageable, IDamageableEnemy, and IArrowHitReceiver separate damage contracts and special arrow reactions; PinArrow returns the decision used by ArrowManager to gate damage.
- [x] B01.2-check — Final follow-up verification: floor scale/collider adjustment and angled-arrow attachment; shield block versus accepted hit (damage once); tower/wall melee and boulder damage. User confirmed this final validation complete on 2026-10-15.
- 2026-10-15: Source search confirms OnEnemyHit, OnEnemyHitWithArrow, and OnDamageDealt are absent from Assets/Scripts. No gameplay files changed by the assistant. Broader B01 remains open; next session begins with MenuClick.OnFadeAudio / AudioManager targeted fading.
- 2026-10-20: Closed B01, B02, B12, B13, and B14. Targeted UI commands, settings publication, camera shake ownership, ads continuations/lifecycle, retained C# events, and bow input filtering completed. Runtime validation is user-reported; assistant performed source checks, not Unity builds. Rewarded ads/currency deferred until store design. Recommended next investigation: B05 retry destination across two levels in one session. Earlier validation-log entries describe historical status.
