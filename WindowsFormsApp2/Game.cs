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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Controls;
using Windows.Media.Capture;

//using System.Windows.Media;


namespace WindowsFormsApp2
{
	class Game
	{
		private Gamepad gamepad;

		private GameSprite playerSprite;
		private SoundPlayer laserSound;
		//private SpaceShip ship;
		private SpaceShip explosion;

		private List<SpaceShip> listOfSpaceships = new List<SpaceShip>();

		// To limit the time number of keypresses
		private Timer inputRateLimitTimer;
		private bool canProcessInput = true;
		private int inputRateLimitMilliseconds = 200;

		// For Generating enemies
		private DateTime gameStart = DateTime.Now;
		private DateTime lastGenerated = DateTime.Now;

		private int score = 0;
		private Rectangle resolution = Screen.PrimaryScreen.Bounds;
		//private bool spaceToggleFlag = false;

		public Size Resolution { get; set; }

		public void Load()
		{

			//Rectangle resolution = Screen.PrimaryScreen.Bounds;

			// Load new sprite class
			playerSprite = new GameSprite();
			// Load sprite image
			playerSprite.SpriteImage = Properties.Resources.crosshairs_smaller;
			// Set sprite height & width in pixels
			playerSprite.Width = playerSprite.SpriteImage.Width;
			playerSprite.Height = playerSprite.SpriteImage.Height;
			// Set sprite coodinates
			playerSprite.X = resolution.Width / 2 ;
			playerSprite.Y = resolution.Bottom - 200;
			// Set sprite Velocity
			playerSprite.Velocity = 500;

			laserSound = new SoundPlayer(Properties.Resources.alien_blaster);
			//laserSound.Play();

			// Rate limiting for keypress
			inputRateLimitTimer = new Timer();
			inputRateLimitTimer.Interval = inputRateLimitMilliseconds;
			inputRateLimitTimer.Tick += InputRateLimitTimer_Tick;
			inputRateLimitTimer.Start();


			//Initial Ships that are created. 
			SpaceShip createdship = createShip(200, 200);
			listOfSpaceships.Add(createdship);
			createdship = createShip(400, 400);
			listOfSpaceships.Add(createdship);
			createdship = createShip(600, 600);
			listOfSpaceships.Add(createdship);

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
				//Console.WriteLine("gaming input here");
				double x = reading.LeftThumbstickX;
				double y = reading.LeftThumbstickY;
				//Console.WriteLine("x: "+x.ToString());
				//Console.WriteLine("y: "+y.ToString());
				if (reading.Buttons == GamepadButtons.A)
				{
					Console.Write("fire!");
					fireMissile();
				};
				
				//Console.WriteLine(reading.ToString());
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

			// Add a spaceship at a set interval.
			addSpaceship();

			// Draw Graphics
			foreach (SpaceShip item in listOfSpaceships)
			{
				item.Draw(gfx);
			}

//			ship.Draw(gfx);
			playerSprite.Draw(gfx);

			if (explosion != null) {
				explosion.Draw(gfx);
			}

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
			//playLaser();
			laserSound.Play();
			if (checkIfHitShip())
			{
				//explodeShip();
				Console.WriteLine("--hit the ship"); 
			}
			else
			{
				Console.WriteLine("--completely missed.");
			}

			playerSprite.SpriteImage = Properties.Resources.crosshair159;
			await Task.Delay(250);
			playerSprite.SpriteImage = Properties.Resources.crosshairs_smaller;
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
			int index = 0;
			bool returnValue = false;

			foreach (SpaceShip shipItem in listOfSpaceships)
			{
				//Console.WriteLine((shipItem.X).ToString()+"--"+ (shipItem.Y).ToString()+ "--"+ (shipItem.Width).ToString()+ "--"+ (shipItem.Height).ToString());
				//Console.WriteLine(playerSprite.X.ToString() + "--" + playerSprite.Y.ToString() + "--" + playerSprite.Width.ToString() + "--" + playerSprite.Height.ToString());

				// You can add more buffer here if you want to be able to be a little further from the target when it is hit.
				RectangleF shipRectangle = new RectangleF(shipItem.X, shipItem.Y, shipItem.Width, shipItem.Height);
				RectangleF playerRectangle = new RectangleF(playerSprite.X, playerSprite.Y, playerSprite.Width, playerSprite.Height);

				if (playerRectangle.IntersectsWith(shipRectangle))
				{
					returnValue = true;
					shipItem.SpriteImage = Properties.Resources.explosion;
					explodeShip(shipItem);
					listOfSpaceships.RemoveAt(index);
					break;
				}

				
				index++;
			}

			return returnValue;

		}

		// TODO: Have parameters to identify **which** ship explodes.
		async Task explodeShip(SpaceShip shipItem) {
			explosion = createShip((int)shipItem.X, (int)shipItem.Y);
			explosion.SpriteImage = Properties.Resources.explosion;
			await Task.Delay(250);
			explosion.SpriteImage = null;
			score++;
		}

		public int getScore() { 
			return score;
		}

		private SpaceShip createShip(int x, int y) {
			SpaceShip shipToReturn;
			shipToReturn = new SpaceShip();

			shipToReturn.X = x;
			shipToReturn.Y = y;
			shipToReturn.SpriteImage = Properties.Resources.enemyShip;
			shipToReturn.Width = shipToReturn.SpriteImage.Width;
			shipToReturn.Height = shipToReturn.SpriteImage.Height;

			return shipToReturn;
		}

		private int[] getXYForNewShip() {

			int[] returnArray = new int[2];

			int maxY = resolution.Height - 50;
			int maxX = resolution.Width;
			Random randomNumber = new Random();

			int xPos = randomNumber.Next(10, maxX-50 );
			int yPos = randomNumber.Next( 10, maxY-50 );

			returnArray[0] = xPos;
			returnArray[1] = yPos;

			return returnArray;

		}

		private SpaceShip createRandomSpaceShip() { 
			SpaceShip newShip = new SpaceShip();

			int[] XY = getXYForNewShip();
			newShip = createShip(XY[0], XY[1]);

			return newShip;
		}

		/**
		 * Based on a set amount of time, a new spaceship appears. 
		 * TODO: To make the game harder, you could have the spaceships appear quicker as the game progresses 
		 *			(based on the gamestart datetime.)
		 */
		private void addSpaceship() {
			if ((DateTime.Now - lastGenerated).TotalSeconds >= 1 ) { 
				listOfSpaceships.Add(createRandomSpaceShip());
				lastGenerated = DateTime.Now;
			}
		}

	}
}
