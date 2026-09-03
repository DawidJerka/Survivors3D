# 3D Sci-Fi Survivor

> A stylized 3D survivor-like action game developed in Unity 6 as a portfolio project.

![Gameplay](Docs/Media/gameplay.gif)

---

## Overview

**3D Sci-Fi Survivor** is a small 3D action game inspired by the survivor-like genre.

The player controls a robotic character inside a futuristic arena while increasingly difficult waves of enemies approach from all directions.

Combat is mostly automatic. The player's role is to:

- move and position efficiently,
- avoid incoming enemies,
- collect experience,
- choose upgrades,
- combine weapons and passive items,
- survive increasing enemy pressure.

The project was created primarily to practice and demonstrate gameplay programming, Unity architecture, data-driven design, reusable systems, animation integration, audio systems and visual polish.

The current goal is a polished vertical slice rather than a large amount of content.

---

## Gameplay

The core gameplay loop is fully playable:

1. The player starts with an automatic weapon.
2. Enemies continuously spawn around the player.
3. Weapons attack enemies automatically.
4. Defeated enemies drop experience gems.
5. Nearby experience gems are attracted toward the player.
6. Collecting enough XP increases the player level.
7. The game pauses and presents several random upgrade choices.
8. The player selects a new item or upgrades an existing one.
9. Enemy density and statistics increase as the run progresses.
10. The run ends when the player's health reaches zero.

---

# Features

## Player Movement

The player uses Rigidbody-based movement on the XZ plane.

The movement system supports:

- WASD input,
- normalized diagonal movement,
- Rigidbody interpolation,
- movement speed modifiers,
- smooth visual rotation,
- movement-independent physics rotation.

The gameplay object and visual model are deliberately separated.

```text
Player
├── Rigidbody
├── Collider
├── PlayerMovement
├── Health
├── PlayerDamageReceiver
├── PlayerStats
├── PlayerExperience
├── PlayerLoadout
├── EnemyTargeting
├── WeaponManager
│
├── Visuals
│   └── PlayerModel
│
└── WeaponContainer
```

The Rigidbody, collider and gameplay systems remain independent from the character model.

This makes it possible to replace or modify the player's visual representation without affecting movement, collision or combat logic.

---

## Player Animation

The player currently supports:

- Idle animation,
- Run animation,
- transitions based on actual Rigidbody velocity,
- smooth rotation toward movement direction,
- animation playback speed scaling with movement speed.

Root Motion is disabled.

The Rigidbody remains the authoritative source of player movement.

Because animation speed is derived from the player's real movement velocity, movement-speed upgrades also affect the running animation cadence.

---

# Combat

Combat uses an extensible weapon architecture.

```text
Weapon
├── TimedWeapon
│   └── MagicMissileWeapon
│
└── OrbitingBladesWeapon
```

`Weapon` provides shared initialization and upgrade functionality.

`TimedWeapon` provides reusable cooldown-based behaviour for weapons that attack periodically.

Persistent weapons such as Orbiting Blades derive directly from `Weapon`.

This allows weapons with significantly different behaviours to use the same loadout and level-up systems.

---

## Magic Missile

The player starts each run with **Magic Missile**.

Magic Missile:

- automatically searches for the nearest enemy,
- fires toward the selected target,
- uses a Rigidbody projectile,
- deals damage on collision,
- destroys itself after impact,
- plays an impact sound,
- scales attack speed with weapon level.

Its current upgrade progression focuses on attack speed.

Example:

```text
Level 1
Attack Speed: 1.00/s

Level 2
+15% Attack Speed

Level 3
+15% Attack Speed
```

The weapon uses its own projectile spawn point inside the weapon prefab.

---

## Orbiting Blades

Orbiting Blades is a persistent weapon that creates blades rotating around the player.

Features include:

- configurable blade count,
- configurable orbit radius,
- configurable orbit height,
- configurable rotation speed,
- evenly distributed blades,
- additional blades gained through upgrades,
- damage applied once per enemy contact/pass,
- impact sound effects,
- emissive visual effects,
- particle effects.

Each blade tracks enemies currently touching it.

This prevents damage from being applied every physics frame while still allowing the same enemy to be damaged again after leaving and re-entering the blade collider.

The current visual version uses a stylized cyber sword model with custom sci-fi VFX.

---

# Health and Damage

The project uses a reusable `Health` component for both the player and enemies.

It supports:

- configurable base maximum health,
- current health,
- maximum health multipliers,
- damage,
- death events,
- health change events.

Player damage is handled by `PlayerDamageReceiver`.

After receiving damage, the player gains a short invulnerability period.

This prevents contact damage from being applied continuously every physics frame while overlapping an enemy.

---

## Game Over

When player health reaches zero:

- gameplay pauses,
- the Game Over UI appears,
- the player can restart the run,
- the current scene is reloaded.

---

# Experience System

Enemies drop experience gems when defeated.

Each gem contains a configurable XP value.

Experience gems are attracted toward the player when entering the player's attraction radius.

```text
Experience Gem
      ↓
Player enters attraction range
      ↓
Gem starts moving toward Player
      ↓
Gem accelerates
      ↓
Gem reaches Player
      ↓
Experience is awarded
      ↓
Pickup SFX is played
```

The attraction radius is based on player statistics.

This makes the system ready for a future passive item that increases pickup range.

---

# Level and Progression System

`PlayerExperience` tracks:

- current level,
- current XP,
- XP required for the next level.

The XP requirement increases after every level.

The implementation also supports receiving enough XP to gain multiple levels in a short period.

---

## Level-Up Selection

When the player gains a level:

```text
Gameplay
   ↓
Time.timeScale = 0
   ↓
Generate eligible upgrades
   ↓
Choose random options
   ↓
Display Level Up UI
   ↓
Player selects an option
   ↓
Apply upgrade
   ↓
Resume gameplay
```

The Level Up UI is dynamically generated from reusable card prefabs.

---

## Item Pool

All level-up items derive from:

```text
LevelUpItemData
```

Two item categories currently exist:

```text
Weapon
Passive
```

The level-up system understands whether an item:

- has not yet been acquired,
- is already owned,
- can still be upgraded,
- has reached its maximum level,
- can be added without exceeding the category slot limit.

This allows new weapons and passive items to be introduced mainly through ScriptableObjects instead of modifying the level-up system itself.

---

# Passive Items

Passive items derive from:

```text
PassiveData
```

Each passive applies modifiers through `PlayerStats`.

Current passive items include:

## Mechanical Boots

Increases movement speed.

Example:

```text
+10% Movement Speed per level
```

Movement speed affects both:

- actual Rigidbody movement,
- running animation playback speed.

---

## Robo Heavy Armor

Increases maximum player health.

Example:

```text
+20% Max Health per level
```

Increasing maximum health also increases current health by the amount gained from the upgrade.

---

## Player Statistics

Current calculated player statistics include:

```text
MoveSpeedMultiplier
MaxHealthMultiplier
PickupRangeMultiplier
```

Passive modifiers are recalculated from currently owned items whenever a passive upgrade changes.

This avoids permanently modifying base values and keeps derived statistics predictable.

---

# Enemy System

Enemies use a reusable data-driven architecture.

Each enemy prefab contains shared gameplay components:

```text
Enemy
├── Rigidbody
├── Collider
├── Health
├── EnemyMovement
├── EnemyContactDamage
├── EnemyInitializer
├── EnemyDeath
│
└── Visuals
    └── EnemyModel
```

As with the Player, the enemy visual model is separated from the gameplay root.

---

## Enemy Movement

Enemies currently use simple direct pursuit.

```text
Enemy
   ↓
Calculate direction toward Player
   ↓
Normalize direction
   ↓
Set Rigidbody velocity
```

This approach was intentionally chosen for the current open-arena prototype.

There is currently no NavMesh or obstacle avoidance.

For this reason, gameplay-critical obstacles inside the arena are intentionally limited.

---

## Enemy Data

Enemy statistics are stored in `EnemyData` ScriptableObjects.

Each enemy definition contains:

```text
Max Health
Move Speed
Contact Damage
```

Multiple enemy prefabs can therefore reuse the same gameplay code while having different statistics and visual models.

---

## Current Enemy Types

### Basic Enemy

A general-purpose enemy with balanced:

- health,
- movement speed,
- contact damage.

### Runner

A lighter enemy focused on mobility.

Compared with the Basic Enemy it has:

- lower health,
- higher movement speed,
- lower contact damage,
- a visually distinct robot model.

The architecture is ready for additional enemy types such as:

```text
Tank
Elite
Ranged
Boss
```

without requiring separate movement implementations.

---

# Enemy Animation

Enemies use a shared visual controller.

`EnemyVisualController` handles:

- Idle/Run animation switching,
- rotation toward movement direction,
- animation speed based on actual Rigidbody velocity,
- visual rotation independent of physical rotation.

Individual enemy types can define their own reference animation speed.

Example:

```text
Basic Enemy
Base Speed: 3.0
Animation Reference Speed: 3.0

Runner
Base Speed: 5.5
Animation Reference Speed: 5.5
```

Difficulty modifiers can increase movement speed while animation playback automatically adjusts to match.

Humanoid animation retargeting is used for compatible character models.

---

# Enemy Spawning

`EnemySpawner` continuously generates enemies around the player.

Features include:

- configurable spawn distance,
- configurable spawn interval,
- configurable maximum enemy count,
- multiple enemy types,
- weighted random enemy selection.

Example:

```text
Basic Enemy: 4
Runner:      1
```

The spawner also tracks currently alive enemies to prevent exceeding the configured limit.

---

# Difficulty System

Difficulty is controlled by `GameDirector`.

The run is divided into configurable stages represented by `DifficultyStageData` ScriptableObjects.

Each stage can modify:

```text
Start Time
Spawn Interval
Maximum Enemy Count
Available Enemy Types
Enemy Spawn Weights
Enemy Health Multiplier
Enemy Speed Multiplier
Enemy Damage Multiplier
```

Example progression:

```text
Stage 1
0:00
Slow spawning
Basic Enemy only

Stage 2
1:00
Faster spawning
Runner introduced

Stage 3
2:00
More enemies
Higher enemy statistics

Stage 4
3:00
High enemy density
Additional stat scaling
```

Difficulty multipliers are applied when enemies spawn.

Existing enemies retain the statistics they had when they were created.

---

# Run Timer

The UI displays elapsed run time using `GameDirector.ElapsedTime`.

Example:

```text
00:00
01:37
04:12
```

The timer naturally pauses during:

- Level Up screens,
- Game Over,

because those systems pause gameplay through `Time.timeScale`.

---

# User Interface

The current UI includes:

- player health bar,
- health values,
- experience bar,
- current player level,
- current / required XP,
- run timer,
- dynamic Level Up screen,
- Game Over screen,
- restart button.

---

## Dynamic Level-Up Cards

Upgrade cards are instantiated from a reusable prefab.

Each card can display:

```text
Icon
Item Name
NEW / Current Level → Next Level
Upgrade Description
```

The UI does not contain a fixed set of permanently configured buttons.

Instead:

```text
LevelUpManager
      ↓
Generates options
      ↓
LevelUpUI
      ↓
Instantiates LevelUpOptionUI prefabs
```

This makes the number of displayed choices configurable without restructuring the UI.

---

# Audio

The project uses Unity's **Audio Random Containers** for repeated sound effects.

Current SFX categories include:

```text
Player footsteps
Enemy footsteps
Player hit
Experience pickup
Magic Missile impact
Orbiting Blade impact
```

Audio Random Containers provide variation through:

- multiple audio clips,
- randomized selection,
- repetition avoidance,
- pitch variation,
- volume variation.

This prevents repeated effects such as footsteps from sounding identical.

---

## Central Audio Manager

Sound playback is centralized through `AudioManager`.

Gameplay objects do not keep their own persistent SFX AudioSources.

Instead, the manager owns pools of reusable emitters:

```text
AudioManager
├── 2D SFX Emitters
└── 3D SFX Emitters
```

Gameplay objects request sounds from the manager.

Example:

```text
Enemy animation event
        ↓
AudioManager.Play3D()
        ↓
Find available emitter
        ↓
Move emitter to enemy position
        ↓
Assign Audio Random Container
        ↓
Play SFX
```

This architecture provides several benefits:

- avoids large numbers of AudioSources attached to enemies,
- allows multiple effects to overlap,
- centralizes audio configuration,
- supports positional 3D audio,
- simplifies projectile impact sounds,
- prevents an impact sound from being destroyed together with its projectile,
- prepares the project for future Audio Mixer integration.

---

# Animation Events

Footsteps are triggered directly from character animation clips.

Animation clips invoke:

```text
OnFootstep
```

which is handled by `AnimationFootstepEvents`.

The same system is reused by:

```text
Player
Basic Enemy
Runner
Future animated enemies
```

Each character can use a different Audio Random Container while sharing the same event-handling code.

---

# Arena

The arena separates gameplay collision from visual geometry.

```text
Arena
├── FloorCollision
├── FloorVisuals
├── Border
├── Decorations
└── Background
```

`FloorCollision` is a single large collider.

`FloorVisuals` contains multiple visual floor panels without individual gameplay colliders.

This keeps the physics representation simple.

---

## Floor Tiles

The arena floor consists of a grid of sci-fi metal panels.

Visual tiles:

- use different metal materials,
- contain small gaps between panels,
- do not contain gameplay colliders,
- sit slightly above the invisible collision floor.

The environment uses stylized metallic materials and futuristic visual accents.

---

## Arena Art Direction

The visual direction is a colorful, stylized sci-fi action game.

The intended visual language includes:

- readable silhouettes,
- exaggerated mechanical shapes,
- cool metallic surfaces,
- orange technological accents,
- emissive weapon elements,
- futuristic arena panels,
- energetic VFX.

Gameplay readability is prioritized over realism.

---

# Visual Effects

The current visual pass includes:

- emissive weapon materials,
- particle effects,
- weapon motion effects,
- sci-fi energy accents.

Orbiting Blades use additional effects to make the weapon readable while moving quickly around the player.

Further post-processing and environmental effects are planned.

---

# ScriptableObject Architecture

Several gameplay systems use ScriptableObjects to separate configuration from runtime behaviour.

Current ScriptableObject types include:

```text
EnemyData
DifficultyStageData
LevelUpItemData
WeaponData
MagicMissileData
OrbitingBladesData
PassiveData
MovementSpeedPassiveData
MaxHealthPassiveData
```

This makes gameplay values editable directly from the Unity Inspector without hard-coding them into runtime behaviours.

---

# Project Architecture

Simplified dependency overview:

```text
                 GameDirector
                      │
                      ▼
                EnemySpawner
                      │
                      ▼
                   Enemy
                      │
                      ▼
                ExperienceGem
                      │
                      ▼
Player ───────► PlayerExperience
  │                   │
  │                   ▼
  │              LevelUpManager
  │                   │
  │                   ▼
  │               LevelUpUI
  │
  ├────────► PlayerLoadout
  │               │
  │               ├────► WeaponManager
  │               │
  │               └────► PassiveManager
  │
  ├────────► PlayerStats
  │
  └────────► AudioManager
```

The project aims to keep individual gameplay components focused on a single responsibility.

---

# Controls

| Action | Input |
| --- | --- |
| Move Forward | `W` |
| Move Backward | `S` |
| Move Left | `A` |
| Move Right | `D` |
| Select Upgrade | Mouse |
| Restart after Game Over | UI button |

Combat is automatic.

---

# Technologies

The project currently uses:

- **Unity 6**
- **Universal Render Pipeline (URP)**
- **C#**
- **Unity Input System**
- **Rigidbody Physics**
- **TextMeshPro**
- **Unity UI**
- **Animator**
- **Humanoid Animation Retargeting**
- **ScriptableObjects**
- **Audio Random Containers**
- **AudioSource pooling**
- **Particle System**
- **URP Lit / Unlit materials**
- **Emission-based VFX**

---

# Third-Party Assets

The project uses selected third-party assets for visual content.

Gameplay systems, behaviours and integrations are implemented independently from those assets.

## Player

**Robot Kyle URP**

Used as the player's visual model.

Movement, collision, combat, statistics, animation control and progression are handled by the project's own systems.

## Enemies

**3D Character Sci-Fi Robots Bundle**

Used for enemy visual models.

Enemy movement, health, damage, animation control, spawning and difficulty scaling are implemented by the project.

## Orbiting Blades

**Lowpoly Cyber Ninja Sword**

Used as the visual basis for the Orbiting Blades weapon.

Orbit logic, collision, damage, upgrades, emissive materials and additional VFX are implemented separately.

## Environment

Selected free metal materials are used as part of the arena visual pass.

> Third-party assets remain subject to their respective licenses and are not intended to be redistributed independently from this project.

---

# Current Development Status

The project currently has a complete core gameplay loop and is transitioning from prototype visuals toward a more polished vertical slice.

## Implemented

- Player movement
- Rigidbody interpolation
- Player animation
- Visual character rotation
- Animation speed scaling
- Health and damage system
- Invulnerability frames
- Player health UI
- Game Over
- Restart system
- Automatic enemy targeting
- Extensible weapon architecture
- Magic Missile
- Orbiting Blades
- Weapon upgrades
- Experience drops
- XP attraction
- XP pickup audio
- Player levels
- Dynamic Level Up selection
- Starting weapon
- Passive item architecture
- Movement Speed passive
- Max Health passive
- Player stat modifiers
- Enemy ScriptableObjects
- Basic Enemy
- Runner Enemy
- Enemy animation
- Enemy visual rotation
- Weighted enemy spawning
- Time-based difficulty stages
- Enemy stat scaling
- Run timer
- Experience UI
- Dynamic upgrade cards
- Sci-fi arena prototype
- Player visual model
- Enemy visual models
- Weapon VFX
- Emissive weapon materials
- Particle effects
- Player footsteps
- Enemy footsteps
- Player hit SFX
- Magic Missile impact SFX
- Orbiting Blade impact SFX
- Audio Random Containers
- Central pooled AudioManager
- Positional 3D SFX

---

# Planned Improvements

## Gameplay

- additional weapons,
- additional passive items,
- pickup range passive,
- additional enemy archetypes,
- elite enemies,
- bosses,
- weapon evolution / synergy system,
- longer difficulty progression.

## Technical

- enemy object pooling,
- projectile pooling,
- experience gem pooling,
- VFX pooling,
- optimized enemy targeting,
- centralized enemy registry,
- improved spawn bounds,
- improved enemy steering,
- Audio Mixer integration.

## Presentation

- additional arena decorations,
- environmental background,
- improved lighting,
- Bloom and post-processing,
- additional weapon VFX,
- enemy hit feedback,
- damage flashes,
- enemy death effects,
- additional UI polish,
- camera feedback,
- additional gameplay juice.

---

# Known Technical Limitations

The current version intentionally keeps some systems relatively simple while validating the gameplay loop.

## Enemy Targeting

Magic Missile currently searches for enemies using scene tag queries.

This is sufficient for the current vertical slice but should eventually be replaced by a centralized enemy registry or another more scalable targeting solution.

---

## Object Creation

Several gameplay objects currently use `Instantiate` and `Destroy`.

Future versions should introduce pooling for:

```text
Enemies
Projectiles
Experience Gems
VFX
```

The audio system already uses pooled emitters.

---

## Enemy Navigation

Enemy movement currently follows a direct vector toward the player.

There is no obstacle avoidance or navigation system.

The arena is therefore intentionally designed with a mostly open playable area.

---

## Spawn Bounds

Enemies currently spawn around the player based primarily on distance.

A future version should constrain spawn positions to arena boundaries and ensure enemies always appear in valid gameplay locations.

---

# Design Goals

The project is designed around several development goals.

## 1. Reusable Systems

Weapons, passive items, enemy statistics and audio use shared systems rather than one-off implementations.

## 2. Data-Driven Configuration

ScriptableObjects hold gameplay configuration where practical.

## 3. Separation of Gameplay and Presentation

Character models and animation are separated from Rigidbody-driven gameplay roots.

## 4. Extensibility

Adding a new weapon, passive item or enemy type should require minimal changes to existing systems.

## 5. Gameplay Readability

Enemy silhouettes, weapon VFX, UI and audio are designed to communicate gameplay information clearly.

## 6. Portfolio-Quality Vertical Slice

The objective is not maximum content quantity.

The focus is on demonstrating a complete set of interconnected gameplay systems and the ability to move a prototype toward a polished game experience.

---

# Running the Project

1. Clone the repository.
2. Open the project using the compatible Unity 6 version.
3. Allow Unity to import all project assets and packages.
4. Import any required third-party Asset Store packages that are not included in the repository.
5. Open the main gameplay scene.
6. Press **Play**.

Some third-party Asset Store content may need to be imported separately depending on repository distribution and asset licensing requirements.

---

# Author

**Dawid Jerka**

Unity / C# portfolio project.