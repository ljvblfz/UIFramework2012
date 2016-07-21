using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class ChartColorConverter : ExpandableObjectConverter 
	{
		public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
		{
			if (destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor)) 
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}
		
		public override object ConvertTo ( ITypeDescriptorContext context , System.Globalization.CultureInfo culture , object value , Type destinationType ) 
		{
			if (destinationType == typeof(string))
			{
				string res = value.GetType().ToString();

				if (value.GetType() == typeof(ChartColor)) 
				{
					ChartColor scs = ((ChartColor)value);
					res = res + "," 
						+ scs.Reflection.ToString() + "," 
						+ scs.LogPhong.ToString() + "," 
						+ scs.Color.Name;
				} 
				else
				{
					throw new NotSupportedException(value.GetType().ToString() 
						+ " not suported in " + this.GetType().ToString());
				}
 

				return res;
			}
			if (destinationType == typeof(InstanceDescriptor))
			{
				if (value.GetType() == typeof(ChartColor)) 
				{
					ChartColor s = (ChartColor)value;

					System.Reflection.ConstructorInfo ci = 
						typeof(ChartColor).GetConstructor(new Type[3] {typeof(float), typeof(int) , typeof(Color)});
					return new InstanceDescriptor(ci, new object[] {s.Reflection, s.LogPhong, s.Color});
				} 
				else
				{
					throw new NotSupportedException(value.GetType().ToString() 
						+ " not suported in " + this.GetType().ToString());
				}

			}
			return base.ConvertTo(context, culture, value, destinationType);
		}


		public override bool CanConvertFrom(
			ITypeDescriptorContext context,
			Type sourceType
			) 
		{

			if (sourceType == typeof(string)) 
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}


		public override object ConvertFrom(
			ITypeDescriptorContext context,
			CultureInfo culture,
			object value
			) 
		{
			if (value is string) 
			{
				string toParse = (string)value;
				string [] tokens = toParse.Split(new char [] {','});
				if (tokens.Length == 0) 
				{
					throw new ArgumentException("String " + toParse + " cannot be parsed.");
				}

				if (tokens[0].EndsWith("ChartColor"))
				{

					float f = float.Parse(tokens[1]);
					int i = int.Parse(tokens[2]);
					Color c = ColorArrayConverter.ColorFromString(tokens[3]);
					return new ChartColor(f,i,c);
				} 
				else 
				{
					throw new ArgumentException("Cannot convert " + tokens[0] + " to type in " + this.GetType().ToString());
				}
			}

			return base.ConvertFrom(context,culture,value);
		}

	}

	internal class ChartColorEditor : System.Drawing.Design.UITypeEditor
	{        
		public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return true;
		}
		
		public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(((ChartColor)e.Value).Color), e.Bounds);
		}
	}
}

