# Boating Through

**Boating Through** is a fast-paced, infinite runner-style arcade game built in **Unity**.

## Gameplay

Guide your boat through a procedurally generated path of tiles. The path zig-zags, and one wrong turn sends you sinking into the abyss.

## How to Play

1. **Start**: Click anywhere to begin.
2. **Steer**: Click (or tap) to switch direction. The boat alternates between moving **Forward** and **Left**.
3. **Goal**: Survive as long as possible and beat your high score.

## Features

- **One-Tap Controls**: Simple, addictive gameplay mechanics.
- **Dynamic Path**: The course is generated as you play.
- **Graphics**: Features a vibrant cel-shaded art style and stylized water rendering for a unique visual aesthetic.


## Project Structure

- `Assets/Scripts/GameManager.cs`: Handles game state (Menu, Playing, Game Over), scoring, and time manipulation.
- `Assets/Scripts/Player.cs`: Manages boat movement, input handling, and "sinking" physics.
- `Assets/Scripts/TileManager.cs`: (Inferred) Likely handles the spawning of new path tiles.

## Script Reference

### Core Logic
- **`GameManager.cs`**: The central singleton. Manages high-level game states (Menu, Playing, GameOver), coordinates the slow-motion death effect, and handles the auto-restart timer.
- **`Player.cs`**: Controls the boat. Handles the "tap-to-turn" input logic, constant forward movement, and the raycast-based ground check that triggers the sinking mechanic.
- **`TileManager.cs`**: Procedural generation system. Uses **Object Pooling** (`Stack<GameObject>`) to efficiently recycle path tiles (Top/Left variants) and randomly spawns pickups.

### Systems
- **`UIManager.cs`**: Updates the HUD (Score) and Game Over screens (Restart countdown).
- **`AudioManager.cs` / `Sound.cs`**: A custom audio system for playing SFX and Music.
- **`Pickups.cs`**: Handles the floating collectibles, adding score upon collision with the player.
- **`Tile.cs`**: Logic for individual path segments (e.g., attach points).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
Copyright (c) 2026 ARGUS