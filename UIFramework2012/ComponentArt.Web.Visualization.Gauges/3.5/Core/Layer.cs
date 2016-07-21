using System;
using System.Collections;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Gauges
{
	internal enum LayerRoleKind
	{
		Background=0,
		Frame=1,
		Cover=2,
		Pointer=3,
		Hub=4,
		Marker=5,
		DigitMask=6
	}

	/// <summary>
	/// Summary description for ILayer.
	/// </summary>
	[Serializable]
	internal class Layer : NamedObject
	{
		private LayerVisualPartCollection parts;
		private LayerVisualPart shadowPart;
		private LayerVisualPart regionPart;
		private Color backgroundColor;
		private MapAreaCollection mapAreas;

		private GaugeKind kind = GaugeKind.Circular;
		private LayerRoleKind layerRoleKind;

		private Size2D cornerSize = new Size2D(0,0);

		public Layer(string name, GaugeKind kind, LayerRoleKind layerRoleKind, LayerVisualPart part, LayerVisualPart shadowPart, LayerVisualPart regionPart)
			: base(name)
		{
			this.kind = kind;
			this.layerRoleKind = layerRoleKind;
			if(layerRoleKind == LayerRoleKind.Marker)
				kind = GaugeKind.Circular; // all marker layers are circular, i.e. GaugeKind is ignored
			this.parts = new LayerVisualPartCollection();
			parts.Add(part);
			this.shadowPart = shadowPart;
			this.regionPart = regionPart;
			if(parts != null)
				(parts as IObjectModelNode).ParentNode = this;
			if(shadowPart != null)
				(shadowPart as IObjectModelNode).ParentNode = this;
			if(regionPart != null)
				(regionPart as IObjectModelNode).ParentNode = this;
		}

		public Layer(string name, GaugeKind kind, LayerRoleKind layerRoleKind, LayerVisualPartCollection parts, LayerVisualPart shadowPart, LayerVisualPart regionPart)
			: base(name)
		{
			this.kind = kind;
			this.layerRoleKind = layerRoleKind;
			this.parts = parts;
			this.shadowPart = shadowPart;
			this.regionPart = regionPart;
			if(parts != null)
				(parts as IObjectModelNode).ParentNode = this;
			if(shadowPart != null)
				(shadowPart as IObjectModelNode).ParentNode = this;
			if(regionPart != null)
				(regionPart as IObjectModelNode).ParentNode = this;
		}

		#region --- Properties ---

		internal LayerVisualPartCollection MainVisualParts { get { return parts; } }
		internal LayerVisualPart Shadow { get { return shadowPart; } }
		internal LayerVisualPart Region { get { return regionPart; } }
		internal Color BackgroundColor { get { return backgroundColor; } set { backgroundColor = value; } }
		internal GaugeKind Kind { get { return kind; } }
		internal LayerRoleKind LayerRoleKind { get { return layerRoleKind; } }
		internal MapAreaCollection MapAreas { get { if(mapAreas == null) mapAreas = regionPart.CreateMapAreas(); return mapAreas; } }

		internal Size2D CornerSize 
		{
			get { return cornerSize; } 
			set
			{
				if(parts != null)
					foreach(LayerVisualPart part in parts)
						part.CornerSize = value;
				if(shadowPart != null)
					shadowPart.CornerSize = value;
				if(regionPart != null)
					regionPart.CornerSize = value;
				cornerSize = value; 
			}
		}

		internal Size2D Size
        {
            get
            {
                if (MainVisualParts != null && MainVisualParts.Count > 0)
                    return MainVisualParts[0].Size;
                if (Region != null)
                    return Region.Size;
                return new Size2D(0, 0);
            }
        }

		#endregion

        #region --- Rendering ---

		internal void Render(RenderingContext context)
		{
			LayerVisualPart region = Region;
			if (region != null)
				region.RenderAsRegion(context, BackgroundColor);

			LayerVisualPartCollection parts = MainVisualParts;
			if (parts != null)
			{
				for (int i = 0; i < parts.Count; i++)
				{
					parts[i].CornerSize = CornerSize;
					parts[i].Render(context);
				}
			}
		}

		internal void RenderShadow(RenderingContext context, Size2D shadowTargetOffset)
		{
			LayerVisualPart shadow = Shadow;
			if(shadow == null)
				return;

			shadow.CornerSize = CornerSize;
			Rectangle2D newTargetArea = new Rectangle2D(
				context.TargetArea.X + shadowTargetOffset.Width,
				context.TargetArea.Y + shadowTargetOffset.Height,
				context.TargetArea.Width,
				context.TargetArea.Height);
			RenderingContext shadowContext = context.SetTargetArea(newTargetArea);
			shadow.Render(shadowContext);
		}

        #endregion
    }
	[Serializable]
	internal class LayerCollection : CollectionBase,IObjectModelNode
	{
		public string[] GetSkinNames(GaugeKind kind)
		{
			return null;
		}

		public int Add(Layer layer)
		{
			int ix = IndexOf(layer.Name,layer.Kind);
			if(ix>=0)
			{
				List.RemoveAt(ix);
				List.Insert(ix,layer);
				return ix;
			}
			else
			{
				return List.Add(layer);
			}
		}

		public Layer this[int ix]
		{
			get { return List[ix] as Layer; }
			set { List[ix] = value; }
		}

		public Layer this[string name,GaugeKind kind]
		{
			get 
			{
				int ix = IndexOf(name,kind);
				if(ix>=0)
					return this[ix];
				else
					return null;
			}
			set 
			{
				int ix = IndexOf(name,kind);
				if(ix>=0)
				{
					List.RemoveAt(ix);
					List.Insert(ix,value);
				}
				else
					List.Add(value);
			}
		}

		private int IndexOf(string name,GaugeKind kind)
		{
			for(int i=0; i<List.Count; i++)
			{
				Layer layer = this[i];
				if(layer.Name == name && layer.Kind == kind)
					return i;
			}
			return -1;

		}
  
		#region --- IObjectModelNode Implementation ---

		private IObjectModelNode parent;
		IObjectModelNode IObjectModelNode.ParentNode
		{
			get { return parent; }
			set { parent = value; }
		}

		#endregion
	}

}
