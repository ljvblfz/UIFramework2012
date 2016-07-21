using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using ComponentArt.Web.Visualization.Charting.Design;
using ComponentArt.Web.Visualization.Charting.Geometry;

namespace ComponentArt.Web.Visualization.Charting.Geometry
{
    internal class ChartText : GeometricObject
    {
        private LabelStyle labelStyleRef = null;
        private string labelStyle = null;
        private Vector3D P;
        private string text;

		public ChartText() { }

        public ChartText(string labelStyle, Vector3D P, string text)
        {
            this.labelStyle = (labelStyle == "" || labelStyle == null) ? "Default" : labelStyle;
            this.P = P;
            this.text = text;
        }

        public ChartText(LabelStyle labelStyleRef, Vector3D P, string text)
        {
            this.labelStyleRef = labelStyleRef;
            if (labelStyleRef == null)
                labelStyle = "Default";
            this.P = P;
            this.text = text;
        }

		internal override double OrderingZ()
		{
			if (labelStyleRef != null)
			{
				Vector3D PP = new Vector3D(P), VxTxt, VyTxt;
				labelStyleRef.GetWCSTextRectangle(this.Mapping, text, ref PP, out VxTxt, out VyTxt);
				return Mapping.Map(PP + VxTxt/2+VyTxt/2).Z + 0.1;
			}
			else
				return Mapping.Map(P).Z + 0.1;
		}


        internal override TargetCoordinateRange CoordinateRange(bool usingTexts)
        {
            TargetCoordinateRange tcr = new TargetCoordinateRange();
			if(!usingTexts)
				return tcr;
            if (labelStyleRef != null)
            {
                Vector3D PP = new Vector3D(P), VxTxt, VyTxt;
                labelStyleRef.GetWCSTextRectangle(this.Mapping, text, ref PP, out VxTxt, out VyTxt);
				tcr.Include(Mapping.Map(PP));
				tcr.Include(Mapping.Map(PP + VxTxt));
				tcr.Include(Mapping.Map(PP + VyTxt));
				tcr.Include(Mapping.Map(PP + VxTxt + VyTxt));
			}
            return tcr;
        }

        #region --- Properties ---

        public string Text { get { return text; } set { text = value; } }
        public Vector3D Location { get { return P; } set { P = value; } }
        public string LabelStyleName { get { return labelStyle; } set { labelStyle = value; } }
        internal LabelStyle LabelStyleRef { get { return labelStyleRef; } }

        #endregion
    }
}
