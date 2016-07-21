namespace ComponentArt.Web.Visualization.Charting
{
    using System;
    using System.ComponentModel;

    [AttributeUsage(AttributeTargets.All)]
    public sealed class SRDescriptionAttribute : DescriptionAttribute
    {
        public SRDescriptionAttribute(string description) : base(description)
        {
        }

        public override string Description
        {
            get
            {
                return ChartBase.ResourceMngr.GetString(base.Description);
            }
        }
    }
}

