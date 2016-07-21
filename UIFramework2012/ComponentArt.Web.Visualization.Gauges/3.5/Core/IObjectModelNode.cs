using System;

namespace ComponentArt.Web.Visualization.Gauges
{
	/// <summary>
	/// Summary description for IObjectModelNode.
	/// </summary>
	interface IObjectModelNode
	{
		IObjectModelNode ParentNode { get; set; }
	}
}
