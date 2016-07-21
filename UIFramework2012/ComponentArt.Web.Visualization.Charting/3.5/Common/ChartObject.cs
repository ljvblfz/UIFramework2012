using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using ComponentArt.Web.Visualization.Charting.Design;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Classes that derive from this abstract class represent objects in the chart.
	/// </summary>
	[Editor(typeof(UITypeEditor), typeof(UITypeEditor))]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[Serializable()]
	public abstract class ChartObject 
	{
		private ChartObject	owner;
		private ArrayList		twoDObjects;
		private   bool			visible;
		private ChartObjects	subObjects = null;

		internal ChartObject()
		{
			twoDObjects = new ArrayList();
			visible = true;
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ChartObject"/> is displayed.
		/// </summary>
		[SRCategory("CatBehavior")]
		[SRDescription("ChartObjectVisibleDescr")]
		[DefaultValue(true)]
		public virtual bool Visible
		{
			get 
			{ 
				return visible;
			}
			set 
			{ 
				visible = value; 
			}
		}

		internal void AddSubobject(ChartObject obj)
		{
			if(subObjects == null)
				subObjects = new ChartObjects();
			subObjects.Add(obj);
		}

		internal ChartObjects SubObjects { get { return subObjects; } }

		internal Geometry.GeometricEngine GE { get { return OwningChart.GeometricEngine; } }

		#region --- Debuging Helper Functions ---

		internal int SubobjectCount { get { return (subObjects==null)? 0:subObjects.Count; } }

		internal string Structure(int depth)
		{
			string str = depth.ToString("000 ");
			for(int i=0;i<depth;i++) str += "  ";
			str += SubobjectCount.ToString("##0 ");
			str += this.GetType().Name + "\n";
			return str;
		}
		#endregion

		#region --- Navigation ---

		internal ChartObject Owner
		{	
			get { return owner; }
		}
		
		internal ArrayList TwoDObjects 
		{
			get {return twoDObjects;}
		}

		internal virtual ChartSpace Space
		{
			get 
			{
				if(owner == null)
					return null;
				else
					return owner.Space; 
			}
		}

		internal virtual void SetContext(Object obj)
		{
			if(obj is ChartObject)
				SetOwner(obj as ChartObject);
		}

		internal virtual void SetOwner(ChartObject owner)
		{
			this.owner = owner;
		}

		internal virtual ChartBase OwningChart
		{
			get 
			{
				if(Space == null)
					return null;
				else
					return Space.OwningChart; 
			}
		}
		#endregion

		#region --- 2D Graphics Objects ---

		internal void Add(Chart2DObject obj2d)
		{
			twoDObjects.Add(obj2d);
			obj2d.SetOwner(this);
		}

		internal void Render2DObjects(Bitmap bmp)
		{
			if(twoDObjects.Count == 0)
				return;
			Graphics g = Graphics.FromImage(bmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = TextRenderingHint.AntiAlias;
			foreach(Object obj in twoDObjects)
				((Chart2DObject)obj).Render(g);
			g.Dispose();
		}
		#endregion

		#region --- Building and Rendering ---
		internal virtual void Build()
		{
		}

		internal virtual void Render()
		{
		}
		#endregion
	}
}
