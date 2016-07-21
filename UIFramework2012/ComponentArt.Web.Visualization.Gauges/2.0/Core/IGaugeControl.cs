using System;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Defines a common interface between the SubGauge class and any wrapper classes.
	/// </summary>
	public interface IGaugeControl
	{
        #region --- Properties ---

		#region --- Control Properties ---
		
		string Name { get; }

		#endregion

        #region --- Appearance Category ---

         Color BackColor { get; set; }

//#if __COMPILING_FOR_2_0_AND_ABOVE__
//        // NB: Here we need translations between ComponentArt.Web.Visualization.Gauges.ImageLayout and System.Windows.Forms.ImageLayout
//        System.Windows.Forms.ImageLayout BackgroundImageLayout { get; set; }
//#else
//        ImageLayout BackgroundImageLayout { get; set; }
//#endif
        #endregion

        #region --- "Gauge Active Style" Category ---

		string ThemeName { get; set; }

		ThemeKind ThemeKind { get; set; }

		GaugeKind GaugeKind { get; set; }

        Theme Theme { get; }
		
		Skin Skin { get; }

        #endregion

        #region --- "Gauge Data" Category ---

        double MinValue { get; set; }

        double MaxValue { get; set; }

        // Note: The main gauge value is handled by these two properties: 'InitialValue' and 'Val'
        // 'InitialValue' is design-time only and uses slider editor, adjusts itself to the value range and
        // actually controls the 'Val' property. 

        SliderValue InitialValue { get; set; }

        double Value { get; set; }
 
        #endregion

        #region --- Subgauges ---
        
		SubGaugeCollection SubGauges { get; }

        #endregion

        #region --- Main Gauge Objects ---

        Range MainRange { get; set; }

        Annotation MainAnnotation { get; set; }

        Pointer MainPointer { get; set; }

        Scale MainScale { get; set; }

        #endregion

        #region --- Collections ---

        ScaleCollection Scales { get; }

        RangeCollection Ranges { get; }

        PointerCollection Pointers { get; }

		AnnotationCollection Annotations { get; }

		IndicatorCollection Indicators { get; }

		TextAnnotationCollection TextAnnotations { get; }

		ImageAnnotationCollection ImageAnnotations { get; }

        #endregion

        #region --- Style Collections ---

        ThemeCollection Themes { get; }

        PointerStyleCollection PointerStyles { get; }

        TextStyleCollection TextStyles { get; }

        MarkerStyleCollection MarkerStyles { get; }

        GaugePaletteCollection Palettes { get; }

		ScaleAnnotationStyleCollection ScaleAnnotationStyles { get; }

        #endregion

		#region --- XML Serialization ---

		void XMLSerialize(string fileName);

		void XMLDeserialize(string fileName);

		void XMLSerializeThemesAndStyles(string fileName);
														
		void XMLDeserializeThemesAndStyles(string fileName);

		#endregion

		Bitmap RenderBitmap(int width, int height);

		#endregion
	}
}
