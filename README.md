# Arena Survival - Unity

Arena Survival is a 2D top-down survival shooter prototype built in Unity and C#.

The project focuses on gameplay programming systems such as player movement, mouse aiming, shooting, enemy waves, health/damage logic, object pooling, pickups, UI feedback, audio feedback, arena boundaries, pause/menu flow, and game state management.

This project was developed as part of my Unity learning path to build a stronger portfolio for junior/intern gameplay programming roles.

---

## Gameplay Overview

The player must survive enemy waves inside an arena.

Enemies spawn in waves, chase the player, and deal damage on contact. The player can move, aim with the mouse, shoot projectiles, collect pickups, pause the game, and survive as many waves as possible.

---

## Screenshots

### Main Menu
![Main Menu](docs/screenshots/main-menu.png)

### Gameplay
![Gameplay](docs/screenshots/gameplay.png)

### Game Over
![Game Over](docs/screenshots/game-over.png)

### Pause Menu
![Pause Menu](docs/screenshots/pause-menu.png)
---

## Controls

| Action | Input |
|---|---|
| Move | WASD / Arrow Keys |
| Aim | Mouse |
| Shoot | Left Mouse Button |
| Pause / Resume | Esc |
| Restart after Game Over | R |

---

## Main Features

- Top-down 2D player movement
- Mouse-based aiming
- Projectile shooting system
- Projectile object pooling
- Enemy chase behavior
- Wave-based enemy spawning
- Wave announcement UI
- Health and damage system
- Player health UI
- Current wave UI
- Enemies alive UI
- Health pickup
- Temporary fire rate power-up
- Game Over system
- Pause menu
- Main menu
- Audio feedback
- Damage flash feedback
- Collision layers
- Arena boundaries

---

## Technical Highlights

### Modular Gameplay Systems

The project is organized around small scripts with clear responsibilities.

For example:

- `PlayerMovement` handles only player movement.
- `PlayerAim` handles mouse aiming.
- `PlayerShooting` handles shooting logic and projectile pooling.
- `Projectile` controls projectile movement and lifetime.
- `Health` manages health, damage, healing, and death events.
- `DamageDealer` applies damage to objects with a `Health` component.
- `EnemyChase` handles enemy movement toward the player.
- `EnemySpawner` manages wave progression and enemy spawning.
- `PickupSpawner` manages pickup spawning.
- `GameManager` handles Game Over and restart logic.
- `PauseMenu` handles pause, resume, main menu, and quit actions.
- `AudioManager` centralizes gameplay sound effects.

This structure keeps the project easier to maintain, debug, and extend.

---

### Health and Damage System

The project uses a reusable `Health` component for both the player and enemies.

Damage is handled through a separate `DamageDealer` component, allowing bullets and enemies to apply damage in different ways.

Examples:

- Bullets damage enemies and return to the object pool.
- Enemies damage the player repeatedly while in contact.
- Health pickups restore player health without duplicating health logic.
- Death events are used to trigger Game Over or update the enemy wave system.

---

### Object Pooling

Projectiles use an object pool instead of constantly using `Instantiate` and `Destroy`.

This improves runtime performance and demonstrates a more professional approach to repeated projectile spawning.

Instead of creating and destroying bullets every time the player shoots, the game reuses inactive projectile objects from a pool.

---

### Wave System

Enemies spawn in waves using a simple scaling formula:

```csharp
int enemiesToSpawn = baseEnemiesPerWave + (currentWave - 1) * enemiesAddedPerWave;
```

This allows each wave to increase difficulty progressively.

Example:

- Wave 1: 3 enemies
- Wave 2: 5 enemies
- Wave 3: 7 enemies
- Wave 4: 9 enemies

A wave announcement appears before each wave starts, showing the wave number and how many enemies will spawn.

---

### Pickups and Power-Ups

The game includes two pickup types:

- Health Pickup: restores player health.
- Fire Rate Pickup: temporarily increases the player's shooting speed.

The pickup spawner supports multiple pickup prefabs and prevents the same pickup type from appearing repeatedly in a row. This keeps the gameplay clearer for short demos and portfolio videos.

---

### UI and Game State

The game includes:

- Main menu
- Pause menu
- Game Over screen
- Player HP display
- Current wave display
- Enemies alive display
- Wave announcement message

The UI is updated through events where appropriate, keeping UI logic separated from core gameplay logic.

---

### Collision Layers

The project uses Unity layers to control valid interactions between objects.

Examples:

- Player projectiles do not damage the player.
- Enemies do not damage each other.
- Pickups only interact with the player.
- Arena boundaries prevent the player and enemies from leaving the play area.

This prevents unwanted interactions and keeps damage behavior predictable.

---

### Audio and Visual Feedback

The game includes basic audio feedback for:

- Shooting
- Taking damage
- Collecting pickups
- Game Over

It also includes damage flash feedback, making it easier for the player to understand when the player or enemies take damage.

---

## Project Structure

```text
Assets/
└── _Project/
    ├── Art/
    ├── Audio/
    ├── Materials/
    ├── Prefabs/
    ├── Scenes/
    ├── Scripts/
    │   ├── Combat/
    │   ├── Core/
    │   ├── Enemies/
    │   ├── Pickups/
    │   ├── Player/
    │   ├── Spawning/
    │   └── UI/
    └── UI/
```

---

## Scenes

| Scene | Description |
|---|---|
| `MainMenu` | Main menu with Start and Quit options |
| `Main` | Main gameplay scene |

---

## How to Run

1. Open the project in Unity.
2. Use Unity version `6000.4.0f1` or a compatible Unity 6 version.
3. Open the `MainMenu` scene.
4. Press Play.

For a Windows build, run the generated `.exe` file from the build folder.

---

## Build Information

Recommended build target:

- Windows

The build starts from the `MainMenu` scene and loads the gameplay scene from the Start button.

Make sure the scenes are included in the Build Profile in this order:

```text
0 MainMenu
1 Main
```

---

## What I Learned

Through this project, I practiced:

- Unity 2D workflow
- Rigidbody2D movement
- Mouse aiming
- Projectile systems
- Object pooling
- Enemy spawning
- Wave progression
- Unity events
- Gameplay UI
- Audio integration
- Collision layers
- Arena boundaries
- Scene management
- Pause and Game Over flow
- Basic game state management
- Building a playable Windows version

---

## Future Improvements

Possible future improvements:

- Add different enemy types
- Add more power-ups
- Improve enemy movement behavior
- Add visual effects for shooting and enemy death
- Add background music and volume settings
- Add score system
- Add difficulty balancing
- Add better art assets
- Add screen shake for stronger feedback

---

## Status

Version 1.0 - Functional gameplay prototype.

The project is complete as a portfolio prototype and is focused on demonstrating gameplay programming fundamentals in Unity and C#.
