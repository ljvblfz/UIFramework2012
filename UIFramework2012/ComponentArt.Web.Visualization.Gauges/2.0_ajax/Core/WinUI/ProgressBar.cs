using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;


namespace ComponentArt.WinUI
{
#if DEBUG
	public
#else
	internal
#endif
	class ProgressBar : UserControl
	{
		// Fields
		private int maximum;
		private int minimum;
		private int step;
		private int value;
		private UserControl userControl11;

		public ProgressBar()
		{
			this.minimum = 0;
			this.maximum = 100;
			this.step = 10;
			this.value = 0;
			base.SetStyle(
#if __COMPILING_FOR_2_0_AND_ABOVE__
                ControlStyles.OptimizedDoubleBuffer
#else
                ControlStyles.DoubleBuffer
#endif
                , true);
		}

		[DefaultValue(10)]
		public int Step
		{
			get
			{
				return this.step;
			}
			set
			{
				this.step = value;
				Invalidate();
			}
		}

		[DefaultValue(100)]
		public int Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				this.maximum = value;
				Invalidate();
			}
		}

		[DefaultValue(0)]
		public int Minimum
		{
			get
			{
				return this.minimum;
			}
			set
			{
				this.minimum = value;
				Invalidate();
			}
		}


		[DefaultValue(0)]
		public int Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				Invalidate();
			}
		}

		void DrawSquare(Graphics g, Rectangle r)
		{
			Rectangle firstRect = new Rectangle(r.X, r.Y, r.Width, r.Height/2);
			Rectangle secondRect = new Rectangle(r.X, r.Y+r.Height/2, r.Width, r.Height - r.Height/2);

			Brush b = new LinearGradientBrush(firstRect, Color.FromArgb(240, 165, 146), Color.FromArgb(223, 52, 8), 90 );
			g.FillRectangle(b, firstRect);

			b = new LinearGradientBrush(secondRect, Color.FromArgb(223, 52, 8), Color.FromArgb(255, 51, 0), 90 );
			g.FillRectangle(b, secondRect);

			g.DrawLine(new Pen(Color.FromArgb(221, 52,9)), new Point(r.X, r.Y), new Point(r.X, r.Y + r.Height-1));
			g.DrawLine(new Pen(Color.FromArgb(221, 52,9)), new Point(r.X, r.Y), new Point(r.X + r.Width-1, r.Y));
		}


		protected override void OnPaintBackground ( System.Windows.Forms.PaintEventArgs e ) 
		{
			e.Graphics.Clear(Color.White);

			Rectangle rect = new Rectangle(0, 0, Width, Height*2/3);
			Brush b = new LinearGradientBrush(rect, Color.FromArgb(239, 239, 239), Color.White, 90 );
			e.Graphics.FillRectangle(b, rect);
		}


		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs e ) 
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;


			int m_graphicalStep = 8;


			int totalNoOfSquares = (Width - 4) / 8; 
			int drawNoOfSquares = totalNoOfSquares * (this.value - this.minimum) / (this.maximum - this.minimum);
			


			g.DrawRectangle(new Pen(Color.FromArgb(170, 170, 170)), 0, 0, Width-1, Height-1);

			for (int i=0; i<drawNoOfSquares; ++i) 
			{
				int startSquareX = 2+m_graphicalStep*i;
				DrawSquare(g, new Rectangle(startSquareX, 2, m_graphicalStep - 1 , Height - 4));
			}

		}


		public void PerformStep()
		{
			this.Increment(this.step);
		}

		private void InitializeComponent()
		{
			this.userControl11 = new UserControl();
			this.SuspendLayout();
			// 
			// userControl11
			// 
			this.userControl11.Location = new System.Drawing.Point(8, 8);
			this.userControl11.Name = "userControl11";
			this.userControl11.TabIndex = 0;
			// 
			// ProgressBar
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.userControl11});
			this.Name = "ProgressBar";
			this.ResumeLayout(false);

		}
 

		public void Increment(int value)
		{
			this.value += value;
			if (this.value < this.minimum)
			{
				this.value = this.minimum;
			}
			if (this.value > this.maximum)
			{
				this.value = this.maximum;
			}
			Invalidate();
		}
 

	}
}
