# Tiki-Bros
2017

By: 

Georgia Bryant (Designer)

Brodie Harbridge (Designer)

Ashleigh Lamb (Artist)

Phillip Pajor (Designer)

Andrew Spencer (Programmer)

Niamh Thirwell-Towler (Artist)


## Intro

Tiki-Bros Is an adveture/ platformer game about two tiki brothers, one happy and one angry. 
Angry tiki ends up taking the teasure from the volcano god, he is mean and selfish. 
This makes the volcano god angry, happy tiki tries to stop him but, the volcano god ends up angry at them both. 
They end up running and falling off a cliff, happy tiki survives but, angry tiki does not.
Angry tiki ended up hitting a rock on the way down sending parts of him and the treasure across the island.
Happy tiki now has to collect the volcano god's treasure to restore order, as well as find the wooden parts of his brother to put him back together.
On this island are many dangers and obsticales that you must over come.

## Usage

### Controls

Input | Action
---|---
WASD / Arrow Keys | Movement
LMB / left ctrl | Punch
Spacebar | Jump
Scrollwheel | Zoom
ESC / P | Pause

### Gameplay

Move through the level collecting the treasure items. Touch the hand at the top of the giant flower 
to finish the level.

Walk towards checkpoints to activate them. You can tell it's active when it lights up and a drumbeat 
plays. You respawn at the active checkpoint on dying.

Touching enemies kills you. Turtles can't be hurt, crabs die in one punch and scorpions die in two.

Falling into water or lava is also fatal.

## Design

The project brief required the following features in the game

1. A main menu giving the option to begin or quit the game
2. At least one level which contains some form of interactive gameplay
3. A completion state, either through reaching the end or dying
4. The user can quit back to the main menu from the game, and return there from game over or 
completion states
5. The game must feature graphics, particle effects, and audio
6. The game should attempt to maintain a framerate of 60 frames per second

These are implemented as follows:

1. The game starts at a main menu screen, which as well as Play and Quit buttons has the option to 
check controls and view the game's credits
2. The game is a 3D platformer, involving jumping between platforms to reach new areas and killing or
 avoiding enemies. One level of the game has been created
3. After dying three times, the game is over, sending the player to the Game Over scene. Also, reaching
 the hand at the end of the level finishes the game.
4. On pausing the game, the player has the option to return to the main menu. This is also available on
 the various game over or completion scenes.
5. Particle effects are used in various places, for the fire and smoke in torches (both in game and on 
some menu screens), for a waterfall effect, for sparkles around treasure, and as explosions when 
characters are killed. As well as background music, audio is used for sound effects from collecting 
items, attacking and punching sounds, and footsteps.
6. The game runs at the default 60 frames per second