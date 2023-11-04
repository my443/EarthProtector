# EarthProtector

Your hands feel sweaty as you prepare for the next wave of assailants. 

Your job is to protect the Earth from invasion by aliens. After peace talks failed, countries built earth-based laser launchers to save Earth from the expected onslaught. 

You have one piece of sky to protect. Your success or failure could change humanity's future. 

We call people who have risen to the call **Earth Protectors**.



## Game Play
Use the arrow keys or number pad to aim your laser-missiles: Space-bar fires your missiles. 

Your missile-launcher has some protection built in, but if you are hit directly three times your launcher will be destroyed. Earth Protectors refer to each direct hit as 'losing a life.'

## Code Details
This game is a foray into game programming in C#.

## Attributions 
A special thanks to Thomas Marco de Haan and his template on [Codmentor](https://www.codementor.io/@dewetvanthomas/tutorial-game-loop-for-c-128ovxgrig) for helping me get a quick start.

Crosshairs provided by the [Kenney Crosshair Pack](https://www.kenney.nl/assets/crosshair-pack) licenced under the [Creative Commons CC0](https://creativecommons.org/publicdomain/zero/1.0/)

Alien Blaster Sound from [here](https://opengameart.org/content/alien-blaster)

Graphics for space ships [Spaceart](https://opengameart.org/content/space-shooter-art)

Graphics for explosions [Smoke Particles](https://kenney.nl/assets/smoke-particles)

## ScreenShots
![A crosshair and three space ships](/images/EarthDefender-1.png "Starting here")

![More spaceships appearing on screen](/images/EarthDefender-2.png "More threats arrive every second")

![A successful hit](/images/EarthDefender-3.png "A successful hit!")

## TODOs

The game is complete to play, but there are a number of things that I would want to do if I wanted to publish this. 

1. The sound and change in crosshairs can be a bit smoother. Even though they are mostly in sync, some attention could be paid to making this even better.
2. There should be a way to win, and to lose. 
	-Win: If you get to 100 points then then aliens decide that it isn't worth it to invade earth. 
	-Lose: If there are too many aliens that appear on the screen. (or if to many get to the bottom - see below.) 
			- Or if too many aliens just get off the screen. - If they leave the area, they can be considered 'out of reach.'
3. It would be nice to add movement to the aliens. Each ship could have an x, y, and direction (and maybe later a speed.) The aliens could be moving towards earth. 
4. The aliens, and the crosshairs, should only be allowed to move within the screen (limit where they can go. )
5. Add a welcome screen and a game-over screen. 
6. Paint stars in the background. 
