using System;
using System.Xml;
using ComponentArt.Web.Visualization.Charting.Design;


namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// A collection of <see cref="SeriesStyle"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class SeriesStyleCollection : NamedCollection
	{
		internal SeriesStyleCollection(Object owner, bool initialize) 
			: base (typeof(SeriesStyle),owner) 
		{
			if(initialize)
				InitializeContents();
		}

		internal SeriesStyleCollection(Object owner) : this(owner,true) { }
		
		internal SeriesStyleCollection() : this(null) { }

		/// <summary>
		/// Indicates the <see cref="SeriesStyle"/> at the specified indexed location in the <see cref="SeriesStyleCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based integer index or name 
		/// or a <see cref="SeriesStyleKind"/> to retrieve a <see cref="SeriesStyle"/> from the <see cref="SeriesStyleCollection"/> object.</param>
		public new SeriesStyle this[object index]   
		{ 
			get 
			{ 
				if(index is SeriesStyleKind)
					index = ((SeriesStyleKind)index).ToString();
				return (SeriesStyle)base[index]; 
			} 
			set 
			{
				if(index is SeriesStyleKind)
					index = ((SeriesStyleKind)index).ToString();
				base[index] = value; 
			} 
		}

		#region --- Member Creation Interface ---

		public SeriesStyle CreateNew(string newMemberName)
		{
			return CreateFrom(newMemberName,"Default");
		}

		/// <summary>
		/// Clones and stores the specified <see cref="SeriesStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned style.</param>
		/// <param name="oldMemberName">Name of the original style.</param>
		/// <returns>Returns the cloned style.</returns>
		/// <remarks>If the original style does not exist, the function returns null. 
		/// If the collection already contents the style with the cloned style name, the old style will be overriden.
		/// </remarks>
		public SeriesStyle CreateFrom(string newMemberName, string oldMemberName)
		{
			SeriesStyle original = this[oldMemberName];
			if(original == null)
				return null;
			StyleCloner cloner = new StyleCloner();
			SeriesStyle clonedStyle = cloner.Clone(original) as SeriesStyle;
			clonedStyle.Name = newMemberName;
			original.OwningCollection.Add(clonedStyle);

			return clonedStyle;
		}

		/// <summary>
		/// Clones and stores the specified <see cref="SeriesStyle"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned style.</param>
		/// <param name="oldMemberKind"><see cref="SeriesStyleKind"/> of the original style.</param>
		/// <returns>Returns the cloned style.</returns>
		/// <remarks> 
		/// If the collection already contents the style with the cloned style name, the old style will be overriden.
		/// </remarks>
		public SeriesStyle CreateFrom(string newMemberName, SeriesStyleKind oldMemberKind)
		{
			return CreateFrom(newMemberName,oldMemberKind.ToString());
		}
		#endregion

		#region --- XML Serialization ---

		internal void Serialize(XmlCustomSerializer S)
		{
			S.Comment("    ============  ");
			S.Comment("    Chart Styles  ");
			S.Comment("    ============  ");
			if(S.Reading)
			{
				Clear();
				if(S.GoToFirstChild("SeriesStyle"))
				{
					SeriesStyle style = new SeriesStyle();
					style.Serialize(S);
					Add(style);
					while(S.GoToNext("SeriesStyle"))
					{
						style = new SeriesStyle();
						style.Serialize(S);
						Add(style);
					}
					S.GoToParent();
				}
			}
			else
			{
				foreach(SeriesStyle style in this)
				{
					if(S.BeginTag("SeriesStyle"))
					{
						style.Serialize(S);
						S.EndTag();
					}
				}
			}
		}

		internal void CreateDOM(XmlElement parent)
		{
		}

		internal static SeriesStyleCollection CreateFromDOM(XmlElement root)
		{
			SeriesStyleCollection styles = new SeriesStyleCollection(null);
			XmlNode subNode = root.FirstChild;
			while(subNode != null)
			{
				if(subNode.Name.ToLower() == "chartstyle")
				{
					SeriesStyle S = SeriesStyle.CreateFromDOM(subNode as XmlElement);
					if(S != null)
					{
						styles.Add(S);
					}
				}
				subNode = subNode.NextSibling;
			}
			return styles;
		}
		#endregion

		#region --- Collection Initialization ---

		private void InitializeContents()
		{
			SetupSeriesStyle(ChartKind.Cylinder);
			SetupSeriesStyle(ChartKind.Block);
			SetupSeriesStyle(ChartKind.Hexagon);
			SetupSeriesStyle(ChartKind.Paraboloid);
			SetupSeriesStyle(ChartKind.Cone);
			SetupSeriesStyle(ChartKind.Prism3);
			SetupSeriesStyle(ChartKind.Pyramid6);
			SetupSeriesStyle(ChartKind.Pyramid);
			SetupSeriesStyle(ChartKind.Pyramid3);
			SetupSeriesStyle(ChartKind.Rectangle);
			SetupSeriesStyle(ChartKind.Line);
			SeriesStyle CSLSm  = SetupSeriesStyle(ChartKind.Line,"LineSmooth");
			SeriesStyle CSLSt  = SetupSeriesStyle(ChartKind.Line,"LineStep");
			SetupSeriesStyle(ChartKind.Line2D);
			SeriesStyle CSLSm2 = SetupSeriesStyle(ChartKind.Line2D,"Line2DSmooth");
			SeriesStyle CSLSt2 = SetupSeriesStyle(ChartKind.Line2D,"Line2DStep");
			SetupSeriesStyle(ChartKind.Area);
			SeriesStyle CSASm  = SetupSeriesStyle(ChartKind.Area,"AreaSmooth");
			SeriesStyle CSASt  = SetupSeriesStyle(ChartKind.Area,"AreaStep");
			SetupSeriesStyle(ChartKind.Area2D);
			SeriesStyle CSASm2 = SetupSeriesStyle(ChartKind.Area2D,"Area2DSmooth");
			SeriesStyle CSASt2 = SetupSeriesStyle(ChartKind.Area2D,"Area2DStep");
			
			SetupSeriesStyle(ChartKind.Doughnut);
			SeriesStyle dntThin = SetupSeriesStyle(ChartKind.Doughnut,"DoughnutThin");
			SeriesStyle dntRound = SetupSeriesStyle(ChartKind.Doughnut,"DoughnutRound");
			SeriesStyle dntInverted = SetupSeriesStyle(ChartKind.Doughnut,"DoughnutInverted");

			SetupSeriesStyle(ChartKind.Pie);
			SeriesStyle pieThin = SetupSeriesStyle(ChartKind.Pie,"PieThin");
			SeriesStyle pieRound = SetupSeriesStyle(ChartKind.Pie,"PieRound");

			SetupSeriesStyle(ChartKind.Bubble);
			SetupSeriesStyle(ChartKind.Bubble2D);
			SetupSeriesStyle(ChartKind.CandleStick);
			SetupSeriesStyle(ChartKind.HighLowOpenClose);
			SeriesStyle Rl = SetupSeriesStyle(ChartKind.Line,"RadarLine");
			SeriesStyle Ra = SetupSeriesStyle(ChartKind.Area2D,"RadarArea");
			SeriesStyle Rm = SetupSeriesStyle(ChartKind.Marker,"RadarMarker");

			CSLSm.UpperBoundLineKind = LineKind.Smooth;
			CSLSm.LineKind = LineKind.Smooth;

			CSLSt.UpperBoundLineKind = LineKind.Step;
			CSLSt.LineKind = LineKind.Step;

			CSLSm2.UpperBoundLineKind = LineKind.Smooth;
			CSLSm2.LineKind = LineKind.Smooth;
			
			CSLSt2.UpperBoundLineKind = LineKind.Step;
			CSLSt2.LineKind = LineKind.Step;

			CSASm.UpperBoundLineKind = LineKind.Smooth;
			CSASm.LowerBoundLineKind = LineKind.Smooth;
			CSASt.UpperBoundLineKind = LineKind.Step;
			CSASt.LowerBoundLineKind = LineKind.Step;

			CSASm2.UpperBoundLineKind = LineKind.Smooth;
			CSASm2.LowerBoundLineKind = LineKind.Smooth;
			CSASt2.UpperBoundLineKind = LineKind.Step;
			CSASt2.LowerBoundLineKind = LineKind.Step;

			dntThin.RelativeInnerRadius = 0.3;
			dntThin.RelativeInnerEdgeSmoothingRadius = 0.05;
			dntThin.RelativeOuterEdgeSmoothingRadius = 0.05;
			dntThin.RelativeHeight = 0.1;

			dntRound.RelativeInnerRadius = 0.3;
			dntRound.RelativeInnerEdgeSmoothingRadius = 0.5;
			dntRound.RelativeOuterEdgeSmoothingRadius = 0.5;
			dntRound.RelativeHeight = 0.35;

			dntInverted.RelativeInnerRadius = 0.3;
			dntInverted.RelativeInnerEdgeSmoothingRadius = 0.9;
			dntInverted.RelativeOuterEdgeSmoothingRadius = 0.05;
			dntInverted.RelativeHeight = 0.5;

			pieThin.RelativeInnerEdgeSmoothingRadius = 0.05;
			pieThin.RelativeOuterEdgeSmoothingRadius = 0.05;
			pieThin.RelativeHeight = 0.1;

			pieRound.RelativeInnerEdgeSmoothingRadius = 0.5;
			pieRound.RelativeOuterEdgeSmoothingRadius = 0.5;
			pieRound.RelativeHeight = 0.35;

			Rl.IsRadar = true;
			Ra.IsRadar = true;
			Rm.IsRadar = true;

			CSLSm.IsDefault = true;
			CSLSt.IsDefault = true;
			CSLSm2.IsDefault = true;
			CSLSt2.IsDefault = true;
			CSASm.IsDefault = true;
			CSASt.IsDefault = true;
			CSASm2.IsDefault = true;
			CSASt2.IsDefault = true;

			dntThin.IsDefault = true;
			dntRound.IsDefault = true;
			dntInverted.IsDefault = true;
			pieThin.IsDefault = true;
			pieRound.IsDefault = true;

			Rl.IsDefault = true;
			Ra.IsDefault = true;
			Rm.IsDefault = true;
			
			SetupMarkerStyles();

			SeriesStyle s = SetupSeriesStyle(ChartKind.Bubble,"DefaultMissingPointsStyle");
		}

		private void SetupMarkerStyles()
		{
			SeriesStyle s;
			s = SetupSeriesStyle(ChartKind.Marker,"BlockMarker");
			s.MarkerStyleName = "Block";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"CircleMarker");
			s.MarkerStyleName = "Circle";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"Diamond");
			s.MarkerStyleName = "Diamond";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"Triangle");
			s.MarkerStyleName = "Triangle";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"InvertedTriangle");
			s.MarkerStyleName = "InvertedTriangle";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"LeftTriangle");
			s.MarkerStyleName = "LeftTriangle";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"RightTriangle");
			s.MarkerStyleName = "RightTriangle";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"Cross");
			s.MarkerStyleName = "Cross";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"XShape");
			s.MarkerStyleName = "XShape";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"ArrowE");
			s.MarkerStyleName = "ArrowE";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"ArrowW");
			s.MarkerStyleName = "ArrowW";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"ArrowN");
			s.MarkerStyleName = "ArrowN";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"ArrowS");
			s.MarkerStyleName = "ArrowS";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"ArrowNE");
			s.MarkerStyleName = "ArrowNE";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"ArrowNW");
			s.MarkerStyleName = "ArrowNW";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"ArrowSE");
			s.MarkerStyleName = "ArrowSE";
			s.IsDefault = true;
			s = SetupSeriesStyle(ChartKind.Marker,"ArrowSW");
			s.MarkerStyleName = "ArrowSW";
			s.IsDefault = true;
		}
		
		private SeriesStyle SetupSeriesStyle(ChartKind kind)
		{
			return SetupSeriesStyle(kind,kind.ToString());
		}
		private SeriesStyle SetupSeriesStyle(ChartKind kind, string kindName)
		{
			SeriesStyle CS = this[kindName];
			if(CS == null) 
			{
				CS = new SeriesStyle	(kind,kindName);
				CS.ForceSquareBase = true;
				CS.IsDefault = true;
				CS.Removable = false;	
				if(Owner != null)
					CS.OwningChart = (ChartBase)Owner;
				if(kind == ChartKind.Marker)
					CS.ForceSquareBase = false;
				Add(CS);
			}
			return CS;
		}
		#endregion
	}
}
