using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComponentArt.WinUI
{
#if DEBUG
	public
#else
	internal
#endif
	class ComboBox : System.Windows.Forms.ComboBox 
	{
		Color m_backColor = Color.White;
		Color m_foreColor = Color.Black;


		public ComboBox() 
		{
			base.DrawMode = DrawMode.OwnerDrawFixed;
		}


		protected override void OnDrawItem ( System.Windows.Forms.DrawItemEventArgs e ) 
		{
			base.OnDrawItem(e);

			if (e.Index == -1)
				return;

			m_backColor = Color.White;
			m_foreColor = Color.Black;
			
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				m_backColor = Color.FromArgb(221, 52, 9);
				m_foreColor = Color.White;
			}

			e.Graphics.FillRectangle(new SolidBrush(m_backColor), new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

			OnDrawItemText(e);
			e.DrawFocusRectangle();
		}


		protected virtual void OnDrawItemText ( System.Windows.Forms.DrawItemEventArgs e ) 
		{
			if (e.Index >= Items.Count)
				return;
			
			string txt = GetItemText(Items[e.Index]);
			SizeF s = e.Graphics.MeasureString(txt, Font);

			e.Graphics.DrawString(txt, Font, new SolidBrush(m_foreColor), e.Bounds.X, e.Bounds.Y + (int)((e.Bounds.Height - s.Height)/2.0 + 0.5) );
		}
	}
}
