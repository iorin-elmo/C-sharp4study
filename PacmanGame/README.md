# Pacman Game

## Overview
This project is a simple console-based implementation of the classic Pacman game. The player navigates through a maze, collecting food while avoiding enemies. The game ends when the player either collects all the food or collides with an enemy.

## Project Structure
- **Program.cs**: Entry point of the application. Initializes the game and handles the main game loop.
- **Entities**:
  - **Player.cs**: Contains the `Player` class, which manages player movement and interactions.
  - **Enemy.cs**: Contains the `Enemy` class, which manages enemy movement and behavior.
  - **Entity.cs**: Abstract class that defines common properties and methods for all game entities.
- **Utils**:
  - **MapUtils.cs**: Contains methods for creating the game map and checking for walls and T-shape points.
  - **Keyboard.cs**: Provides functionality to check if specific keys are pressed.
- **Info.cs**: Holds global game state variables such as dimensions of the game field, food count, and flags for win and death conditions.

## How to Run the Game
1. Clone the repository to your local machine.
2. Open the project in your preferred C# development environment.
3. Build the project to restore dependencies.
4. Run the `Program.cs` file to start the game.

## Features
- Player movement using arrow keys.
- Enemy movement with random direction changes.
- Collectible food items that decrease the food count.
- Game over conditions based on player collisions with enemies or successful food collection.

Enjoy playing the game!