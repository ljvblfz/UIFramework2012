using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

using System.Reflection;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class NamedCollectionEditor : CollectionWithTypeEditor
	{
		protected string m_newName = "";
		ArrayList m_removedNames = new ArrayList();
		public NamedCollectionEditor(Type type): base(type) {}


		protected override void DestroyInstance(object instance) 
		{
			m_removedNames.Add(((INamedObject)instance).Name);
			base.DestroyInstance(instance);
		}

		protected override bool CanRemoveInstance(object value) 
		{
			bool b = base.CanRemoveInstance(value);
			if (b)
				m_removedNames.Add(((INamedObject)value).Name);
			return b;
		}

		protected override object CreateInstance(Type itemType) 
		{
			// We know that the collection is NamedCollection
			NamedCollection namedColl = m_coll as NamedCollection;

			if (m_objectToCopy != null)
				itemType = m_objectToCopy.GetType();

			// Generate unique name
			m_newName = namedColl.NextAvailableName(itemType.Name);

			foreach (object o in m_removedNames) 
			{
				if (((string)o) == m_newName) 
				{
					m_removedNames.Remove(o);
					break;
				}
			}

			object newObj = BuildInstance(itemType);
			return newObj;
		}

		protected virtual object BuildInstance(Type itemType) 
		{
			if (m_objectToCopy != null) 
			{
				INamedObject NamedStyleObject = (INamedObject)CopiedInstance();
		
				NamedStyleObject.Name = m_newName;
				m_objectToCopy = null;
                return NamedStyleObject;
			} 
			else 
			{
				System.Reflection.ConstructorInfo ci = 
					itemType.GetConstructor(new Type[] {typeof(string)});

				INamedObject newobj = (INamedObject)ci.Invoke(new object[] {m_newName});
				newobj.OwningCollection = (NamedCollection)m_coll;
				SetContext(newobj, m_coll.Owner);
				return newobj;
			}
		}
	}
}
