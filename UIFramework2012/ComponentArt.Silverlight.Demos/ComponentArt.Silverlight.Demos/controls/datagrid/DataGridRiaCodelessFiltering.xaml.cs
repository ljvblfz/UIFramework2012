using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ComponentArt.Silverlight.UI.Data;


namespace ComponentArt.Silverlight.Demos
{
    public partial class DataGridRiaCodelessFiltering : UserControl
    {
        ComponentArt.Silverlight.Demos.Web.PostsDomainContext _postsDomainContext = new ComponentArt.Silverlight.Demos.Web.PostsDomainContext();

        public DataGridRiaCodelessFiltering()
        {
            InitializeComponent();
        }

        // We react to a filter change on the DataGrid, and translate its new filter specification for the DomainDataSource.
        private void DataGrid1_FilterChanged(object sender, DataGridFilterChangedEventArgs e)
        {
            using (PostsDomainDataSource.DeferLoad())
            {
                PostsDomainDataSource.FilterDescriptors.Clear();

                FilterDescriptorCollection fdc = TranslateFilterDescriptors(e.Filters);
                foreach (FilterDescriptor fd in fdc)
                {
                    PostsDomainDataSource.FilterDescriptors.Add(fd);
                }

            }
        }

        private FilterOperator TranslateFilterOperator(DataGridDataConditionOperand o)
        {
            switch (o)
            {
                case DataGridDataConditionOperand.Equals:
                    return FilterOperator.IsEqualTo;
                case DataGridDataConditionOperand.Contains:
                    return FilterOperator.Contains;
                case DataGridDataConditionOperand.GreaterThan:
                    return FilterOperator.IsGreaterThan;
                case DataGridDataConditionOperand.GreaterThanOrEqual:
                    return FilterOperator.IsGreaterThanOrEqualTo;
                case DataGridDataConditionOperand.LessThan:
                    return FilterOperator.IsLessThan;
                case DataGridDataConditionOperand.LessThanOrEqual:
                    return FilterOperator.IsLessThanOrEqualTo;
                case DataGridDataConditionOperand.NotEqualTo:
                    return FilterOperator.IsNotEqualTo;
                case DataGridDataConditionOperand.StartsWith:
                    return FilterOperator.StartsWith;
                case DataGridDataConditionOperand.In:
                    return FilterOperator.IsContainedIn;
                default:
                    return FilterOperator.IsEqualTo;
            }
        }

        private FilterDescriptorCollection TranslateFilterDescriptors(DataGridDataConditionCollection conditions)
        {
            FilterDescriptorCollection filters = new FilterDescriptorCollection() { };
            foreach (ComponentArt.Silverlight.UI.Data.DataGridDataCondition c in conditions)
            {
                FilterDescriptor f = new FilterDescriptor();
                f.PropertyPath = c.DataFieldName;
                f.Value = c.DataFieldValue;
                f.Operator = TranslateFilterOperator(c.Operand);
                filters.Add(f);
            }

            return filters;
        }
    }
}
