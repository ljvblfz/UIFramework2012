using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Design;
#if WEB
using System.Web;
#endif

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Allows users to place custom images within a gauge.
	/// </summary>
	[Serializable]
	[TypeConverter(typeof(NamedObjectConverter))]
	public class ImageAnnotation : NamedObject
	{
		private Point2D location;
		private Point2D relativeLocation = new Point2D(50, 25);
		private Size2D size = new Size2D(0, 0);
		private object image = null;
		private string imageFile = "";
		private bool visible = true;

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New ImageAnnotation instances should be created with <see cref="ImageAnnotationCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// </summary>
		public ImageAnnotation() { }

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New ImageAnnotation instances should be created with <see cref="ImageAnnotationCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// </summary>
		/// <param name="name">name of the annotation</param>
		public ImageAnnotation(string name) : base(name) { }

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New ImageAnnotation instances should be created with <see cref="ImageAnnotationCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// If using this method to create an instance of TextAnnotation, make sure it is added to the Gauge's <see cref="ImageAnnotationCollection"/>
		/// </summary>
		/// <param name="name">name of this annotation</param>
		/// <param name="imageFile">path to the image file</param>
		/// <param name="relativeLocation">image location on the gauge</param>
		public ImageAnnotation(string name, string imageFile, Point2D relativeLocation)
			: base(name)
		{
			this.imageFile = imageFile;
			this.relativeLocation = relativeLocation;
		}

		/// <summary>
		/// This method should not be used, and is public for compatibility with the framework and design environment.
		/// New ImageAnnotation instances should be created with <see cref="ImageAnnotationCollection"/>'s AddNewMember and AddNewMemberFrom methods.
		/// If using this method to create an instance of TextAnnotation, make sure it is added to the Gauge's <see cref="ImageAnnotationCollection"/>
		/// </summary>
		/// <param name="name">name of this annotation</param>
		/// <param name="image">image object</param>
		/// <param name="relativeLocation">image location on the gauge</param>
		public ImageAnnotation(string name, object image, Point2D relativeLocation)
			: base(name)
		{
			this.image = image;
			this.relativeLocation = relativeLocation;
		}

		#region --- Properties ---

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // don't serialize
		[Browsable(false)]
		internal SubGauge Gauge { get { return ObjectModelBrowser.GetOwningGauge(this); } }

		internal Point2D Location { get { return location; } set { location = value; } }

		/// <summary>
		/// Relative location on the gauge between (0,0) and (100,100) where the center-point of the image will be placed
		/// </summary>
		[DefaultValue(null)]
		[Editor(typeof(SizePositionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Point2D RelativeLocation { get { return relativeLocation; } set { relativeLocation = value; ObjectModelBrowser.NotifyChanged(this);  } }

		/// <summary>
		/// Size of the image annotation, relative to the entire gauge size
		/// </summary>
		[DefaultValue(null)]
		[Editor(typeof(SizePositionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Size2D Size { get { return size; } set { size = value; ObjectModelBrowser.NotifyChanged(this); } }

		/// <summary>
		/// Whether the annotation is visible or not
		/// </summary>
		[Category("General")]
		[Description("Annotation visible")]
		[NotifyParentProperty(true)]
		[DefaultValue(true)]
		public bool Visible { get { return visible; } set { visible = value; ObjectModelBrowser.NotifyChanged(this); } }

#if WEB
		/// <summary>
		/// Relative path within the Web Application to the image used for the annotation
		/// </summary>
		[DefaultValue("")]
		//[TypeConverter(typeof(System.Drawing.ImageConverter))]
		[Editor(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public string ImageURL 
		{ 
			get { return imageFile; } 
			set 
			{ 
				if (value.StartsWith("http://")) 
					throw new InvalidOperationException("Image annotations must contain relative paths to the image.");
				else
					imageFile = value; 
			} 
		}
#else
		[DefaultValue(null)]
		[TypeConverter(typeof(System.Drawing.ImageConverter))]
		[Editor(typeof(GaugeImageEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public object Image { get { return image; } set { image = value; } }
#endif
		#endregion

		internal void Render(RenderingContext context)
		{
			if (!Visible)
				return;
			
            Point2D loc = Location;
			if (loc == null)
				loc = new Point2D(
					context.TargetArea.X + RelativeLocation.X * context.TargetArea.Width * 0.01f,
					context.TargetArea.Y + RelativeLocation.Y * context.TargetArea.Height * 0.01f);

			Object img = image;
			if (img == null)
			{
//#if WEB		
                SubGauge topmostGauge = ObjectModelBrowser.GetOwningTopmostGauge(this);
                if (!topmostGauge.InDesignMode)
				{
					try
					{
#if WEB					
                        Gauge thisGauge = topmostGauge.gaugeWrapper as Gauge;
                        string directory = Utils.ConvertUrl(HttpContext.Current, thisGauge.TemplateSourceDirectory, imageFile);
                        string absolutePath = Utils.MapPhysicalPath(directory);
                        img = new Bitmap(absolutePath);
#else
                        img = new Bitmap(imageFile);
#endif
                    }
					catch (Exception)
					{
						img = null;
					}
				}
				else
				{
					PointF[] points = new PointF[]
					{
						new PointF(loc.X,loc.Y),
						new PointF(loc.X,loc.Y + Size.Height*context.TargetArea.Height*0.01f),
						new PointF(loc.X + Size.Width*context.TargetArea.Width*0.01f,loc.Y + Size.Height*context.TargetArea.Height*0.01f),
						new PointF(loc.X + Size.Width*context.TargetArea.Width*0.01f,loc.Y)
					};
					context.Engine.FillArea(points, Color.FromArgb(100, 228, 128, 128), context);
				}
//#else
//				img = context.Engine.GetImage(imageFile);
//#endif
			}

			// we let null image to be rendered as a placeholder
			context.Engine.DrawImage(img, size, loc, context);
		}

	}


    /// <summary>
    /// Contains a collection of <see cref="ImageAnnotation"/> objects.
    /// </summary>
	[Serializable]
	public class ImageAnnotationCollection : NamedObjectCollection
	{
		internal override NamedObject CreateNewMember()
		{
			ImageAnnotation newMember = new ImageAnnotation();
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
		public ImageAnnotation AddNewMember(string newMemberName)
		{
			ImageAnnotation newMember = new ImageAnnotation(newMemberName);
			Add(newMember);

			return newMember;
		}

		/// <summary>
		/// Clones and stores the specified <see cref="ImageAnnotation"/>.
		/// </summary>
		/// <param name="newMemberName">Name of the cloned collection member.</param>
		/// <param name="oldMemberName">Name of the original collection member.</param>
		/// <returns>Returns the cloned member.</returns>
		/// <remarks>If the original object does not exist, the function returns null. 
		/// If the collection already contents the member with the cloned member name, the old member will be overriden.
		/// </remarks>
		public new ImageAnnotation AddNewMemberFrom(string newMemberName, string oldMemberName)
		{
			return base.AddNewMemberFrom(newMemberName, oldMemberName) as ImageAnnotation;
		}

		#endregion

		/// <summary>
		/// Retrieves the <see cref="ImageAnnotation"/> by name.
		/// </summary>
		/// <param name="ix">name of the ImageAnnotation instance</param>
		/// <returns>requested <see cref="ImageAnnotation"/> object</returns>
		public new ImageAnnotation this[object ix]
		{
			get { return base[ix] as ImageAnnotation; }
			set { base[ix] = value; }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] // don't serialize
		[Browsable(false)]
		internal SubGauge Gauge { get { return ObjectModelBrowser.GetOwningGauge(this); } }
	}

	#region --- Converters ---

	internal class TypeConverterWithDefaultConstructor : ExpandableConverterWithPropertyRelevance
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor)
				|| base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				System.Reflection.ConstructorInfo ci = value.GetType().GetConstructor(new Type[] { });
				if (ci == null)
					throw new Exception("Type '" + value.GetType().Name + "' doesn't have default constructor.");
				else
					return new InstanceDescriptor(ci, new Object[] { }, false);
			}
			else
				return base.ConvertTo(context, culture, value, destinationType);
		}
	}
	/*
		public class ImageAnnotationConverter :  ExpandableObjectConverter
		{
			public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
			{
				return destinationType == typeof(InstanceDescriptor) 
					|| base.CanConvertTo(context, destinationType);
			}
		
			public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
			{
				if(value is ImageAnnotation)
				{
					if (destinationType == typeof(InstanceDescriptor))
					{
						System.Reflection.ConstructorInfo ci = typeof(ImageAnnotation).GetConstructor
							(new Type[] {});
						return new InstanceDescriptor(ci, new Object[] {}, false);
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
		*/
	#endregion

	#region --- Editors ---

	internal class GaugeImageEditor : UITypeEditor
	{
		private SubGauge gauge;
		private UITypeEditor editor;

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider == null || context == null)
				return value;

			gauge = ObjectModelBrowser.GetOwningTopmostGauge(context.Instance as IObjectModelNode);
			bool isEditing = gauge.Editing;
			gauge.Editing = true;
			if (editor == null)
				editor = gauge.Factory.GetImageEditor();
			object result = editor.EditValue(context, provider, value);
			gauge.Editing = isEditing;
			return result;

		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			if (gauge == null)
				return;

			if (editor == null)
				editor = gauge.Factory.GetImageEditor();
			editor.PaintValue(e);
		}

		/* 
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
		*/
	}


	#endregion
}
