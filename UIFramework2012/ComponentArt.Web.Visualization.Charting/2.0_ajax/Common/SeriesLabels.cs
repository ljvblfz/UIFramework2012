using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	// ------------------------------------------------------------------------------------------------------
	//		Series Labels
	// ------------------------------------------------------------------------------------------------------

	/// <summary>
	/// Represents labels belonging to a <see cref="Series"/> object.
	/// </summary>
	[TypeConverter(typeof(GenericExpandableObjectConverter))]
	public class SeriesLabels : ChartObject, IDisposable
	{
		private string				labelStyleName="Default";
		private DataPointLabelStyle labelStyle = null;
		private string				labelExpression="y";
		private bool				finished = false;
		private ArrayList			dataPointLabels;

		private bool				inherited = false;

		/// <summary>
		/// Initializes a new instance of <see cref="SeriesLabels"/> class.
		/// </summary>
		public SeriesLabels()
		{
			dataPointLabels = new ArrayList();
		}

		internal bool Inherited { get { return inherited; } set { inherited = value; } }

		/// <summary>
		/// Gets or sets the name of the label style.
		/// </summary>
		[TypeConverter(typeof(SelectedDataPointLabelStyleConverter))]
		[Description("The name of the label style, or expression like If(S0:y>50,'Default','AboveVertical')")]
		public	 string	LabelStyleName				
		{
			get 
			{
				return labelStyleName; 
			}
			set 
			{
				labelStyleName = value; 
				labelStyle = null; 
				// If this is after DataBind() of the owning chart, we have to redo DataBind() of this
				if(OwningSeries != null && OwningSeries.DataPoints.Count > 0)
					DataBind();
			}
		}

		/// <summary>
		/// Gets or sets the name of the label style kind.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public	 DataPointLabelStyleKind	LabelStyleKind			
		{
			get 
			{
				return DataPointLabelStyle.StyleKindOf(labelStyleName); 
			}
			set 
			{
				LabelStyleName = value.ToString();
			}
		}
		
		/// <summary>
		/// Gets or sets an expression for the label.
		/// </summary>
		[Description("Indicates an expression used for the label")]
		public	 string	LabelExpression				
		{
			get 
			{
				return labelExpression; 
			} 
			set 
			{ 
				labelExpression = value; 
				// If this is after DataBind() of the owning chart, we have to redo DataBind() of this
				if(OwningSeries != null && OwningSeries.DataPoints.Count > 0)
					DataBind();
			} 
		}

		internal Series					OwningSeries	{ get { return Owner as Series; } }

		internal DataPointLabelStyle LabelStyle
		{
			get 
			{ 
				if(labelStyle == null || labelStyle.Name != labelStyleName)
				{
					// Clone the style from collection
					DataPointLabelStyle dpls = OwningChart.DataPointLabelStyles[labelStyleName] as DataPointLabelStyle;
					labelStyle = new DataPointLabelStyle();
					if(dpls != null)
						labelStyle.LoadFrom(dpls);
				}
				return labelStyle;
			}
			set
			{
				if(value != null)
					labelStyleName = value.Name;
				labelStyle = value;
			}
		}
		
		internal StringVariable Texts
		{
			get 
			{
				if(labelExpression=="")
					return null;
				
                Variable r = null;

                // Try to get it as input variable
                InputVariable iVar = OwningChart.InputVariables[ValueInputVariableName];
                if (iVar != null)
                {
                    r = iVar.EvaluatedValue;
                }
                else
                {
                    // Try to get it as parameter
                    DataDescriptor des = OwningSeries.DataDescriptors[labelExpression];
                    if (des != null)
                        r = des.ComputeVariable();
                    if (r == null)
                        r = OwningChart.DataProvider.Evaluate(labelExpression);
                }
				if(r != null)
				{
					if(r is StringVariable)
						return (r as StringVariable);
					else
						return r.Format(LabelStyle.FormattingString);
				}
				
				return new StringVariable("labels","");
			}
		}

        internal StringVariable StyleNames
        {
            get
            {
				if(OwningChart.DataPointLabelStyles[labelStyleName] != null)
					return new StringVariable("PointLabelStyles", labelStyleName);
				else
					return OwningChart.DataProvider.Evaluate(labelStyleName) as StringVariable;
            }
        }

		/// <summary>
		/// Gets a <see cref="DataPointLabel"/> corresponding to the x coordinate of the <see cref="DataPoint"/>.
		/// </summary>
		public DataPointLabel this[object dataPointXCoordinate] 
		{ 
			get 
			{ 
				if(dataPointXCoordinate is int)
				{
					return (DataPointLabel) dataPointLabels[(int)dataPointXCoordinate];
				}
				else
				{
					for(int i = 0; i<dataPointLabels.Count; i++)
					{
						DataPointLabel dpl = (DataPointLabel) dataPointLabels[i];
						if(OwningSeries.XDimension.Compare(dpl.DataPoint.XDCS(),dataPointXCoordinate) == 0)
							return dpl;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Gets the number of the <see cref="DataPointLabel"/>s belonging to this <see cref="SeriesLabels"/> object.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int					Count		{ get { return  dataPointLabels.Count; } }

													  [Browsable(false)]
		internal bool Finished { get { return finished; } set { finished = value; } }
		private  bool ShouldSerializeFinished() { return false; }

		internal void DataBind()
		{
            if (dataPointLabels == null)
                dataPointLabels = new ArrayList();
            else
			    dataPointLabels.Clear();
			Series series = OwningSeries;
			if(series==null || labelStyleName == "" || Texts == null)
				return;

			StringVariable texts = Texts;

			StringVariable styleNames = StyleNames;

			for(int i=0;i<OwningSeries.DataPoints.Count;i++)
			{
				DataPoint dataPoint = OwningSeries.DataPoints[i];
				int labelIndex = dataPoint.OriginalIndex;
				if(labelIndex >= 0)
				{
					DataPointLabelStyle DPLS = OwningChart.DataPointLabelStyles[styleNames[labelIndex]];
					Add(new DataPointLabel(dataPoint,DPLS,texts[labelIndex]));
				}
			}
		}

        internal void RegisterVariables()
        {
            RegisterValueVariable();
        }

        private void RegisterValueVariable()
        {
            Series series = OwningSeries;
            if (labelExpression == null || labelExpression == "")
                return;
            // we'll reuse the existing data descriptor while evaluating texts
            foreach (DataDescriptor ddes in series.DataDescriptors)
            {
                if (ddes.Name == labelExpression)
                    return;
            }

            // Create new variable for values

            string paramName = ValueParameterName;
            DataDescriptor des = null;
            foreach (DataDescriptor ddes in series.DataDescriptors)
            {
                if (ddes.Name == paramName)
                {
                    des = ddes;
                    break;
                }
            }
            if (des == null)
            {
                des = new DataDescriptor(paramName);
                series.DataDescriptors.Add(des);
            }
            des.Required = true;
            // We'll create the inputVariable as well
            string iVarName = ValueInputVariableName;
            InputVariable var = OwningChart.InputVariables[iVarName];
            if (var == null)
            {
                var = new InputVariable(iVarName);
                OwningChart.InputVariables.Add(var);
            }
            if (labelExpression[0] == '=')
            {
#if __BUILDING_CRI_DESIGNER__
                var.RSExpression = labelExpression;
#endif
            }
            else
                var.ValueExpression = labelExpression;
        }

        internal string NamePrefix
        {
            get
            {
                SeriesLabelsCollection labels = OwningSeries.Labels;
                int index = -1;
                for (int i = 0; i < labels.Count; i++)
                    if (labels[i] == this)
                    {
                        index = i;
                        break;
                    }
                return "Labels_" + index.ToString();
            }
        }

        internal string ValueParameterName { get { return NamePrefix + "_Value"; } }
        internal string ValueInputVariableName { get { return OwningSeries.Name + ":" + ValueParameterName; } }
        internal string StyleParameterName { get { return NamePrefix + "_Style"; } }
        internal string StyleInputVariableName { get { return OwningSeries.Name + ":" + StyleParameterName; } }

        internal string[] RequiredParameters
        {
            get
            {
				bool labelRequired = (labelExpression != null && labelExpression != "");
				if(labelRequired)
					return new string[] { ValueParameterName };
				return null;
            }
        }

		internal override void Build()
		{
		}

		internal int Add( Object value )  
		{
			if (value is DataPointLabel)
			{
				DataPointLabel dpl = value as DataPointLabel;
				dpl.SetOwner(this);
				return dataPointLabels.Add(dpl);
			}
			else if( value is Chart2DObject)
				Space.Add(value as Chart2DObject);

			return 0;
		}

		internal override void Render()
		{
			StringVariable styleNames = StyleNames;
			for(int i=0; i<dataPointLabels.Count;i++)
			{
				DataPointLabel DPL = dataPointLabels[i] as DataPointLabel;
				DPL.PositionLabel();
			}
			RepositionLabels();
			foreach(DataPointLabel DPL in dataPointLabels)
			{
				if(OwningSeries.TargetArea.IsTwoDimensional)
					DPL.LabelStyle.LocalRefPoint = new Vector3D(
						DPL.LabelStyle.LocalRefPoint.X,
						DPL.LabelStyle.LocalRefPoint.Y,
						1.01);
				DPL.Render();
			}
		}

		private Vector3D Center;

		private void RepositionLabels()
		{
			if(OwningSeries.Style.ChartKindCategory != ChartKindCategory.PieDoughnut)
				return;
			if (dataPointLabels == null || dataPointLabels.Count == 0)
				return;

			DataPointLabel lab0 = dataPointLabels[0] as DataPointLabel;
			Center = OwningSeries.Mapping.Map(lab0.DataPoint.Center);
			int n1 = 0;
			int n2 = 0;
			int i;
			float fontSize = 0;
			Font LSfont = null;
			for(i=0; i<dataPointLabels.Count; i++)
			{
				DataPointLabel lab = (DataPointLabel) dataPointLabels[i];
				if(lab.LabelStyle.PieLabelPosition == PieLabelPositionKind.Outside)
					n1++;
				if(lab.LabelStyle.PieLabelPosition == PieLabelPositionKind.OutsideAligned)
					n2++;
				LSfont = lab.LabelStyle.Font;
				fontSize = Math.Max(fontSize,LSfont.Size);
			}
			int n = n1+n2;
			if(n == 0)
				return;

			Graphics g = OwningChart.WorkingGraphics;
			double fac = (OwningChart.TargetSize.Width+OwningChart.TargetSize.Height)/(double)(OwningChart.NativeSize.Width+OwningChart.NativeSize.Height);
			Font font = new Font(LSfont.Name,(float)(fontSize*fac),LSfont.Style);

			Vector3D T0, T1, T2;

			PieLabelPositionKind pieLabelPosition = (n1>n2? PieLabelPositionKind.Outside: PieLabelPositionKind.OutsideAligned);
			
			for(i=0; i<dataPointLabels.Count; i++)
			{
				DataPointLabel lab = (DataPointLabel) dataPointLabels[i];
				if(lab.LabelStyle.PieLabelPosition == PieLabelPositionKind.Outside ||
					lab.LabelStyle.PieLabelPosition == PieLabelPositionKind.OutsideAligned)
					lab.LabelStyle.PieLabelPosition = pieLabelPosition;						
			}

			double[] y = new double[n];
			double[] x = new double[n];
			int[] ixL = new int[n];
			int[] ixD = new int[n];
			DataPointLabel[] labels = new DataPointLabel[n];
			SizeF[] size = new SizeF[n];
			int nL = 0;
			int nD = 0;
			i = 0;
			for(int j = 0; j<dataPointLabels.Count; j++)
			{
				DataPointLabel lab = (DataPointLabel) dataPointLabels[j];
				if(lab.LabelStyle.PieLabelPosition != pieLabelPosition)
					continue;
				labels[i] = lab;
				T0 = lab.Point0;
				T1 = lab.Point1;
				T2 = lab.Point2;
				x[i] = T2.X;
				y[i] = T2.Y;
				size[i] = g.MeasureString(lab.Text,font);
				if(T1.X < T0.X)
				{
					ixL[nL] = i;
					nL++;
				}
				else
				{
					ixD[nD] = i;
					nD++;
				}
				i++;
			}
			font.Dispose();

			if(pieLabelPosition == PieLabelPositionKind.OutsideAligned)
			{
				AdjustTextPositionOutsideAligned(ixL,x,y,nL,size);
				AdjustTextPositionOutsideAligned(ixD,x,y,nD,size);
			}
			else
			{
				AdjustTextPositionOutside(x,y,size);
			}
			
			for(i=0;i<n;i++)
			{
				DataPointLabel lab = labels[i];
				lab.Point2 = new Vector3D(x[i], y[i], lab.Point2.Z);
			}

			for(i=0;i<n;i++)
			{
				DataPointLabel lab = labels[i];
				if(lab.Point0.Y < lab.Point1.Y && y[i] < lab.Point1.Y ||
					lab.Point0.Y > lab.Point1.Y && y[i] > lab.Point1.Y)
				{
					double a = (y[i]-lab.Point0.Y)/(lab.Point1.Y - lab.Point0.Y);
					if(a<0)
					{
						lab.Point1 = 0.5*lab.Point0 + 0.5*lab.Point2;
					}
					else
					{
						a = Math.Min(1.0,Math.Max(0.0,a));
						lab.Point1 = a*lab.Point1 + (1-a)*lab.Point0;
					}
				}
			}
		}

		private void AdjustTextPositionOutsideAligned(int[] ix,double[] x, double[] y, int n, SizeF[] size) // ... to avoid labels overlapping
		{
			if(n<2)
				return;

			// Sort
			int i,j;
			for(i=0;i<n-1;i++)
				for(j=i+1;j<n;j++)
				{
					if(y[ix[j]]<y[ix[i]])
					{
						int xx = ix[i];
						ix[i] = ix[j];
						ix[j] = xx;
					}
				}

			int m = x.Length;
			double[] y1 = new double[m];
			double[] y2 = new double[m];

			for(i=0;i<m;i++)
			{
				y1[i] = y[i];
				y2[i] = y[i];
			}
			for(i=1; i<n; i++)
				y1[ix[i]] = Math.Max(y1[ix[i]],y1[ix[i-1]]+size[ix[i-1]].Height);

			for(i=n-2; i>=0; i--)
				y2[ix[i]] = Math.Min(y2[ix[i]],y2[ix[i+1]]-size[ix[i]].Height);

			for(i=0; i<n; i++)
			{
				y[ix[i]] = (y1[ix[i]]+y2[ix[i]])/2.0;
			}
		}

		private void AdjustTextPositionOutside(double[] x, double[] y, SizeF[] size) // ... to avoid labels overlapping
		{
			if(x.Length<2)
				return;

			int m = x.Length, i,j;
			double[] X1 = new double[m];
			double[] X2 = new double[m];
			double[] Y1 = new double[m];
			double[] Y2 = new double[m];

			for(i=0;i<m;i++)
			{
				X1[i] = x[i];
				Y1[i] = y[i];
				X2[i] = x[i];
				Y2[i] = y[i];
			}

			for(j=0; j<m-1; j++)
				for(i=j+1; i<m; i++)
				{
					double x0 = X1[j];
					double y0 = Y1[j];
					double x1 = X1[i];
					double y1 = Y1[i];

					double dx = x[i] - x[j];
					double dy = y[i] - y[j];

					double h0 = size[j].Height;
					double w0 = size[j].Width;
					double h1 = size[i].Height;
					double w1 = size[i].Width;

					// X-overlapping
					double xovl = Math.Min(x0+w0,x1+w1)-Math.Max(x0,x1);
					// Y-overlapping
					double yovl = Math.Min(y0+h0,y1+h1)-Math.Max(y0,y1);

					if(xovl>0 && yovl>0)
					{
						double a = Math.Min(xovl/w0,yovl/h0);
						double abs = Math.Sqrt(dx*dx + dy*dy);
						dx += dx/abs*w0*a;
						dy += dy/abs*h0*a;
						X1[i] = x0+dx;
						Y1[i] = y0+dy;
					}
					else
					{
						X1[i] = x1;
						Y1[i] = y1;
					}
				}
			
			for(j=m-1; j>0; j--)
				for(i=j-1; i>=0; i--)
				{
					double x0 = X2[j];
					double y0 = Y2[j];
					double x1 = X2[i];
					double y1 = Y2[i];

					double dx = x[i] - x[j];
					double dy = y[i] - y[j];

					double h0 = size[j].Height;
					double w0 = size[j].Width;
					double h1 = size[i].Height;
					double w1 = size[i].Width;

					// X-overlapping
					double xovl = Math.Min(x0+w0,x1+w1)-Math.Max(x0,x1);
					// Y-overlapping
					double yovl = Math.Min(y0+h0,y1+h1)-Math.Max(y0,y1);

					if(xovl>0 && yovl>0)
					{
						double a = Math.Min(xovl/w0,yovl/h0);
						double abs = Math.Sqrt(dx*dx + dy*dy);
						dx += dx/abs*w0*a;
						dy += dy/abs*h0*a;
						X2[i] = x0+dx;
						Y2[i] = y0+dy;
					}
					else
					{
						X2[i] = x1;
						Y2[i] = y1;
					}
				}

			for(i=0; i<m; i++)
			{		
				x[i] = (X1[i]+X2[i])/2.0;
				y[i] = (Y1[i]+Y2[i])/2.0;
			}
		}

		#region IDisposable Members

		internal void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (labelStyle != null) 
				{
					labelStyle.Dispose();
					LabelStyle = null;
				}
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

	}
}
