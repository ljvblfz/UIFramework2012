using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ComponentArt.Web.Visualization.Gauges
{
	internal sealed class ValueRangeAttribute : Attribute
	{
		private double minValue;
		private double maxValue;
		private double step;

		public ValueRangeAttribute(double minValue, double maxValue, double step)
		{
			this.minValue = minValue;
			this.maxValue = maxValue;
			this.step = step;
		}

		public ValueRangeAttribute(double minValue, double maxValue)
		{
			this.minValue = minValue;
			this.maxValue = maxValue;
			// Choose step to get resolution of 1/200 of the range, or a bit better
			if(minValue != maxValue)
			{
				double step1 = Math.Abs(maxValue-minValue)/200;
				double log10 = Math.Floor(Math.Log10(step1));
				step = Math.Pow(log10,10.0);
			}

		}

		public double MinValue { get { return minValue; } }
		public double MaxValue { get { return maxValue; } }
		public double Step { get { return step; } }
	}

	internal delegate void SliderValueChangedHandler(object sender, SliderValue sliderValue);
	internal delegate void SliderSessionDoneHandler(object sender);

	
	/// <summary>
	/// Summary description for SliderControl.
	/// </summary>
	
	internal class SliderControl : System.Windows.Forms.UserControl
	{
		public event SliderValueChangedHandler ValueChanged;
		public event SliderSessionDoneHandler SessionDone;

		private SliderValue sliderValue = new SliderValue(0,100,30,0.5);

		int x0;
		int x1;
		int y1;
		int y0;

		private bool firstDisplay = true;
		int r = 7; // Radius of the circle
		int marg = 5;
		int h = 10; // Height of the strip

		private System.Windows.Forms.TextBox textBox;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SliderControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}

		internal SliderValue SliderValue
		{ 
			get { return new SliderValue(sliderValue.MinValue,sliderValue.MaxValue,sliderValue.Value,sliderValue.Step); } 
			set { sliderValue = value; UpdateTextBox(); } 
		}

		private void UpdateTextBox()
		{
			// Value
			textBox.Text = sliderValue.Value.ToString();
			// Position
			Size sz = textBox.Size;
			int x = ToClient((float)(sliderValue.Value));
			int xt = x - sz.Width/2;
			xt = Math.Max(0,Math.Min(xt,Size.Width-sz.Width));
			textBox.Location = new Point(xt,(y1+y0)/2-r-1-sz.Height);
//			g.DrawString(valString,font,Brushes.DarkBlue,xt,y0-1.3f*sz.Height);
		}

		private int ToClient(float x)
		{
			return x0 + (int)Math.Round((x1-x0)*(x-sliderValue.MinValue)/(sliderValue.MaxValue-sliderValue.MinValue));
		}


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.BackColor = System.Drawing.Color.White;
			this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox.ForeColor = System.Drawing.SystemColors.Highlight;
			this.textBox.Location = new System.Drawing.Point(48, 8);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(48, 13);
			this.textBox.TabIndex = 0;
			this.textBox.Text = "0.000";
			this.textBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// SliderControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.textBox});
			this.Name = "SliderControl";
			this.Size = new System.Drawing.Size(150, 32);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SliderControl_MouseUp);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.SliderControl_Paint);
			this.Leave += new System.EventHandler(this.SliderControl_Leave);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SliderControl_MouseMove);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SliderControl_MouseDown);
			this.ResumeLayout(false);

		}
		#endregion

		private void SliderControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			x0 = marg;
			x1 = ClientRectangle.Width - marg;
			y1 = ClientRectangle.Height - marg;
			y0 = y1 - h;

			double a = (sliderValue.Value-sliderValue.MinValue)/(sliderValue.MaxValue-sliderValue.MinValue);
			int x = x0 + (int)(a*(x1-x0));

			if(firstDisplay)
			{
				firstDisplay = false;
				UpdateTextBox();
			}

			// Create working graphics to avoid flicker
			Bitmap bmp = new Bitmap(ClientRectangle.Width,ClientRectangle.Height);
			Graphics g = Graphics.FromImage(bmp);
			g.Clear(Color.White);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

			// Scale
			g.FillRectangle(Brushes.LightBlue,x0,y0,x-x0,y1-y0);
			g.DrawRectangle(Pens.SteelBlue,x0,y0,x1-x0,y1-y0);

			// Marker
			g.FillEllipse(Brushes.LightGray,x-r,(y0+y1)/2-r,2*r,2*r);
			g.DrawEllipse(Pens.White,x-r+1,(y0+y1)/2-r+1,2*r-2,2*r-2);
			g.DrawEllipse(Pens.DarkGray,x-r,(y0+y1)/2-r,2*r,2*r);
			g.DrawLine(Pens.DarkGray,x,y0,x,y1);

			// Value
//			string valString = sliderValue.Value.ToString();
//			Font font = new Font("Arial",8f);
//			SizeF sz = g.MeasureString(valString,font);
//			float xt = x - (float)a*sz.Width;
//			g.DrawString(valString,font,Brushes.DarkBlue,xt,y0-1.3f*sz.Height);
//			font.Dispose();
			
			// Draw
			e.Graphics.DrawImage(bmp,0,0);

			g.Dispose();
			bmp.Dispose();
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// Do nothing to avoid flicker.
		}

		#region --- Handling mouse events ---

		private void SliderControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button != MouseButtons.Left)
				return;
			SetSliderValue(e.X,e.Y);
			this.Cursor = Cursors.SizeWE;
		}

		private void SetSliderValue(int x, int y)
		{
			if(x<=x0)
			{
				x = x0;
				sliderValue.Value = sliderValue.MinValue;
			}
			else if(x>=x1)
			{
				x = x1;
				sliderValue.Value = sliderValue.MaxValue;
			}
			else
			{
				sliderValue.Value = sliderValue.MinValue + (x-x0)*(sliderValue.MaxValue - sliderValue.MinValue)/(x1-x0);
				sliderValue.Value = sliderValue.AdjustedValue;
			}

			UpdateTextBox();
			Refresh();
			if(ValueChanged != null)
				ValueChanged(this,sliderValue);
			//Invalidate();
		}

		private void SliderControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.Cursor = Cursors.Default;		
			if(SessionDone != null)
				SessionDone(this);
		}

		private void SliderControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button != MouseButtons.Left)
				return;
			SetSliderValue(e.X,e.Y);
		}
		#endregion

		private void SliderControl_Leave(object sender, System.EventArgs e)
		{
			try
			{
				double newValue = double.Parse(textBox.Text);
				if(newValue == sliderValue.Value)
					return;

				sliderValue.Value = newValue;

				UpdateTextBox();
				Refresh();// Invalidate();
				if(ValueChanged != null)
					ValueChanged(this,sliderValue);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return;
			}

		}
	}


    /// <summary>
    /// Represents a bounded value.
    /// </summary>
	[Editor(typeof(SliderEditor),typeof(UITypeEditor))]
    [TypeConverter(typeof(SliderValueTypeConverter))]
	public class SliderValue 
	{
		double minValue = 0;
		double maxValue = 100;
		double val = 30;
		double valueStep = 0;

		public SliderValue(double minValue, double maxValue, double val, double valueStep)
		{
			this.minValue = minValue;
			this.maxValue = maxValue;
			this.val = val;
			this.valueStep = valueStep;
		}

		public double Step
		{
			get
			{
				if(valueStep > 0)
					return valueStep;
				// default step 1% of the range
				return (maxValue - minValue)/100;
			}
			set { if(value > 0) valueStep = value; }
		}

		public double AdjustedValue
		{
			get
			{
				int n = (int)(val/Step + 0.5);
				return n*Step;
			}
		}
		public double MinValue { get { return minValue; } set { minValue = value; } }
		public double MaxValue { get { return maxValue; } set { maxValue = value; } }
		public double Value { get { return val; } set { val = value; } }

	}

	internal class SliderEditor : UITypeEditor
	{
		// This editor edits numeric values between given min and max using the SliderControl

		private SliderControl slider;
		private IWindowsFormsEditorService service;
		private PropertyDescriptor propertyDescriptor;
		private object control;
		private bool isSLiderValue;
        private SubGauge gauge;

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}
 
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider == null || context == null)
				return value;
			propertyDescriptor = context.PropertyDescriptor;
			control = context.Instance;
			gauge = ObjectModelBrowser.GetOwningTopmostGauge(control as IObjectModelNode);

            bool isEditing = false;
            if (gauge != null)
            {
                isEditing = gauge.Editing;
                gauge.Editing = true;
            }
			isSLiderValue = true;

			// Try as slider value
			SliderValue sValue = value as SliderValue;
			if(sValue == null)
			{
				// Try as double with ValueRangeAttribute
				isSLiderValue = false;
				if(!(value is double))
					return value;
				double dblValue = (double)value;
				ValueRangeAttribute vaa = propertyDescriptor.Attributes[typeof(ValueRangeAttribute)] as ValueRangeAttribute;
				if(vaa == null)
					return value;
				if(double.IsNaN(dblValue)) // need effective value
				{
					dblValue = (double)propertyDescriptor.GetValue(control);
				}
				sValue = new SliderValue(vaa.MinValue,vaa.MaxValue,dblValue,vaa.Step);
			}

			service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
			if (service == null)
			{
				return value;
			}

			if (slider == null)
			{
				slider = new SliderControl();
				slider.ValueChanged += new SliderValueChangedHandler(OnValueChanged);
				slider.SessionDone += new SliderSessionDoneHandler(OnSessionDone);
			}
			slider.SliderValue = sValue;

			if(sValue.Value < sValue.MinValue)
				sValue.Value = sValue.MinValue;
			if(sValue.Value > sValue.MaxValue)
				sValue.Value = sValue.MaxValue;
			service.DropDownControl(slider);
            if(gauge != null)
			    gauge.Editing = isEditing;

			if(isSLiderValue)
				return slider.SliderValue;	
			else
				return slider.SliderValue.Value;
		}
 
		void OnValueChanged(object sender, SliderValue sliderValue)
		{
			if(isSLiderValue)
				propertyDescriptor.SetValue(control,sliderValue);
			else
				propertyDescriptor.SetValue(control,sliderValue.Value);
			
			if (gauge != null)
				gauge.Invalidate();
			else if(control is Control)
			{
				Control g = control as Control;
				g.Invalidate();
			}
		}
		void OnSessionDone(object sender)
		{
			service.CloseDropDown();
			service = null;
		}
	}


    internal class SliderValueTypeConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string)
                || base.CanConvertTo(context, destinationType);
        }

        public override System.Boolean CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            return sourceType == typeof(string)
                || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            SliderValue sv = value as SliderValue;
            if (sv != null && destinationType == typeof(string))
            {
                return sv.Value.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override System.Object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, System.Object value)
        {
            string sValue = value as string;
            if (sValue != null)
            {
                double v = double.Parse(sValue);
                return new SliderValue(v, v, v, 0);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }


}
