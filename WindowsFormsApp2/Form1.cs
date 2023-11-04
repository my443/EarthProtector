using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
	public partial class Form1 : Form
	{
		Timer graphicsTimer;
		GameLoop gameLoop = null;

		public Form1()
		{
			InitializeComponent();
			// Initialize Paint Event
			Paint += Form1_Paint;
			// Initialize graphicsTimer
			graphicsTimer = new Timer();
			graphicsTimer.Interval = 1000 / 120;
			graphicsTimer.Tick += GraphicsTimer_Tick;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// Set the window properties (fullsize, not resizable)
			this.WindowState = FormWindowState.Maximized;
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;

			Rectangle resolution = Screen.PrimaryScreen.Bounds;

			Label Mylablel = new Label();
			Mylablel.Location = new Point(50, resolution.Height - 100);
			Mylablel.Text = "100";
			Mylablel.AutoSize = true;
			Mylablel.BackColor = Color.CornflowerBlue;
			Mylablel.ForeColor = Color.White;
			this.Controls.Add(Mylablel);


			// Initialize Game
			Game myGame = new Game();

			myGame.Resolution = new Size(resolution.Width, resolution.Height);

			// Initialize & Start GameLoop
			gameLoop = new GameLoop();
			gameLoop.Load(myGame);
			gameLoop.Start();

			// Start Graphics Timer
			graphicsTimer.Start();




		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			if (gameLoop != null)
			{
				// Draw game graphics on Form1
				gameLoop.Draw(e.Graphics);
			}
		}

		private void GraphicsTimer_Tick(object sender, EventArgs e)
		{
			// Refresh Form1 graphics
			Invalidate();
		}
	}
}
