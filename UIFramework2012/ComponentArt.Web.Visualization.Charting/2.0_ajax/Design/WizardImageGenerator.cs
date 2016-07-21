using System;
using System.Collections;
using System.Drawing;
using ComponentArt.Web.Visualization.Charting;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	/// <summary>
	/// Summary description for WizardImageGenerator.
	/// </summary>
	internal class WizardImageGenerator
	{

		private WizardImageGenerator() 
		{
		}

		static Size origSize = new Size(100,72);
		static Size imageSize = new Size(100,72);
		static Point startingCropPoint = new Point(0, 0);

		static WinChart imageGenerationChart;
		static Series m_series2;

		static void SetupChart(WinChart winChart1, WizardSeriesStyleDialog.ChartTypeThumbnail ctt) 
		{			
			winChart1.Clear();

			Series series1 = new Series("S0");
			winChart1.Series.Add(series1);

			m_series2 = new Series("S1");
			winChart1.Series.Add(m_series2);

			winChart1.Frame.FrameKind = ChartFrameKind.NoFrame;
			
			double [] low = new double []
				{
					13, 25, 45, 20, 7
				};

			double [] high = new double []
				{
					45, 65, 54, 27, 15
				};


			double [] open = new double []
				{
					24, 46, 46, 25, 9
				};


			double [] close = new double []
				{
					31, 59, 51, 22, 9
				};


			double[] sales = new double[]
				{
					52.880, 
					77.870, 
					54.200, 
					84.000, 
					64.350
				};
			double[] sales2 = new double[]
				{
					64.350,	// jan
					66.840, // feb
					63.790, // march
					52.360, // april
					52.880 // may
				};
			string [] months = new string[]
					{
						"Jan 04",
						"Feb 04",
						"Mar 04",
						"Apr 04",
						"May 04",
						"June 04",
						"July 04",
						"Aug 04"
					};
			int[] x = new int[] { 0,1,2,3,4 };
			
			for (int i=0; i<sales.Length; ++i) 
			{
				sales[i]  = 10*(1-InputVariable.yy[0][i]) + 100*InputVariable.yy[0][i];
				sales2[i] = 10*(1-InputVariable.yy[1][i]) + 100*InputVariable.yy[1][i];
			}

			winChart1.DataSource = null;
            winChart1.InputVariables.Clear();
			winChart1.DefineValue("x",x);
            winChart1.DefineValue("S0:y", sales);
            winChart1.DefineValue("S1:y", sales2);
			winChart1.DefineValue("S0:high", high);
			winChart1.DefineValue("S0:open",open);
			winChart1.DefineValue("S0:close",close);
			winChart1.DefineValue("S0:low",low);
			winChart1.DefineValue("S1:high", high);
			winChart1.DefineValue("S1:open",open);
			winChart1.DefineValue("S1:close",close);
			winChart1.DefineValue("S1:low",low);
			winChart1.CoordinateSystem.XAxis.SetDimension(null);
			winChart1.CoordinateSystem.YAxis.SetDimension(null);
			winChart1.CoordinateSystem.ZAxis.SetDimension(null);
			winChart1.CoordinateSystem.ZAxis.DefaultCoordinateSet = null;
			winChart1.CoordinateSystem.ZAxis.AxisAnnotations.Clear();

			winChart1.View.Kind = ctt.ProjectionKind;
			winChart1.MainStyle = ctt.StyleName;
			winChart1.CompositionKind = ctt.CompositionKind;
			winChart1.View.ViewDirection = new Vector3D(10,10,20);
			winChart1.Size = origSize;

			if(ctt.ChartKindCategory == ChartKindCategory.Financial && ctt.CompositionKind != CompositionKind.MultiArea)
				winChart1.Series.Remove("S1");

			imageGenerationChart.DataBind();

		}		

		static System.Windows.Forms.ImageList m_il;

		internal static WizardSeriesStyleDialog.ChartTypeThumbnail [] Generate(WinChart winchart, System.Windows.Forms.ImageList il)
		{
			m_index=0;
			m_il = il;

			foreach (Image img in m_il.Images) 
			{
				img.Dispose();
			}

			m_il.Images.Clear();
			
			ArrayList m_bitmapList = new ArrayList();

			imageGenerationChart = new WinChart();

            if (imageGenerationChart.Palettes.IndexOf(winchart.SelectedPaletteName) != -1)
            {
                imageGenerationChart.SelectedPaletteName = winchart.SelectedPaletteName;
            }

			imageGenerationChart.Size = new Size(100,72);

			foreach (ProjectionKind pks in new ProjectionKind [] {ProjectionKind.TwoDimensional, ProjectionKind.CentralProjection}) 
			{
				ProjectionKind pk = pks;

				foreach (SeriesStyle ss in imageGenerationChart.SeriesStyles) 
				{
                    ChartKind ck = ss.ChartKind;
					
					WizardSeriesStyleDialog.ChartTypeThumbnail ctt;

					if (ss.IsBar)
						foreach (CompositionKind compK in Enum.GetValues(typeof(CompositionKind))) 
						{
							if (compK == CompositionKind.Concentric)
								continue;

							ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, compK, ss);
							m_bitmapList.Add(ctt);

						}
					else if (ss.IsArea)
					{
						foreach (CompositionKind compK in new CompositionKind [] {CompositionKind.Sections, CompositionKind.Stacked, CompositionKind.Stacked100, CompositionKind.MultiArea}) 
						{
							ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, compK, ss);
							m_bitmapList.Add(ctt);
						}						
	
						if (!ss.IsRadar)
						{
							ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, CompositionKind.MultiSystem, ss);
							m_bitmapList.Add(ctt);
						}
					}
					else if (ss.IsLine)
					{
						foreach (CompositionKind compK in new CompositionKind [] {CompositionKind.Sections, CompositionKind.Stacked, CompositionKind.Stacked100, CompositionKind.Merged, CompositionKind.MultiArea}) 
						{
							ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, compK, ss);
							m_bitmapList.Add(ctt);
						}

						if (!ss.IsRadar) 
						{
							ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, CompositionKind.MultiSystem, ss);
							m_bitmapList.Add(ctt);
						}
					}
					else if (ss.ChartKind == ChartKind.Bubble2D || ss.ChartKind == ChartKind.Bubble) 
					{
						ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, CompositionKind.Sections, ss);
						m_bitmapList.Add(ctt);
						ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, CompositionKind.Merged, ss);
						m_bitmapList.Add(ctt);
						ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, CompositionKind.MultiSystem, ss);
						m_bitmapList.Add(ctt);
						ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, CompositionKind.MultiArea, ss);
						m_bitmapList.Add(ctt);
					}
					else if (ss.ChartKind == ChartKind.Pie || ss.ChartKind == ChartKind.Doughnut) 
					{
						ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, CompositionKind.Concentric, ss);
						m_bitmapList.Add(ctt);
						ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, CompositionKind.MultiSystem, ss);
						m_bitmapList.Add(ctt);
						ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, CompositionKind.MultiArea, ss);
						m_bitmapList.Add(ctt);
					}
					else 
					{
						ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, CompositionKind.Sections, ss);
						m_bitmapList.Add(ctt);

						if (!ss.IsRadar) 
						{
							ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, CompositionKind.MultiSystem, ss);
							m_bitmapList.Add(ctt);
							ctt = new WizardSeriesStyleDialog.ChartTypeThumbnail(-1, pk, ck, CompositionKind.MultiArea, ss);
							m_bitmapList.Add(ctt);
						}
					}
				}
			}
			return (WizardSeriesStyleDialog.ChartTypeThumbnail [])
				m_bitmapList.ToArray(typeof(WizardSeriesStyleDialog.ChartTypeThumbnail));
		}

		internal static void AssignImage(WizardSeriesStyleDialog.ChartTypeThumbnail ctt) 
		{
			SetupChart(imageGenerationChart,ctt);

			Bitmap drawBmp = null; 

			try 
			{
				drawBmp = imageGenerationChart.Draw();//g, new Rectangle(new Point(0, 0), origSize));
			} 
			catch 
			{
				System.Windows.Forms.MessageBox.Show(imageGenerationChart.Exception.StackTrace, imageGenerationChart.Exception.Message);
			}

			if (ctt.ChartKind == ChartKind.HighLowOpenClose || ctt.ChartKind == ChartKind.CandleStick) 
				imageGenerationChart.Series.Add(m_series2);

			Bitmap saveBmp = new Bitmap(imageSize.Width, imageSize.Height);
			Graphics g = Graphics.FromImage(saveBmp);
			g.DrawImage(drawBmp, new Point(-startingCropPoint.X, -startingCropPoint.Y));

			m_il.Images.Add(saveBmp);

			ctt.ImageIndex = m_index;

			++m_index;

			g.Dispose();
			drawBmp.Dispose();
		}

		static int m_index=0;
	}
}
