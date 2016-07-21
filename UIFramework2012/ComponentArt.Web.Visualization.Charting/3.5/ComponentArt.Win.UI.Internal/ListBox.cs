using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Data;
using System.Windows.Forms;

namespace ComponentArt.Win.UI.Internal
{
	/// <summary>
	/// Summary description for MyListBox.
	/// </summary>
	internal class ThumbnailListBox : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.HScrollBar hScrollBar1;
		private int bmpWidth=1,bmpHeight=1/*, nBitmaps*/;
		private int nx = 0;
		private int selected=-1, highlighted=-1;
		private int itemHeight, itemWidth;

		ArrayList items = new ArrayList();

		private int topMargin=5, sideMargin = 8, bottomMargin=15;
		private System.Windows.Forms.ImageList m_chartsImageList;
		private System.ComponentModel.IContainer components;

		public ThumbnailListBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// 
			// m_chartsImageList
			// 
			this.m_chartsImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.m_chartsImageList.ImageSize = new System.Drawing.Size(100, 72);
			this.m_chartsImageList.TransparentColor = System.Drawing.Color.Transparent;


			bmpWidth = m_chartsImageList.ImageSize.Width;
			bmpHeight = m_chartsImageList.ImageSize.Height;
			itemWidth = bmpWidth+2*sideMargin;
			itemHeight = bmpHeight + topMargin + bottomMargin;

			nx = Width/itemWidth;
			hScrollBar1.LargeChange = nx;
			hScrollBar1.BackColor = Color.White;
		}

		public ImageList ImageList 
		{
			get 
			{
				return m_chartsImageList;
			}
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
			this.components = new System.ComponentModel.Container();
			this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
			this.m_chartsImageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// hScrollBar1
			// 
			this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.hScrollBar1.Location = new System.Drawing.Point(0, 184);
			this.hScrollBar1.Name = "hScrollBar1";
			this.hScrollBar1.Size = new System.Drawing.Size(320, 16);
			this.hScrollBar1.TabIndex = 0;
			this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
			// 
			// m_chartsImageList
			// 
			this.m_chartsImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.m_chartsImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.m_chartsImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// ListBox
			// 
			this.BackColor = System.Drawing.Color.White;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.hScrollBar1});
			this.Name = "ListBox";
			this.Size = new System.Drawing.Size(320, 200);
			this.ResumeLayout(false);

		}
		#endregion


		protected override void OnPaintBackground(PaintEventArgs pe)
		{
			hScrollBar1.Maximum = (Items.Count+1)/2-1;

			Region region = new Region(new Rectangle(0,0,Width,Height));
			
			for(int j=0; j<2; j++)
				for(int i=0;i<nx;i++)
				{
					region.Exclude(new Rectangle(i*itemWidth+sideMargin,j*itemHeight+topMargin,bmpWidth,bmpHeight));
				}
			pe.Graphics.IntersectClip(region);
			pe.Graphics.Clear(BackColor);
		}

		protected override void OnPaint ( System.Windows.Forms.PaintEventArgs e )
		{
			base.OnPaint(e);

			for(int j=0; j<2;j++)
				for(int i=0;i<nx;i++)
				{
					int item = ((hScrollBar1.Value == -1 ? 0 : hScrollBar1.Value) + i)*2 + j;
					PaintItem(e.Graphics,item,i,j);
				}
		}	

		public ArrayList Items 
		{
			get 
			{
				return items;
			}
		}

		public ScrollBar ScrollBar 
		{
			get {return hScrollBar1;}
		}

		private void PaintHint(Graphics g)
		{
			
			int y = 2*itemHeight+2;
			RectangleF rec = new RectangleF(2,y,Width-4,Height-hScrollBar1.Height-y);
			if(highlighted < 0 )
			{
				g.FillRectangle(new SolidBrush(BackColor),rec);
				return;
			}
			Font font = new Font("Verdana",7);
			g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			g.FillRectangle(new SolidBrush(Color.FromArgb(255,255,128)),rec);
		}

		private void PaintFrame(Graphics g)
		{
			Pen pen = new Pen(Color.FromArgb(180,180,192),1);
			g.DrawRectangle(pen,0,0,Width-1,Height-1);
		}

		private void PaintItem(Graphics g,int index, int ix, int iy)
		{
			int x = ix*itemWidth;
			int y = iy*itemHeight;
			if(index>=Items.Count || index < 0)
			{
				g.FillRectangle(new SolidBrush(BackColor),x+1,y+1,itemWidth-2,itemHeight-2);
				return;
			}
			Color color;
			if(index==selected)
				color = Color.FromArgb(221, 52, 9);
			else if(index==highlighted)
				color = Color.FromArgb(102, 102, 102);
			else
				color = Color.FromArgb(170, 170, 170);

			Pen pen;
			if(index==selected || index==highlighted)
				pen = new Pen(color,1);
			else
				pen = new Pen(Color.FromArgb(221, 221, 221),1);

			Font font = new Font("Verdana",7);

			g.FillRectangle(new SolidBrush(BackColor),x+4,y+4+bmpHeight,itemWidth-4,bottomMargin);
			g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			ListBoxThumbnail ctt = (ListBoxThumbnail)Items[index];
			g.DrawImage(ImageList.Images[ctt.ImageIndex],x+sideMargin,y+topMargin,bmpWidth,bmpHeight);

			g.DrawRectangle(pen,x+sideMargin,y+topMargin,bmpWidth,bmpHeight);

			Font labelFont = new Font("Verdana", 10, (SelectedIndex == index)?FontStyle.Underline:FontStyle.Regular, GraphicsUnit.Pixel);			
			g.DrawString(ctt.ShortCaption, labelFont, new SolidBrush(color),x+7,y+bmpHeight+5);
			labelFont.Dispose();
		}

		protected override void OnMouseMove ( System.Windows.Forms.MouseEventArgs e )
		{
			base.OnMouseMove(e);

            int item = GetItem(e.X, e.Y);

			if (item != highlighted)
			{
				Invalidate(highlighted);
				if (item >= Items.Count)
					item = -1;
				highlighted = item;
				OnHighlightedIndexChanged(EventArgs.Empty);
				Invalidate(highlighted);
				InvalidateHint();
			}
		}

		private void hScrollBar1_ValueChanged(object sender, System.EventArgs e)
		{
			Invalidate();
		}
		
		protected override void OnResize(System.EventArgs e) 
		{
			base.OnResize(e);
			if (itemWidth != 0) 
			{
				nx = Width/itemWidth;
				hScrollBar1.LargeChange = nx;
			}
			Invalidate();
		}

        int GetItem(int x, int y)
        {
            int xm = x / itemWidth;
            int ym = y / itemHeight;
            int item = (hScrollBar1.Value != -1 ? hScrollBar1.Value * 2 : 0) + xm * 2 + ym;
            if (xm >= nx || ym > 1 || ym < 0 || xm < 0)
                item = -1;
            return item;
        }

		protected override void OnMouseDown ( System.Windows.Forms.MouseEventArgs e )
		{
			base.OnMouseDown(e);
            
            int item = GetItem(e.X, e.Y);

			if (item != selected && item < Items.Count)
			{
				Invalidate(selected);
				SelectedIndex = item;
				Invalidate(selected);
			}
		}

		protected override void OnMouseLeave(EventArgs e) 
		{
			base.OnMouseLeave(e);
			Invalidate(highlighted);
			highlighted = -1;
			Invalidate(highlighted);
		}
	
		private void InvalidateHint()
		{
			int y = 2*itemHeight+2;
			Invalidate(new Rectangle(2,y,Width-4,Height-hScrollBar1.Height-y));
		}
		private void Invalidate(int item)
		{
			if(item<0)
				return;

			int ix = item/2-(hScrollBar1.Value == -1 ? 0 : hScrollBar1.Value);
			int iy = (item%2 == 0)? 0 : 1 ;

			Invalidate(new Rectangle(ix*itemWidth,iy*itemHeight,itemWidth,itemHeight));
		}

		#region --- Get rid of these ---
		public bool Refreshing 
		{
			get {return false;}
		}

		public System.Windows.Forms.DrawMode DrawMode 
		{
			get {return System.Windows.Forms.DrawMode.OwnerDrawFixed;}
			set {}
		}

		public bool MultiColumn 
		{
			get {return true;}
			set {}
		}
		public int ItemHeight 
		{
			get {return 90;}
			set {}
		}
		public bool IntegralHeight 
		{
			get {return true;}
			set {}
		}

		#endregion

		private void MoveScrollbarToDisplaySelectedItem()
		{
			int minValue = (SelectedIndex / (Height/itemHeight)) - ((Width / itemWidth) - 1);
			int maxValue = (SelectedIndex / (Height/itemHeight));
			if (hScrollBar1.Value < minValue)
				hScrollBar1.Value = minValue;
			if (hScrollBar1.Value > maxValue)
				hScrollBar1.Value = maxValue;
		}

		public int SelectedIndex 
		{
			get 
			{
				return selected;
			}
			set 
			{
				if (selected == value)
					return;
				selected = value;
				OnSelectedIndexChanged(EventArgs.Empty);
				MoveScrollbarToDisplaySelectedItem();
				Invalidate();
			}
		}

		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			if (SelectedIndexChanged != null) 
				SelectedIndexChanged(this, EventArgs.Empty);
			
		}
		public event EventHandler SelectedIndexChanged;

		public object SelectedItem 
		{
			get {return SelectedIndex == -1 ? null : Items[SelectedIndex];}
			set {SelectedIndex = Items.IndexOf(value);}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Right)
			{
				if (SelectedIndex < Items.Count-1)
					SelectedIndex ++;
				return true;
			}
			else if (keyData == Keys.Left)
			{
				if (SelectedIndex > 0)
					SelectedIndex --;
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);;
		}

		public int HighlightedIndex 
		{
			get 
			{
				return highlighted;
			}
		}

		protected virtual void OnHighlightedIndexChanged(EventArgs e)
		{
			if (HighlightedIndexChanged != null) 
				HighlightedIndexChanged(this, EventArgs.Empty);
			
		}
		public event EventHandler HighlightedIndexChanged;


	}

	internal class ListBoxThumbnail 
	{
		string m_caption = "";
		string m_shortCaption = "";
		int m_imageIndex = 0;
		object m_item;
		
		public ListBoxThumbnail() {}
		public ListBoxThumbnail(string caption, int imageIndex, object item) 
		{
			m_shortCaption = m_caption = caption;
			m_imageIndex = imageIndex;
			m_item = item;
		}

		public ListBoxThumbnail(string caption, string shortCaption, int imageIndex, object item) 
		{
			m_shortCaption = shortCaption;
			m_caption = caption;
			m_imageIndex = imageIndex;
			m_item = item;
		}

		public string Caption 
		{
			get {return m_caption;}
			set {m_caption = value;}
		}

		public string ShortCaption 
		{
			get {return m_shortCaption;}
			set {m_shortCaption = value;}
		}

		public int ImageIndex 
		{
			get {return m_imageIndex;}
			set {m_imageIndex = value;}
		}

		public object Item 
		{
			get {return m_item;}
			set {m_item = value;}
		}

	}
}
