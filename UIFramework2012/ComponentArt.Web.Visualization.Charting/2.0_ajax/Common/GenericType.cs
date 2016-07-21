using System;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ComponentArt.Web.Visualization.Charting
{
	/// <summary>
	/// Wrapper class for double, DateTime and string objects.
	/// </summary>
	/// <remarks>
	///   <para>
	///     The following properties are of type <see cref="GenericType"/>:
	///     <list type="bullet">
	///       <item><see cref="Axis.Minimum"/>, 
	///       <item><see cref="Axis.Maximum"/>,
	///       <item><see cref="SeriesBase.Reference"/>, and
	///       <item><see cref="IChart.Reference"/>.
	///     </list>
	///   </para>
	///   <para>
	///   This type is used in places where otherwise an <see cref="object"/> type would have been used.
	///   The advantage of the <see cref="GenericType"/> is that it makes setting the property
    ///   values at design time possible (using VS property view).
	///   </para>
	///   <para>
	///   The property <see cref="GenericType.CurrentType"/> can be used to query the basic type of the 
	///   encapsulated value.
	///   </para>
	///   <para>
	///   A <see cref="GenericType"/> object can be assigned double, DateTime or string value. 
	///   It also has implicit operators that convert it to these three basic types. For example:
	///   <code>
	///     GenericType g = 123.45;
	///     double d = g;
	///   </code>
	///   is valid code. When a string is assigned to a <see cref="GenericType"/>, an attempt is made to
	///   parse the string as a double and DateTime and if the parsing is successful, the <see cref="CurrentType"/> 
	///   is set to double or DateTime. If such a parsing isn't successful, the <see cref="CurrentType"/> is set to string.
	///   Therefore, assigning <c>123.45</c> to a <see cref="GenericType"/> is the same as assigning <c>"123.45"</c>.
	///   </para>
	///   <para>
	///   When converting to basic types an exception is thrown if the <see cref="CurrentType"/> is not the same
	///   as the destination type. For example, the following code throws exception:
	///   <code>
	///     GenericType g = 123.45;
	///     DateTime d = g;
	///   </code>
	///   </para>
	/// </remarks>
    [TypeConverter(typeof(GenericTypeConverter))]
    public class GenericType
    {
        private object val = null;
		private PropertyInfo pi = null;
		internal event EventHandler ValueSet;

        #region --- Constructors ---

        public GenericType()
        {
            val = null;
        }

		internal GenericType(PropertyInfo pi, object target)
		{
			this.pi = pi;
			val = target;
		}

        public GenericType(double dbl)
        {
            val = dbl;
        }

        public GenericType(DateTime dt)
        {
            val = dt;
        }

        public GenericType(string str)
        {
            if (str == "")
                val = null;
            else
                Value = str;
        }
        #endregion

        #region --- Cast Operators ---

        #region --- Casting to GenericType

        public static implicit operator GenericType(double d)
        {
            return new GenericType(d);
        }

        public static implicit operator GenericType(DateTime d)
        {
            return new GenericType(d);
        }

        public static implicit operator GenericType(string d)
        {
            return new GenericType(d);
        }
        #endregion

        #region --- Casting from GenericType
        public static implicit operator Double(GenericType mtv)
        {
            if (mtv.IsNull)
                throw new Exception("Cannot convert null to double");
            if (mtv.val is Double)
                return (double)mtv.val;
            throw new Exception("Cannot convert a '" + mtv.val.GetType().Name + "' to double");
        }

        public static implicit operator DateTime(GenericType mtv)
        {
            if (mtv.IsNull)
                throw new Exception("Cannot convert null to DateTime");
            if (mtv.val is DateTime)
                return (DateTime)mtv.val;
            throw new Exception("Cannot convert a '" + mtv.val.GetType().Name + "' to a DateTime");
        }

        public static implicit operator String(GenericType mtv)
        {
            if (mtv.IsNull)
                throw new Exception("Cannot convert null to string");
            if (mtv.val is String)
                return (string)mtv.val;
            return (string)TypeDescriptor.GetConverter(mtv).ConvertToString(mtv);
        }
        #endregion
        #endregion

        [Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsNull { get { return val == null; } }
        [Browsable(false)]
		[DefaultValue("")]
        public string Value
        {
            get
            {
                if (val == null)
                    return "";
                return TypeDescriptor.GetConverter(this).ConvertToString(this);
            }
			set
			{
				GenericType mtv = (GenericType)TypeDescriptor.GetConverter(this).ConvertFromString(value);
				if(pi != null)
					pi.SetValue(val,mtv.InternalValue,null);
				else
					val = mtv.val;
				if(ValueSet != null)
					ValueSet(this,new EventArgs());
			}
        }

        internal object InternalValue 
		{ 
			get 
			{ 
				if(pi == null)
					return val; 
				else
					return pi.GetValue(val,null);
			}
			set
			{
				if(pi == null)
					val = value;
				else
					pi.SetValue(val,value,null);
			}
		}
        [Description("Curent type of the base value")]
        public Type CurrentType 
        { 
            get 
            {
				object v;
				if(pi == null)
					v = val;
				else
					v = pi.GetValue(val,null);

				if (v == null)
                    return null;
                if (v is Double)
                    return typeof(double);
                if (v is DateTime)
                    return typeof(DateTime);
                return typeof(string);
            }
        }

        public override string ToString()
        {
            return Value;
        }


    }

    class GenericTypeConverter : System.ComponentModel.ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) ||
                sourceType == typeof(DateTime) ||
                TypeDescriptor.GetConverter(typeof(double)).CanConvertFrom(sourceType) ||
                base.CanConvertFrom(context, sourceType);
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) ||
                 base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is DateTime)
                return new GenericType((DateTime)value);
            if (value is string)
            {
				string strValue = (string)value;
				try
				{
					return new GenericType(double.Parse(strValue));
				}
				catch 
				{ }
				try
				{
                    return new GenericType(DateTime.Parse(strValue));
				}
				catch 
				{ }
                if (strValue.StartsWith("\"") && strValue.EndsWith("\""))
                    strValue = strValue.Substring(1, strValue.Length - 2);
				GenericType gVal = new GenericType();
				if(strValue != "")
					gVal.InternalValue = strValue;
				return gVal;
            }
            TypeConverter numConverter = TypeDescriptor.GetConverter(typeof(double));
            if (numConverter.CanConvertFrom(value.GetType()))
                return new GenericType((double)numConverter.ConvertFrom(context, culture, value));
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value is GenericType && destinationType == typeof(string))
            {
                GenericType mtv = value as GenericType;
                if (mtv.InternalValue == null)
                    return "";
                if (mtv.CurrentType == typeof(string))
                    return "\"" + (string)(mtv.InternalValue) + "\"";
                return TypeDescriptor.GetConverter(mtv.InternalValue).ConvertToString(context,culture,mtv.InternalValue);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
