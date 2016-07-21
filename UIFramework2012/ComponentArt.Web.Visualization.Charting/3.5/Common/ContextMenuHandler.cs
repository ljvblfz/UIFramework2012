using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using ComponentArt.Web.Visualization.Charting.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;


namespace ComponentArt.Web.Visualization.Charting
{
	internal class ContextMenuHandler
	{
		private ChartBase		chart;
		private WinChart	winChart;
		private ContextMenu menu;
		private MenuItem	contentsMenuItem;
		private DialogForm	form;

		internal ContextMenuHandler(WinChart winChart, ChartBase chart, ContextMenu menu)
		{
			this.winChart = winChart;
			this.chart = chart;
			this.menu = menu;
		}

		private void HandleDialogClose(object obj, EventArgs e)
		{
			form = null;
		}

		public bool ContentsEnabled 
		{
			get { return (contentsMenuItem != null); }
			set
			{
				if(value)
				{
					// Add the menu item, if not already there
					if(contentsMenuItem != null)
						return;
					else 
					{
					}
				}
				else
				{
					// Remove menu item
					if(contentsMenuItem != null)
					{
						menu.MenuItems.Remove(contentsMenuItem);
						contentsMenuItem = null;
					}
				}
			}
		}

		// ==============================================================================

		private class DialogForm : Form
		{
			private WinChart		winChart;
			private ChartBase			chart;
			private PropertyGrid	propertyGrid;

			internal DialogForm(WinChart winChart, ChartBase chart)
			{
				this.winChart = winChart;
				this.chart = chart;
				this.Size = new Size(300,450);
				this.FormBorderStyle = FormBorderStyle.SizableToolWindow;

				propertyGrid = new PropertyGrid();
				propertyGrid.CommandsVisibleIfAvailable = true;
				int xGrid = 8;
				int yGrid = 8;
				int wGrid = ClientRectangle.Width - xGrid - 8;
				int hGrid = ClientRectangle.Height - 8 - yGrid;
				propertyGrid.Location = new Point(xGrid,yGrid);
				propertyGrid.Size = new System.Drawing.Size(wGrid,hGrid);
				propertyGrid.Text = "Property Grid";
				propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.PropertiesChanged);
				propertyGrid.Dock = DockStyle.Fill;
				this.Text = "Chart Properties Editor";

				this.Controls.Add(propertyGrid);

				this.Visible = false;
			}

			internal PropertyGrid PropertyGrid { get { return propertyGrid; } }

			public void PropertiesChanged(object obj,PropertyValueChangedEventArgs pva)
			{
				chart.Invalidate();
				winChart.Invalidate();
			}
		};
	}
}
