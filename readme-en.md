# Junior Programmer Personal Project - Tail Rush

An endless runner game built in Unity. The player controls a character running along parallel tracks, jumps over obstacles, drops instantly to the ground, collects grapes to restore health, and avoids poison and oncoming trains. The game dynamically accelerates over time, challenging the player to survive as long as possible.

## What I Learned

### [Lab 1 - Start Your Personal Project](https://learn.unity.com/pathway/junior-programmer/unit/getting-started-1/tutorial/lab-1-start-your-personal-project?version=6.3)

**New progress**

- **Created a Game Design Document (GDD):** Documented the design, mechanics, aesthetics, and goals of my personal project. See my [Tail Rush Game Design Document](https://docs.google.com/document/d/1EVPopBPAVQPoHID1BOLbACJpeHnu7KlxIryFtXTH3OM/edit?tab=t.0#heading=h.ic97nye8eswm).

**New concepts & skills**

- **Game Design Planning:** Learned to map out game mechanics, loops, and control parameters prior to starting coding.

### [Lab 2 - New Project with Primitives](https://learn.unity.com/pathway/junior-programmer/unit/basic-gameplay/tutorial/lab-2-new-project-with-primitives?version=6.3)

**New progress**

- **New project for my Personal Project:** Created a new Unity project with an Endless Runner theme named Tail Rush.
- **Camera positioned and rotated based on project type:** Configured a third-person follow camera positioned behind the tracks for a forward-facing runner game layout.
- **All key objects in the scene with unique materials:** Created placeholder primitive models for the runner character, multi-lane tracks, grapes (green/purple), poison (red), and obstacles/trains (gray).

**New concepts & skills**

- **Primitives:** Built initial level pieces and obstacle prefabs from Unity's 3D shapes.
- **Create new materials:** Created distinct colored materials for different game assets to easily differentiate collectibles, hazards, and pathways.
- **Export Unity packages:** Documented project versions and learned how to export package updates as backups.

### [Lab 3 - Player Control](https://learn.unity.com/pathway/junior-programmer/unit/sound-and-effects/tutorial/lab-3-player-control?version=6.3)

**New progress**

- **Player can move based on user input:** Integrated the new Input System via custom Action maps in [PlayerController.cs](Assets/Scripts/Player/PlayerController.cs) to handle lane switching (Left/Right), Jumping, and Instant Drop.
- **Player movement is constrained to suit the game:** Constrained the player to move horizontally within a set number of parallel lanes, dynamically calculated using track spacing parameters.

**New concepts & skills**

- **Program in C# independently:** Planned and implemented lane-based lateral movement using custom Coroutines to smoothly interpolate the player's position between lane coordinates.
- **Troubleshoot issues independently:** Resolved movement drift and overlap issues by replacing standard Rigidbody collisions with a custom physics system [CustomPhysics.cs](Assets/Scripts/Player/CustomPhysics.cs) utilizing `Physics.BoxCast` for stable ground checks and exact snapping.

### [Lab 4 - Basic Gameplay](https://learn.unity.com/pathway/junior-programmer/unit/gameplay-mechanics/tutorial/lab-4-basic-gameplay?version=6.3)

**New functionality**

- **Non-player object prefabs have basic movement:** Implemented [MovingObject.cs](Assets/Scripts/Objects/MovingObject.cs) to translate obstacles, trains, and collectibles towards the player based on the project speed.
- **Objects are recycled when they leave the screen:** Recycled off-screen objects and trains back to the pool manager using despawn bounds detectors.
- **Collisions between objects are handled appropriately:** Handled triggers via interface detection like `IDamageable` and `IHittable` for clean damage applications and lateral bounce impacts.
- **Objects are spawned at appropriate locations on timed intervals:** Created a dynamic level spawner [LevelSpawner.cs](Assets/Scripts/Managers/LevelSpawner.cs) to randomly generate obstacles, trains, and collectibles across the active tracks.

**New concepts & skills**

- **Create basic gameplay independently:** Structured independent object behaviors for hazards and collectables, utilizing inheritance from [Collectible.cs](Assets/Scripts/Objects/Collectibles/Collectible.cs) for custom grape and poison reactions.

### [Lab 5 - Swap Out Your Assets](https://learn.unity.com/pathway/junior-programmer/unit/user-interface/tutorial/lab-5-swap-out-your-assets?version=6.3)

**New functionality**

- **Primitive visuals replaced without rewriting gameplay:** Swapped block placeholders with styled 3D models (like trains and character models) by decoupling scripts and nesting visual elements in visual child prefabs.
- **Environment presentation updated:** Set up stylized UI materials, character run animations, and particle bursts when items are collected.
- **UI typography updated:** Replaced default fonts with TextMesh Pro fonts styled for a premium feel.

**New concepts & skills**

- **Art workflow:** Imported third-party assets and textured packages into the local environment.
- **Nested Prefabs:** Visual components were embedded as sub-children to ensure scripts, colliders, and events remain unbroken upon replacement.

### Extras

- **Model-View-Presenter (MVP) UI Pattern:** Separated gameplay state from visual components. [CollectedGrapePresenter.cs](Assets/Scripts/MVP/Presenters/CollectedGrapePresenter.cs) monitors data updates inside [GameData.cs](Assets/Scripts/ScriptableObjects/GameData.cs) and forwards changes to [CollectedGrapeView.cs](Assets/Scripts/MVP/Views/Ingame/CollectedGrapeView.cs).
- **Custom Physics Engine:** Programmed ground detection, snap offsets, and custom gravity forces inside [CustomPhysics.cs](Assets/Scripts/Player/CustomPhysics.cs) without relying on Unity's default 3D Rigidbody physics.
- **Infinite Ground Repetition:** Built an axis-based repeating system in [AutoRepeat.cs](Assets/Scripts/Objects/AutoRepeat.cs) to repeat terrain/scenery assets based on mesh sizes.
- **ScriptableObject Architecture:** Saved configurable options such as music/SFX toggles inside [SettingsData.cs](Assets/Scripts/ScriptableObjects/SettingsData.cs), automatically persisting choices using `PlayerPrefs`.
- **Advanced Object Pooling:** Built a flexible, multi-category pooling component [PoolManager.cs](Assets/Scripts/Managers/PoolManager.cs) that handles dynamic instantiation and reuse of particles, obstacle assets, and floating indicator UIs ([IncrementText.cs](Assets/Scripts/UI/IncrementText.cs)).
- **Interactive UI Elements:** Created reusable custom wrappers [CustomButton.cs](Assets/Scripts/UI/CustomButton.cs) and [CustomToggle.cs](Assets/Scripts/UI/CustomToggle.cs) supporting click delays and dynamic sprite swaps based on active toggle states.
- **Dynamic Floating Oscillations:** Added sinus-wave height deviations to collectable pickups using [FloatingObject.cs](Assets/Scripts/Objects/FloatingObject.cs) to make items spin and bob attractively.

## Project Structure

```text
Personal Project/
|-- Assets/
|   |-- Animations/
|   |-- Audios/
|   |-- Fonts/
|   |-- Materials/
|   |-- Models/
|   |-- Prefabs/
|   |   |-- Background/
|   |   |-- Characters/
|   |   |-- Collectibles/
|   |   |-- Trains/
|   |   `-- UI/
|   |-- Scenes/
|   |   |-- MainMenu.unity
|   |   `-- Gameplay.unity
|   |-- Settings/
|   |-- Sprites/
|   |-- TextMesh Pro/
|   `-- Scripts/
|       |-- Camera/
|       |   |-- FollowTarget.cs
|       |   `-- LookAtCamera.cs
|       |-- Events/
|       |   |-- GameEvents.cs
|       |   |-- PlayerEvents.cs
|       |   `-- UIEvents.cs
|       |-- MVP/
|       |   |-- Presenters/
|       |   |   |-- CollectedGrapePresenter.cs
|       |   |   `-- SettingsPanelPresenter.cs
|       |   `-- Views/
|       |       |-- Ingame/
|       |       |   |-- CollectedGrapeView.cs
|       |       |   |-- GameplayView.cs
|       |       |   |-- PauseMenuButtonsView.cs
|       |       |   `-- PauseMenuView.cs
|       |       |-- MainMenu/
|       |       `-- SettingsPanelView.cs
|       |-- Managers/
|       |   |-- AudioManager.cs
|       |   |-- GameManager.cs
|       |   |-- LevelSpawner.cs
|       |   |-- PoolManager.cs
|       |   `-- SceneLoader.cs
|       |-- Objects/
|       |   |-- Collectibles/
|       |   |   |-- Collectible.cs
|       |   |   |-- GrapeCollectible.cs
|       |   |   `-- PoisonCollectible.cs
|       |   |-- AutoRepeat.cs
|       |   |-- Deadbox.cs
|       |   |-- FloatingObject.cs
|       |   |-- Hitbox.cs
|       |   |-- IPoolObject.cs
|       |   |-- MovingObject.cs
|       |   |-- MovingObjectsDespawnZone.cs
|       |   |-- PooledParticle.cs
|       |   `-- Train.cs
|       |-- Player/
|       |   |-- CollectiblesDetector.cs
|       |   |-- CustomPhysics.cs
|       |   |-- IDamageable.cs
|       |   |-- IHealable.cs
|       |   |-- IHittable.cs
|       |   |-- PlayerController.cs
|       |   `-- PlayerHealth.cs
|       `-- UI/
|           |-- CustomButton.cs
|           |-- CustomToggle.cs
|           |-- IncrementText.cs
|           |-- IngameUI.cs
|           `-- MainMenuUI.cs
|-- Packages/
`-- ProjectSettings/
```

## Play Online

[Play Tail Rush on Itch.io](https://nguyenthanhvu.itch.io/tail-rush)

## How to Run Locally

1. Open the project in Unity Editor (compatible with Unity 6 / Unity 6000+).
2. Open the scene `Assets/Scenes/MainMenu.unity`.
3. Press **Play** in the editor.
4. Click the Play button in the UI to launch gameplay. Use Left/Right keys (or keys mapped under `InputSystem_Actions`) to change lanes, Jump, and Drop.
