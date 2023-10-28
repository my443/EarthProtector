using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowsFormsApp2
{
	class Game
	{
		private GameSprite playerSprite;

		public Size Resolution { get; set; }

		public void Load()
		{
			// Load new sprite class
			playerSprite = new GameSprite();
			// Load sprite image
			playerSprite.SpriteImage = Properties.Resources.bomber_sprite;
			// Set sprite height & width in pixels
			playerSprite.Width = playerSprite.SpriteImage.Width;
			playerSprite.Height = playerSprite.SpriteImage.Height;
			// Set sprite coodinates
			playerSprite.X = 300;
			playerSprite.Y = 300;
			// Set sprite Velocity
			playerSprite.Velocity = 300;
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
			if ((Keyboard.GetKeyStates(Key.Right) & KeyStates.Down) > 0)
			{
				playerSprite.X += moveDistance;
			}
			else if ((Keyboard.GetKeyStates(Key.Left) & KeyStates.Down) > 0)
			{
				playerSprite.X -= moveDistance;
			}
			else if ((Keyboard.GetKeyStates(Key.Up) & KeyStates.Down) > 0)
			{
				playerSprite.Y -= moveDistance;
			}
			else if ((Keyboard.GetKeyStates(Key.Down) & KeyStates.Down) > 0)
			{
				playerSprite.Y += moveDistance;
			}
		}

		public void Draw(Graphics gfx)
		{
			// Draw Background Color
			gfx.FillRectangle(new SolidBrush(Color.CornflowerBlue), new Rectangle(0, 0, Resolution.Width, Resolution.Height));

			// Draw Player Sprite
			playerSprite.Draw(gfx);
		}
	}
}
