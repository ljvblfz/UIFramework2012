using System;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Represents a visual pointer element of a <see cref="Scale"/> object.
	/// </summary>
	[TypeConverter(typeof(NamedObjectConverter))]
	[Serializable]
    public class Pointer : NamedObject
    {
        private double val = 0;
		private float relLength = 100f;
		private string styleName = "Auto";
		private bool inValueChanging = false;

		private SliderValue sliderValue = null;

		/// <summary>
		/// New pointers should be created thorough the PointerCollection.AddNewMember() method
		/// </summary>
		public Pointer() : base(string.Empty) { }
		internal Pointer(string name) : base(name) { }

		/// <summary>
		/// The value this pointer shows or points to in the gauge.
		/// </summary>
        [NotifyParentProperty(true)]
		[DefaultValue(0.0)]
        public double Value 
		{
			get { return val; } 
			set
			{
				if(val != value)
				{
					if(!inValueChanging)
					{
						inValueChanging = true; // protection against infinite loop
						if(TopGauge != null)
							TopGauge.HandleValueChange(this, val, value);
						inValueChanging = false;
					}
					val = value; 
				}
			} 
		}

		internal SliderValue PointerValue
		{
			get
			{
				Scale scale = ObjectModelBrowser.GetAncestorByType(this,typeof(Scale)) as Scale;
				sliderValue = new SliderValue(scale.MinValue,scale.MaxValue,Value,0);
				return sliderValue;
			}
			set
			{
				sliderValue = value;
				Value = value.Value;
			}
		}
		/// <summary>
		/// The length of the needle pointer (if applicable) relative to the size of the entire gauge.
		/// </summary>
        [Editor(typeof(SliderEditor),typeof(UITypeEditor))]
		[ValueRange(0,120,1)]
		public double RelativeLength { get { return relLength; } set { relLength = (float)value; } }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // don't serialize
		[Browsable(false)]
		internal Scale Scale { get { return ObjectModelBrowser.GetAncestorByType(this,typeof(Scale)) as Scale; } }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // don't serialize
		[Browsable(false)]
		internal SubGauge Gauge { get { return ObjectModelBrowser.GetOwningGauge(this); } }

		public override string ToString()
        {
            return "\"" + Name + "\"=" + Value.ToString();
        }

		/// <summary>
		/// The PointerStyle that all look related properties are taken from.
		/// </summary>
		[TypeConverter(typeof(PointerStyleNameConverter))]
		[DefaultValue("Auto")]
		public string StyleName 
		{
			get 
			{
				if(IsMain())
				{
					if(ObjectModelBrowser.InSerialization(this))
						return "Auto"; // we don't serialize this; it is serialized in the theme/skin
					return Gauge.Skin.PointerStyleName;
				}
				else
					return styleName; 
			}
			set 
			{
				if(IsMain())
					Gauge.Skin.PointerStyleName = value;
				else
					styleName = value; 
			}
		}

		internal bool IsMain()
		{
			return (Name == "Main" && Scale != null && Scale.Name == "Main");
		}

		internal PointerStyle Style { get { return TopGauge.PointerStyles[StyleName]; } }

		internal SubGauge TopGauge 
		{
			get 
			{
				return ObjectModelBrowser.GetOwningTopmostGauge(this);
			}
		}
		
		#region --- Rendering ---
		private bool visible = true;
		/// <summary>
		/// Whether pointer is visible or not.
		/// </summary>
		[Category("General")]
		[Description("Indicator visible")]
		[NotifyParentProperty(true)]
		[DefaultValue(true)]
		public bool Visible { get { return visible; } set { visible = value; ObjectModelBrowser.NotifyChanged(this); } }

		internal void Render(RenderingContext context)
		{
			if(!Visible || !Scale.Visible)
				return;

			if(SubGauge.IsInGroup(Gauge.GaugeKind,GaugeKindGroup.Radial) && Scale.ScaleLayout.EffectivePosition(Gauge) <= 0)
				return;

			PointerStyle style = Style;
			if(style == null)
				return;
			switch(style.PointerKind)
			{
				case PointerKind.Needle:
					RenderNeedleKind(context);
					break;
				case PointerKind.Marker:
					RenderMarkerKind(context);
					break;
				case PointerKind.Bar:
					RenderBarKind(context);
					break;
				default: break;
			}
		}
		
		internal void RenderMarkerKind(RenderingContext context)
		{
			PointerStyle style = Style;
			MarkerStyle markerStyle = TopGauge.MarkerStyles[style.MarkerStyleName];
			if(markerStyle == null)
				return;

			Scale scale = this.Scale;
			Point2D scalePoint = scale.WorldToRenderingCoordinates(Value);
			
			TickMark mark = new TickMark();
			mark.Style = markerStyle;
			mark.Value = Value;
			mark.Size = style.MarkerSize;

			mark.Offset = 0;//style.MarkerOffset;

			// Adjust offset to respect pointer end-point
			if(SubGauge.IsInGroup(Gauge.GaugeKind,GaugeKindGroup.Radial)) 
			{
				Point2D cp = Scale.CenterPoint();
				Point2D vp = Scale.WorldToRenderingCoordinates(Value);
			}

			Point2D valuePoint = scale.WorldToRenderingCoordinates(mark.Value,style.MarkerOffset);
			Size2D vec;
			GaugeKind kind = scale.Gauge.GaugeKind;
			float rad = GaugeGeometry.LinearSize(scale.Gauge);
			if(SubGauge.IsInGroup(kind,GaugeKindGroup.Radial)) // NB: Should we handle this in the Geometry?
			{
				Point2D centerPoint = scale.CenterPoint();
				vec = (valuePoint-centerPoint).Unit();
			}
			else if(kind == GaugeKind.LinearHorizontal)
				vec = new Size2D(0,-1);
			else
				vec = new Size2D(1,0);
			double pos =  mark.Offset*0.01;
			Point2D hotSpot = markerStyle.RelativeHotSpot;
			float hsX = hotSpot.X*0.01f;
			float hsY = hotSpot.Y*0.01f;
			// Points on the centerline of the marker rectangle
			//double pos0 = rad*(pos-0.5*mark.Size.Height*0.01);
			//double pos1 = rad*(pos+0.5*mark.Size.Height*0.01);
			double pos0 = rad*(1-hsY)*mark.Size.Height*0.01;
			double pos1 = rad*(-hsY)*mark.Size.Height*0.01;
			Point2D PC0 = valuePoint + pos0*vec;
			Point2D PC1 = valuePoint + pos1*vec;
			// Points on the left edge
			float dy = (float)(PC1-PC0).Abs();
			float dx = (float)(mark.Size.Width/mark.Size.Height*dy); 
			Size2D norm = (PC1-PC0).Normal().Unit();
			Point2D P00 = PC0 + (1-hsX)*dx*norm;
			Point2D P01 = PC1 + (1-hsX)*dx*norm;

			RenderingContext markerContext = context.DefineMapping(new Point2D(0,0),new Point2D(0,dy), P00,P01);

			MultiColor mc = style.PointerBackColor;
			if(mc.IsEmpty)
				mc = TopGauge.Palette.PointerBaseColor;
			TickMarkRenderingContext tmContext = 
				ObjectModelBrowser.GetFactory(this).CreateTickMarkRenderingContext(markerStyle);
			float relValue = (float)((Value-scale.MinValue)/(scale.MaxValue-scale.MinValue));

			tmContext.BaseColor = mc.ColorAt(relValue);
			
			if(mark.Style.IsImage)
			{
				if(mark.Style.MarkerLayer!= null)
				{
					mark.Style.MarkerLayer.BackgroundColor = mark.Style.BaseColor;
					RenderingContext tickmarkContext = markerContext.SetAreaMapping(
						new Rectangle2D(0,0,mark.Style.MarkerLayer.Size.Width,mark.Style.MarkerLayer.Size.Height),
						new Rectangle2D(0,0,dx,dy), true);
					mark.Style.MarkerLayer.Render(tickmarkContext);
					if(TopGauge.RenderPointersMapAreas)
					{
						MapArea mapArea = context.Engine.TickMarkMapArea(mark,dx,dy,markerContext);
						mapArea.SetObject(this);
						TopGauge.MapAreas.Add(mapArea);
					}
				}
			}
			else
			{
				context.Engine.DrawTickMark(mark,dx,dy,markerContext,tmContext);
				if(TopGauge.RenderPointersMapAreas)
				{
					MapArea mapArea = context.Engine.TickMarkMapArea(mark,dx,dy,markerContext);
					mapArea.SetObject(this);
					TopGauge.MapAreas.Add(mapArea);
				}
			}

		}
		
		internal void RenderBarKind(RenderingContext context)
		{
			Range barRange = new Range();
			(barRange as IObjectModelNode).ParentNode = this;
			MultiColor mc = Style.PointerBackColor;
			if(mc.IsEmpty)
				mc = TopGauge.Palette.PointerBaseColor;
			barRange.Color = mc;
			barRange.RangeLayout.Offset = this.RelativeLength - 100;

			// Adjust offset to respect pointer end-point
			if(SubGauge.IsInGroup(Gauge.GaugeKind,GaugeKindGroup.Radial)) 
			{
				Point2D centerPoint = Scale.CenterPoint();
				Point2D valuePoint = Scale.WorldToRenderingCoordinates(Value);
				Size2D rad = valuePoint-centerPoint;
				barRange.RangeLayout.Offset = barRange.RangeLayout.Offset*rad.Abs()/GaugeGeometry.LinearSize(Scale.Gauge);
			}

			barRange.MinValue = this.Scale.MinValue;
			barRange.MaxValue = this.Value;
			barRange.StartWidth = Style.BarStartWidth;
			barRange.EndWidth = Style.BarEndWidth;
			barRange.RenderStrip(context);
		}

        internal void RenderNeedleKind(RenderingContext context)
        {
			if(!SubGauge.IsInGroup(this.Gauge.GaugeKind,GaugeKindGroup.Radial))
				return; // NB: Some pointers should be handled here

            PointerStyle style = Style;
			if(!style.Visible)
				return;
			else if(!style.HubVisible)
			{
				RenderNeedle(context);
			}
			else if (style.HubAboveNeedle)
            {
				RenderHubShadow(context);
				RenderNeedle(context);
                RenderHub(context);
            }
            else
            {
				RenderHubShadow(context);
				RenderHub(context);
                RenderNeedle(context);
            }
        }

		private void RenderHub(RenderingContext context)
		{
			PointerStyle style = Style;
			Layer hubLayer = style.HubLayer;
			if(hubLayer != null)
			{
				hubLayer.BackgroundColor = style.HubBackColor;
				if (style.HubBackColor.IsEmpty)
					hubLayer.BackgroundColor = TopGauge.Palette.HubBaseColor;
				if (hubLayer != null && hubLayer.MainVisualParts != null && hubLayer.MainVisualParts.Count>0)
				{
					Size2D hSize = hubLayer.MainVisualParts[0].Size;
					float relateiveHubSize = (float)style.RelativeHubRadius;
					if(relateiveHubSize <= 0)
						return;
					float maxDiameter = Scale.MaximumRadius()*2;
					Size2D hTargetSize = (new Size2D(maxDiameter,maxDiameter))*relateiveHubSize*0.01;
					Rectangle2D hubTarget = new Rectangle2D(Scale.CenterPoint() - hTargetSize/2,hTargetSize);
					RenderingContext hubContext = context.SetAreaMapping( new Rectangle2D(new Point2D(0, 0), hSize),hubTarget, true);
					hubLayer.Render(hubContext);
				}
			}
		}

		private void RenderHubShadow(RenderingContext context)
		{
			PointerStyle style = Style;
			Layer hubLayer = style.HubLayer;
			Theme theme = this.Gauge.Theme;
			if(hubLayer != null)
			{
				Size2D hSize = hubLayer.Size;
				float relateiveHubSize = (float)style.RelativeHubRadius;
				if(relateiveHubSize <= 0)
					return;
				float maxDiameter = Scale.MaximumRadius()*2;
				Size2D hTargetSize = (new Size2D(maxDiameter,maxDiameter))*relateiveHubSize*0.01;
				Rectangle2D hubTarget = new Rectangle2D(Scale.CenterPoint() - hTargetSize/2,hTargetSize);
				RenderingContext hubContext = context.SetAreaMapping( new Rectangle2D(new Point2D(0, 0), hSize),hubTarget, true);
				Size2D shadowOffset = theme.HubShadowOffset*GaugeGeometry.LinearSize(this.Gauge)*0.01f;
				hubLayer.RenderShadow(hubContext,shadowOffset);
			}
		}


		internal void RenderNeedle(RenderingContext context)
		{
			PointerStyle style = Style;
			Scale scale = Scale;
			Point2D centerPoint = scale.CenterPoint();
			Point2D scalePoint = scale.WorldToRenderingCoordinates(Value);
			Point2D tipPoint = centerPoint + RelativeLength*(scalePoint-centerPoint)*0.01f;
			
			Layer needleLayer = style.NeedleLayer;
			if(needleLayer == null)
				return;
			
			// Shadow
			Theme theme = Gauge.Theme;
			LayerVisualPart shadow = needleLayer.Shadow;
			if(shadow != null)
			{
				// NB: Take these two from the style
                Size2D shadowOffset = theme.NeedleShadowOffset * GaugeGeometry.LinearSize(scale.Gauge) * 0.01f;
				Color shadowColor = Color.FromArgb(128,0,0,0);

				shadow.RelativeCenterPoint = style.RelativeCenterPoint;
				shadow.RelativeEndPoint = style.RelativeEndPoint;
				context.Engine.DrawNiddleRegion(shadow,centerPoint+shadowOffset,tipPoint+shadowOffset,context,shadowColor);
			}

			// Region
			
			LayerVisualPart needleRegion = needleLayer.Region;
			if(needleRegion != null)
			{
				needleRegion.RelativeCenterPoint = style.RelativeCenterPoint;
				needleRegion.RelativeEndPoint = style.RelativeEndPoint;
				MultiColor mc = style.PointerBackColor;
				if(mc.IsEmpty)
					mc = TopGauge.Palette.PointerBaseColor;
				float relValue = (float)((Value-scale.MinValue)/(scale.MaxValue-scale.MinValue));
				style.EffectivePointerBackColor = mc.ColorAt(relValue);
				context.Engine.DrawNiddleRegion(needleRegion,centerPoint,tipPoint,context,style.EffectivePointerBackColor);
			}

			LayerVisualPartCollection needleParts = needleLayer.MainVisualParts;
			if(needleParts != null)
			{
				foreach(LayerVisualPart part in needleParts)
				{
					part.RelativeCenterPoint = style.RelativeCenterPoint;
					part.RelativeEndPoint = style.RelativeEndPoint;
					context.Engine.DrawNiddle(part,centerPoint,tipPoint,context);
				}
			}

			// Map areas

			if(TopGauge.RenderPointersMapAreas)
			{
				RenderingContext needleCtx = context.GetNeedleContext(needleRegion,centerPoint,tipPoint);
				MapAreaCollection maps = needleRegion.CreateMapAreas();
				maps = needleCtx.Transform(maps);
				for(int i=0; i<maps.Count; i++)
				{
					maps[i].SetObject(this);
					TopGauge.MapAreas.Add(maps[i]);
				}
			}
		}

		#endregion

    #region --- Client-side serialization ---
#if WEB 
        
    internal Hashtable ExportJsObject()
    {
      Hashtable pointer = new Hashtable();

      pointer.Add("name", Name);
      pointer.Add("value", Value);
      pointer.Add("visible", Visible);

      return pointer;
    }

    internal void ImportJsObject(Hashtable pointer)
    {
      if (pointer.ContainsKey("value"))
        Value = (double)pointer["value"];

      if (pointer.ContainsKey("visible"))
        Visible = (bool)pointer["visible"];
    }

#endif
    #endregion

    }

	/// <summary>
	/// Contains a collection of <see cref="Pointer"/> objects.
	/// </summary>
	[Serializable]
    public class PointerCollection : NamedObjectCollection
    {
        internal int Add(Pointer pointer)
        {
            return base.Add(pointer);
        }

		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates new member of the collection by cloning the member called "Main". If member named "Main" doesn't exist, a new
		/// instance of MarkerStyle is created.
		/// </summary>
		/// <param name="newMemberName">Name of the new member.</param>
		/// <returns>Returns the created object.</returns>
		public Pointer AddNewMember(string newMemberName)
		{
			Pointer newMember = AddNewMemberFrom(newMemberName,"Main");
			if(newMember == null)
			{
				newMember = new Pointer(newMemberName);
				Add(newMember);
			}
			return newMember;
			
		}

		/// <summary>
		/// Clones and stores the specified <see cref="Pointer"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new Pointer AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName,oldMemberName) as Pointer;
		}

		#endregion

		
		internal override NamedObject CreateNewMember()
		{
			Pointer newPointer = new Pointer();
			SelectGenericNewName(newPointer);
			Add(newPointer);
			return newPointer;
		}

		/// <summary>
		/// Retrieves the pointer from the collection based on its order of addition to the collection.  Main pointer is 0.
		/// </summary>
		/// <param name="ix">the number or the name of the pointer as it was being added</param>
		/// <returns>the pointer</returns>
		public new Pointer this[object ix]
		{
			get { return base[ix] as Pointer; }
			set { base[ix] = value; }
		}
//
//		/// <summary>
//		/// Retrieves the pointer from the collection based on its name, given when the pointer was created.
//		/// </summary>
//		/// <param name="name">the name of the requested pointer</param>
//		/// <returns>the pointer</returns>
//		public new Pointer this[string name]
//		{
//			get { return base[name] as Pointer; }
//			set { base[name] = value; }
//		}

    #region --- Client-side serialization ---
#if WEB 

    internal ArrayList ExportJsArray()
    {
      ArrayList pointers = new ArrayList();

      for (int i = 0; i < this.Count; i++)
        pointers.Insert(i, this[i].ExportJsObject());

      return pointers;
    }

    internal void ImportJsArray(IEnumerable pointers)
    {
      foreach (Hashtable pointer in pointers)
        this[(string)pointer["name"]].ImportJsObject(pointer);
    }

#endif
    #endregion

  }
}
