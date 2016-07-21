using System;
using System.Reflection;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting
{
	internal class GenericCloner
	{
		Assembly [] m_assemblyArray;

		ArrayList typesWithNoDefConstructor = new ArrayList();
		ArrayList fieldsWithTypesFromOtherAssemblies = new ArrayList();

		protected FieldInfo m_currentField = null;

		public GenericCloner() {}
		public GenericCloner(Assembly [] aa) 
		{
			m_assemblyArray = aa;
		}

		Hashtable m_hash = new Hashtable();

		protected virtual void OnTypeFromOtherAssembly(Type type) 
		{

			bool found = false;
			foreach (FieldInfo f in fieldsWithTypesFromOtherAssemblies) 
			{
				if (m_currentField == f) 
				{
					found = true;
					break;
				}
			}

			if (!found)
				fieldsWithTypesFromOtherAssemblies.Add(m_currentField);
		}

		protected virtual Type [] DoNotCloneTypes()
		{
			return new Type [0];
		}

		/// <summary>
		/// Clones the fields an object
		/// </summary>
		/// <param name="srcObj">Source object</param>
		/// <param name="dstObj">Destination object</param>
		protected void CloneFields (object srcObj, object dstObj) 
		{

			ArrayList al = new ArrayList();
			
			// Iterate through the base types and get all the fields
			Type t = srcObj.GetType();
			while (t.Assembly == this.GetType().Assembly) 
			{
				FieldInfo [] fields 
					= t.GetFields( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

				al.AddRange(fields);
				t = t.BaseType;
			}

            FieldInfo [] allFields = (FieldInfo []) al.ToArray(typeof(FieldInfo));
			

			Type [] dontCloneTypes = DoNotCloneTypes();
            

			FieldInfo [] fieldsToClone;

			// Determine what fields should not be cloned
			if (dontCloneTypes != null && dontCloneTypes.Length != 0) 
			{

				ArrayList fieldsToCloneList = new ArrayList();

				foreach (FieldInfo f in allFields) 
				{
					bool found = false;
					foreach (Type dontCloneType in dontCloneTypes) 
					{
						if (dontCloneType == f.FieldType) 					
							found = true;					
					}
                    if (found)
                        continue;

                    DoNotCloneAttribute dnc = (DoNotCloneAttribute)Attribute.GetCustomAttribute(f, typeof(DoNotCloneAttribute));
                    if (dnc == null)
                        fieldsToCloneList.Add(f);
				}

				fieldsToClone = (FieldInfo []) fieldsToCloneList.ToArray(typeof(FieldInfo));
			}
			else 
			{
				fieldsToClone = allFields;
			}

			FieldInfo savedField = m_currentField;
			foreach (FieldInfo fi in fieldsToClone)
			{
				m_currentField = fi;
				m_currentField.SetValue(dstObj, CloneRecursive(m_currentField.GetValue(srcObj)));
			}
			m_currentField = savedField;

		}

		

		protected void CloneItems (IList srcObj, IList dstObj) 
		{
			for (int i=0; i<srcObj.Count; ++i)
			{
				object itemToClone = srcObj[i];
				object newItem = CloneRecursive(itemToClone);

				if (newItem != null)
					dstObj.Add(newItem);
			}
		}

		private void AddToHash (object src, object dst) 
		{
			m_hash.Add(src, dst);
		}

		public virtual object CloneReferenceObject(object srcObj)
		{
			object dstObj;


			// If the object is ICloneable (like string) use the Clone method
			if (srcObj is ICloneable && !(srcObj is IList)) 
			{
				dstObj = ((ICloneable)srcObj).Clone();
				AddToHash(srcObj, dstObj);
				return dstObj;
			} 


			// Get the type of the source object
			Type type = srcObj.GetType();

			

			// Check which assembly the file belongs to
			if (type.Assembly != this.GetType().Assembly) 
			{

				bool foundAssembly=false;

				if (m_assemblyArray != null)
					foreach (Assembly a in m_assemblyArray) 
					{
						if (type.Assembly == a) 
						{
							foundAssembly = true;
							break;
						}
					}

				if (!foundAssembly) 
				{
					this.OnTypeFromOtherAssembly(type);
					return null;
				}
			}

			// Create a copy using a default constructor
			ConstructorInfo ci = type.GetConstructor(
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
				null, new Type [0], null);

			if (ci == null) 
			{
				bool found = false;
				foreach (Type t in typesWithNoDefConstructor) 
				{
					if (type == t) 
					{
						found = true;
						break;
					}
				}

				if (!found)
					typesWithNoDefConstructor.Add(type);

				return null;
			}

			// Get the default constructor
			dstObj = ci.Invoke(new object [0]);
			AddToHash(srcObj, dstObj);

			// Clone Fields
			CloneFields(srcObj, dstObj);

			if (srcObj is IList) 
			{
				CloneItems((IList)srcObj, (IList)dstObj);
			}

			return dstObj;
		}

		public Type [] TypesWithNoDefConstructor 
		{
			get 
			{
				return (Type []) this.typesWithNoDefConstructor.ToArray(typeof(System.Type));
			}
		}

		public FieldInfo [] FieldsWithTypesFromOtherAssemblies 
		{
			get 
			{
				return (FieldInfo []) this.fieldsWithTypesFromOtherAssemblies.ToArray(typeof(FieldInfo));
			}
		}


		private object m_originalObject;

		protected object OriginalObject 
		{
			get 
			{
				return m_originalObject;
			}
		}

		/// <summary>
		/// Clones the object
		/// </summary>
		/// <param name="srcObj">Object to clone</param>
		/// <returns>Copy of the object</returns>
		public virtual object Clone(object srcObj) 
		{
			m_originalObject = srcObj;
			return CloneRecursive(srcObj);
		}
		
		protected object CloneRecursive(object srcObj) 
		{

			object dstObj = null;
			try 
			{
				if (srcObj == null)
					return null;

				// If System.ValueType, simply copy the value
				if (srcObj is System.ValueType) 
				{
					dstObj = srcObj;
					return dstObj;
				}

				// If was already cloned just return the stored pointer
				if (m_hash.ContainsKey(srcObj)) 
				{
					return m_hash[srcObj];
				}

				dstObj = CloneReferenceObject(srcObj);
			}
			catch (Exception e)
			{
				string m = e.Message;
				throw e;
			}
			return dstObj;
		}


		/// <summary>
		/// Constructs an error message
		/// </summary>
		/// <returns>Returns a constructed error message as a string.</returns>
		public string Message() 
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			if (TypesWithNoDefConstructor.Length > 0) 
			{
				sb.Append("Types with no default constructors are: ");
				for (int i=0; i<TypesWithNoDefConstructor.Length; ++i) 
				{
					sb.Append(TypesWithNoDefConstructor[i].ToString() + ", ");
				}
				sb.Remove(sb.Length-2, 2);
				sb.Append(". ");
			}

			if (FieldsWithTypesFromOtherAssemblies.Length > 0) 
			{
				sb.Append("Fields from other assemblies are: ");
				for (int i=0; i<FieldsWithTypesFromOtherAssemblies.Length; ++i) 
				{
					sb.Append(FieldsWithTypesFromOtherAssemblies[i].FieldType + " " + FieldsWithTypesFromOtherAssemblies[i].DeclaringType + "." + FieldsWithTypesFromOtherAssemblies[i].Name + ", ");
				}
				sb.Remove(sb.Length-2, 2);
				sb.Append(".");
			}
			return sb.ToString();
		}
	}


    class DoNotCloneAttribute : Attribute
    {
    }

}
