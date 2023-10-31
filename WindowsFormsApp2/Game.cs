using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
//using System.Windows.Media;


namespace WindowsFormsApp2
{
	class Game
	{
		private GameSprite playerSprite;
		private SoundPlayer laserSound;
		private SpaceShip ship;

		// To limit the time number of keypresses
		private Timer inputRateLimitTimer;
		private bool canProcessInput = true;
		private int inputRateLimitMilliseconds = 200;

		public Size Resolution { get; set; }

		public void Load()
		{
			// Load new sprite class
			playerSprite = new GameSprite();
			// Load sprite image
			playerSprite.SpriteImage = Properties.Resources.crosshair089;
			// Set sprite height & width in pixels
			playerSprite.Width = playerSprite.SpriteImage.Width;
			playerSprite.Height = playerSprite.SpriteImage.Height;
			// Set sprite coodinates
			playerSprite.X = 300;
			playerSprite.Y = 300;
			// Set sprite Velocity
			playerSprite.Velocity = 250;

			laserSound = new SoundPlayer(Properties.Resources.alien_blaster);
			//laserSound.Play();

			// Rate limiting for keypress
			inputRateLimitTimer = new Timer();
			inputRateLimitTimer.Interval = inputRateLimitMilliseconds;
			inputRateLimitTimer.Tick += InputRateLimitTimer_Tick;
			inputRateLimitTimer.Start();

			ship = new SpaceShip();
			ship.X = 200;
			ship.Y = 200;
			ship.SpriteImage = Properties.Resources.enemyShip;
			ship.Width = ship.SpriteImage.Width;
			ship.Height = ship.SpriteImage.Height;
		}

		public void Unload()
		{
			// Unload graphics
			// Turn off game music
		}

		public void Update(TimeSpan gameTime)
		{	


			// Gametime elapsed
			double gameTimeElapsed = gameTime.TotalMilliseconds / 1000;
			// Calculate sprite movement based on Sprite Velocity and GameTimeElapsed
			int moveDistance = (int)(playerSprite.Velocity * gameTimeElapsed);


			// Move player sprite, when Arrow Keys are pressed on Keyboard
			if (moveMissileScope("right"))
			{
				playerSprite.X += moveDistance;
			}
			else if (moveMissileScope("left"))
			{
				playerSprite.X -= moveDistance;
			}
			else if (moveMissileScope("up"))
			{
				playerSprite.Y -= moveDistance;
			}
			else if (moveMissileScope("down"))
			{
				playerSprite.Y += moveDistance;
			}
			else if (moveMissileScope("up-left"))
			{
				playerSprite.X -= moveDistance;
				playerSprite.Y -= moveDistance;
			}
			else if (moveMissileScope("up-right"))
			{
				playerSprite.X += moveDistance;
				playerSprite.Y -= moveDistance;
			}
			else if (moveMissileScope("down-left"))
			{
				playerSprite.X -= moveDistance;
				playerSprite.Y += moveDistance;
			}
			else if (moveMissileScope("down-right"))
			{
				playerSprite.X += moveDistance;
				playerSprite.Y += moveDistance;
			}



			//	canProcessInput = false; // Disable input processing

			//	// Start the timer to re-enable input processing
			//	inputRateLimitTimer.Start();
			//}

		}

		public void Draw(Graphics gfx)
		{
			// Draw Background Color
			gfx.FillRectangle(new SolidBrush(Color.CornflowerBlue), new Rectangle(0, 0, Resolution.Width, Resolution.Height));

			// Draw Graphics
			ship.Draw(gfx);
			playerSprite.Draw(gfx);

		}

		private bool moveMissileScope(string direction)
		{
			bool returnValue;
			if (canProcessInput) {
				if ((Keyboard.GetKeyStates(Key.Space) & KeyStates.Down) > 0)
				{
					fireMissile();
				}

				canProcessInput = false; // Disable input processing

				// Start the timer to re-enable input processing
				inputRateLimitTimer.Start();
			}
			switch (direction) {
				case "right":
					{

						returnValue = ((Keyboard.GetKeyStates(Key.Right) & KeyStates.Down) > 0) || ((Keyboard.GetKeyStates(Key.NumPad6) & KeyStates.Down) > 0);
						break;
					}
				case "left":
					{
						returnValue = ((Keyboard.GetKeyStates(Key.Left) & KeyStates.Down) > 0) || ((Keyboard.GetKeyStates(Key.NumPad4) & KeyStates.Down) > 0);
						break;
					}
				case "up":
					{
						returnValue = ((Keyboard.GetKeyStates(Key.Up) & KeyStates.Down) > 0) || ((Keyboard.GetKeyStates(Key.NumPad8) & KeyStates.Down) > 0);
						break;
					}
				case "down":
					{
						returnValue = ((Keyboard.GetKeyStates(Key.Down) & KeyStates.Down) > 0) || ((Keyboard.GetKeyStates(Key.NumPad2) & KeyStates.Down) > 0);
						break;
					}
				case "up-right":
					{
						returnValue = ((Keyboard.GetKeyStates(Key.NumPad9) & KeyStates.Down) > 0);
						break;
					}
				case "up-left":
					{
						returnValue = ((Keyboard.GetKeyStates(Key.NumPad7) & KeyStates.Down) > 0); 
						break;
					}
				case "down-right":
					{
						returnValue = ((Keyboard.GetKeyStates(Key.NumPad3) & KeyStates.Down) > 0); 
						break;
					}
				case "down-left":
					{
						returnValue = ((Keyboard.GetKeyStates(Key.NumPad1) & KeyStates.Down) > 0); 
						break;
					}
				case "fire":
					{
						returnValue = ((Keyboard.GetKeyStates(Key.Space) & KeyStates.Down) > 0);
						break;
					}
				default:
					returnValue = false;
					break;

			}

			return returnValue;

		}


		async Task fireMissile() {
			if (checkIfHitShip())
			{
				explodeShip();
				Console.WriteLine("hit the ship"); 
			}
			else
			{
				Console.WriteLine("completely missed.");
			}

			playerSprite.SpriteImage = Properties.Resources.crosshair159;
			playLaser();
			await Task.Delay(250);
			playerSprite.SpriteImage = Properties.Resources.crosshair089;

		}

		private void playLaser() {
			Task.Run(() => laserSound.Play()); 
		}



		private void InputRateLimitTimer_Tick(object sender, EventArgs e)
		{
			canProcessInput = true;		// Re-enable input processing
			inputRateLimitTimer.Stop(); // Stop the timer until the next keypress
		}

		// TODO: Honestly, I'd rather pass the Gamesprite and ShipSprite in as parameters. 
		private bool checkIfHitShip()
		{

			RectangleF shipRectangle = new RectangleF(ship.X, ship.Y, ship.Width, ship.Height);
			RectangleF playerRectangle = new RectangleF(playerSprite.X, playerSprite.Y, playerSprite.Width, playerSprite.Height);

			if (playerRectangle.Contains(shipRectangle)) { return true; }
			else { return false; }

		}

		// TODO: Have parameters to identify **which** ship explodes.
		async Task explodeShip() {
			ship.SpriteImage = Properties.Resources.explosion;
			await Task.Delay(250);
			ship.SpriteImage = null;
		}

	}
}
