using System;
using System.Web.UI;
using System.ComponentModel;


namespace ComponentArt.Web.UI
{
  /// <summary>
  /// Collection of <see cref="SplitterPaneContent"/> controls.
  /// </summary>
  [Editor("System.Windows.Forms.Design.CollectionEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
  public class SplitterPaneContentCollection : ControlCollection
  {
    // Needed by Intellisense in framework 2, but causes a runtime error in framework 1
    public new SplitterPaneContent this[int index]
    {
      get
      {
        return (SplitterPaneContent)base[index];
      }
    }

    /// <summary>
    /// Initializes a new instance of a SplitterPaneContentCollection. 
    /// </summary>
    /// <param name="owner">The parent Splitter control.</param>
    public SplitterPaneContentCollection(Splitter owner) : base(owner)
    {
    }

    /// <summary>
    /// Verifies that a child control is a SplitterPaneContent.
    /// If it is, then certain properties are set.
    /// If it is not, then an exception is thrown.
    /// </summary>
    /// <param name="child">The child control.</param>
    private void VerifyChild(Control child)
    {
      if (child is SplitterPaneContent)
      {
        //((SplitterPaneContent)child).ParentSplitter = (Splitter)Owner;
        return;
      }

      throw new Exception("Only SplitterPaneContent controls can be placed in Content.");
    }

    /// <summary>
    /// Adds a control to the collection.
    /// </summary>
    /// <param name="child">The child control.</param>
    public override void Add(Control child)
    {
      VerifyChild(child);
      base.Add(child);
    }

    /// <summary>
    /// Adds a control to the collection at a specific index.
    /// </summary>
    /// <param name="index">The index where the control should be added.</param>
    /// <param name="child">The child control.</param>
    public override void AddAt(int index, Control child)
    {
      VerifyChild(child);
      base.AddAt(index, child);
    }
  }
}
