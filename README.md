# Dungeon Crawler Combat Demo
This is a multiplayer combat demo for Dungeon Crawler, a simple turn-based RPG terminal game. It uses a D20-based system found in many tabletop RPGs, and borrows most of its mechanics from D&D. Completed as part of the Code Louisville September 2020 C# Class.

## How to play
In this demo, you and three other players control a group of characters who are entrenched in combat. Your goal is to destroy every other character and be the last one standing.

At the start of the game, four pre-generated player characters are loaded and randomly placed on the map. Each character has some basic stats that determine how they interact with the world. These include:
- Race, a character's biological species
- Caste, a character's profession
- Ability scores, including Strength (STR), Constitution (CON), Dexterity (DEX), Intelligence (INT), Wisdom (WIS), and Charisma (CHA)
- Hit points, derived from CON score and racial hit die
- Carry weight, derived from STR score
- Movement speed, derived from DEX score
- Passive perception, derived from WIS score
- Actions, which may be taken during their turn (e.g., attack, hide, use item, etc.)

Each character inherits actions, innate abilities, and limitations from from its race and caste. For example, Eldfar is an elven wizard. As an elf, he has the `entrance` action and as a wizard he has the `slow` action. Like other elves, he is especially intelligent and wise. Because he is a wizard, he may only wear cloth armor and use magical implements as weapons.

The map is also populated with randomly selected items. These may be consumable (potions), weapons, armor, or useless junk. Items can be picked up, dropped, used (if they are consumable), or even knocked out of your hands by an opponent.

At the start of combat, each player rolls initiative (based on DEX) to determine in which order they will take their turns. Each player then takes their turn until only one is left standing. During their turn, each player has a major action, move action, and two minor actions. Once they expend these actions, their turn ends. Alternatively, they may also pass their turn. A note: the move action is unique from other action types, as the player may split their movement accross their turn. (For example, a player with 30ft of movement may move 10ft, attack an enemy, then retreat 20ft). The move action is only expended when they have moved their entire movement speed.

Each cell in the map represents a 5x5ft square. Only one object (including items, creatures, and walls) may occupy a cell at a time. Objects can block your movement and line of sight to other items and creatures. This means that you will have to move around obstacles to get to your destination, and make sure you can see your enemies before attempting to attack them.

Your character cannot interact with an item or creature if they do not know about it. Therefore you will have to use the `search` action to determine what is around you. The radius of your search is determined by a perception roll (based on WIS). Any found items will be stored in your character's memory, and can be recalled using the `recall` command. An enemy can attempt to evade your search by taking the `hide` action (a DEX roll). You will not be able to attack an enemy which is hidden.

These are just some of the mechanics you will be interacting with. As you play, you'll better understand the flow of combat. Good luck and have fun!

## Features
- Implements a master loop where users repeatedly enter commands (see `Player.TakeTurn()`)
- Several classes which inherit from parent classes
- Lots of dictionaries and lists
- Reads data from CSV files (found in `data`)
- Uses a regex (see `ExtensionsAndHelpers.FromTitleOrCamelCase()`)
- Uses many LINQ queries
- Data is visualized using a map
### Bonus Features!
- Uses Reflection (see `StatusEffect.ParseStatusEffects()` and `Entity.ApplyStatusEffect()`)
- Has some extension methods (see `ExtensionsAndHelpers`)
- Uses delegates (see `IEntityAction`, `TargetedAction`, `NonTargetedAction`)

## Instructions
Make sure the `data` folder is located in the same directory as `dungeon-crawler.exe`, then open `dungeon-crawler.exe` to start the game.

A note: This program uses some characters that require UTF-8 encoding. If some characters are not rendered properly (you can test this by typing `legend` on a character's turn), make sure you are using consolas or courier new. These should be default. If this doesn't work, you can also try `chcp 65001` in Command Prompt / Powershell prior to running the game to ensure UTF-8 is enabled.