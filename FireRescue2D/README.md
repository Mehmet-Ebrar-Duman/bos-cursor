## Fire Rescue 2D (Unity C#)

A 2D firefighting game prototype for post-wildfire education and awareness. Extinguish fires, rescue animals, protect trees, collect power-ups, and redeem points for seeds/saplings to reforest.

### Quick Start

1) Requirements
- Unity 2021 LTS or newer (2D template recommended)

2) Setup
- Create a new 2D project in Unity.
- Close the Unity Editor.
- Copy the `Assets` folder from this repository into your Unity project's root (replace/merge).
- Reopen the project in Unity.

3) Scene and Prefabs (minimal manual setup)
- Create an empty scene and save it as `Main`.
- Create empty GameObjects and add the scripts below:
  - `GameManager` (add `GameManager.cs`)
  - `UIManager` (add `UIManager.cs`), assign Text UI references
  - `Player` (add `PlayerController.cs` and a `WaterStream` child object with `WaterStream.cs`)
  - `FireSpawner` (add `FireSpawner.cs`)
  - `Shop` (optional, add `Shop.cs`)
  - `PlantingSystem` (optional, add `PlantingSystem.cs`)

4) Sprites and Colliders
- Add simple 2D sprites (square/circle) for player, trees, animals, fires, power-ups.
- Ensure `Fire`, `Tree`, `Animal`, `PowerUp`, `SafeZone` have appropriate 2D colliders (trigger for pickups/zones). 

5) Layers & Tags (recommended)
- Tags: `Player`, `Fire`, `Tree`, `Animal`, `PowerUp`, `SafeZone`.
- Layers: optional for collision filtering.

6) Controls
- Move: WASD or Arrow Keys
- Spray water: Left Mouse Button (hold)
- Toggle Plant Mode: P (when you own seeds/saplings)
- Interact with Shop: E (inside shop trigger)

7) Gameplay Loop
- Extinguish fires to earn score.
- Rescue animals by guiding them to `SafeZone`.
- Collect power-ups (water refill, score boost, speed boost).
- Buy seeds/saplings in the shop with score; plant new trees to restore forest.

8) Notes
- Some fields in scripts are `[SerializeField]` and must be wired in the Inspector (e.g., UI Text, prefabs, radii, speeds).
- Fire spreading here is simplified; tune parameters to your needs.

### Folder Structure

```
Assets/
  Scripts/
    Managers/
      GameManager.cs
      UIManager.cs
    Player/
      PlayerController.cs
      WaterStream.cs
    World/
      Fire.cs
      FireSpawner.cs
      Tree.cs
      Animal.cs
      SafeZone.cs
    Items/
      PowerUp.cs
    Economy/
      Shop.cs
      PlantingSystem.cs
```

### Licensing
This code is provided as-is for educational purposes under the MIT License. Provide your own art/audio.

