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
using Windows.Gaming.Input;
using Windows.Gaming.UI;
using System.Reflection.Emit;

//using System.Windows.Media;


namespace WindowsFormsApp2
{
	class Game
	{
		private Gamepad gamepad;

		private GameSprite playerSprite;
		private SoundPlayer laserSound;
		private SpaceShip ship;

		// To limit the time number of keypresses
		private Timer inputRateLimitTimer;
		private bool canProcessInput = true;
		private int inputRateLimitMilliseconds = 200;
		
		//private bool spaceToggleFlag = false;

		public Size Resolution { get; set; }

		public void Load()
		{



			Rectangle resolution = Screen.PrimaryScreen.Bounds;

			// Load new sprite class
			playerSprite = new GameSprite();
			// Load sprite image
			playerSprite.SpriteImage = Properties.Resources.crosshair089;
			// Set sprite height & width in pixels
			playerSprite.Width = playerSprite.SpriteImage.Width;
			playerSprite.Height = playerSprite.SpriteImage.Height;
			// Set sprite coodinates
			playerSprite.X = resolution.Width / 2 ;
			playerSprite.Y = resolution.Bottom - 200;
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


			gamepad = Gamepad.Gamepads.FirstOrDefault();
			GamepadReading reading = new GamepadReading(); 
			reading.LeftThumbstickX = 0;
			reading.LeftThumbstickY = 0;

			if (gamepad != null)
			{
				reading = gamepad.GetCurrentReading();
				Console.WriteLine("gaming input here");
				double x = reading.LeftThumbstickX;
				double y = reading.LeftThumbstickY;
				Console.WriteLine("x: "+x.ToString());
				Console.WriteLine("y: "+y.ToString());
				if (reading.Buttons == GamepadButtons.A)
				{
					Console.Write("fire!");
					fireMissile();
				};
				
				Console.WriteLine(reading.ToString());
			}


			// Gametime elapsed
			double gameTimeElapsed = gameTime.TotalMilliseconds / 1000;
			// Calculate sprite movement based on Sprite Velocity and GameTimeElapsed
			int moveDistance = (int)(playerSprite.Velocity * gameTimeElapsed);


			// Move player sprite, when Arrow Keys are pressed on Keyboard
			if (moveMissileScope("right", reading))
			{
				playerSprite.X += moveDistance;
			}
			else if (moveMissileScope("left", reading))
			{
				playerSprite.X -= moveDistance;
			}
			else if (moveMissileScope("up", reading))
			{
				playerSprite.Y -= moveDistance;
			}
			else if (moveMissileScope("down", reading))
			{
				playerSprite.Y += moveDistance;
			}
			else if (moveMissileScope("up-left", reading))
			{
				playerSprite.X -= moveDistance;
				playerSprite.Y -= moveDistance;
			}
			else if (moveMissileScope("up-right", reading))
			{
				playerSprite.X += moveDistance;
				playerSprite.Y -= moveDistance;
			}
			else if (moveMissileScope("down-left", reading))
			{
				playerSprite.X -= moveDistance;
				playerSprite.Y += moveDistance;
			}
			else if (moveMissileScope("down-right", reading))
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

		private bool moveMissileScope(string direction, GamepadReading reading)
		{
			bool returnValue;
			if (canProcessInput) {
				if ((Keyboard.GetKeyStates(Key.Space) & KeyStates.Down) > 0)
				{
					fireMissile();
				}
				
				//canProcessInput = false; // Disable input processing
				// Start the timer to re-enable input processing
				//inputRateLimitTimer.Start();
				
				startRateLimting();
			}


			switch (direction) {
				case "right":
					{	
						bool key = ((Keyboard.GetKeyStates(Key.Right) & KeyStates.Down) > 0);
						bool num = ((Keyboard.GetKeyStates(Key.NumPad6) & KeyStates.Down) > 0);
						bool gamepad = ((reading.LeftThumbstickX == 1));
						returnValue = ( key == true || num == true || gamepad == true);
						
						break;
					}
				case "left":
					{
						bool key = ((Keyboard.GetKeyStates(Key.Left) & KeyStates.Down) > 0);
						bool num = ((Keyboard.GetKeyStates(Key.NumPad4) & KeyStates.Down) > 0);
						bool gamepad = ((reading.LeftThumbstickX == -1));
						returnValue = (key == true || num == true || gamepad == true);
						break;
					}
				case "up":
					{
						bool key = ((Keyboard.GetKeyStates(Key.Up) & KeyStates.Down) > 0);
						bool num = ((Keyboard.GetKeyStates(Key.NumPad8) & KeyStates.Down) > 0);
						bool gamepad = ((reading.LeftThumbstickY == 1));
						returnValue = (key == true || num == true || gamepad == true);
						break;
					}
				case "down":
					{
						bool key = ((Keyboard.GetKeyStates(Key.Down) & KeyStates.Down) > 0);
						bool num = ((Keyboard.GetKeyStates(Key.NumPad2) & KeyStates.Down) > 0);
						bool gamepad = ((reading.LeftThumbstickY == -1));
						returnValue = (key == true || num == true || gamepad == true);
						break;
					}
				case "up-right":
					{
						bool key = ((Keyboard.GetKeyStates(Key.NumPad9) & KeyStates.Down) > 0);
						bool gamepadY = ((reading.LeftThumbstickY >= 0.45 && reading.LeftThumbstickY <= 0.95));
						bool gamepadX = ((reading.LeftThumbstickX >= 0.45 && reading.LeftThumbstickX <= 0.95));
						returnValue = (key == true || ( gamepadX == true && gamepadY == true));						
						break;
					}
				case "up-left":
					{
						bool key = ((Keyboard.GetKeyStates(Key.NumPad7) & KeyStates.Down) > 0);
						bool gamepadY = ((reading.LeftThumbstickY >= 0.45 && reading.LeftThumbstickY <= 0.95));
						bool gamepadX = ((reading.LeftThumbstickX <= -0.45 && reading.LeftThumbstickX >= -0.95));
						returnValue = (key == true || (gamepadX == true && gamepadY == true));
						break;
					}
				case "down-right":
					{
						bool key = ((Keyboard.GetKeyStates(Key.NumPad3) & KeyStates.Down) > 0);
						bool gamepadY = ((reading.LeftThumbstickY <= -0.45 && reading.LeftThumbstickY >= -0.95));
						bool gamepadX = ((reading.LeftThumbstickX >= 0.45 && reading.LeftThumbstickX <= 0.95));
						returnValue = (key == true || (gamepadX == true && gamepadY == true));					
						break;
					}
				case "down-left":
					{
						bool key = ((Keyboard.GetKeyStates(Key.NumPad1) & KeyStates.Down) > 0);
						bool gamepadY = ((reading.LeftThumbstickY <= -0.45 && reading.LeftThumbstickY >= -0.95));
						bool gamepadX = ((reading.LeftThumbstickX <= -0.45 && reading.LeftThumbstickX >= -0.95));
						returnValue = (key == true || (gamepadX == true && gamepadY == true));					
						break;
					}
				case "fire":
					{
						bool key = ((Keyboard.GetKeyStates(Key.Space) & KeyStates.Down) > 0);
						bool buttonA = reading.Buttons == GamepadButtons.A;
						returnValue = (key == true || buttonA == true);
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
			playLaser();
			playerSprite.SpriteImage = Properties.Resources.crosshair159;
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

		private void startRateLimting() {
			canProcessInput = false; // Disable input processing

			// Start the timer to re-enable input processing
			inputRateLimitTimer.Start();

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

		//private void Gamepad_ButtonPressed(Gamepad sender, GamepadButtonEventArgs args)
		//{
		//	// Handle button presses here
		//	// Example: Check if the A button is pressed
		//	if (args.Button == GamepadButtons.A)
		//	{
		//		// Do something when the A button is pressed
		//	}
		//}

		//private void Gamepad_ButtonReleased(Gamepad sender, GamepadButtonEventArgs args)
		//{
		//	// Handle button releases here
		//	// Example: Check if the A button is released
		//	if (args.Button == GamepadButtons.A)
		//	{
		//		// Do something when the A button is released
		//	}
		//}

	}
}
