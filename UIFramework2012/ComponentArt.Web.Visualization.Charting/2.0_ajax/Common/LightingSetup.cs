using System;
using System.ComponentModel;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting
{
	public class LightingSetup : NamedObjectBase
	{
		LightCollection m_lightCollection = new LightCollection();
		
		public LightingSetup()
            : this("")
        {
        }

        public LightingSetup(string name)
            : base(name)
        {
        }

#if __BuildingWebChart__
		[System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
#endif
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public LightCollection Lights
		{
			get 
			{
				return m_lightCollection;
			}
		}

		internal bool HasChanged 
		{
			get
			{
				return m_lightCollection.HasChanged;
			}
		}

	}


	/// <summary>
	/// A collection of <see cref="LightingSetup"/> objects. This class cannot be inherited.
	/// </summary>
	public sealed class LightingSetupCollection : NamedCollection  
	{

		internal LightingSetupCollection(Object owner, bool initialize) 
			: base (typeof(LightingSetup),owner) 
		{
			if(initialize)
			InitializeContents();
		}


		private bool hasChanged = true;

		internal LightingSetupCollection(Object owner) : base(typeof(LightingSetup), owner)
		{ }

		internal LightingSetupCollection() : this(null) { }

		/// <summary>
		/// Indicates the <see cref="LightingSetup"/> at the specified indexed location in the <see cref="LightingSetupCollection"/> object. 
		/// </summary>
		/// <param name="index">Zero-based index to retrieve a <see cref="LightingSetup"/> from the <see cref="LightingSetupCollection"/> object.</param>
		public new LightingSetup this[object index]   
		{ 
			get { return ((LightingSetup)base[index]); } 
			set { base[index] = value; } 
		}

		private void InitializeContents()
		{
			LightingSetup ls;

			ls = new LightingSetup("Default");
			ls.Lights.Add(new Light(5));
			ls.Lights.Add(new Light(5,new Vector3D(12,0,-20)));
			ls.Lights.Add(new Light(5,new Vector3D(15,-15,15)));
			ls.Lights.HasChanged = false; 
			Add(ls);

			ls = new LightingSetup("Single");
			ls.Lights.Add(new Light(5));
			ls.Lights.Add(new Light(5,new Vector3D(12,0,-20)));
			ls.Lights.HasChanged = false; 
			Add(ls);

			ls = new LightingSetup("SinglePie");
			ls.Lights.Add(new Light(5));
			ls.Lights.Add(new Light(5,new Vector3D(15,-15,15)));
			ls.Lights.HasChanged = false; 
			Add(ls);
		}
	}
}

namespace ComponentArt.Web.Visualization.Charting.Design
{
    /// <summary>
    /// Lighting setup converter
    /// </summary>
	internal class SelectedLightingSetupConverter : SelectedNameConverter
	{

		protected override NamedCollection GetNamedCollection(ChartBase chart) 
		{
			return chart.LightingSetups;
		}

		public override Type GetReferencedType() {return typeof(LightingSetup);}

	}
}