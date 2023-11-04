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
		Label ScoreLabel = new Label();

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

			
			ScoreLabel.Location = new Point(50, resolution.Height - 150);
			ScoreLabel.Text = "Score: 100";
			ScoreLabel.AutoSize = true;
			ScoreLabel.BackColor = Color.CornflowerBlue;
			ScoreLabel.ForeColor = Color.Black;
			ScoreLabel.Font = new Font("Arial", 48);
			this.Controls.Add(ScoreLabel);


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
				int score = gameLoop._myGame.getScore();
				UpdateLabelText("Score: " + score.ToString());
			}
		}

		private void GraphicsTimer_Tick(object sender, EventArgs e)
		{
			// Refresh Form1 graphics
			Invalidate();
		}
		public void UpdateLabelText(string newText)
		{
			ScoreLabel.Text = newText;
		}
	}
}
