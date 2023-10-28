using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
	class GameSprite
	{
		public Bitmap SpriteImage { get; set; }
		public float X { get; set; }
		public float Y { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public int Velocity { get; set; }

		public GameSprite()
		{
		}

		public void Draw(Graphics gfx)
		{
			// Draw sprite image on screen
			gfx.DrawImage(SpriteImage, new RectangleF(X, Y, Width, Height));
			Console.WriteLine("soemthing works");
		}
	}
}
