
namespace ComponentArt.Web.Visualization.Charting
{
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.All)]
    public sealed class SRCategoryAttribute : CategoryAttribute
    {
        // Methods
        public SRCategoryAttribute(string category) : base(category)
        {
        }

        protected override string GetLocalizedString(string value)
        {
            return ChartBase.ResourceMngr.GetString(value);
        }

    }
}

