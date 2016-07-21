using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing.Design;

namespace ComponentArt.Web.Visualization.Gauges
{
    /// <summary>
    /// Represents a visual tick-mark element of a <see cref="TickMarkSet"/> object.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TickMark : IObjectModelNode
    {
		private MarkerStyle markerStyle = new MarkerStyle();
		private Size2D size = new Size2D(3,5);
		private Color color = Color.Empty;
		private double val = 0;
		private double offset = 0;
		private bool visible = true;

        public TickMark() { }

		public double Value { get { return val; } set { val = value; } }
		public double Offset { get { return offset; } set { offset = value; } }

		public Size2D Size { get { return size; } set { size = value; } }
		public MarkerStyle Style { get { return markerStyle; } set { markerStyle = value; } }

		internal bool Visible { set { visible = value; } get { return visible; } }

		#region --- IObjectModelNode Implementation ---

		private IObjectModelNode parent;
		IObjectModelNode IObjectModelNode.ParentNode
		{
			get { return parent; }
			set { parent = value; }
		}

		#endregion
	}

    /// <summary>
    /// Represents a set of tick-mark elements for a <see cref="Range"/> object.
    /// </summary>
    [TypeConverter(typeof(TickMarkSetObjectConverter))]
	[Serializable]
    public class TickMarkSet : IObjectModelNode, ISizePositionRangeProvider
    {
		#region --- Member Variables ---
		private string markerStyleName="Auto";
		private Size2D size = new Size2D(0,0);
        private double offset = double.NaN; // Take it from the skin

        // Values
        private double step = double.NaN;
        private double minValue = double.NaN;
        private double maxValue = double.NaN;

        private bool customValues; 
        private double[] values;
        private double[] valuesUsedForTickmarks;

		private ArrayList list = new ArrayList();
		#endregion

		#region --- Construction and Population ---

		public TickMarkSet() { }

		public int Add(TickMark tMark)
		{
			(tMark as IObjectModelNode).ParentNode = this;
			return list.Add(tMark);
		}

		#endregion

		#region --- Properties ---

        [DefaultValue(double.NaN)]
        [TypeConverter(typeof(DoubleWithAutoConverter))]
        public double Step { get { return step; } set { if (step != value && !customValues) values = null; step = value; } }

        [DefaultValue(double.NaN)]
        [TypeConverter(typeof(DoubleWithAutoConverter))]
        public double MinValue { get { return minValue; } set { if (minValue != value && !customValues) values = null; minValue = value; } }

        [DefaultValue(double.NaN)]
        [TypeConverter(typeof(DoubleWithAutoConverter))]
        public double MaxValue { get { return maxValue; } set { if (maxValue != value && !customValues) values = null; maxValue = value; } }

        [Editor(typeof(SizePositionEditor),typeof(UITypeEditor))]
        [DefaultValue(null)]
        public Size2D Size 
		{
            get
            {
				if(size.Abs() > 0)
					return size;
				else if (ObjectModelBrowser.InSerialization(this))
					return null;
				else
				{
					SubGauge gauge = ObjectModelBrowser.GetOwningGauge(this);
					if (IsMajor)
						return gauge.Skin.ScaleAnnotationStyle.MajorTickmarkSize;
					else
						return gauge.Skin.ScaleAnnotationStyle.MinorTickmarkSize;
				}
            }
            set
            {
                if (Range != null && Range.IsMain)
                {
                    SubGauge gauge = ObjectModelBrowser.GetOwningGauge(this);
                    if (IsMajor)
                        gauge.Skin.ScaleAnnotationStyle.MajorTickmarkSize = value;
                    else
                        gauge.Skin.ScaleAnnotationStyle.MinorTickmarkSize = value;
                }
                else
                    size = value;
                list = null;
            }
		}

		[DefaultValue(double.NaN)]
		[TypeConverter(typeof(DoubleWithAutoConverter))]
		[Editor(typeof(SliderEditor),typeof(UITypeEditor)),ValueRange(-20,20)]
		public double Offset { get { if( Editing) return EffectiveOffset; else return offset; } set{ offset = value; list = null; } }

		internal double EffectiveOffset 
		{
			get 
			{
				if(double.IsNaN(offset))
				{
					SubGauge gauge = ObjectModelBrowser.GetOwningGauge(this);
					if(IsMajor)
						return gauge.Skin.ScaleAnnotationStyle.MajorTickmarkOffset;
					else
						return gauge.Skin.ScaleAnnotationStyle.MinorTickmarkOffset;
				}
				return offset; 
			} 
		}

		internal bool CustomValues { get { return customValues; } }
		
		private bool Editing 
		{
			get 
			{ 
				SubGauge topGauge = ObjectModelBrowser.GetOwningTopmostGauge(this);
				return topGauge != null && topGauge.Editing; 
			}
		}

		internal bool IsMajor { get { return (ObjectModelBrowser.GetAncestorByType(this,typeof(Range)) as Range).MajorTickMarks == this; } }

        // Supporting SizePositionEditor
        public virtual void GetRangesAndSteps(string propertyName,
            ref float x0, ref float x1, ref float stepX,
            ref float y0, ref float y1, ref float stepY)
        {
            if (propertyName == "Size")
            {
                x0 = 0;
                x1 = 20;
                stepX = 0.2f;

                y0 = 0;
                y1 = 20;
                stepY = 0.2f;
            }
        }
        [TypeConverter(typeof(MarkerStyleNameConverter))]
		[DefaultValue("Auto")]
		public string MarkerStyleName { get { return markerStyleName; } set { markerStyleName = value; } }
		
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MarkerStyleKind MarkerStyleKind { get { return MarkerStyle.NameToKind(markerStyleName); } set { markerStyleName = MarkerStyle.KindToName(value); } }

		internal MarkerStyle Style 
		{
			get 
			{
				MarkerStyle style = null;
				SubGauge gauge = ObjectModelBrowser.GetOwningGauge(this);
				if(markerStyleName == "Auto")
                    style = gauge.MarkerStyles[gauge.Skin.EName(IsMajor ? gauge.Skin.ScaleAnnotationStyle.MajorTickMarkStyleName : gauge.Skin.ScaleAnnotationStyle.MinorTickMarkStyleName)]; 
				else
					style = gauge.MarkerStyles[markerStyleName]; 
				if(style == null)
					style = gauge.MarkerStyles["Default"];
				return style;
			}
		}

		[Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double[] Values 
        {
            get 
            {
                if (values == null)
                    CreateValues();
                return values; 
            } 
            set 
            {
                values = value; customValues = (value != null); 
            }
        }

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Count { get { return (list==null)?0:list.Count; } }

		#region --- Internal Properties

        private double EffectiveMinValue
        {
            get
            {
                if (double.IsNaN(minValue))
                    return Range.EffectiveMinValue;
                else
                    return minValue;
            }
        }
        private double EffectiveMaxValue
        {
            get
            {
                if (double.IsNaN(maxValue))
                    return Range.EffectiveMaxValue;
                else
                    return maxValue;
            }
        }
        private double EffectiveStep
        {
            get
            {
                if (double.IsNaN(step))
                {
                    if (Range.MinorTickMarks == this)
                        return EffectiveMinorStep();
                    else
                        return EffectiveMajorStep();
                }
                else
                    return step;
            }
        }

        // NB: Refine the methos so that it deppends on the metrics of fonts/labels/range

        private double EffectiveMajorStep()
        {
            double[] fac = new double[] { 2, 2.5, 2 };
            int nSteps = GaugeGeometry.DefaultNumberOfSteps(Range);
            double min = EffectiveMinValue;
            double max = EffectiveMaxValue;

            double d = max - min;
            // starting power of 10 greater than the interval size
            d = Math.Pow(10, Math.Ceiling(Math.Log10(d)));
            // subdivision
            int ix = 0;
            while (true)
            {
                double min1 = Math.Floor(min / d) * d;
                double max1 = Math.Floor(max / d) * d;
                int k = 0;
                if ((min - min1) / d < 0.001) k++;
                k += (int)((max1 - min1) / d);
                if (k >= nSteps)
                    break;
                d = d / fac[ix % 3];
                ix++;
            }

            return d;
        }

        private double EffectiveMinorStep()
        {
            double[] fac = new double[] { 2, 2.5, 2 };
            int nSteps = GaugeGeometry.DefaultNumberOfSteps(Range);
            double min = EffectiveMinValue;
            double max = EffectiveMaxValue;
            double d = max - min;
            // starting power of 10 greater than the interval size
            d = Math.Pow(10, Math.Ceiling(Math.Log10(d)));
            // subdivision
            int ix = 0;
            while (true)
            {
                double min1 = Math.Floor(min / d) * d;
                double max1 = Math.Floor(max / d) * d;
                int k = 0;
                if ((min - min1) / d < 0.001) k++;
                //if ((max - max1) / d < 0.001) k++;
                k += (int)((max1 - min1) / d);
                if (k >= nSteps)
                    break;
                d = d / fac[ix % 3];
                ix++;
            }
            d = d / fac[ix  % 3] / fac[(ix + 1) % 3];
            return d;
        }

		internal Range Range { get { return ObjectModelBrowser.GetAncestorByType(this,typeof(Range)) as Range; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TickMark this[int ix]
        {
            get 
            { 
                return List[ix] as TickMark; 
            }
        }

        private ArrayList List
        {
            get
            {
                if (list == null || Values != valuesUsedForTickmarks)
                    CreateTickmarks(Values);
                return list;
            }
        }

		#endregion
		#endregion

		#region --- IObjectModelNode Implementation ---

		private IObjectModelNode parent;
		IObjectModelNode IObjectModelNode.ParentNode
		{
			get { return parent; }
			set { parent = value; }
		}

		#endregion

		#region --- Rendering ---

        internal void CreateValues()
        {
            if (Range.Scale.IsLogarithmic)
				values = GetLogatithmicValues();
			else
				values = GetArithmeticValues();
        }

		private double[] GetArithmeticValues()
		{
			ArrayList a = new ArrayList();
			Range range = this.Range;
			Scale scale = range.Scale;
			double min = range.EffectiveMinValue;
			double max = range.EffectiveMaxValue;

			double step = EffectiveStep;
			int n = (int)(min / step);
			double val = n * step;
			while (val < max + 0.001 * step)
			{
				if (val >= min - 0.001 * step)
					a.Add(val);
				val += step;
			}
			return (double[])a.ToArray(typeof(double));
		}

		private double[] GetLogatithmicValues()
		{
			ArrayList a = new ArrayList();
			Range range = this.Range;
			Scale scale = range.Scale;
			double min = range.EffectiveMinValue;
			double max = range.EffectiveMaxValue;

			double logV0 = Math.Floor(Math.Log10(min));
			double v0 = Math.Pow(10, logV0);
			double v = v0;
			double eps = v0*0.001;

			// Populate with powers of 10
			while (v <= max + eps)
			{
				if (v >= min - eps)
					a.Add(v);
				v *= 10;
			}
			if(a.Count <= 2)
				return GetArithmeticValues();

			double[] fac = new double[] { 2,2.5,2 };

			if (Range.MinorTickMarks == this)
			{
				v = v0;
				a.Clear();
				while (v <= max + eps)
				{
					for(int ix = 0; ix<3; ix++)
					{
						if (v >= min - eps)
							a.Add(v);
						v *= fac[ix];
						if(v > max + eps)
							break;
					}
				}
			}
			return (double[])a.ToArray(typeof(double));
		}

		internal void CreateTickmarks(double[] values)
		{
            list = new ArrayList();
			MarkerStyle style = this.Style;
			Range range = this.Range;
            valuesUsedForTickmarks = values;
            for (int i = 0; i < values.Length; i++)
			{
				TickMark mark = new TickMark();
				mark.Size = Size;
				mark.Style = style;
				mark.Offset = EffectiveOffset;
				mark.Value = values[i];
				Add(mark);
			}
		}
		
		private bool visible = false;
		[Category("General")]
		[Description("Tick marks visible")]
		[NotifyParentProperty(true)]
		[DefaultValue(false)]
		public bool Visible { get { return visible; } set { visible = value; ObjectModelBrowser.NotifyChanged(this); } }

		internal void Render(RenderingContext context)
		{
			if(!Visible)
				return;

			if(this.Style == null)
				return;
			Range range = Range;
			SubGauge topGauge = ObjectModelBrowser.GetOwningTopmostGauge(range);
			TickMarkRenderingContext tmContext = ObjectModelBrowser.GetFactory(this).CreateTickMarkRenderingContext(this.Style);

			GaugeKind kind = range.Scale.Gauge.GaugeKind;
			double rMin = range.EffectiveMinValue;
			double rMax = range.EffectiveMaxValue;
			float rad = range.Scale.MaximumRadius();
			rad = GaugeGeometry.LinearSize(Range.Gauge);
			MarkerStyle markerStyle = topGauge.MarkerStyles[markerStyleName];
			if(markerStyle == null)
				markerStyle = topGauge.MarkerStyles["Default"];
			Point2D hotSpot = markerStyle.RelativeHotSpot;
			float hsX = hotSpot.X*0.01f;
			float hsY = hotSpot.Y*0.01f;

			for(int j=0; j<List.Count; j++)
			{
				TickMark mark = this[j];
				if(!mark.Visible)
					continue;

				Point2D valuePoint = range.WorldToRenderingCoordinates(mark.Value);
				Size2D vec;
				if(SubGauge.IsInGroup(kind,GaugeKindGroup.Radial)) // NB: Should we handle this in the Geometry?
				{
					Point2D centerPoint = range.Scale.CenterPoint();
					vec = (valuePoint-centerPoint).Unit();
				}
				else if(kind == GaugeKind.LinearHorizontal)
					vec = new Size2D(0,-1);
				else
					vec = new Size2D(1,0);
				double pos =  mark.Offset*0.01;
				// Points on the centerline of the marker rectangle
				double pos0 = rad*(pos+(1-hsY)*mark.Size.Height*0.01);
				double pos1 = rad*(pos+(-hsY)*mark.Size.Height*0.01);
				Point2D PC0 = valuePoint + pos0*vec;
				Point2D PC1 = valuePoint + pos1*vec;
				// Points on the left edge
				float dy = (float)(PC1-PC0).Abs();
				float dx = (float)(mark.Size.Width/mark.Size.Height*dy); 
				Size2D norm = (PC1-PC0).Normal().Unit();
				Point2D P00 = PC0 + (1-hsX)*dx*norm;
				Point2D P01 = PC1 + (1-hsX)*dx*norm;

				RenderingContext markerContext = context.DefineMapping(new Point2D(0,0),new Point2D(0,dy), P00,P01);
				if(mark.Style.BaseColor.IsEmpty)
				{
					MultiColor mc = (IsMajor? topGauge.Palette.MajorTickmarkBaseColor:topGauge.Palette.MinorTickmarkBaseColor);
					tmContext.BaseColor = mc.ColorAt((float)((mark.Value-rMin)/(rMax-rMin)));
				}

				if(mark.Style.IsImage)
				{
					if(mark.Style.MarkerLayer!= null)
					{
						mark.Style.MarkerLayer.BackgroundColor = mark.Style.BaseColor;
						RenderingContext tickmarkContext = markerContext.SetAreaMapping(
							new Rectangle2D(0,0,mark.Style.MarkerLayer.Size.Width,mark.Style.MarkerLayer.Size.Height),
							new Rectangle2D(0,0,dx,dy), true);
						mark.Style.MarkerLayer.Render(tickmarkContext);
					}
				}
				else
				{
					context.Engine.DrawTickMark(mark,dx,dy,markerContext,tmContext);
					if(topGauge.RenderTickMarksMapAreas)
						topGauge.MapAreas.Add(context.Engine.TickMarkMapArea(mark,dx,dy,markerContext));
				}
			}
		}


		#endregion

    }

	// ====================================================================================================
	// Object converter is needed to provide instance descriptor for serialization
	// ====================================================================================================
	
	internal class TickMarkSetObjectConverter: ExpandableObjectConverter
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			if (destinationType == typeof(InstanceDescriptor)) 
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if (destinationType == typeof(InstanceDescriptor) && 
				(value is TickMarkSet || value.GetType().IsSubclassOf(typeof(TickMarkSet))))
			{
				System.Reflection.ConstructorInfo ci = value.GetType().GetConstructor
					(new Type[] {});
				return new InstanceDescriptor(ci, new Object[] {}, false);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
