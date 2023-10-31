using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
	internal class SpaceShip
	{
		public Bitmap SpriteImage { get; set; }
		public float X { get; set; }
		public float Y { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }
		public int Velocity { get; set; }
		public int XDirection { get; set; }
		public int YDirection { get; set; }

		public SpaceShip()
		{
		}

		public void Draw(Graphics gfx)
		{
			// Draw sprite image on screen
			// But if the image has been taken away, don't draw it.
			if (SpriteImage != null) { 
			gfx.DrawImage(SpriteImage, new RectangleF(X, Y, Width, Height));
			}
		}
	}
}
