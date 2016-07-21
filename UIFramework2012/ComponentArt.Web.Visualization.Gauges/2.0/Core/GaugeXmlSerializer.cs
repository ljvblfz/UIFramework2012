using System;
using System.ComponentModel;
using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace ComponentArt.Web.Visualization.Gauges
{
	internal class GaugeXmlSerializer
	{
		private XmlDocument	doc = null;
		private int			depth = 0;
		private Assembly[]	assems = null;
		private bool        ignoreUnknownProperty = true;
		private bool		serializingTemplate = false;
		private bool		oldTemplate = false;

		public GaugeXmlSerializer()
        {
            doc = new XmlDocument();
        }

		public bool IgnoreUnknownProperty { get { return ignoreUnknownProperty; } set { ignoreUnknownProperty = value; } }
		public bool SerializingTemplate { get { return serializingTemplate; } set { serializingTemplate = value; } }

		public GaugeXmlSerializer(Object obj, string rootTag)
		{
			doc = new XmlDocument();
			if(oldTemplate)
			{
				XmlNode node = Build(obj,rootTag);
				doc.AppendChild(node);
			}
			else
			{
				BuildNode(null,rootTag,obj);
			}
		}

		public static XmlDocument GetDOM(Object obj)
		{
			GaugeXmlSerializer s = new GaugeXmlSerializer(obj,obj.GetType().Name);
			return s.doc;
		}

		public static object GetObject(XmlDocument doc)
		{
			object result = CreateObject(doc.FirstChild.Name);
			GaugeXmlSerializer s = new GaugeXmlSerializer();
			s.PopulateObjectFromNode(result,doc.FirstChild);
			return result;
		}

		public static void DiagnoseInnerProperties(Type  type)
		{
			PropertyDescriptorCollection descriptors = TypeDescriptor.GetProperties(type);
			foreach(PropertyDescriptor descr in descriptors)
			{
				TypeConverter tc = descr.Converter;
				if(tc.CanConvertTo(typeof(String))  && tc.CanConvertFrom(typeof(string)))
				{
					continue; // because this property is serialized as attribute
				}
				AttributeCollection attributes = descr.Attributes;
				bool toReport = true;
				for(int i=0; i<attributes.Count; i++)
				{
					if(attributes[i] is DesignerSerializationVisibilityAttribute)
					{
						DesignerSerializationVisibilityAttribute dsa = attributes[i] as DesignerSerializationVisibilityAttribute;
						if(dsa.Visibility == DesignerSerializationVisibility.Hidden)
						{
							toReport = false;
							break;
						}
					}

					if(attributes[i].GetType().Name == "PersistenceModeAttribute")
					{
						toReport = false;
						break;
					}

				}
				if(toReport)
					Debug.WriteLine(type.Name + '.' + descr.Name);

				DiagnoseInnerProperties(descr.PropertyType);
			}

		}

		internal bool OldTemplate { get { return oldTemplate; } set { oldTemplate = value; } }

		#region --- New Implementation ---

		#region --- Building DOM from object - new implementation ---

		// Scan all members (properties or collection items) and append them to the parent node
		internal protected void BuildNode(XmlElement parentNode, string valueName, object valueObject)
		{
			if(valueObject == null)
				return;

			// If object value can be converted from/to string, serialize as attribute
			TypeConverter tc = TypeDescriptor.GetConverter(valueObject);
			if(tc.CanConvertTo(typeof(string)) && tc.CanConvertFrom(typeof(string)))
			{
				parentNode.SetAttribute(valueName,tc.ConvertToInvariantString(valueObject));
			}
			else // create element and serialize coll items or properties
			{
				Type objType = valueObject.GetType();

				// Serialize bitmap
				if(objType == typeof(Bitmap))
				{
					string bmpstr = XMLUtils.BmpToString(valueObject as Bitmap);
					if(bmpstr != "" && bmpstr != null)
					parentNode.InnerText = bmpstr;
					else
						Debug.WriteLine(" 1. Empty bmp ");
				}

					// Serialize collection
				else if(objType.IsSubclassOf(typeof(CollectionBase)))
				{
					CollectionBase collection = valueObject as CollectionBase;
					// We'll skip empty collection, unless it's at the document root
					if(parentNode == null || collection.Count > 0)
					{
						XmlElement element = doc.CreateElement(valueName);
						//Debug.WriteLine(LB+"Collection " + valueName);
						if(parentNode != null)
							parentNode.AppendChild(element);
						else
							doc.AppendChild(element);
						depth++;
						foreach(object member in collection)
						{
							BuildNode(element,member.GetType().Name,member);
						}
						depth--;
					}
				}
				// Serialize properties
				else
				{
					XmlElement element = doc.CreateElement(valueName);
					PropertyDescriptorCollection propertyDescriptors = TypeDescriptor.GetProperties(valueObject);
					depth++;
					//move "Name" to the first position
					ArrayList descrList = new ArrayList(propertyDescriptors);
					PropertyDescriptor nameDes = propertyDescriptors["Name"];
					if(nameDes != null)
					{
						descrList.Remove(nameDes);
						descrList.Insert(0,nameDes);
					}
					foreach(PropertyDescriptor pDes in descrList)
					{
						if( pDes.SerializationVisibility == DesignerSerializationVisibility.Hidden || 
							!pDes.ShouldSerializeValue(valueObject))
							continue;

						object propertyValue = pDes.GetValue(valueObject);
						Type valueType = (propertyValue == null? null:propertyValue.GetType());
						depth++;
						BuildNode(element,pDes.Name,propertyValue);
						depth--;
					}
					depth--;
					if(element.FirstChild != null || element.Attributes.Count > 0 || parentNode == null )
					{
						if(parentNode != null)
							parentNode.AppendChild(element);
						else
							doc.AppendChild(element);
					}
				}
			}

		}

        private string LB // leading blanks for debug printing
		{
			get
			{
				string s = "";
				for(int i=0; i<depth; i++)
					s += "  ";
				return s;
			}
		}
		
		#endregion

		#region --- Populating object from DOM - new implementaton ---

		internal void PopulateObjectFromNode(object target, XmlNode node)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(target);

			// visit all attributes
			foreach(XmlAttribute attribute in node.Attributes)
			{
				PropertyDescriptor pDes = properties[attribute.Name];
				if(pDes != null)
				{
					string stringValue = attribute.Value;
					if(pDes.PropertyType == typeof(string))
						pDes.SetValue(target,stringValue);
					else
					{
						TypeConverter tc = TypeDescriptor.GetConverter(pDes.PropertyType);
						if(tc != null && tc.CanConvertFrom(typeof(string)))
							pDes.SetValue(target,tc.ConvertFromInvariantString(stringValue));

					}
				}
				else
				{
					bool handled = false;
#if !WEB
					if(attribute.Name == "#text") // something stored in the inner text
					{
						if(target is ImageAnnotation)
						{
							ImageAnnotation ia = target as ImageAnnotation;
							ia.Image = XMLUtils.StringToBmp(node.InnerText);
							handled = true;
						}
					}
#endif
					if(!handled)
						throw new Exception("Property '" + attribute.Name + "' unknown for object type '" + target.GetType().Name + "'.");
				}
			}

			// visit all children
			XmlNode child = node.FirstChild;

			if(target.GetType().IsSubclassOf(typeof(CollectionBase))) // populate collection
			{
				// Cash collection of "add" methods to reduce search
				MethodInfo[] methods = null;
				int nAddMethods = 0;
				ParameterInfo[] addParams = null;
				methods = target.GetType().GetMethods();
				addParams = new ParameterInfo[methods.Length];
				for(int i=0; i<methods.Length; i++)
				{
					ParameterInfo[] pars = methods[i].GetParameters();
					if(methods[i].Name == "Add" && pars.Length == 1)
					{
						methods[nAddMethods] = methods[i];
						addParams[nAddMethods] = pars[0];
						nAddMethods++;
					}
				}
				while(child != null)
				{
					object member = CreateObject(child.Name);
					if(member == null)
						throw new Exception("Cannot create member class '" + child.Name + "' for a(n) '" + target.GetType().Name + "' collection.");
					PopulateObjectFromNode(member,child);
					// find and run appropriate Add methood
					for(int i=0; i<nAddMethods; i++)
					{

						if(member.GetType() == addParams[i].ParameterType 
							|| member.GetType().IsSubclassOf(addParams[i].ParameterType))
						{
							methods[i].Invoke(target,new object[] { member });
							break;
						}
					}
					child = child.NextSibling;
				}
			}

			else // Populate properties
			{
				while(child != null)
				{
					string propertyName = child.Name;
					PropertyDescriptor pDes = properties[propertyName];
					if(pDes != null)
					{
						Type propertyType = pDes.PropertyType;
						XmlAttribute typeAttribute = child.Attributes["type"];
						if(typeAttribute != null)
						{
							string pTypeString = typeAttribute.Value;
							if(pTypeString != "")
								propertyType = this.GetObjectType(pTypeString);
						}
						object obj = pDes.GetValue(target);
						if(obj == null)
						{
							ConstructorInfo ci = propertyType.GetConstructor(Type.EmptyTypes);
							obj = ci.Invoke(null);
							pDes.SetValue(target,obj);
						}
						PopulateObjectFromNode(obj,child);
					}
					else
					{
						bool handled = false;
#if !WEB
						if(propertyName == "#text") // something stored in the inner text
						{
							if(target is ImageAnnotation)
							{
								ImageAnnotation ia = target as ImageAnnotation;
								ia.Image = XMLUtils.StringToBmp(node.InnerText);
								handled = true;
							}
						}
#endif
						if(!handled)
							throw new Exception("Property '" + propertyName + "' unknown for object type '" + target.GetType().Name + "'.");
					}
					child = child.NextSibling;
				}
			}
		}

		private static object CreateObject(string typeName)
		{
			try
			{
				string typeNamePrefix = typeof(SubGauge).FullName;
				int ix = typeNamePrefix.LastIndexOf(".");
				string fullTypeName = typeNamePrefix.Substring(0,ix+1) + typeName;
				return Assembly.GetAssembly(typeof(SubGauge)).CreateInstance(fullTypeName);
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
				if(ex.InnerException != null)
				{
					Debug.WriteLine("======= Inner Exception : ");
					Debug.WriteLine(ex.InnerException.Message);
					Debug.WriteLine(ex.InnerException.StackTrace);
				}
			}
			return null;
		}

		#endregion

		#endregion

		#region --- Old Implementation ---

		#region --- Custom conversions to and from string ---

		private string ConvertToString(Color c)
		{
			uint x = c.A;
			x = (x<<8) + c.R;
			x = (x<<8) + c.G;
			x = (x<<8) + c.B;
			return "0x" + x.ToString("x8");
		}

//		private string ConvertToString(ChartColorCollection ccc)
//		{
//			string s = "", sep = "";
//			foreach(ChartColor cc in ccc)
//			{
//				s = s + sep + ConvertToString(cc.Color);
//				sep = ",";
//			}
//			return s;
//		}

		private Color ConvertColorFromString(string str)
		{
			try
			{
				ColorConverter cc = new ColorConverter();
				return (Color)cc.ConvertFromString(str);
			}
			catch { }

			if(str == string.Empty)
				return Color.Empty;

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
		
		#region --- Building DOM from the object model - old implementation ---

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

			// Cpecial treatment of bitmaps
			if(objType == typeof(Bitmap))
			{
				string bmpstr = XMLUtils.BmpToString(obj as Bitmap);
				if(bmpstr != "" && bmpstr != null)
					element.InnerText = bmpstr;
				else
					Debug.WriteLine(" 2. Empty bmp ");
				return element;
			}
				// If the type can be serialized from string we rely on that and don't go through 
				// the rest of the logics
			else if(tc.CanConvertFrom(typeof(string)))
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
//								Debug.WriteLine("Getting " + obj.GetType().Name + "." + pi.Name);
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
			if(depth>10)
				return true;
			return false;
		}

		#endregion

		#region --- Building Object from Dom - old implementation ---

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

			string valueStr = elem.GetAttribute("value");

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
				
				// Handling bitmaps

				if(objType == typeof(Bitmap))
				{
					return XMLUtils.StringToBmp(elem.InnerText);
				}
				// Try to get value from the value string

				if(valueStr != null)
				{
					// Note: string and color can have valueStr == ""
					if(objType == typeof(string))
						return valueStr;

					else if(objType == typeof(Color))
						return ConvertColorFromString(valueStr);

					if(valueStr != string.Empty)
					{
						if (objType.IsEnum)
						{
							TypeConverter tce = TypeDescriptor.GetConverter(objType);
							obj = tce.ConvertFromString(valueStr);
						}
					
						else
						{
							TypeConverter tConv = TypeDescriptor.GetConverter(objType);
							if(tConv != null && tConv.CanConvertFrom(typeof(string)))
								obj = tConv.ConvertFromInvariantString(valueStr);
							else
								obj = Convert.ChangeType(valueStr,objType);
						}
						return obj;
					}
				}

				// Populate object's properties from children nodes

				// Make sure the object exists

				if(obj == null)
				{
					ConstructorInfo ci = objType.GetConstructor(new Type[] { });
					if(ci == null)
						throw new Exception("There is no default constructor for class '" + objectTypeStr + "'");
					obj = ci.Invoke(null);
				}

				// Populate collection items

				if(objType.IsSubclassOf(typeof(CollectionBase)))
				{
					CollectionBase coll = obj as CollectionBase;

					MethodInfo[] mis = objType.GetMethods();
					//MethodInfo mi = objType.GetMethod("Add",new Type[] { typeof(object) });

					XmlElement cElem = elem.FirstChild as XmlElement;
					while(cElem != null)
					{
						object item = BuildObject(null, cElem);
						if(item != null)
						{
							// Looking for appropriate "Add" method
							Type itemType = item.GetType();
							for(int i=0; i<mis.Length; i++)
							{
								if(mis[i].Name != "Add")
									continue;
								ParameterInfo[] pis = mis[i].GetParameters();
								for(int j = 0; j<pis.Length; j++)
								{
									Type pt = pis[j].ParameterType;
									if(itemType == pt || itemType.IsSubclassOf(pt))
									{
										mis[i].Invoke(coll,new object[] { item });
										break;
									}
								}
							}
						}
						cElem = cElem.NextSibling as XmlElement;
					}
				}

				else // Populate properties
				{
					XmlElement cElem = elem.FirstChild as XmlElement;
					while (cElem != null)
					{
						PropertyInfo pi = objType.GetProperty(cElem.Name);
						if (pi != null)
						{
							if(pi.CanWrite)
							{
								pi.SetValue(obj, BuildObject(null, cElem), new object[] { });
							}
							else
							{
								// Don't assign property value if it is a non-null collection (only populate)
								object propValue = null;
								Type piType = pi.PropertyType;
								if(piType.IsSubclassOf(typeof(CollectionBase)))
									propValue = pi.GetValue(obj, null);
								if(propValue != null)
								{
									// We just use the dom subtree to build contents of the collection
									// using existing value of the property, i.e. there is no assignment here
									BuildObject(propValue, cElem);
								}
								else
								{
									// Here we have non-collection property, or null value collection property 
									object result = BuildObject(propValue, cElem);
									bool rightType = result != null && (result.GetType() == piType || result.GetType().IsSubclassOf(piType));
									if (rightType )
									{
										if(pi.CanWrite)
											pi.SetValue(obj, result, new object[] { });
										else
										{
											//Debug.WriteLine(pi.DeclaringType.Name + "." + pi.Name + " doesn't have setter method.");
										}
									}
								}
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
				if(ex.InnerException !=null)
					msg = msg + "\n" + ex.InnerException.Message;
				string val = elem.GetAttribute("value");
				msg = "<" + elem.Name + ((valueStr!=null && valueStr!="")? " value="+valueStr:" ") +
					"\n  " + msg.Replace("\n","\n  ") + "\n>";
#if DEBUG
				//Debug.WriteLine(ex.StackTrace);
				if(ex.InnerException !=null)
				{
					Debug.WriteLine("Inner exception");
					Debug.WriteLine(ex.InnerException.StackTrace);
				}
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
		
		public override string ToString()
		{
			// default with indentation
			return ToString(true);
		}

		public string ToString(bool indentXml)
		{
			string xml = "";

			if (doc != null)
			{
				if (indentXml)
				{
					// write to memory stream
					MemoryStream ms = new MemoryStream();
					Write(ms);

					// convert memory stream to string
					xml = Encoding.UTF8.GetString(ms.GetBuffer());
					ms.Close();

					// trim BOM and trailing NULLs
					xml = xml.Trim();
					xml = xml.Trim(new char[] { '\0' });
				}
				else
				{
					// use streamlined xml (no whitespace)
					xml = doc.OuterXml;
				}
			}

			return xml;
		}
		

//#if !__BUILDING_CRI__
//		public static bool SaveXmlTemplate(object obj, string rootTag)
//		{
//			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
// 
//			saveFileDialog1.Filter = "XML Files(*.XML)|*.XML|All files (*.*)|*.*";
//			saveFileDialog1.FilterIndex = 1 ;
//			saveFileDialog1.RestoreDirectory = true ;
//
//			string name = "";
//			Type objType = obj.GetType();
//			PropertyInfo pi = objType.GetProperty("Name");
//			if(pi != null)
//			{
//				name = (string)pi.GetValue(obj,null);
//				saveFileDialog1.FileName = name + ".XML";
//			}
//
//			if(saveFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.FileName != "")
//			{
//				try
//				{
//#if __BUILDING_CRI_DESIGNER__
//                    RSDomBuilder bld = new RSDomBuilder(obj, rootTag);
//#else
//					DomBuilder bld = new DomBuilder(obj,rootTag);
//#endif
//					bld.SerializingTemplate = true;
//					bld.Write(saveFileDialog1.FileName);
//					return true;
//				}
//				catch
//				{
//					return false;
//				}
//			} 
//			else
//				return false;
//		}
//#endif
		#endregion

		#endregion

		#region --- Public Input Methods ---

		public static object ReadObject(Stream inStream)
		{
			GaugeXmlSerializer ser = new GaugeXmlSerializer();
			ser.Read(inStream);
			if(ser.OldTemplate)
				return ser.BuildObject(null,ser.doc.FirstChild as XmlElement);
			else
			{
				object obj = CreateObject(ser.doc.FirstChild.Name);
				ser.PopulateObjectFromNode(obj,ser.doc.FirstChild);
				return obj;
			}
		}

		public static object ReadObjectOld(Stream inStream)
		{
			GaugeXmlSerializer ser = new GaugeXmlSerializer();
			ser.OldTemplate = true;
			ser.Read(inStream);
			if(ser.OldTemplate)
				return ser.BuildObject(null,ser.doc.FirstChild as XmlElement);
			else
			{
				object obj = CreateObject(ser.doc.FirstChild.Name);
				ser.PopulateObjectFromNode(obj,ser.doc.FirstChild);
				return obj;
			}
		}

		public static object ReadObject(string inputFileName)
		{
			GaugeXmlSerializer ser = new GaugeXmlSerializer();
			ser.Read(inputFileName);
			if(ser.OldTemplate)
				return ser.BuildObject(null,ser.doc.FirstChild as XmlElement);
			else
			{
				object obj = CreateObject(ser.doc.FirstChild.Name);
				ser.PopulateObjectFromNode(obj,ser.doc.FirstChild);
				return obj;
			}
		}

		public static object ReadObjectOld(object obj, string inputFileName)
		{
			GaugeXmlSerializer ser = new GaugeXmlSerializer();
			ser.OldTemplate = true;
			ser.Read(inputFileName);
			if(ser.OldTemplate)
				ser.BuildObject(obj,ser.doc.FirstChild as XmlElement);
			else
				ser.PopulateObjectFromNode(obj,ser.doc.FirstChild);
			return obj;
		}
		public static object ReadObject(object obj, string inputFileName)
		{
			GaugeXmlSerializer ser = new GaugeXmlSerializer();
			ser.Read(inputFileName);
			if(ser.OldTemplate)
				ser.BuildObject(obj,ser.doc.FirstChild as XmlElement);
			else
				ser.PopulateObjectFromNode(obj,ser.doc.FirstChild);
			return obj;
		}

		public static object ReadObject(object obj, Stream inputStream)
		{
			GaugeXmlSerializer ser = new GaugeXmlSerializer();
			ser.Read(inputStream);
			if(ser.OldTemplate)
				ser.BuildObject(obj,ser.doc.FirstChild as XmlElement);
			else
				ser.PopulateObjectFromNode(obj,ser.doc.FirstChild);
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

		#endregion
	}

	internal class NotIncludedInTemplate : Attribute
	{
	}

    // ======================================================================================================
    // Static XML Utilities
    // ======================================================================================================

    internal class XMLUtils
    {

		public static bool reduceBitmaps = false;

        #region --- Exporting and Importing Layers ---
        // Used in assembly building for GDI+ framework

        internal static XmlElement CreateXmlNode(XmlDocument doc, Layer layer)
        {
            if (layer == null)
                return null;

            XmlElement layerNode = doc.CreateElement("Layer");
            layerNode.SetAttribute("Name", layer.Name);
            layerNode.SetAttribute("GaugeKind", layer.Kind.ToString());
            layerNode.SetAttribute("Role", layer.LayerRoleKind.ToString());
			if(layer.CornerSize.Abs() != 0)
				layerNode.SetAttribute("CornerSize",layer.CornerSize.ToString());

            if (layer.MainVisualParts != null && layer.MainVisualParts.Count > 0)
            {
                XmlElement vpNode = doc.CreateElement("Image");
                for (int i = 0; i < layer.MainVisualParts.Count; i++)
                {
                    ComponentArt.Web.Visualization.Gauges.GDIEngine.GDIBitmapVisualPart vp = layer.MainVisualParts[i] as ComponentArt.Web.Visualization.Gauges.GDIEngine.GDIBitmapVisualPart;
                    if (vp != null)
                    {
						XmlElement child = vp.GetXmlNode(doc);
						if(child != null)
							vpNode.AppendChild(child);
                    }
                }
                if (vpNode.FirstChild != null)
                    layerNode.AppendChild(vpNode);
            }

            if (layer.Region != null)
            {
                XmlElement vpNode = doc.CreateElement("Region");
                ComponentArt.Web.Visualization.Gauges.GDIEngine.GDIBitmapVisualPart vp = layer.Region as ComponentArt.Web.Visualization.Gauges.GDIEngine.GDIBitmapVisualPart;
				if (vp != null)
				{
					XmlElement child = vp.GetXmlNode(doc);
					if(child != null)
					{
						vpNode.AppendChild(child);
						layerNode.AppendChild(vpNode);
					}
				}
            }

            if (layer.Shadow != null)
            {
                XmlElement vpNode = doc.CreateElement("Shadow");
                ComponentArt.Web.Visualization.Gauges.GDIEngine.GDIBitmapVisualPart vp = layer.Shadow as ComponentArt.Web.Visualization.Gauges.GDIEngine.GDIBitmapVisualPart;
                if (vp != null)
                {
					XmlElement child = vp.GetXmlNode(doc);
					if(child != null)
					{
						vpNode.AppendChild(child);
						layerNode.AppendChild(vpNode);
					}
				}
            }
            if (layerNode.FirstChild != null)
                return layerNode;
            else
                return null;
        }

        internal static void CreateLayersNodes(XmlDocument doc, XmlElement skinNode, Skin skin)
        {
            XmlElement newNode = CreateXmlNode(doc, skin.Background());
            if (newNode != null)
                skinNode.AppendChild(newNode);
            newNode = CreateXmlNode(doc, skin.Frame());
            if (newNode != null)
                skinNode.AppendChild(newNode);
            newNode = CreateXmlNode(doc, skin.Cover());
            if (newNode != null)
                skinNode.AppendChild(newNode);
        }

        #endregion

        #region --- Writing and Reading XML Documents ---

        internal static void Write(XmlDocument doc, string outputFileName)
        {
            FileStream fs = new FileStream(outputFileName, FileMode.Create);
            Write(doc, fs);
            fs.Close();
        }

        internal static void Write(XmlDocument doc, Stream outStream)
        {
            XmlTextWriter wrt = new XmlTextWriter(outStream, System.Text.Encoding.UTF8);
            wrt.Formatting = Formatting.Indented;
            doc.WriteTo(wrt);
            wrt.Close();
        }

        #endregion

        #region --- Reading and Writing Bitmaps ---

        internal static string BmpToString(Bitmap bitmap)
        {
			Bitmap bmp = null;
			bool bitmapCreated = false;
			if(reduceBitmaps && bitmap.Width == 400 && bitmap.Height == 400)
			{
				bmp = new Bitmap(200,200);
				Graphics g = Graphics.FromImage(bmp);
				g.DrawImage(bitmap,0,0,200,200);
				g.Dispose();
				bitmapCreated = true;
			}
			else
				bmp = bitmap;

            string str;
            TypeConverter imageConv = TypeDescriptor.GetConverter(typeof(Image));
            if (imageConv.CanConvertTo(typeof(byte[])))
            {
                byte[] bytes = (byte[])imageConv.ConvertTo(bmp, typeof(byte[]));
                str = Convert.ToBase64String(bytes);
                StringBuilder sb = new StringBuilder(bytes.Length + 2 * (int)(bytes.Length / 60 + 1));
                int bLen = str.Length;
                for (int i = 0; i < bLen; i += 60)
                {
                    sb.Append(str, i, Math.Min(60, bLen - i));
                    sb.Append("\r\n");
                }
                str = sb.ToString();
            }
            else
                str = "";

			if(bitmapCreated)
				bmp.Dispose();

            return str;
        }

        internal static Bitmap StringToBmp(string str)
        {
			try
			{
				str = str.Replace("\r\n", "");
				byte[] bytes = Convert.FromBase64String(str);
				TypeConverter imageConv = TypeDescriptor.GetConverter(typeof(Image));
				return (Bitmap)imageConv.ConvertFrom(bytes);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Can't deserialize bitmap from string:");
				Debug.WriteLine(str);
				Debug.WriteLine("(End of String)");
				Debug.WriteLine(ex.StackTrace);
				return null;
				throw new Exception("Can't deserialize bitmap from string.",ex);
			}
        }
        
        #endregion

    }
}
