using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ComponentArt.Web.Visualization.Gauges.GDIEngine
{
	internal enum LayerSourceKind
	{
		File,
		GaugesAssemblyResource,
		CallingAssemblyResource,
		Procedural
	};

	/// <summary>
	/// Summary description for StyleDescriptor.
	/// </summary>
	internal class GDILayerDescriptor
	{
		private GaugeKind gaugeKind;
		private LayerRoleKind role;
		private LayerSourceKind source = LayerSourceKind.File;
        private string themeName = "";
        private string name = "";
        private string image = "";
		private string region = "";
		private string shadow = "";
		private Size2D cornerSize;

		private object sourceObj = null;

		public GDILayerDescriptor() { }
		public GDILayerDescriptor(string themeName, GaugeKind gaugeKind, LayerRoleKind role, LayerSourceKind source, string name, 
			string image, string region, string shadow, Size2D cornerSize)
		{
            this.themeName = themeName;
            this.gaugeKind = gaugeKind;
            this.role = role;
			this.source = source;
			this.name = name;
			this.image = image;
			this.region = region;
			this.shadow = shadow;
			this.cornerSize = cornerSize;
			if(cornerSize.Abs() != 0)
				role = role;
		}
		public GaugeKind GaugeKind { get { return gaugeKind; } set { gaugeKind = value; } }
		public Size2D CornerSize { get { return cornerSize; } set { cornerSize = value; } }
		
		public LayerRoleKind Role { get { return role; } set { role = value; } }

		[DefaultValue(typeof(LayerSourceKind),"File")]
		public LayerSourceKind Source { get { return source; } set { source = value; } }

        [DefaultValue("")]
        public string ThemeName { get { return themeName; } set { themeName = value; } }

        [DefaultValue("")]
        public string Name { get { return name; } set { name = value; } }

		[DefaultValue("")]
		public string Image { get { return image; } set { image = value; } }

		[DefaultValue("")]
		public string Region { get { return region; } set { region = value; } }

		[DefaultValue("")]
		public string Shadow { get { return shadow; } set { shadow = value; } }

		internal object SourceObject { get { return sourceObj; } set { sourceObj = value; } }
	}

	// ===============================================================================================================

	internal class GDILayerDescriptorCollection : CollectionBase
	{
		public GDILayerDescriptor this[int i]
		{
			get
			{
				return InnerList[i] as GDILayerDescriptor;
			}
			set
			{
				InnerList[i] = value;
			}
		}

		public int Add(GDILayerDescriptor ld)
		{
			return InnerList.Add(ld);
		}

        public int Add(string themeName, GaugeKind gaugeKind, LayerRoleKind role, LayerSourceKind source, string name, string image, 
			string region, string shadow, Size2D cornerSize)
		{
            return InnerList.Add(new GDILayerDescriptor(themeName,gaugeKind, role, source, name, image, region, shadow,cornerSize));
		}

		#region --- XML Serialization ---

		public void ExportXML(string fileName)
		{
			XmlDocument doc = CreateDom();

			FileStream outStream = new FileStream(fileName, FileMode.Create);
			XmlTextWriter wrt = new XmlTextWriter(outStream,System.Text.Encoding.UTF8);
			wrt.Formatting = Formatting.Indented;
			doc.WriteTo(wrt);
			wrt.Close();
			outStream.Close();
		}
		#region --- Importing from files ---
		public void ImportXML(string fileName)
		{
			if(!File.Exists(fileName))
				return;
			FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			int ix = fileName.LastIndexOf(@"\");
			string folderName = (ix>0? fileName.Substring(0,ix):@".\");
			ImportXML(fs,folderName);
			fs.Close();
		}

		private void ImportXML(Stream inStream, string baseFolder)
		{
			GaugeKind kind;

			string imagesFolder=null;

			TypeConverter kindConverter = TypeDescriptor.GetConverter(typeof(GaugeKind));
			TypeConverter roleConverter = TypeDescriptor.GetConverter(typeof(LayerRoleKind));
			TypeConverter sourceConverter = TypeDescriptor.GetConverter(typeof(LayerSourceKind));

			XmlDocument doc = new XmlDataDocument();
			doc.Load(inStream);

			XmlElement themeNode = doc.FirstChild.FirstChild as XmlElement;
			while(themeNode != null)
			{
				string iFolder = themeNode.GetAttribute("ImagesFolder");
				if(iFolder != "")
					imagesFolder = iFolder;

				if(themeNode.Name.ToLower() == "layer")
				{
					AddLayerDescriptorFromNode(themeNode,"",GaugeKind.Circular,baseFolder+imagesFolder);
				}
				else
				{
					if(themeNode.Name.ToLower() != "theme")
						throw new Exception("'" + themeNode.Name + "' is not allowed as the first level node.");
					string themeName = themeNode.GetAttribute("Name");

					if(themeName == null)
						throw new Exception("'Theme' element doesn't have 'Name' attribute.");

					XmlElement skinNode = themeNode.FirstChild as XmlElement;
					while(skinNode != null)
					{
						if(skinNode.Name.ToLower() != "skin")
							throw new Exception("'" + skinNode.Name + "' is not allowed child of 'Theme' node.");
				
						string kindStr = skinNode.GetAttribute("GaugeKind");
						if(kindStr == null)
							throw new Exception("'Skin' element doesn't have 'GaugeKind' attribute.");
						kind = (GaugeKind)(kindConverter.ConvertFromString(kindStr));

						XmlElement layerNode = skinNode.FirstChild as XmlElement;
						while(layerNode != null)
						{
							AddLayerDescriptorFromNode(layerNode,themeName,kind,baseFolder+imagesFolder);
							layerNode = layerNode.NextSibling as XmlElement;
						}
						skinNode = skinNode.NextSibling as XmlElement;
					}
				}
				themeNode = themeNode.NextSibling as XmlElement;
			}
		}

		private void AddLayerDescriptorFromNode(XmlElement layerNode, string themeName, GaugeKind kind, string folder)
		{

			TypeConverter kindConverter = TypeDescriptor.GetConverter(typeof(GaugeKind));
			TypeConverter roleConverter = TypeDescriptor.GetConverter(typeof(LayerRoleKind));
			TypeConverter sourceConverter = TypeDescriptor.GetConverter(typeof(LayerSourceKind));

			if(layerNode.Name.ToLower() != "layer")
				throw new Exception("Cannot create layer descriptor from a(n) '" + layerNode.Name + "' node.");
					
			string layerName = layerNode.GetAttribute("Name");
			if(layerName == "")
				layerName = themeName;

			string roleStr = layerNode.GetAttribute("Role");
			if(roleStr == null)
				throw new Exception("'Layer' element doesn't have 'Role' attribute.");
			LayerRoleKind role = (LayerRoleKind)(roleConverter.ConvertFromString(roleStr));
			if(role == LayerRoleKind.DigitMask)
				kind = GaugeKind.Numeric;

			string s = layerNode.GetAttribute("Source");
			LayerSourceKind source = LayerSourceKind.File;
			if(s != "")
				source = (LayerSourceKind)(sourceConverter.ConvertFromString(s));
			string image = layerNode.GetAttribute("Image");
			string region = layerNode.GetAttribute("Region");
			string shadow = layerNode.GetAttribute("Shadow");
			if(image.ToLower().EndsWith(".png"))
				image = image.Substring(0,image.Length-4);
			if(region.ToLower().EndsWith(".png"))
				region = region.Substring(0,region.Length-4);
			if(shadow.ToLower().EndsWith(".png"))
				shadow = shadow.Substring(0,shadow.Length-4);
			string cornerStr = layerNode.GetAttribute("CornerSize");
			Size2D corner = new Size2D(0,0);
			if(cornerStr!="")
				corner = new Size2D(cornerStr);
			int ix = Add(themeName, kind, role, source, layerName, image, region, shadow, corner);
			if(source == LayerSourceKind.File)
				this[ix].SourceObject = folder;
			else
				this[ix].SourceObject = null;
		}

		#endregion

		#region --- Import from resources ---
		public void ImportResourceXML(string resourcePath)
		{
			Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
			ImportXML(stream,null);
		}

		private XmlDocument CreateDom()
		{
			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement("Layers");
			doc.AppendChild(root);
			for(int i=0; i<Count; i++)
			{
				XmlElement e = doc.CreateElement("Layer");
				e.SetAttribute("Kind",this[i].GaugeKind.ToString());
				e.SetAttribute("Name",this[i].Name);
				e.SetAttribute("Role",this[i].Role.ToString());
				if(this[i].Source != LayerSourceKind.File)
					e.SetAttribute("Source",this[i].Source.ToString());
				if(this[i].Image != "")
					e.SetAttribute("Image",this[i].Image);
				if(this[i].Region != "")
					e.SetAttribute("Region",this[i].Region);
				if(this[i].Shadow != "")
					e.SetAttribute("Shadow",this[i].Shadow);
				root.AppendChild(e);
			}
			return doc;
		}

		#endregion
	
		#endregion
	}

	// ===============================================================================================================


}
