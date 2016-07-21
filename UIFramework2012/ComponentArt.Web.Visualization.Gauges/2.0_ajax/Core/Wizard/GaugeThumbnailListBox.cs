using System;
using System.Collections;
using System.Drawing;

namespace ComponentArt.Web.Visualization.Gauges
{
    internal class GaugeThumbnailPlaceholder
    {
        int m_imageIndex;
        GaugeKind m_gaugeKind;
        String m_themeName;
		NumericDisplayKind m_numericDisplayKind;

        public GaugeThumbnailPlaceholder(GaugeKind gaugeKind, String themeName)
        {
            m_imageIndex = -1;
            m_gaugeKind = gaugeKind;
            m_themeName = themeName;
        }

        public int ImageIndex
        {
            get { return m_imageIndex; }
            set { m_imageIndex = value; }
        }

        public GaugeKind GaugeKind
        {
            get { return m_gaugeKind; }
            set { m_gaugeKind = value; }
        }

        public String ThemeName
        {
            get { return m_themeName; }
            set { m_themeName = value; }
        }

		public NumericDisplayKind NumericDisplayKind
		{
			get { return m_numericDisplayKind; }
			set { m_numericDisplayKind = value; }
		}

        public ListBoxThumbnail ToListBoxThumbnail()
        {
            string gaugeKind = (m_gaugeKind==GaugeKind.Numeric)?(m_gaugeKind.ToString()+m_numericDisplayKind.ToString()):m_gaugeKind.ToString();
            string shortGaugeKind = gaugeKind.Length > 16 ? gaugeKind.Substring(0, 15) + "..." : gaugeKind;
            return new ListBoxThumbnail(gaugeKind, shortGaugeKind, m_imageIndex, this);
        }
    }

    internal class GaugeThumbnailListBox : ThumbnailListBox
    {
        Size m_imageSize = new Size(90, 90);
        //Point m_startingCropPoint = new Point(0, 0);

		ProgressBar m_progressBar = null;

        ArrayList m_GaugeThumbnailPlaceholders;

        String m_ThemeCriteria;
        GaugeKindGroup m_GaugeKindCriteria;

		GaugeKind m_selectedKind;
		NumericDisplayKind m_selectedNumericDisplayKind;

        internal static GaugeKindGroup[] SelectableGaugeKindGroups = {GaugeKindGroup.All, GaugeKindGroup.Radial, GaugeKindGroup.Linear,
                                                                 GaugeKindGroup.Numeric};
#if WEB
		Gauge m_imageGenerationGauge = null;
		public Gauge ImageGenerationGauge
#else
		WinGauge m_imageGenerationGauge = null;
		public WinGauge ImageGenerationGauge
#endif
		{
			set
			{
				m_imageGenerationGauge = value;
			}
		}

        public GaugeThumbnailListBox()
        {
            m_GaugeThumbnailPlaceholders = new ArrayList();

        }

        public ProgressBar ProgressBar
        {
            set { m_progressBar = value; }
        }

        // Include gauge thumbnails according to the new criteria and select one thumbnail
        public void SetNewCriteria(GaugeKindGroup gaugeKindGroup, String themeName, GaugeKind selectedKind)
        {
            //Check whether the theme name is valid
            bool themeFound = false;
            foreach (Theme t in m_imageGenerationGauge.Themes)
            {
                if (themeName == t.Name)
                    themeFound = true;
            }
            if (!themeFound)
                return;

            m_ThemeCriteria = themeName;
            m_GaugeKindCriteria = gaugeKindGroup;
			m_selectedKind = selectedKind;

            UpdateThumbnails();
        }

		public void SetNewCriteria(GaugeKindGroup gaugeKindGroup, String themeName, NumericDisplayKind selectedNumericDisplayKind)
		{
			m_selectedNumericDisplayKind = selectedNumericDisplayKind;
			SetNewCriteria(gaugeKindGroup, themeName, GaugeKind.Numeric);
		}


        // Create placeholders for all gauge kinds and all themes. Do not render any images at this time.
        public void GeneratePlaceholders()
        {
			m_GaugeThumbnailPlaceholders.Clear();
			foreach (Theme t in m_imageGenerationGauge.Themes)
			{
				foreach (GaugeKind gk in Enum.GetValues(typeof(GaugeKind)))
				{
					if (gk != GaugeKind.Numeric)
					{
						m_GaugeThumbnailPlaceholders.Add(new GaugeThumbnailPlaceholder(gk, t.Name));
					}
					else
					{
						foreach (NumericDisplayKind ndk in Enum.GetValues(typeof(NumericDisplayKind)))
						{
							GaugeThumbnailPlaceholder placeholder = new GaugeThumbnailPlaceholder(gk, t.Name);
							placeholder.NumericDisplayKind = ndk;
							m_GaugeThumbnailPlaceholders.Add(placeholder);
						}
					}
                }
            }
        }

        // Add gauge thumbnails according to the new criteria rendering only those that have not been rendered before
        private void UpdateThumbnails()
        {
            if (m_GaugeThumbnailPlaceholders.Count == 0)
                return;

            ScrollBar.Value = ScrollBar.Minimum;

            int needToRender = RenderingNeeded();
            if (needToRender > 0 && m_progressBar != null)
            {
                Visible = false;

                m_progressBar.Minimum = 0;
                m_progressBar.Maximum = needToRender;
                m_progressBar.Value = 1;
                m_progressBar.Step = 1;
                m_progressBar.Visible = true;

                Refresh();
            }

            Items.Clear();
			this.SelectedIndex = -1;
            int index = ImageList.Images.Count;

			SubGauge origGauge = m_imageGenerationGauge.SubGauge;
			//While drawing gauge type thumbnails work on a copy of the SubGauge object so that the original settings remain intact
			m_imageGenerationGauge.SubGauge = GaugeXmlSerializer.GetObject(GaugeXmlSerializer.GetDOM(m_imageGenerationGauge.SubGauge)) as SubGauge;
			m_imageGenerationGauge.OverwriteInternalDesignMode(true);
			
            foreach (GaugeThumbnailPlaceholder placeholder in m_GaugeThumbnailPlaceholders)
            {
                if (SubGauge.IsInGroup(placeholder.GaugeKind, m_GaugeKindCriteria) && placeholder.ThemeName == m_ThemeCriteria)
                {
                    //need to render
                    if (placeholder.ImageIndex == -1)
                    {
                        // Render image
                        Bitmap drawBitmap = null;
                        m_imageGenerationGauge.GaugeKind = placeholder.GaugeKind;
                        m_imageGenerationGauge.ThemeName = placeholder.ThemeName;
						if (placeholder.GaugeKind == GaugeKind.Numeric)
							m_imageGenerationGauge.Skin.NumericDisplayKind = placeholder.NumericDisplayKind;
						
						if (m_imageGenerationGauge.BackColor.A == 0)
						{
							m_imageGenerationGauge.BackColor = Color.White;
						}

                        try
                        {
                            drawBitmap = m_imageGenerationGauge.RenderBitmap(m_imageSize.Width, m_imageSize.Height);
                        }
                        catch (Exception) { }
                        if (drawBitmap == null)
                            continue;

                        ImageList.Images.Add(drawBitmap);
                        placeholder.ImageIndex = index;
                        index++;

                        if (m_progressBar != null)
                        {
                            // Update the progress bar
                            m_progressBar.PerformStep();
                            m_progressBar.Refresh();
                        }
                    }
                    Items.Add(placeholder.ToListBoxThumbnail());
					if (placeholder.GaugeKind == m_selectedKind)
					{
						if (placeholder.GaugeKind == GaugeKind.Numeric)
						{
							if (placeholder.NumericDisplayKind == m_selectedNumericDisplayKind)
								this.SelectedIndex = Items.Count-1;
						}
						else
						{
							this.SelectedIndex = Items.Count-1;
						}
					}
                }
            }
			
			//restore originals
			m_imageGenerationGauge.SubGauge = origGauge;
			m_imageGenerationGauge.OverwriteInternalDesignMode(true);

            if (Items.Count > 0 && this.SelectedIndex == -1)
                SelectedIndex = 0;

            if (needToRender > 0 && m_progressBar != null)
            {
                m_progressBar.Visible = false;
                Visible = true;
            }

            ScrollBar.Enabled = Items.Count > 10;
            Invalidate();
        }

        private int RenderingNeeded()
        {
            int count = 0;
            foreach (GaugeThumbnailPlaceholder placeholder in m_GaugeThumbnailPlaceholders)
            {
                if (SubGauge.IsInGroup(placeholder.GaugeKind, m_GaugeKindCriteria) && placeholder.ThemeName == m_ThemeCriteria)
                {
                    //need to render
                    if (placeholder.ImageIndex == -1)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
