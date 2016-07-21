using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Gauges
{
    /// <summary>
    /// Specifies how annotation labels are rendered relative to a <see cref="Range"/> object.
    /// </summary>
	public enum LabelOrientation
	{
		ScaleDirection,
		NormalToScale,
		Horizontal,
		Vertical
	}

	/// <summary>
    /// Represents a user-defined visual text annotation element of a <see cref="Gauge"/> object.
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(NamedObjectConverter))]
	public class TextAnnotation : NamedObject
	{
		private string text = "";
		private TextStyle textStyle;
		private string textStyleName = "DefaultIndicatorLabelStyle";
		private Point2D location;
		private Point2D relativeLocation = new Point2D(50,25);
		private double angleDegrees;
		private object tag;

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New TextAnnotation instances should be created with <see cref="TextAnnotationCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// </summary>
		public TextAnnotation() : this(null,null,null) { }

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New TextAnnotation instances should be created with <see cref="TextAnnotationCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// </summary>
		/// <param name="name">name of the text annotation</param>
		public TextAnnotation(string name) : this(name, null, null) { }

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New TextAnnotation instances should be created with <see cref="TextAnnotationCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// If using this method to create an instance of TextAnnotation, make sure it is added to the Gauge's <see cref="TextAnnotationCollection"/>
		/// </summary>
		/// <param name="name">name of the text annotation</param>
		/// <param name="text">The text that is displayed in the annotation</param>
		/// <param name="textStyle">The name of the <see cref="TextStyle"/> applied to the annotation</param>
		public TextAnnotation(string name, string text, TextStyle textStyle) : base(name)
		{
			this.text = text;
			this.textStyle = textStyle;
		}

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New TextAnnotation instances should be created with <see cref="TextAnnotationCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// If using this method to create an instance of TextAnnotation, make sure it is added to the Gauge's <see cref="TextAnnotationCollection"/>
		/// </summary>
		/// <param name="name">name of the text annotation</param>
		/// <param name="text">The text that is displayed in the annotation</param>
		/// <param name="textStyle">The name of the <see cref="TextStyle"/> applied to the annotation</param>
		/// <param name="location">Relative point on the gauge where the centre of the annotation is placed</param>
		/// <param name="angleDegrees">Tilt angle of the annotation, 0-360</param>
        public TextAnnotation(string name, string text, TextStyle textStyle, Point2D location, double angleDegrees) : base(name)
		{
			this.text = text;
            this.textStyle = textStyle;
			this.location = location;
			this.angleDegrees = angleDegrees;
        }

        #region --- Properties ---

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // don't serialize
		[Browsable(false)]
		internal SubGauge Gauge { get { return ObjectModelBrowser.GetOwningGauge(this); } }

		/// <summary>
		/// The text that is displayed by the annotation
		/// </summary>
		[DefaultValue("")]
        public string Text { get { return text; } set { text = value; ObjectModelBrowser.NotifyChanged(this); } }

		/// <summary>
		/// The name of the <see cref="TextStyle"/> applied to the annotation
		/// </summary>
		[DefaultValue("DefaultIndicatorLabelStyle")]
		public string TextStyleName { get { return textStyleName; } set { textStyleName = value; } }

		/// <summary>
		/// One of predefined <see cref="TextStyle"/>s that will be applied to the annotation
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextStyleKind TextStyleKind { get { return TextStyle.NameToKind(textStyleName); } set { textStyleName = TextStyle.KindToName(value); } }

		/// <summary>
		/// Instance of <see cref="TextStyle"/> that is assigned to this annotation
		/// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [NotifyParentProperty(true)]
		public TextStyle TextStyle 
		{
			get 
			{
				if(textStyle != null)
				{
					return textStyle;
				}
				return ObjectModelBrowser.GetOwningTopmostGauge(this).TextStyles[textStyleName]; 
			}
		}

		/// <summary>
		/// Relative location on the gauge between (0,0) and (100,100) where the center-point of the annotation will be placed
		/// </summary>
		[Editor(typeof(SizePositionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		[DefaultValue(null)]
        public Point2D RelativeLocation { get { return relativeLocation; } set { relativeLocation = value; ObjectModelBrowser.NotifyChanged(this); } }

		internal Point2D Location { get { return location; } set { location = value; } }

		/// <summary>
		/// The tilt angle of the annotation, between 0-360
		/// </summary>
		[DefaultValue(0)]
		public double AngleDegrees { get { return angleDegrees; } set { angleDegrees = value; } }
		internal object Tag { get { return tag; } set { tag = value; } }

        #endregion

        #region --- Rendering ---

        internal void Render(RenderingContext context,float linearSize)
        {
			Point2D loc = Location;
			if(loc == null)
				Location = new Point2D(
					context.TargetArea.X + RelativeLocation.X*context.TargetArea.Width * 0.01f,
					context.TargetArea.Y + RelativeLocation.Y*context.TargetArea.Height * 0.01f);
            TextRenderingContext tContext = context.Engine.Factory.CreateTextRenderingContext(TextStyle, context,linearSize);
            Render(context, tContext);
            tContext.Dispose();
			Location = loc;
        }
		
		private bool visible = true;
		/// <summary>
		/// Whether this annotation is visible or not
		/// </summary>
		[Category("General")]
		[Description("Text annotation visible")]
		[NotifyParentProperty(true)]
		[DefaultValue(true)]
		public bool Visible { get { return visible; } set { visible = value; ObjectModelBrowser.NotifyChanged(this); } }

        internal void Render(RenderingContext context, TextRenderingContext textRenderingContext) 
		{
			if(Visible)
				context.Engine.DrawText(this,context,textRenderingContext);
		}
  
        #endregion
    }


    /// <summary>
    /// Contains a collection of <see cref="TextAnnotation"/> objects.
    /// </summary>
	[Serializable]
	public class TextAnnotationCollection : NamedObjectCollection
	{
		internal override NamedObject CreateNewMember()
		{
			TextAnnotation newMember = new TextAnnotation();
			SelectGenericNewName(newMember);
			Add(newMember);
			return newMember;
		}

		#region --- Member Creation Interface ---

		/// <summary>
		/// Creates new member of the collection by cloning the member called "Main". If member named "Main" doesn't exist, a new
		/// instance of MarkerStyle is created.
		/// </summary>
		/// <param name="newMemberName">Name of the new member.</param>
		/// <returns>Returns the created object.</returns>
		public TextAnnotation AddNewMember(string newMemberName)
		{
			TextAnnotation newMember = new TextAnnotation(newMemberName);
			Add(newMember);
			
			return newMember;			
		}

		/// <summary>
		/// Clones and stores the specified <see cref="TextAnnotation"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new TextAnnotation AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName,oldMemberName) as TextAnnotation;
		}

		#endregion

		/// <summary>
		/// Retrieves the <see cref="TextAnnotation"/> by name.
		/// </summary>
		/// <param name="ix">name of the TextAnnotation instance</param>
		/// <returns>requested <see cref="TextAnnotation"/> object</returns>
		public new TextAnnotation this[object ix]
		{
			get { return base[ix] as TextAnnotation; }
			set { base[ix] = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // don't serialize
		[Browsable(false)]
		internal SubGauge Gauge { get { return ObjectModelBrowser.GetOwningGauge(this); } }

//
//		public new TextAnnotation this[string name] 
//		{ 
//			get { return base[name] as TextAnnotation; }
//			set { base[name] = value; }
//		}	
	}

	#region --- Converters ---

	internal class TextAnnotationConverter :  ExpandableObjectConverter
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			return destinationType == typeof(InstanceDescriptor) 
				|| base.CanConvertTo(context, destinationType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if(value is TextAnnotation)
			{
				if (destinationType == typeof(InstanceDescriptor))
				{
					System.Reflection.ConstructorInfo ci = typeof(TextAnnotation).GetConstructor
						(new Type[] {});
					return new InstanceDescriptor(ci, new Object[] {}, false);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
	#endregion
}
