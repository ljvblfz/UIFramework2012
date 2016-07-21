using System;
using System.ComponentModel;
using System.Collections;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Drawing;
using System.Diagnostics;
#if !__BUILDING_CRI__
using System.Windows.Forms;
#endif

namespace ComponentArt.Web.Visualization.Charting
{
	internal class XmlCustomSerializer
	{
		private XmlNode node;
		private bool	reading;
		private bool	alwaysSerialize = false;

		#region --- Internal Methods ---

		internal XmlCustomSerializer(XmlNode node)
		{
			this.node = node;
			reading = true;
		}
		internal XmlCustomSerializer() :this(null) {}

		internal bool Reading { get { return reading; } set { reading = value; } }
		internal bool AlwaysSerialize { get { return alwaysSerialize; } set { alwaysSerialize = value; } }

		internal bool AttributeProperty(object obj,string propertyName)
		{
			return AttributeProperty(obj,propertyName, propertyName);
		}

		internal bool AttributeProperty(object obj, string attributeName, string propertyName)
		{
			return AttributeProperty(obj,attributeName,propertyName,null);
		}

		internal void SetAttribute(string attributeName, string attributeValue)
		{
			XmlElement e = node as XmlElement;
			e.SetAttribute(attributeName,attributeValue);
		}

		internal bool AttributeProperty(object obj, string attributeName, string propertyName, string format)
		{
			XmlElement e = node as XmlElement;
			if(e == null)
				return false;

			Type T = obj.GetType();
			PropertyInfo PI = T.GetProperty(propertyName,BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic);
			if(PI == null)
				return false;
			TypeConverter TK = TypeDescriptor.GetConverter(PI.PropertyType);

			if(reading)
			{
				XmlAttribute A = e.Attributes[attributeName];
				if(A == null)
					return false;
				string val = A.Value;

				if(PI.PropertyType == typeof(string))
					PI.SetValue(obj,val,null);
				else
				{
					if(TK.CanConvertFrom(typeof(string)))
						PI.SetValue(obj,TK.ConvertFromInvariantString(val),null);
					else
						return false;
				}
			}
			else
			{
				// Do we have to serialize this property?
				if(!alwaysSerialize)
				{
					bool shouldSerialize = true;
					MethodInfo mInfo = T.GetMethod("ShouldSerialize"+propertyName,BindingFlags.NonPublic | BindingFlags.Instance);

					if(mInfo != null)
					{
						object result = mInfo.Invoke(obj,null);
						shouldSerialize = result == null || !(result is bool) || (bool)result;
					}
					if(!shouldSerialize)
						return true;
				}
				object val = PI.GetValue(obj,null);
				if(val != null)
				{
					if(format != null && format != "" && val.GetType() == typeof(double))
						e.SetAttribute(attributeName,((double)val).ToString(format));
					else
					{
						e.SetAttribute(attributeName,TK.ConvertToInvariantString(val));
					}
				}
			}
			return true;
		}

		internal bool BeginTag(string elementName)
		{
			if(reading)
			{
				XmlNode e = node[elementName];
				if(e == null)
					return false;
				else
				{
					node = e;
					return true;
				}
			}
			else
			{
				XmlElement e = node.OwnerDocument.CreateElement(elementName);
				node.AppendChild(e);
				node = e;
				return true;
			}
		}

		internal bool EndTag()
		{
			if(node == null)
				return false;
			else
			{
				bool removeThisNode = node.Attributes.Count == 0 && node.FirstChild == null;
				XmlNode parentNode = node.ParentNode;
				if(removeThisNode)
					parentNode.RemoveChild(node);
				node = parentNode;
				return true;
			}
		}

		internal void Comment(string comment)
		{
			if(!reading)
				node.AppendChild(node.OwnerDocument.CreateComment(comment));
		}


		internal bool GoToFirstChild(string tagName)
		{
			XmlNode cnode = FindSibling(node.FirstChild,tagName);
			if(cnode == null)
				return false;
			else
			{
				node = cnode;
				return true;
			}
		}

		internal bool GoToNext(string tagName)
		{
			XmlNode cnode = FindSibling(node.NextSibling,tagName);
			if(cnode == null)
				return false;
			else
			{
				node = cnode;
				return true;
			}
		}

		internal bool GoToParent()
		{
			if(node != null)
				node = node.ParentNode;
			return (node != null);
		}

		internal void SetAttribute(string attributeName, object obj)
		{
			if(obj != null)
			{
				XmlElement e = node as XmlElement;
				if(e != null)
				{
					TypeConverter TK = TypeDescriptor.GetConverter(obj.GetType());
					e.SetAttribute(attributeName,TK.ConvertToInvariantString(obj));
				}
			}
		}

		internal object GetAttribute(string attributeName, Type T)
		{
			XmlAttribute A = node.Attributes[attributeName];
			if(A == null)
				return null;
			TypeConverter TK = TypeDescriptor.GetConverter(T);
			if(TK.CanConvertFrom(typeof(string)))
				return TK.ConvertFromInvariantString(A.Value);
			else if(T == typeof(Type))
				return Type.GetType(A.Value);
			else
				throw new XmlException("Cannot convert from string", null);
		}


		internal XmlNode FindSibling(XmlNode cnode, string tagName)
		{
			// Search for the element sibling with specified tag
			while(cnode != null && !(cnode.NodeType == XmlNodeType.Element && (cnode.Name == tagName || tagName == "*")))
				cnode = cnode.NextSibling;
			return cnode;
		}

		#endregion

		#region --- Public Output Methods ----

		public static void Write(string outputFileName, object obj, string rootTag)
		{
			FileStream fs = new FileStream(outputFileName, FileMode.Create);
			Write(fs,obj,rootTag);
			fs.Close();
		}

		public static void Write(Stream outStream, object obj, string rootTag)
		{
			// The root node
			if(rootTag == null || rootTag == "")
				rootTag = obj.GetType().Name;

			XmlDocument doc = new XmlDocument();
			XmlNode root = doc.CreateElement(rootTag);
			doc.AppendChild(root);

			WriteToXmlNode(root,obj);

			// Writing data

			XmlTextWriter wrt = new XmlTextWriter(outStream,System.Text.Encoding.UTF8);
			wrt.Formatting = Formatting.Indented;

			doc.WriteTo(wrt);
			wrt.Close();
		}

		public static void WriteToXmlNode(XmlNode node,object obj)
		{
			XmlCustomSerializer xmlSer = new XmlCustomSerializer(node);
			xmlSer.Reading = false;
			MethodInfo mi = obj.GetType().GetMethod("Serialize",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null,new Type[] { typeof(XmlCustomSerializer) },null);
			if(mi == null)
				throw new XmlException("Object type '" + obj.GetType().Name + "' cannot be serialized to XML file " + 
					"because it does not have method 'Serialize(XmlCustomSerializer)' ", null);
			mi.Invoke(obj,new object[] { xmlSer } );
		}

		#endregion

		#region --- Public Input Functions ---

		public static void Read(string inputFileName, object obj)
		{
			FileStream fs = new FileStream(inputFileName, FileMode.Open,FileAccess.Read);
			Read(fs,obj);
			fs.Close();
		}

		public static void Read(Stream inputStream, object obj)
		{
			XmlDocument doc = new XmlDocument();
			XmlTextReader rdr = new XmlTextReader(inputStream);
			doc.Load(rdr);
			rdr.Close();

			ReadFromXmlNode(doc.FirstChild as XmlElement,obj);
		}

		public static void ReadFromXmlNode(XmlNode node, object obj)
		{
			XmlCustomSerializer xmlSer = new XmlCustomSerializer(node);
			xmlSer.Reading = true;

			MethodInfo mi = obj.GetType().GetMethod("Serialize",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null,new Type[] { typeof(XmlCustomSerializer) },null);
			if(mi == null)
				throw new XmlException("Object type '" + obj.GetType().Name + "' cannot be serialized from XML file " + 
					"because it does not have method 'Serialize(XmlCustomSerializer)' ", null);

			mi.Invoke(obj,new object[] { xmlSer } );
		}

		#endregion
	}
    /// <summary>
    /// Chart XML serializer
    /// </summary>
	public class ChartXmlSerializer
	{
		private XmlDocument	doc = null;
		private int			depth = 0;
		private Assembly[]	assems = null;
        private bool        ignoreUnknownProperty = true;
		private bool		serializingTemplate = false;

		public ChartXmlSerializer()
		{ }

		public bool IgnoreUnknownProperty { get { return ignoreUnknownProperty; } set { ignoreUnknownProperty = value; } }
		public bool SerializingTemplate { get { return serializingTemplate; } set { serializingTemplate = value; } }

		public ChartXmlSerializer(Object obj, string rootTag)
		{
			ChartBase c = null;
			if( obj.GetType().GetInterface("IChart") != null)
			{
                PropertyInfo pi = obj.GetType().GetProperty("ChartBase", BindingFlags.NonPublic | BindingFlags.Instance);
				if(pi != null)
				{
					c = (ChartBase)pi.GetValue(obj,null);
					c.InSerialization = true;
				}
			}

			doc = new XmlDocument();
			XmlNode node = Build(obj,rootTag);
			doc.AppendChild(node);
			if(c != null)
				c.InSerialization = false;
		}

		#region --- Building Dom from Object ---

		protected virtual XmlNode Build(object obj, string nodeTag)
		{
			if(obj == null)
				return null;

			Type objType = obj.GetType();

			if(serializingTemplate)
			{
				object[] typeAttributes = objType.GetCustomAttributes(true);
				foreach(Attribute attribute in typeAttributes)
				{
					if(attribute is NotIncludedInTemplate)
						return null;
				}
			}

			XmlElement element = doc.CreateElement(nodeTag);
			element.SetAttribute("type",objType.ToString());

			TypeConverter tc = TypeDescriptor.GetConverter(objType);

			// If the type can be serialized from string we rely on that and don't go through 
			// the rest of the logics
			if(tc.CanConvertFrom(typeof(string)))
			{
				element.SetAttribute("value",tc.ConvertToInvariantString(obj));
				return element;
			}

			if(objType.IsEnum)
			{
				element.SetAttribute("value",obj.ToString());
				return element;
			}
			else if(objType.IsValueType)
			{
				if(objType == typeof(Color))
					element.SetAttribute("value",ConvertToString((Color)obj));
				else
				{
					if(tc != null && tc.CanConvertTo(typeof(string)))
						element.SetAttribute("value",tc.ConvertToInvariantString(obj));
					else
						element.SetAttribute("value",obj.ToString());
				}
				return element;
			}

			else if(objType.IsSubclassOf(typeof(CollectionBase)))
			{
				if(objType == typeof(ChartColorCollection))
				{
					element.SetAttribute("value",ConvertToString((ChartColorCollection)obj));
				}
				else
				{
					CollectionBase collection = obj as CollectionBase;
					depth++;
					foreach(object member in collection)
					{
						XmlNode memberNode = Build(member,"item");
						if(memberNode != null)
							element.AppendChild(memberNode);
					}
					depth--;
					if(element.FirstChild == null)
						element = null;
				}
			}
			else if(objType == typeof(string))
			{
				element.SetAttribute("value",obj as string);
			}
			else
			{
				depth ++;
				PropertyInfo[] props = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
				ReorderIfNecessary(props);
				foreach(PropertyInfo pi in props)
				{
					if(!SpecialHandling(pi, ref element))
					{
						object defaultValue = null;
						bool defaultValueExists = false;
						// Check if property is not to be serialized
						object[] attributes = pi.GetCustomAttributes(true);
						bool toSerialize = true;
						foreach(Attribute attr in attributes)
						{
							DesignerSerializationVisibilityAttribute vis = attr as DesignerSerializationVisibilityAttribute;
							if(vis != null && vis.Visibility == DesignerSerializationVisibility.Hidden)
							{
								toSerialize = false;
								break;
							}
							if(attr is DefaultValueAttribute)
							{
								defaultValueExists = true;
								defaultValue = (attr as DefaultValueAttribute).Value;
							}
						}
						if(toSerialize)
						{
							// We serialize only properties that have both get and set methods
							if(pi.CanRead && (pi.CanWrite || pi.PropertyType.IsSubclassOf(typeof(CollectionBase))))
							{
								object objValue = pi.GetValue(obj,null);
								if(!(defaultValueExists && object.Equals(objValue,defaultValue)))
								{
									XmlNode eNode = Build(objValue,pi.Name);
									if(eNode != null)
										element.AppendChild(eNode);
								}
							}
						}
					}
				}
				depth --;
				if(element.FirstChild == null)
					element = null;
			}

			return element;
		}

		private void ReorderIfNecessary(PropertyInfo[] props)
		{
			// We have to set series style on a composite series (or chart control) before
			// setting the style in subordinated series, to avoid propagation of style from
			// parent to children. To achieve this, we'll move the style property to the 
			// beginning of the list
			int ix;
			for(ix = 0; ix< props.Length; ix++)
			{
				if(props[ix].Name == "StyleName" || props[ix].Name == "MainStyle")
				{
					PropertyInfo pi = props[ix];
					for(int i=ix; i>0; i--)
						props[i] = props[i-1];
					props[0] = pi;
					break;
				}
			}
		}
		private string[] skipped = new string[] 
				{
					"ActiveControl",
					"Parent",
					"WindowTarget",
					"Site",
					"EnableTheming",
					"DataSourceID",
					"SkinID",
                    "Verbs"
				};

		private bool IgnorePropertyInInput(string pName)
		{
			foreach(string propName in skipped)
			{
				if(pName == propName)
					return true;
			}
			return false;
		}

		private bool SpecialHandling(PropertyInfo pi, ref XmlElement node)
		{
			foreach(string propName in skipped)
			{
				if(pi.Name == propName)
					return true;
			}
			if(pi.PropertyType == typeof(DataPoint))
				return true;
			if(depth>10)
				return true;
			return false;
		}

		#endregion

		#region --- Custom conversions to and from string ---

		private string ConvertToString(Color c)
		{
			uint x = c.A;
			x = (x<<8) + c.R;
			x = (x<<8) + c.G;
			x = (x<<8) + c.B;
			return "0x" + x.ToString("x8");
		}

		private string ConvertToString(ChartColorCollection ccc)
		{
			string s = "", sep = "";
			foreach(ChartColor cc in ccc)
			{
				s = s + sep + ConvertToString(cc.Color);
				sep = ",";
			}
			return s;
		}

		private Color ConvertColorFromString(string str)
		{
			if(str.Length == 8)
				str = "FF" + str.Substring(2,6);
			else
				str = str.Substring(2,str.Length-2);
			uint x = uint.Parse(str,System.Globalization.NumberStyles.HexNumber);
			return Color.FromArgb((int)((x>>24) & 0xFF), (int)((x>>16) & 0xFF), (int)((x>>8) & 0xFF), (int)(x & 0xFF));
		}

		private Color[] ConvertColorCollectionFromString(string str)
		{
			string[] parts = str.Split(',');
			int n = parts.Length;
			Color[] colors = new Color[n];
			for(int i=0; i<n; i++)
				colors[i] = ConvertColorFromString(parts[i]);
			return colors;
		}

		#endregion

		#region --- Building Object from Dom ---

		private string PathOf(XmlNode node)
		{
			string path = node.Name;
			XmlNode nd = node.ParentNode;
			while(nd != null)
			{
				path = nd.Name + "->" + path;
				nd = nd.ParentNode;
			}
			return path;
		}

		private Type GetObjectType(string objectTypeName)
		{
			Type objType = Type.GetType(objectTypeName);
			if(objType == null)
			{
				if(assems == null)
					assems = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly a in assems)
				{
					objType = a.GetType(objectTypeName);
					if(objType != null)
						break;
				}
			}
			return objType;
		}

		public virtual object BuildObject(object obj, XmlElement elem)
		{
			// --- Getting object and the type ---

			try
			{
				// Getting the object type
				
				Type objType;
				string objectTypeStr = null;
				// If the object is known, we know the type 
				if(obj != null)
				{
					objType = obj.GetType();
					objectTypeStr = objType.FullName;
				}
				else
				{
					objectTypeStr = elem.GetAttribute("type");
					if(objectTypeStr == "")
					{
						if(IgnoreUnknownProperty)
							return null;
						else
							throw new Exception("The 'type' attribute is not known for '" + elem.Name + "'.");
					}
					else
						objType = GetObjectType(objectTypeStr);
				}

				if(objType == null)
				{
					if(objectTypeStr == "System.Drawing.Color")
						objType = typeof(System.Drawing.Color);
					else
					{
						if(IgnoreUnknownProperty)
							return null;
						else
							throw new XmlException("Cannot get type '" + objectTypeStr + "'.", null);
					}
				}

				if(objType != null && serializingTemplate)
				{
					object[] typeAttributes = objType.GetCustomAttributes(true);
					foreach(Attribute attribute in typeAttributes)
					{
						if(attribute is NotIncludedInTemplate)
							return null;
					}
				}
				string valueStr = elem.GetAttribute("value");

				TypeConverter tc = TypeDescriptor.GetConverter(objType);
				if(tc.CanConvertFrom(typeof(string)))
                {
                    obj = tc.ConvertFromInvariantString(valueStr);
				}

				else if(objType == typeof(string))
					obj = valueStr;

                else if (objType.IsEnum)
                {
                    TypeConverter tce = TypeDescriptor.GetConverter(objType);
                    obj = tce.ConvertFromString(valueStr);
                }

                else if (valueStr != null && valueStr != "")
				{
					if(objType == typeof(Color))
						obj = ConvertColorFromString(valueStr);
					else if(objType == typeof(ChartColor))
						obj = new ChartColor(ConvertColorFromString(valueStr));
					else if (objType == typeof(ChartColorCollection))
					{
						Color[] colors = ConvertColorCollectionFromString(valueStr);
						if(obj == null)
							obj = new ChartColorCollection(colors);
						else
						{
							ChartColorCollection ccc = obj as ChartColorCollection;
							ccc.Clear();
							for(int i=0; i<colors.Length; i++)
								ccc.Add(new ChartColor(colors[i]));
						}
					}
					else
					{
						TypeConverter tConv = TypeDescriptor.GetConverter(objType);
						if(tConv != null && tConv.CanConvertFrom(typeof(string)))
							obj = tConv.ConvertFromInvariantString(valueStr);
						else
							obj = Convert.ChangeType(valueStr,objType);
					}
				}

				else if(objType.IsSubclassOf(typeof(CollectionBase)))
				{
					if(obj == null)
					{
						ConstructorInfo ci = objType.GetConstructor(new Type[] { });
						if(ci == null)
							throw new Exception("There is no default constructor for class '" + objectTypeStr + "'");
						obj = ci.Invoke(null);
					}
					CollectionBase coll = obj as CollectionBase;
					if (coll is SeriesCollection || coll is LightCollection)
						coll.Clear();

					MethodInfo mi = objType.GetMethod("Add",new Type[] { typeof(object) });

					if(mi != null)
					{
						XmlElement cElem = elem.FirstChild as XmlElement;
						while(cElem != null)
						{
							object item = BuildObject(null, cElem);
							if(item != null)
							{
								mi.Invoke(coll,new object[] { item });
								Palette p = item as Palette;
							}
							cElem = cElem.NextSibling as XmlElement;
						}
					}
				}

                else
                {
                    if (obj == null)
                    {
                        ConstructorInfo ci = objType.GetConstructor(new Type[] { });
                        if (ci == null)
                            throw new Exception("There is no default constructor for class '" + objectTypeStr + "'");
                        obj = ci.Invoke(null);
                    }
                    // Assigning and/or building properties
                    XmlElement cElem = elem.FirstChild as XmlElement;
                    while (cElem != null)
                    {
                        PropertyInfo pi = objType.GetProperty(cElem.Name);
                        if (pi != null)
                        {
                            // Don't assign property value if it is a non-null collection (only populate)
                            object propValue = null;
                            Type piType = pi.PropertyType;
                            if (piType.IsSubclassOf(typeof(CollectionBase)))
                                propValue = pi.GetValue(obj, null);
                            if (propValue is ChartColorCollection)	// TEST
                                propValue = null;					// TEST
                            object result = BuildObject(propValue, cElem);
                            bool rightType = result != null && (result.GetType() == piType || result.GetType().IsSubclassOf(piType));
							if (rightType && propValue == null)
							{
								pi.SetValue(obj, result, new object[] { });
							}
                        }
                        else
                        {
							if (!IgnoreUnknownProperty && !IgnorePropertyInInput(cElem.Name))
                                throw new Exception("Unknown property '" + cElem.Name + "' for class '" + objType.Name + "'");
                        }
                        cElem = cElem.NextSibling as XmlElement;
                    }
                }
			}
			catch (Exception ex)
			{
				string msg = ex.Message;
				msg = "<" + elem.Name + "\n  " + msg.Replace("\n","\n  ") + "\n>";
#if DEBUG
                Debug.WriteLine(ex.StackTrace);
#endif
				throw new Exception(msg);
			}

			return obj;
		}


		#endregion

		#region --- Public Output Methods ----

		public void Write(string outputFileName)
		{
			FileStream fs = new FileStream(outputFileName, FileMode.Create);
			Write(fs);
			fs.Close();
		}

		public void Write(Stream outStream)
		{
			XmlTextWriter wrt = new XmlTextWriter(outStream,System.Text.Encoding.UTF8);
			wrt.Formatting = Formatting.Indented;
			doc.WriteTo(wrt);
			wrt.Close();
		}

#if !__BUILDING_CRI__
		public static bool SaveXmlTemplate(object obj, string rootTag)
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
 
			saveFileDialog1.Filter = "XML Files(*.XML)|*.XML|All files (*.*)|*.*";
			saveFileDialog1.FilterIndex = 1 ;
			saveFileDialog1.RestoreDirectory = true ;

			string name = "";
			Type objType = obj.GetType();
			PropertyInfo pi = objType.GetProperty("Name");
			if(pi != null)
			{
				name = (string)pi.GetValue(obj,null);
				saveFileDialog1.FileName = name + ".XML";
			}

			if(saveFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.FileName != "")
			{
				try
				{
#if __BUILDING_CRI_DESIGNER__
                    RSDomBuilder bld = new RSDomBuilder(obj, rootTag);
#else
					ChartXmlSerializer bld = new ChartXmlSerializer(obj,rootTag);
#endif
					bld.SerializingTemplate = true;
                    bld.Write(saveFileDialog1.FileName);
					return true;
				}
				catch
				{
					return false;
				}
			} 
			else
				return false;
		}
#endif
		#endregion

		#region --- Public Input Methods ---

		public object ReadObject(Stream inStream)
		{
			Read(inStream);
			return BuildObject(null,doc.FirstChild as XmlElement);
		}

		public object ReadObject(string inputFileName)
		{
			Read(inputFileName);
			return BuildObject(null,doc.FirstChild as XmlElement);
		}

		public object ReadObject(object obj, string inputFileName)
		{
			Read(inputFileName);
			BuildObject(obj,doc.FirstChild as XmlElement);
			return obj;
		}

		public object ReadObject(object obj, Stream inputStream)
		{
			Read(inputStream);
			BuildObject(obj,doc.FirstChild as XmlElement);
			return obj;
		}

		public XmlDocument Read(string inputFileName)
		{
			FileStream fs = new FileStream(inputFileName, FileMode.Open, FileAccess.Read);
			Read(fs);
			fs.Close();
			return doc;
		}

		public XmlDocument Read(Stream inStream)
		{
			doc = new XmlDataDocument();
			doc.Load(inStream);
			return doc;
		}

#if !__BUILDING_CRI__
		public static bool OpenXmlTemplate(object obj)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.Filter = "XML Files(*.XML)|*.XML|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 1;
			openFileDialog1.RestoreDirectory = true ;

			if(openFileDialog1.ShowDialog() == DialogResult.OK && openFileDialog1.FileName != "")
			{
				ChartXmlSerializer bld = new ChartXmlSerializer();
				bld.SerializingTemplate = true;
				if(bld.ReadObject(obj,openFileDialog1.FileName) == null)
					return false;
				else
					return true;
			}
			else
				return false;
		}
#endif

		#endregion
	}

	internal class NotIncludedInTemplate : Attribute
	{
	}

}
