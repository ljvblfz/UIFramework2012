using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ComponentArt.Web.Visualization.Gauges
{
	internal class SizePositionControl : System.Windows.Forms.UserControl
	{
		public event SizePositionEditor.ValueChangedHandler ValueChanged;
		public event SizePositionEditor.SessionDoneHandler SessionDone;
		private bool firstDisplay = true;

		// Values
		float px,py;
		// Ranges and steps
		float px0=0,px1=100, py0=0,py1=100, stepX, stepY;

		// Working data
		int x0,x1,y0,y1;
		float f;
		int r = 7; // Radius of the circle

		// Working mode
		bool sizeMode = true;
		private System.Windows.Forms.TextBox textBox;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SizePositionControl()
		{
			InitializeComponent();
		
		}

		internal void SetValues(float px0, float px1, float py0, float py1, float stepX, float stepY)
		{
			this.px0 = px0;
			this.px1 = px1;
			this.py0 = py0;
			this.py1 = py1;
			this.stepX = stepX;
			this.stepY = stepY;
			if(stepX <= 0) 
			{
				double s = Math.Abs(px1-px0)/200;
				double log10 = Math.Floor(Math.Log10(s));
				this.stepX = (float)Math.Pow(log10,10.0);
			}
			if(stepY <= 0) 
			{
				double s = Math.Abs(py1-py0)/200;
				double log10 = Math.Floor(Math.Log10(s));
				this.stepY = (float)Math.Pow(log10,10.0);
			}
			UpdateTextBox();
		}

		internal float X { get { return px; } set { px = value; UpdateTextBox(); } }
		internal float Y { get { return py; } set { py = value; UpdateTextBox(); } }
		internal bool SizeMode { get { return sizeMode; } set { sizeMode = value; } }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void UpdateTextBox()
		{
			// Textbox position
			Size sz = textBox.Size;
			float a = (px-px0)/(px1-px0);
			int x = ToClientX(px);
			int y = ToClientY(py);
			int xt = (int)(x - 0.5f*sz.Width);
			if(xt < 2)
				xt = 2;
			else if (x+0.5f*sz.Width > Size.Width)
				xt = (int)(Size.Width-sz.Width);
			float yt = (int)(y - r - sz.Height);
			if(yt < 0)
				yt = (int)(y + r);
			textBox.Text = px.ToString() + "," + py.ToString();
			textBox.Location = new Point((int)xt,(int)yt);	
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.BackColor = System.Drawing.Color.White;
			this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox.ForeColor = System.Drawing.SystemColors.Highlight;
			this.textBox.Location = new System.Drawing.Point(40, 24);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(40, 13);
			this.textBox.TabIndex = 0;
			this.textBox.Text = "";
			this.textBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.textBox.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// SizePositionControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.textBox});
			this.Name = "SizePositionControl";
			this.Resize += new System.EventHandler(this.SizePositionControl_Resize);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Control_Paint);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Control_MouseDown);
			this.ResumeLayout(false);

		}
		#endregion

		private int ToClientX(float x)
		{
			return x0 + (int)Math.Round((x1-x0)*(x-px0)/(px1-px0));
		}
		private int ToClientY(float y)
		{
			int yc = y0 + (int)Math.Round((y1-y0)*(y-py0)/(py1-py0));
			return ClientRectangle.Height - yc;
		}
		private float FromClientX(int x)
		{
			return px0 + (x-x0)*(px1-px0)/(x1-x0);
		}
		private float FromClientY(int y)
		{
			y = ClientRectangle.Height - y;
			return py0 + (y-y0)*(py1-py0)/(y1-y0);
		}

		private void Control_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int marg = 5;

			// Rectangle for the maximum values
			// ...initial values
			x0 = marg;
			x1 = ClientRectangle.Width - marg;
			y1 = ClientRectangle.Height - marg;
			y0 = marg;
			float fx = (x1-x0)/(px1-px0);
			float fy = (y1-y0)/(py1-py0);
			f = Math.Min(fx,fy);
			// ...adjusted values WRT the max values
			x1 = x0 + (int)(f*(px1-px0));
			y1 = y0 + (int)(f*(py1-py0));

			if(firstDisplay)
			{
				firstDisplay = false;
				UpdateTextBox();
			}

			// Current
			int x = ToClientX(px);
			int y = ToClientY(py);

			// Create working graphics to avoid flicker
			Bitmap bmp = new Bitmap(ClientRectangle.Width,ClientRectangle.Height);
			Graphics g = Graphics.FromImage(bmp);
			g.Clear(Color.White);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

			// Rectangle/Lines
			if(sizeMode)
			{
				// Editing Size2D
				g.FillRectangle(Brushes.LightBlue,x0,y,x-x0,y1-y);
				g.DrawRectangle(Pens.SteelBlue,x0,y0,x1-x0,y1-y0);
			}
			else
			{
				// Editing Point2D
				g.DrawLine(Pens.SteelBlue, x,y0, x,y);
				g.DrawLine(Pens.SteelBlue, x0,y, x,y);
			}
			g.DrawRectangle(Pens.LightBlue,x0,y0,x1-x0,y1-y0);

			// Marker
			g.FillEllipse(Brushes.LightGray,x-r,y-r,2*r,2*r);
			g.DrawLine(Pens.DarkGray,x,y-r,x,y+r);
			g.DrawLine(Pens.DarkGray,x-r,y,x+r,y);
			g.DrawEllipse(Pens.White,x-r+1,y-r+1,2*r-2,2*r-2);
			g.DrawEllipse(Pens.DarkGray,x-r,y-r,2*r,2*r);

			// Draw
			e.Graphics.DrawImage(bmp,0,0);

			g.Dispose();
			bmp.Dispose();
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// Do nothing to avoid flicker.
		}

		#region --- Handling mouse events ---

		private void Control_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button != MouseButtons.Left)
				return;
			this.Cursor = Cursors.SizeAll;
			HandleCursorPosition(e.X,e.Y);
		}
		
		private void HandleCursorPosition(int x, int y)
		{
			px = FromClientX(x);
			py = FromClientY(y);
			px = ((int)(px/stepX + 0.5f))*stepX;
			py = ((int)(py/stepY + 0.5f))*stepY;

			px = Math.Max(px0, Math.Min(px1,px));
			py = Math.Max(py0, Math.Min(py1,py));

			UpdateTextBox();

			Refresh();// Invalidate();

			if(ValueChanged != null)
				ValueChanged(this,px,py);
		}

		private void Control_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.Cursor = Cursors.Default;		
			if(SessionDone != null)
				SessionDone(this);
		}

		private void Control_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button != MouseButtons.Left)
				return;
			HandleCursorPosition(e.X,e.Y);
		}
		#endregion

		private void textBox_Leave(object sender, System.EventArgs e)
		{
			try
			{
				Point2D pt = new Point2D(textBox.Text);
				px = pt.X;
				py = pt.Y;

				UpdateTextBox();
				Refresh();// Invalidate();
				if(ValueChanged != null)
					ValueChanged(this,px,py);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return;
			}
		}

		private void SizePositionControl_Resize(object sender, System.EventArgs e)
		{
			if(this.Size.IsEmpty)
				return;
			UpdateTextBox();
		}
	}

	internal class SizePositionEditor : UITypeEditor
	{
		public delegate void ValueChangedHandler(object sender, float x, float y);
		public delegate void SessionDoneHandler(object sender);

		// This editor edits numeric values between given min and max using the SliderControl

		private SizePositionControl editControl;
		private IWindowsFormsEditorService service;
		private PropertyDescriptor propertyDescriptor;
		private object owner;
		private bool sizeMode;
		private SubGauge gauge;

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}
 
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider == null || context == null)
				return value;
			if(!(value is Point2D) && !(value is Size2D))
				return value;

			propertyDescriptor = context.PropertyDescriptor;
			owner = context.Instance;
			gauge = ObjectModelBrowser.GetOwningTopmostGauge(owner as IObjectModelNode);
			sizeMode = (value is Size2D);

			service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
			if (service == null)
			{
				return value;
			}

			if (editControl == null)
			{
				editControl = new SizePositionControl();
				editControl.ValueChanged += new SizePositionEditor.ValueChangedHandler(OnValueChanged);
				editControl.SessionDone += new SizePositionEditor.SessionDoneHandler(OnSessionDone);
			}
			// Getting ranges and steps
			float x0=0,x1=100, y0=0,y1=100, stepX=0,stepY=0;
            ISizePositionRangeProvider prov = owner as ISizePositionRangeProvider;
            if (prov != null)
                prov.GetRangesAndSteps(propertyDescriptor.Name, ref x0, ref x1, ref stepX, ref y0, ref y1, ref stepY);
			
			editControl.SetValues(x0,x1, y0,y1, stepX,stepY);
			editControl.SizeMode = sizeMode;
			if(sizeMode)
			{
				Size2D sz = (value as Size2D);
				editControl.X = sz.Width;
				editControl.Y = sz.Height;
			}
			else
			{
				Point2D pt = (value as Point2D);
				editControl.X = pt.X;
				editControl.Y = pt.Y;
			}

			bool isEditing = gauge.Editing;
			gauge.Editing = true;
			service.DropDownControl(editControl);
			gauge.Editing = isEditing;

			if(value is Size2D)
				return new Size2D(editControl.X,editControl.Y);
			else
				return new Point2D(editControl.X,editControl.Y);
		}
 
		void OnValueChanged(object sender, float x, float y)
		{
			if(sizeMode)
				propertyDescriptor.SetValue(owner,new Size2D(x,y));
			else
				propertyDescriptor.SetValue(owner,new Point2D(x,y));
			if(gauge != null)
				gauge.Invalidate();
		}
		void OnSessionDone(object sender)
		{
			service.CloseDropDown();
			service = null;
		}
	}

}
