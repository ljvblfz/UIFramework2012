using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges.Core.WinUI
{
	/// <summary>
	/// Summary description for ColorStopList.
	/// </summary>
#if DEBUG
	public
#else
	internal
#endif
	 class ColorStopList : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// insert button dimensions
		private int ibw = 15, ibh = 11;
		
		private int x = 0;
		private ArrayList colorStops = new ArrayList();
		private ArrayList insertButtons = new ArrayList();

		public event EventHandler OnChanged;
		private Bitmap insertButtonImage = null;
		private Bitmap insertButtonImageDown = null;

		private Button buttonDown = null;

		public ColorStopList()
		{
			InitializeComponent();
			this.VScroll = true;
			this.HScroll = false;

			// ********** Insert Buttons Bitmaps **********
			
			System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.ButtonArrow.png");
			insertButtonImage = (Bitmap)Bitmap.FromStream(stream);
			stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ComponentArt.Web.Visualization.Gauges.Core.Wizard.Images.ButtonArrowDown.png");
			insertButtonImageDown = (Bitmap)Bitmap.FromStream(stream);

			x = ibw;
		}

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
					if(insertButtonImage != null)
						insertButtonImage.Dispose();
					if(insertButtonImageDown != null)
						insertButtonImageDown.Dispose();
					insertButtonImageDown = null;
					insertButtonImage = null;
				}
			}
			base.Dispose( disposing );
		}

		public ColorStopCollection ColorStops 
		{
			get 
			{
				ColorStopCollection csCollection = new ColorStopCollection();
				foreach(ColorStopControl csc in colorStops)
					csCollection.Add(csc.ColorStop);
				return csCollection;
			}
			set
			{
				PopulateControl(value);
			}
		}

		private ColorStopControl[] ColorStopControls
		{
			get
			{
				return (ColorStopControl[])colorStops.ToArray(typeof(ColorStopControl));
			}
		}

		private Button[] InsertButtons
		{
			get
			{
				return (Button[])insertButtons.ToArray(typeof(Button));
			}
		}

		private void PopulateControl(ColorStopCollection cStops)
		{
			SuspendLayout();
			Controls.Clear();
			colorStops.Clear();
			insertButtons.Clear();
			
			if(cStops != null)
			{
				for(int i=0; i<cStops.Count; i++)
				{
					ColorStopControl csc = InsertColorStop(i,cStops[i]);
					if(i==0 || i==cStops.Count-1)
						csc.DisableDeleteAndValueEdit();
				}
			}
			RedoLayout();
			ResumeLayout(false);

			// We have to do this because the client rectangle width may have been changed because of scrollbar
			ColorStopControl[] stops = ColorStopControls;
			for(int i=0; i<stops.Length; i++)
				stops[i].Width = ClientRectangle.Width - stops[i].Location.X;
		}

		private void HandleInsertColorStop (object sender, EventArgs ea)
		{
			ColorStopControl csc = (sender as Button).Tag as ColorStopControl;
			ColorStopControl[] stops = ColorStopControls;
			int index = -1;
			for(index = 0; index<stops.Length && stops[index] != csc; index++) {}
			if(index >= stops.Length || index < 1)
				return;
			ColorStop newStop = stops[index-1].ColorStop.Interpolate(stops[index].ColorStop,0.5);
			InsertColorStop(index,newStop);
			RedoLayout();
			Refresh();

			if(OnChanged != null)
				OnChanged(this,EventArgs.Empty);
				

		}

		private ColorStopControl InsertColorStop(int index, ColorStop colorStop)
		{
			ColorStopControl csc = new ColorStopControl();
			int h = csc.Height + 1;
			csc.Width = ClientSize.Width-x;
			int y = index*h;
			csc.Location = new Point(x,y);
			csc.ColorStop = colorStop;
			csc.OnChange += new EventHandler(HandleColorStopChanged);
			csc.OnDelete += new EventHandler(HandleColorStopDeleted);
			Controls.Add(csc);
			if(index < colorStops.Count)
				colorStops.Insert(index,csc);
			else
				colorStops.Add(csc);
			if(index > 0)
			{
				Button bt = new Button();
				bt.Text = "+";
				//bt.TextLocation = new Point(3, -2);
				bt.Size = new Size(ibw,ibh);
				bt.Location = new Point(0,y-ibh/2);
				bt.Tag = csc;
				bt.Click += new EventHandler(HandleInsertColorStop);
				bt.Paint += new PaintEventHandler(InsertButton_Paint);
				bt.MouseDown += new MouseEventHandler(InsertButton_MouseDown);
				bt.MouseUp += new MouseEventHandler(InsertButton_MouseUp);
				bt.MouseLeave += new EventHandler(InsertButton_MouseLeave);
				bt.FlatStyle = FlatStyle.Flat;
				csc.Tag = bt;
				Controls.Add(bt);
				if(index -1 < insertButtons.Count)
					insertButtons.Insert(index-1,bt);
				else
					insertButtons.Add(bt);
			}

			return csc;
		}

		#region --- Processing Insert Buttons ---

		private void InsertButton_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Button btn = sender as Button;
			if(btn == null)
				return;
			if(btn == buttonDown)
				e.Graphics.DrawImageUnscaled(insertButtonImageDown,0,0);
			else
				e.Graphics.DrawImageUnscaled(insertButtonImage,0,0);
		}

		private void InsertButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			buttonDown = sender as Button;
		}

		private void InsertButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			buttonDown = null;
		}

		private void InsertButton_MouseLeave(object sender, EventArgs e)
		{
			buttonDown = null;
		}

		#endregion

		private void HandleColorStopChanged(object sender, EventArgs ea)
		{
			ColorStopControl csc = sender as ColorStopControl;
			if(csc == null)
				return;
			if(OnChanged != null)
				OnChanged(this,EventArgs.Empty);
		}

		private void HandleColorStopDeleted(object sender, EventArgs ea)
		{
			ColorStopControl csc = sender as ColorStopControl;
			if(csc == null)
				return;
			Controls.Remove(csc);
			colorStops.Remove(csc);
			if(csc.Tag != null)
			{
				insertButtons.Remove(csc.Tag as Control);
				Controls.Remove(csc.Tag as Control);
			}
			RedoLayout();
			if(OnChanged != null)
				OnChanged(this,EventArgs.Empty);
		}

		private void RedoLayout()
		{
			ColorStopControl[] stops = ColorStopControls;
			Button[] buttons = InsertButtons;
			int offsetY = stops[0].Location.Y;
			for(int i=0; i<stops.Length; i++)
			{
				int h = stops[i].Height + 1;
				stops[i].Location = new Point(stops[i].Location.X,i*h + offsetY);
				stops[i].Width = ClientRectangle.Width - stops[i].Location.X;
			}
			for(int i=0; i<buttons.Length; i++)
			{
				int h = stops[i].Height + 1;
				buttons[i].Location = new Point(buttons[i].Location.X,(i+1)*h - ibh/2 + offsetY);
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ColorStopList
			// 
			this.Name = "ColorStopList";
			this.Resize += new System.EventHandler(this.ColorStopList_Resize);

		}
		#endregion

		private void ColorStopList_Resize(object sender, System.EventArgs e)
		{
			foreach(Control c in ColorStopControls)
			{
				ColorStopControl csc = c as ColorStopControl;
				if(csc != null)
				{
					csc.Width = this.ClientRectangle.Width-csc.Location.X;
				}
			}
		}
	}
}
