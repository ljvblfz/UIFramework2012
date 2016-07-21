using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Web.Visualization.Gauges.Core.WinUI
{
	/// <summary>
	/// Summary description for StateColorStopList.
	/// </summary>
#if DEBUG
	public
#else
	internal
#endif
	 class StateColorStopList : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public event EventHandler OnChanged;

		public StateColorStopList()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.VScroll = true;
			this.HScroll = false;
			
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
				}
			}
			base.Dispose( disposing );
		}

		public StateColorCollection ColorStops 
		{
			get 
			{
				StateColorCollection colorStops = new StateColorCollection();
				foreach(StateColorStopControl scsc in Controls)
					colorStops.Add(scsc.ColorStop);
				return colorStops;
			}
			set
			{
				PopulateControl(value);
			}
		}

		private void PopulateControl(StateColorCollection colorStops)
		{
			SuspendLayout();
			Controls.Clear();
			int y = 0;
			if(colorStops != null)
			{
				for(int i=0; i<colorStops.Count; i++)
				{
					StateColorStopControl scsc = new StateColorStopControl();
					scsc.Width = this.ClientRectangle.Width - 2;
					scsc.Location = new Point(0,y);
					scsc.ColorStop = colorStops[i];
					scsc.OnChange += new EventHandler(HandleColorStopChanged);
					scsc.OnDelete += new EventHandler(HandleColorStopDeleted);
					Controls.Add(scsc);
					y += scsc.Height;
				}
			}
			ResumeLayout(false);
		}

		internal void AddNewStop()
		{
			StateColorStopControl scsc = new StateColorStopControl();
			scsc.Width = this.ClientRectangle.Width - 2;
			scsc.OnChange += new EventHandler(HandleColorStopChanged);
			scsc.OnDelete += new EventHandler(HandleColorStopDeleted);
			int y = 0;
			if (Controls.Count>0)
				y = Controls[Controls.Count-1].Location.Y + Controls[Controls.Count-1].Height;
			scsc.Location = new Point(1, y);
			Controls.Add(scsc);

			scsc.Focus();
			
			//workaround for a bug where a horizontal scrollbar appears until control refreshed
			scsc = new StateColorStopControl();
			scsc.Width = this.ClientRectangle.Width - 2;
			scsc.Location = new Point(1, Controls[Controls.Count-1].Location.Y + Controls[Controls.Count-1].Height);
			Controls.Add(scsc);
			Controls.Remove(scsc);
			
			Refresh();

			if(OnChanged != null)
				OnChanged(this, EventArgs.Empty);
		}

		private void HandleColorStopChanged(object sender, EventArgs ea)
		{
			StateColorStopControl scsc = sender as StateColorStopControl;
			if(scsc == null)
				return;
			if(OnChanged != null)
				OnChanged(this,EventArgs.Empty);
		}

		private void HandleColorStopDeleted(object sender, EventArgs ea)
		{
			StateColorStopControl scsc = sender as StateColorStopControl;
			if(scsc == null)
				return;
			Controls.Remove(scsc);
			int y = 0;
			for(int i=0; i<Controls.Count; i++)
			{
				Controls[i].Location = new Point(1, y);
				y += Controls[i].Height;
			}
			if(OnChanged != null)
				OnChanged(this, EventArgs.Empty);
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// StateColorStopList
			// 
			//this.AutoScroll = true;
			this.Name = "StateColorStopList";
			this.Resize += new System.EventHandler(this.ColorStopList_Resize);

		}
		#endregion

		private void ColorStopList_Resize(object sender, System.EventArgs e)
		{
			foreach(Control c in Controls)
			{
				StateColorStopControl scsc = c as StateColorStopControl;
				if(scsc != null)
				{
					scsc.Width = this.ClientRectangle.Width - scsc.Location.X*2;
				}
			}
		}
	}
}
