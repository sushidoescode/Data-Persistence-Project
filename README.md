Data Persistence Project (Brick Game)

Overview
A polished, arcade-style Breakout game built in Unity. This project was developed to master Data Persistence, demonstrating how to save and load data dynamically both across different game scenes and between entirely separate play sessions.

Key Features
Cross-Scene Data Persistence: Utilizes a Singleton pattern (DontDestroyOnLoad) to carry the player's inputted name from the Main Menu directly into the active gameplay scene.

Cross-Session Data Persistence (JSON): Features a fully functional save system using JsonUtility and System.IO. High scores are serialized and written to a local .json file, ensuring data survives even when the application is completely closed and restarted.

Top 5 Dynamic Leaderboard: Implements C# System.Linq to seamlessly zip, sort, and trim player names and scores into a permanent Top 5 Leaderboard displayed on the main menu.

Complete Game Loop: Includes a fully functional UI flow (Menu -> Gameplay -> Win/Loss State -> Menu), complete with a dynamic "Bricks Remaining" win condition.

Technical Architecture
DataManager.cs: The core Singleton vault. Handles the serialization of lists and the saving/loading of the JSON file to Application.persistentDataPath.

MainManager.cs: Controls the active gameplay loop, ball physics, brick spawning, and triggers the Win/Loss states while communicating with the DataManager to log scores.

MenuUIHandler.cs: Manages scene transitions, dynamically populates the visual Top 5 Leaderboard on boot, and handles safe application quitting (both in-editor and in-build).

How to Play
Launch the game and enter your pilot name on the Main Menu.

Review the Top 5 Leaderboard to see the scores you need to beat!

Click Start to load into the arena.

Use the Left and Right Arrow Keys (or A/D) to move the paddle.

Prevent the ball from hitting the floor and destroy all the glowing bricks to achieve victory.

Return to the Main Menu to see if you made the Top 5!

Author
Sushant
