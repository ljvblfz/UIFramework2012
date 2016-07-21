using System;
using System.Reflection;

using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace ComponentArt.Web.Visualization.Charting.Design
{
	internal class CollectionWithTypeEditor : EnhancedCollectionEditor
	{

		protected CollectionWithType m_coll;
		protected Button m_dropB = null;

		protected Button addB_copy;
		protected Button m_removeB;
		protected Button m_addB = null;
		protected Button m_copyB = null;
		protected Button m_cancelB = null;
		private bool m_cancelling = false;

		ListBox m_listBox;

		private ContextMenu m_collectionListMenu = new ContextMenu();

		// Saves the object we need to copy
		protected object m_objectToCopy = null;


		// Constructor
		public CollectionWithTypeEditor(Type type): base(type) {}

		protected override Type CreateCollectionItemType() 
		{
			return m_coll.Type;
		}

		void DesignerTransactionClose(object sender, DesignerTransactionCloseEventArgs dtcea) 
		{
		}
		void DesignerTransactionOpen(object sender, EventArgs e) 
		{
		}

		/// <summary>
		/// Overriden, so that we can store the reference to collection
		/// </summary>
		/// 
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) 
		{
			m_coll = value as CollectionWithType;

			IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));			

			System.Windows.Forms.Design.IWindowsFormsEditorService service1 = null;
			if (provider != null)
				service1 = (System.Windows.Forms.Design.IWindowsFormsEditorService) provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));

			if (host != null) 
			{
				host.TransactionClosed += new DesignerTransactionCloseEventHandler(DesignerTransactionClose);
				host.TransactionClosing += new DesignerTransactionCloseEventHandler(DesignerTransactionClose);

				host.TransactionOpened += new EventHandler(DesignerTransactionOpen);
				host.TransactionOpening += new EventHandler(DesignerTransactionOpen);
			}            

			ChartBase chart = ChartBase.GetChartFromObject(context.Instance);
			chart.InCollectionEditing = true;
			m_coll.InCollectionEditor = true;

			Type collectionItemType = m_coll.Type;
			PropertyInfo isDefaultPI = collectionItemType.GetProperty("IsDefault", BindingFlags.Instance | BindingFlags.NonPublic);

			// Get array of default objects
			bool [] isDefaultArray = null;
			if (isDefaultPI != null) 
			{
				isDefaultArray = new bool [m_coll.Count];
				for (int i=0; i<m_coll.Count; ++i)
				{
					isDefaultArray[i] = (bool)isDefaultPI.GetValue(((IList)m_coll)[i], null);
				}
			}

			m_coll.OnEditStarting();
			object o = base.EditValue(context, provider, value);
			chart.InCollectionEditing = false;
			m_coll.InCollectionEditor = false;

			// Set array of default objects			
			if (m_cancelling && isDefaultPI != null) 
			{
				for (int i=0; i<m_coll.Count; ++i)
				{
					isDefaultPI.SetValue(((IList)m_coll)[i], isDefaultArray[i], null);
				}

				m_cancelling = false;
			}

			m_coll.OnEditCompleted(m_cancelling);

			return o;
		}


        Control FindControl(Control parent, Type ctrlType)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.GetType() == ctrlType || c.GetType().IsSubclassOf(ctrlType))
                {
                    return c;
                }
                else
                {
                    if (c.Controls.Count > 0)
                    {
                        Control c1 = FindControl(c, ctrlType);
                        if (c1 != null)
                            return c1;
                    }
                }
            }
            return null;
        }

        Button FindButton(Control parent, string text)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button && c.Text == text)
                {
                    return (Button)c;
                }
                else
                {
                    if (c.Controls.Count > 0)
                    {
                        Button b = FindButton(c, text);
                        if (b != null)
                            return b;
                    }
                }
            }
            return null;
        }

        Button FindButtonByName(Control parent, string name)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button && c.Name == name)
                {
                    return (Button)c;
                }
                else
                {
                    if (c.Controls.Count > 0)
                    {
                        Button b = FindButtonByName(c, name);
                        if (b != null)
                            return b;
                    }
                }
            }
            return null;
        }

        void m_addB_Resize(object sender, EventArgs e)
        {
        }

        void table_Resize(object sender, EventArgs e)
        {
        }

        protected override CollectionForm CreateCollectionForm() 
		{
			CollectionEditor.CollectionForm collectionForm = base.CreateCollectionForm();

            Assembly z = Assembly.GetAssembly(typeof(Button));
            Type TableLayoutControlCollectionType = Type.GetType("System.Windows.Forms.TableLayoutControlCollection" + ", " + z.FullName);
            bool frameworkVersion2 = (TableLayoutControlCollectionType != null);

            //return collectionForm;

			Control.ControlCollection cc = collectionForm.Controls;


            Size oldSizeAdd = new Size(0, 0);
            
            m_addB = FindButton(collectionForm, "&Add");

            // Inserted this line for non-english version of the studio
            if (m_addB == null)
            {
                m_addB = FindButtonByName(collectionForm, "addButton");
            }
            
            if (m_addB != null)
                m_addB.Resize += new EventHandler(m_addB_Resize);

            m_removeB = FindButton(collectionForm, "&Remove");
            if (m_removeB == null)
                m_removeB = FindButtonByName(collectionForm, "removeButton");
            m_cancelB = FindButton(collectionForm, "Cancel");
            if (m_cancelB == null)
                m_cancelB = FindButtonByName(collectionForm, "cancelButton");
            m_dropB = FindButton(collectionForm, "");
            if (m_dropB == null)
                m_dropB = FindButtonByName(collectionForm, "downButton");

            if (!frameworkVersion2 && m_addB != null)
            {

                // Change Add
                oldSizeAdd = m_addB.Size;
                m_addB.Size = new Size(oldSizeAdd.Width * 2 / 3, m_removeB.Size.Height);

                // Change Remove
                Size oldSize = m_removeB.Size;
                m_removeB.Size = new Size(oldSize.Width * 2 / 3, m_removeB.Size.Height);
                m_removeB.Location = new Point(m_removeB.Location.X + oldSize.Width - m_removeB.Size.Width, m_removeB.Location.Y);

                // Drop button
                m_dropB.Location
                    = new Point(m_dropB.Location.X - (oldSizeAdd.Width - m_addB.Size.Width),
                    m_dropB.Location.Y);
            }

            m_listBox = (ListBox)FindControl(collectionForm, typeof(ListBox));


			// Cancel Button
			this.m_cancelB.Click += new System.EventHandler(this.m_cancelB_Click);


			// Copy button
			m_copyB = new Button();
			m_copyB.Text = "&Copy";
            if (!frameworkVersion2)
            {
                m_copyB.Location = new Point(m_dropB.Location.X + m_dropB.Size.Width + 5, m_dropB.Location.Y);
                m_copyB.Size = m_addB.Size;
            }
            m_copyB.Name = "m_copyB";

            MethodInfo miAdd = frameworkVersion2 ? TableLayoutControlCollectionType.GetMethod("Add", new Type[] { typeof(Control), typeof(int), typeof(int) }) : null;


            if (frameworkVersion2)
            {
                // 2.0 and higher


                Control table = m_addB.Parent;
                //table.Resize +=new EventHandler(table_Resize);

                table.SuspendLayout();
                //collectionForm.SuspendLayout();

                m_copyB.TabIndex = 3;
#if !ZZZZ

                m_copyB.GetType().GetProperty("AutoSize").SetValue(m_copyB, true, null);
                
                PropertyInfo piAnchor = m_copyB.GetType().GetProperty("Anchor");
                piAnchor.SetValue(m_copyB, piAnchor.GetValue(m_removeB, null), null);

                PropertyInfo piMargin = m_copyB.GetType().GetProperty("Margin");
                piMargin.SetValue(m_copyB, piMargin.GetValue(m_removeB, null), null);

                table.GetType().GetProperty("ColumnCount").SetValue(table, 4, null);

                Type ColumnStyleType = Type.GetType("System.Windows.Forms.ColumnStyle" + ", " + z.FullName);
                Type SizeTypeType = Type.GetType("System.Windows.Forms.SizeType" + ", " + z.FullName);
                Type TableLayoutColumnStyleCollectionType = Type.GetType("System.Windows.Forms.TableLayoutColumnStyleCollection" + ", " + z.FullName);
                MethodInfo miStyleAdd = TableLayoutColumnStyleCollectionType.GetMethod("Add", new Type[] { ColumnStyleType });

                ConstructorInfo ciColumnStyle = ColumnStyleType.GetConstructor(new Type[] {SizeTypeType, typeof(float)});

                object colStyle = ciColumnStyle.Invoke(new object[] { Enum.Parse(SizeTypeType, "Percent"), 50f });

                PropertyInfo piColumnStyles = m_addB.Parent.GetType().GetProperty("ColumnStyles");

                object columnStyles = piColumnStyles.GetValue(table, null);
                miStyleAdd.Invoke(columnStyles, new object[] { colStyle });

                foreach (object col in (IEnumerable)columnStyles)
                {
                    PropertyInfo piColumnWidth = ColumnStyleType.GetProperty("Width");
                    PropertyInfo piSizeType = ColumnStyleType.GetProperty("SizeType");
                    object percentE = Enum.Parse(SizeTypeType, "Percent");
                    object colSizeType = piSizeType.GetValue(col, null);
                    if (Enum.Equals(colSizeType, percentE))
                    {
                        piColumnWidth.SetValue(col, 33f, null);
                    }
                }
                
                table.Controls.Remove(m_removeB);

                miAdd.Invoke(table.Controls, new object[] { m_removeB, 3, 0 });
                miAdd.Invoke(table.Controls, new object[] { m_copyB, 2, 0 });

                m_removeB.TabIndex = 3;
                m_copyB.TabIndex = 2;

#else

                m_copyB.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                m_copyB.AutoSize = true;
                m_copyB.Margin = new Padding(3, 0, 0, 0);


                TableLayoutPanel zzzaddRemoveTableLayoutPanel = (TableLayoutPanel)table;
                zzzaddRemoveTableLayoutPanel.ColumnCount = 4;
                zzzaddRemoveTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));

                foreach (ColumnStyle cs in zzzaddRemoveTableLayoutPanel.ColumnStyles)
                {
                    if (cs.SizeType == SizeType.Percent)
                    {
                        cs.Width = 33.33f;
                    }
                }
                zzzaddRemoveTableLayoutPanel.Controls.Add(m_copyB, 3, 0);
#endif
                table.ResumeLayout(false);
                table.PerformLayout();

                //collectionForm.ResumeLayout(false);
            }
            else
            {
                // 1.1 and below
                m_addB.Parent.Controls.Add(m_copyB);
            }

			m_copyB.Click += new System.EventHandler(this.CopyItem);

			// New Add Button
			if (m_coll.Types != null && m_coll.Types.Length > 1) 
			{
				// Create a substitute button
				addB_copy = new Button();
			
				// Copy the old key properties
				addB_copy.Text = m_addB.Text;
				addB_copy.Location = m_addB.Location;
				addB_copy.Size = m_addB.Size;
				addB_copy.TabIndex = m_addB.TabIndex;
				addB_copy.Name = m_addB.Name;

				// Switch buttons
                if (frameworkVersion2)
                {
                    addB_copy.GetType().GetProperty("AutoSize").SetValue(m_copyB, true, null);

                    PropertyInfo piAnchor = m_copyB.GetType().GetProperty("Anchor");
                    piAnchor.SetValue(addB_copy, piAnchor.GetValue(m_addB, null), null);

                    PropertyInfo piMargin = m_copyB.GetType().GetProperty("Margin");
                    piMargin.SetValue(addB_copy, piMargin.GetValue(m_addB, null), null);

                    miAdd.Invoke(m_addB.Parent.Controls, new object[] { addB_copy, 0, 0 });
                }
                else
                {
                    m_addB.Parent.Controls.Add(addB_copy);
                }
				//collectionForm.Controls.Remove(addB);

				m_addB.Hide();
                m_dropB.Hide();

				// Add a handler
				addB_copy.Click += new System.EventHandler(this.add_Click);

			}

			// Make sure location is updated on resizing
			collectionForm.Resize += new System.EventHandler(this.FormResized);

			m_collectionForm = collectionForm;
			m_listBox.MouseDown += new MouseEventHandler(ListBoxMouseDown);

			m_collectionListMenu.MenuItems.Clear();
			m_collectionListMenu.MenuItems.Add("Copy", new EventHandler(CopyItem));
			m_collectionListMenu.MenuItems.Add("Remove", new EventHandler(RemoveItem));

			return collectionForm;
		}

        
		private void m_cancelB_Click(object sender, System.EventArgs e) 
		{
			m_cancelling = true;
		}


		private void CopyItem(object sender, EventArgs e)
		{
			m_listBox.Invalidate();
			m_collectionForm.Invalidate();
			
			object listItem = m_listBox.SelectedItem;

            
			string name = listItem.ToString();
			TypeConverter tc = TypeDescriptor.GetConverter(listItem.GetType());
			object str = tc.ConvertTo(listItem, typeof(string));
			
            
			PropertyInfo pi = listItem.GetType().GetProperty("Value");
			if (pi != null)
				m_objectToCopy = pi.GetValue(listItem, null);			

			// Can't just take the index of the object since it will not work with arrows.

            if (m_coll.Types != null && m_coll.Types.Length > 1)
            {
                addB_copy.Hide();
                m_addB.Show();
            }

            m_addB.PerformClick();

            if (m_coll.Types != null && m_coll.Types.Length > 1)
            {
                m_addB.Hide();
                addB_copy.Show();
            }
        }

		private void RemoveItem(object sender, EventArgs e)
		{
			m_removeB.PerformClick();
		}

		private void ListBoxMouseDown(object sender, MouseEventArgs e) 
		{
			if (e.Button == MouseButtons.Right) 
			{
				int index = m_listBox.IndexFromPoint(e.X, e.Y);
				m_listBox.ClearSelected();
				m_listBox.SelectedIndex = index;
				m_collectionListMenu.Show(m_listBox, new System.Drawing.Point(e.X, e.Y));
			}
		}

		private void add_Click(object sender, System.EventArgs e)
		{
            object o = m_collectionForm.GetType().GetField("addDownMenu", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(m_collectionForm);
            if (o is ContextMenu)
            {
                ContextMenu mennnnn = (ContextMenu)o;
                mennnnn.Show(addB_copy, new Point(0, this.addB_copy.Height));
            }
#if __COMPILING_FOR_2_0_AND_ABOVE__
            else if (o is ContextMenuStrip)
            {
                ContextMenuStrip cms = (ContextMenuStrip)o;
                cms.Show(addB_copy, new Point(0, this.addB_copy.Height));
            }
#endif
		}

		private void FormResized(object sender, System.EventArgs e)
		{
			if (addB_copy != null)
				addB_copy.Location = m_addB.Location;

			m_copyB.Location = new Point(m_copyB.Location.X, m_addB.Location.Y);
		}

		protected override Type[] CreateNewItemTypes()
		{
			if (m_coll.Types != null)
				return m_coll.Types;

			return base.CreateNewItemTypes();
		}

		protected override bool CanRemoveInstance(object value) 
		{
			System.Reflection.PropertyInfo pi = 
				value.GetType().GetProperty("Removable", BindingFlags.Instance | BindingFlags.NonPublic);

			if (pi != null) 
			{
				if ((bool)pi.GetValue(value, null) == false) 
				{
					return false;
				}
			}
			return true;
		}

		protected object CopiedInstance() 
		{
			StyleCloner cloner = new StyleCloner();
			object o = cloner.Clone(m_objectToCopy);

			string clonerMessage = cloner.Message();

			if (clonerMessage != "") 
			{
				throw new NotSupportedException("Could not clone chart: " + clonerMessage);
			}

			// Set Removable property to true
			System.Reflection.PropertyInfo pi = 
				o.GetType().GetProperty("Removable", BindingFlags.Instance | BindingFlags.Public);

			if (pi != null) 
			{
				pi.SetValue(o, true, null);
			}

			return o;
		}
		
		protected override System.Object CreateInstance ( System.Type itemType ) 
		{
			if (m_objectToCopy != null) 
			{
				return CopiedInstance();
			}

			object newobj = base.CreateInstance(itemType);
			SetContext(newobj, m_coll.Owner);
			return newobj;
		}

		protected void SetContext(object target, object owner) 
		{
			System.Reflection.MethodInfo mi = target.GetType().GetMethod(
				"SetContext", BindingFlags.Instance | BindingFlags.NonPublic, null,
				CallingConventions.Any, new Type[] {typeof(object)}, null);
				
			if (mi != null)
			{
				try 
				{
					mi.Invoke(target, new object [] {owner});
				} 
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

	}
}
