#define IMPLEMENT_STATIC
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Xml;

using ComponentArt.Web.Visualization.Gauges;
using ComponentArt.Web.Visualization.Gauges.GDIEngine;


namespace ComponentArt.Web.Visualization.Gauges.GDIEngine
{
	/// <summary>
	/// Summary description for GDIFactory.
	/// </summary>
	[Serializable] 
	internal class GDIFactory : Factory
	{
		private static XmlDocument doc = null;
		private static ArrayList layers = null;
        private static object staticLock = new Object();

		private GDIEngine engine;
		static internal ArrayList gdiVisualParts = new ArrayList();

		// Test data
		private int nRegistered = 0;
		private int nDisposed = 0;

		internal GDIFactory() 
		{
			lock(staticLock)
			{
				if(doc == null)
				{
					doc = new XmlDocument();
					layers = new ArrayList();
					string name = "ComponentArt.Web.Visualization.Gauges.GDIEngine.Resources.Bitmaps.XML";
					Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
					if (stream == null)
						return;

					doc.Load(stream);
					InitializeLayers();
					stream.Close();
				}
			}
		}

		internal void RegisterVisualPart(GDIBitmapVisualPart part)
		{
			if(!gdiVisualParts.Contains(part))
			{
				nRegistered++;
				gdiVisualParts.Add(part);
				Debug.WriteLine(nRegistered.ToString() + " " + part.Name);
			}
		}

		public override void Dispose()
		{
		}

		internal void DisposeObjects()
		{			
			// dispose only when the last factory instance is disposed

			if(engine != null)
				engine.Dispose();
			engine = null;
			foreach(GDIBitmapVisualPart part in gdiVisualParts)
			{
				nDisposed++;				
				part.Dispose();
			}
			Debug.WriteLine("Registered-Disposed = " + nRegistered.ToString() + "-" + nDisposed.ToString());
			gdiVisualParts.Clear();
			doc = null;			
		}

		#region --- Services ---
		
		internal override System.Drawing.Design.UITypeEditor GetImageEditor()
		{
#if WEB
			return new System.Web.UI.Design.UrlEditor();
#else
			return new System.Drawing.Design.ImageEditor();
#endif
		}

		#endregion

		#region --- Creating Layers ---

		internal void InitializeLayers()
		{
			PopulateLayersFromBitmapResources(layers);
			GDILayerDescriptorCollection ldc = new GDILayerDescriptorCollection();
			ldc.ImportXML("Layers.xml");
			for (int i = 0; i < ldc.Count; i++)
			{
				Layer layer = CreateLayer(ldc[i]);
				if (layer == null)
					continue;
				layers.Add(layer);
			}
		}

		internal override ArrayList CreateLayers()
		{
			if(layers == null)
				InitializeLayers();
			return layers;
		}

		internal Layer CreateLayer(GDILayerDescriptor lDes)
		{
			switch(lDes.Source)
			{
				case LayerSourceKind.File: return CreateLayerFromFile(lDes);
				case LayerSourceKind.Procedural: return CreateProceduralLayer(lDes);
				case LayerSourceKind.CallingAssemblyResource: 
					return CreateLayerFromAssemblyResource(lDes,Assembly.GetCallingAssembly());
				case LayerSourceKind.GaugesAssemblyResource: 
					return CreateLayerFromAssemblyResource(lDes,Assembly.GetExecutingAssembly());
				default:
					throw new Exception("Implementation error: 'CreateLayer()' not implemented for source = '" + lDes.Source.ToString() +"'.");
			}
		}


		internal override LayerVisualPart CreateLayerVisualPart(string name,object image)
		{
			if(image == null) // provide placeholder image
			{
				Bitmap bmp = new Bitmap(30,30);
				Graphics g = Graphics.FromImage(bmp);
				g.Clear(Color.Red);
				g.DrawLine(Pens.Black,0,0,30,30);
				g.DrawLine(Pens.Black,0,30,30,0);
				g.Dispose();
				image = bmp;
			}

			if(!(image is Bitmap))
				throw new Exception("Cannot create visual part from object type '" + image.GetType() + "'.");

			GDIBitmapVisualPart vp = new GDIBitmapVisualPart(name);
			vp.Bitmap = (image as Bitmap);
			RegisterVisualPart(vp);
			return vp;
		}

		private string NodePath(XmlNode node)
		{
			if(node == null)
				return "";
			string pPath = NodePath(node.ParentNode);

			string name = node.Name;
			if(name == "Layer")
			{
				string del = "(";
				for(int i=0; i<node.Attributes.Count; i++)
				{
					name = name + del + node.Attributes[i].Value;
					del = ",";
				}
				name = name +")";
			}
			if(pPath != "")
				return pPath + "." + name;
			else
				return name;
		}
		private void PopulateLayersFromBitmapResources(ArrayList layers)
        {
            // Creating XML Document
//
//            string name = "ComponentArt.Web.Visualization.Gauges.GDIEngine.Resources.Bitmaps.XML";
//            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
//            if (stream == null)
//                return;
//
//            XmlDocument doc = new XmlDocument();
//            doc.Load(stream);
//            stream.Close();

            // Building Layers from Document

            XmlElement root = doc.FirstChild as XmlElement;
            XmlElement layerNode = root.FirstChild as XmlElement;
            // Loop over layers
            while (layerNode != null)
            {
                string layerName = layerNode.Attributes["Name"].Value;
                GaugeKind gaugeKind = (GaugeKind)Enum.Parse(typeof(GaugeKind), layerNode.Attributes["GaugeKind"].Value);
                LayerRoleKind role = (LayerRoleKind)Enum.Parse(typeof(LayerRoleKind), layerNode.Attributes["Role"].Value);
				Size2D cornerSize = null;
				string sizeStr = layerNode.GetAttribute("CornerSize");
				if(sizeStr != null && sizeStr != "")
					cornerSize = new Size2D(sizeStr);

                LayerVisualPartCollection parts = new LayerVisualPartCollection();
                // Loop over bitmaps int the main image
                XmlElement imageNode = layerNode["Image"];
                XmlElement bitmapNode;
                if (imageNode != null)
                {
                    bitmapNode = imageNode.FirstChild as XmlElement;
                    while (bitmapNode != null)
                    {
                        GDIBitmapVisualPart vp = new GDIBitmapVisualPart(NodePath(bitmapNode));
                        //vp.Bitmap = XMLUtils.StringToBmp(bitmapNode.InnerText);
						vp.BitmapNode = bitmapNode;
                        parts.Add(vp);
						RegisterVisualPart(vp);
                        bitmapNode = bitmapNode.NextSibling as XmlElement;
                    }

                }
                // Region
                XmlElement regionNode = layerNode["Region"];
                GDIBitmapVisualPart region = null;
                if (regionNode != null)
                {
                    region = new GDIBitmapVisualPart(NodePath(regionNode));
                    region.BitmapNode = regionNode.FirstChild as XmlElement;//XMLUtils.StringToBmp(regionNode.FirstChild.InnerText);
					RegisterVisualPart(region);
                }
                XmlElement shadowNode = layerNode["Shadow"];
                GDIBitmapVisualPart shadow = null;
                if (shadowNode != null)
                {
                    shadow = new GDIBitmapVisualPart(NodePath(shadowNode));
                    shadow.BitmapNode = shadowNode.FirstChild as XmlElement;//XMLUtils.StringToBmp(shadowNode.FirstChild.InnerText);
					RegisterVisualPart(shadow);
                }

                if (parts.Count > 0 || shadow != null || region != null)
                {
                    Layer layer = new Layer(layerName, gaugeKind, role, parts, shadow, region);
					if(cornerSize != null)
						layer.CornerSize = cornerSize;
                    layers.Add(layer);
                }
                layerNode = layerNode.NextSibling as XmlElement;
            }

        }

		private Layer CreateLayerFromFile(GDILayerDescriptor lDes)
		{
			string folder = lDes.SourceObject as string;
			DirectoryInfo dirInfo = new DirectoryInfo(folder);
			if(!dirInfo.Exists)
				return null;
			
			folder = folder + @"\";
			LayerVisualPartCollection parts = new LayerVisualPartCollection();
			GDIBitmapVisualPart shadowPart = null;
			GDIBitmapVisualPart regionPart = null;

			string name = lDes.Name;
			string image = lDes.Image;
			string shadow = lDes.Shadow;
			string region = lDes.Region;

			try
			{
				GDIBitmapVisualPart part = new GDIBitmapVisualPart(name);
				RegisterVisualPart(part);
				part.GetFromFile(folder + image + ".png");
				parts.Add(part);
				for(int i=1;;i++)
				{
					try
					{
						part = new GDIBitmapVisualPart(name+"-background"+i);
						RegisterVisualPart(part);
						part.GetFromFile(folder + image + i + ".png");
						parts.Add(part);
					}
					catch
					{
						break;
					}
				}
			}
			catch { }
			if(parts.Count == 0)
				return null;
			
			if(shadow != null && shadow != "")
			{
				try
				{
					GDIBitmapVisualPart part = new GDIBitmapVisualPart(name + "-shadow");
					RegisterVisualPart(part);
					part.GetFromFile(folder + shadow + ".png");
					shadowPart = part;
				}
				catch { }
			}

			if(region != null && region != "")
			{
				try
				{
					GDIBitmapVisualPart part = new GDIBitmapVisualPart(name + "-region");
					RegisterVisualPart(part);
					part.GetFromFile(folder + region + ".png");
					regionPart = part;
				}
				catch { }
			}

			if(lDes.Role == LayerRoleKind.Marker)
				lDes.GaugeKind = GaugeKind.Circular;
			else if(lDes.Role == LayerRoleKind.DigitMask)
				lDes.GaugeKind = GaugeKind.Numeric;
			Layer layer = new Layer(name,lDes.GaugeKind,lDes.Role,parts,shadowPart,regionPart);
			layer.CornerSize = lDes.CornerSize;
			return layer;
		}

		private Layer CreateProceduralLayer(GDILayerDescriptor lDes)
		{
			Layer layer = null;

			if(lDes.Name == "Procedural" && lDes.Role == LayerRoleKind.Background && lDes.GaugeKind == GaugeKind.Circular)
			{
				LayerVisualPartCollection parts = new LayerVisualPartCollection();
				parts.Add(new GDIProceduralBackgroundVisualPart("Default",GaugeKind.Circular));
				LayerVisualPart shadow = null;
				LayerVisualPart region = null;
				layer = new Layer(lDes.Name,GaugeKind.Circular,LayerRoleKind.Background,parts,shadow,region);
			}

			else if(lDes.Name == "Procedural" && lDes.Role == LayerRoleKind.Frame && lDes.GaugeKind == GaugeKind.Circular)
			{
				LayerVisualPartCollection parts = new LayerVisualPartCollection();
				parts.Add(new GDIProceduralFrameVisualPart("Default",GaugeKind.Circular,0.10,0.3,0.8));
				LayerVisualPart shadow = null;
				LayerVisualPart region = null;
				layer = new Layer(lDes.Name,GaugeKind.Circular,LayerRoleKind.Frame,parts,shadow,region);
			}

			else if(lDes.Name == "ProceduralWide" && lDes.Role == LayerRoleKind.Frame && lDes.GaugeKind == GaugeKind.Circular)
			{
				LayerVisualPartCollection parts = new LayerVisualPartCollection();
				parts.Add(new GDIProceduralFrameVisualPart("Default",GaugeKind.Circular,0.20,0.3,0.8));
				LayerVisualPart shadow = null;
				LayerVisualPart region = null;
				layer = new Layer(lDes.Name,GaugeKind.Circular,LayerRoleKind.Frame,parts,shadow,region);
			}

			else if(lDes.Name == "Procedural" && lDes.Role == LayerRoleKind.Pointer && lDes.GaugeKind == GaugeKind.Circular)
			{
				string name = "Default";
				LayerVisualPart part = new GDIProceduralNeedleVisualPart(name);
				LayerVisualPart shadow = new GDIProceduralNeedleVisualPart(name);
				LayerVisualPart region = new GDIProceduralNeedleVisualPart(name);
                layer = new Layer(lDes.Name, GaugeKind.Circular,LayerRoleKind.Pointer, part, shadow, region);
			}
			return layer;
		}

		private Layer CreateLayerFromAssemblyResource(GDILayerDescriptor lDes,Assembly asm)
		{
			string sRole = lDes.Role.ToString().ToLower();
			// Get resource names
			string[] names = asm.GetManifestResourceNames();
			string namePrefix;
			if(asm == Assembly.GetExecutingAssembly())
				namePrefix = "ComponentArt.Web.Visualization.Gauges.GDIEngine.Resources.";
			else
				namePrefix = "Resources.";
			LayerVisualPartCollection parts = new LayerVisualPartCollection();
			GDIBitmapVisualPart regionPart = null;
			GDIBitmapVisualPart shadowPart = null;

			for(int i=0; i<names.Length;i++)
			{
				if(names[i] == namePrefix + lDes.Image + ".png")
				{
					GDIBitmapVisualPart part = new GDIBitmapVisualPart(lDes.Name);
					RegisterVisualPart(part);
					part.GetFromResourcePath(names[i]);
					parts.Add(part);
				}
				else if(names[i] == namePrefix + lDes.Region + ".png")
				{
					regionPart = new GDIBitmapVisualPart(lDes.Name);
					regionPart.GetFromResourcePath(names[i]);
					RegisterVisualPart(regionPart);
				}
				else if(names[i] == namePrefix + lDes.Shadow + ".png")
				{
					shadowPart = new GDIBitmapVisualPart(lDes.Name);
					shadowPart.GetFromResourcePath(names[i]);
					RegisterVisualPart(shadowPart);
				}
			}
			if(parts.Count == 0 && regionPart == null)
				return null;
			Layer layer = new Layer(lDes.Name,lDes.GaugeKind,lDes.Role,parts,shadowPart,regionPart);

			return layer;
		}

		#endregion

        #region --- Creating Engine ---

        internal override IEngine CreateEngine() 
		{
			if(engine == null)
			{
				engine = new GDIEngine(); 
				engine.SetFactory(this);
			}
			return engine;
		}
		#endregion

		#region --- Creating Contexts ---

		internal override RenderingContext CreateContext() { return new GDIRenderingContext(); }
		internal override TickMarkRenderingContext CreateTickMarkRenderingContext(MarkerStyle style) 
		{ return new  GDITickMarkRenderingContext(style); }
		internal override TextRenderingContext CreateTextRenderingContext(TextStyle textStyle, RenderingContext context, float linearGaugeSize) 
		{ return new  GDITextRenderingContext(textStyle,context,linearGaugeSize); }
		#endregion
		
	}

	// ====================================================================================	
	// This class has drawing resources shared by all tickmarks.
	// It is used to save repeated instantiation of the same objects for each tickmark

    internal class GDITickMarkRenderingContext : TickMarkRenderingContext, IDisposable
    {
        private Brush backgroundBrush;
        private Pen contourPen;
        private Brush shadowBrush;
        private Color backgroundColor = Color.Empty;
        private Size2D shadowOffset;

        public GDITickMarkRenderingContext(MarkerStyle style)
            : base(style)
        {
            backgroundColor = style.BaseColor;
            if (!backgroundColor.IsEmpty)
                backgroundBrush = new SolidBrush(backgroundColor);
            contourPen = new Pen(style.LineColor, style.LineWidth);
            if (style.HasShadow)
                shadowBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
            shadowOffset = style.ShadowOffset;
        }

        public Brush BackgroundBrush { get { return backgroundBrush; } }
        public Pen ContourPen { get { return contourPen; } }
        public Brush ShadowBrush { get { return shadowBrush; } }
        public MarkerStyle MarkerStyle { get { return style; } }
        public Size2D ShadowOffset { get { return shadowOffset; } }

        public override void Dispose()
        {
            if (backgroundBrush != null)
            {
                backgroundBrush.Dispose();
                backgroundBrush = null;
            }
            if (contourPen != null)
            {
                contourPen.Dispose();
                contourPen = null;
            }
            if (shadowBrush != null)
            {
                shadowBrush.Dispose();
                shadowBrush = null;
            }
        }

        public override Color BaseColor
        {
            set
            {
                if (backgroundColor != value || backgroundBrush == null)
                {
                    if (backgroundBrush != null)
                        backgroundBrush.Dispose();
                    backgroundColor = value;
                    backgroundBrush = new SolidBrush(backgroundColor);
                }
            }
        }
    }

    // ====================================================================================	
	// This class has drawing resources shared multiple texts.
	// It is used to save repeated instantiation of the same objects for each text

	internal class GDITextRenderingContext : TextRenderingContext
	{
		private Font font;
		private Brush fontBrush;
		private Brush alternateFontBrush;
		private Brush fontBackBrush;
		private Brush alternateFontBackBrush;
		private Brush shadowBrush;
		private Pen borderPen;
		private GDIRenderingContext context;
		private Color fontColor = Color.Empty;
		private Color alternateFontColor = Color.Empty;
		private Color fontBackColor = Color.Empty;
		private Color alternateFontBackColor = Color.Empty;
		private bool displayDecimalPoint = true;

		public GDITextRenderingContext() { }

		public GDITextRenderingContext(TextStyle style, RenderingContext context, float linearGaugeSize)
		{
			this.context = context as GDIRenderingContext;
			//Size2D sz = context.TargetArea.Size;
			float fontSize = (float)(linearGaugeSize * style.FontSizePerc * 0.01);
			font = new Font(style.FontName, fontSize,style.FontStyle, GraphicsUnit.Pixel);

			if(!style.FontColor.IsEmpty)
			{
				fontColor = style.FontColor;
				fontBrush = new SolidBrush(fontColor);
			}
			if(!style.DecimalFontColor.IsEmpty)
			{
				alternateFontColor = style.DecimalFontColor;
				alternateFontBrush = new SolidBrush(alternateFontColor);
			}
			if(!style.FontBackColor.IsEmpty)
			{
				fontBackColor = style.FontBackColor;
				fontBackBrush = new SolidBrush(fontBackColor);
			}
			if(!style.DecimalFontBackColor.IsEmpty)
			{
				alternateFontBackColor = style.DecimalFontBackColor;
				alternateFontBackBrush = new SolidBrush(alternateFontBackColor);
			}

			if(style.DrawShadow)
				shadowBrush = new SolidBrush(style.ShadowColor);
			else
				shadowBrush = null;
			borderPen = new Pen(style.OutlineColor,style.OutlineWidth);
		}
		public override void Dispose()
		{
			if (font != null)
			{
				font.Dispose();
				font = null;
			}
			if (fontBrush != null)
			{
				fontBrush.Dispose();
				fontBrush = null;
			}
			if (fontBackBrush != null)
			{
				fontBackBrush.Dispose();
				fontBackBrush = null;
			}
			if (alternateFontBrush != null)
			{
				alternateFontBrush.Dispose();
				alternateFontBrush = null;
			}
			if (alternateFontBackBrush != null)
			{
				alternateFontBackBrush.Dispose();
				alternateFontBackBrush = null;
			}
			if (shadowBrush != null)
			{
				shadowBrush.Dispose();
				shadowBrush = null;
			}
			if (borderPen != null)
			{
				borderPen.Dispose();
				borderPen = null;
			}
		}
		public override bool DisplayDecimalPoint { set { displayDecimalPoint = value; } get { return displayDecimalPoint; } }

		internal Font Font { get { return font; } }
		internal Brush FontBrush { get { return fontBrush; } }
		internal Brush AlternateFontBrush { get { return alternateFontBrush; } }
		internal Brush FontBackBrush { get { return fontBackBrush; } }
		internal Brush AlternateFontBackBrush { get { return alternateFontBackBrush; } }
		internal Brush ShadowBrush { get { return shadowBrush; } }
		internal Pen BorderPen { get { return borderPen; } }

		public override Size2D MeasureString(string text) 
		{
			if(text == null || text == "")
				return new Size2D(0,0);
			Graphics g = context.RenderingTarget as Graphics;
			SizeF sz = g.MeasureString(text,Font,new SizeF(5000,5000),StringFormat.GenericTypographic);
			return new Size2D(sz.Width,sz.Height);
		}

		public override Color FontColor 
		{ 
			set 
			{
				if(value != fontColor || fontBrush == null)
				{
					if(fontBrush != null)
						fontBrush.Dispose();
					fontColor = value;
					fontBrush = new SolidBrush(fontColor);
				}
			} 
			get { return fontColor; }
		}

		public override Color FontBackColor 
		{ 
			set 
			{
				if(value != fontBackColor || fontBackBrush == null)
				{
					if(fontBackBrush != null)
						fontBackBrush.Dispose();
					fontBackColor = value;
					fontBackBrush = new SolidBrush(fontBackColor);
				}
			} 
			get
			{
				return fontBackColor;
			}
		}

		public override Color AlternateFontColor 
		{ 
			set 
			{
				if(value != alternateFontColor || alternateFontBrush == null)
				{
					if(alternateFontBrush != null)
						alternateFontBrush.Dispose();
					alternateFontColor = value;
					alternateFontBrush = new SolidBrush(alternateFontColor);
				}
			} 
			get { return alternateFontColor; }
		}

		public override Color AlternateFontBackColor 
		{ 
			set 
			{
				if(value != alternateFontBackColor || alternateFontBackBrush == null)
				{
					if(alternateFontBackBrush != null)
						alternateFontBackBrush.Dispose();
					alternateFontBackColor = value;
					alternateFontBackBrush = new SolidBrush(alternateFontBackColor);
				}
			} 
			get { return alternateFontBackColor; }
		}

	}
}
