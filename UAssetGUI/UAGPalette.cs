﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UAssetGUI
{
    public class UAGMenuStripRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            UAGUtils.InvokeUI(() =>
            {
                if (!e.Item.Selected && !Form1.IsDropDownOpened(e.Item) && !FileContainerForm.IsDropDownOpened(e.Item))
                {
                    e.Item.ForeColor = UAGPalette.ForeColor;
                    base.OnRenderMenuItemBackground(e);
                }
                else
                {
                    e.Item.ForeColor = UAGPalette.HighlightForeColor;
                    e.Graphics.FillRectangle(new SolidBrush(UAGPalette.HighlightBackColor), new Rectangle(Point.Empty, e.Item.Size));
                }
            });
        }
    }


    public static class UAGPalette
    {
        public static Color BackColor = Color.White;
        public static Color ButtonBackColor = Color.FromArgb(240, 240, 240);
        public static Color ForeColor = Color.Black;
        public static Color HighlightBackColor = SystemColors.Highlight;
        public static Color HighlightForeColor = SystemColors.HighlightText;
        public static Color InactiveColor = Color.FromArgb(211, 211, 211);
        public static Color DataGridViewActiveColor = Color.FromArgb(240, 240, 240);
        public static Color LinkColor = Color.Blue;
        private static UAGTheme CurrentTheme = UAGTheme.Light;

        public static void InitializeTheme()
        {
            if (!string.IsNullOrEmpty(UAGConfig.Data.Theme)) Enum.TryParse(UAGConfig.Data.Theme, out CurrentTheme);
        }

        public static UAGTheme GetCurrentTheme()
        {
            return CurrentTheme;
        }

        public static void SetCurrentTheme(UAGTheme newTheme)
        {
            CurrentTheme = newTheme;
            UAGConfig.Data.Theme = CurrentTheme.ToString();
            UAGConfig.Save();
        }

        public static void RefreshTheme(Form frm)
        {
            UAGUtils.InvokeUI(() =>
            {
                RefreshThemeInternal(frm);
            });
        }

        /// <summary>
        /// null = check based on favorite thing, false = force no comic sans, true = force yes comic sans
        /// </summary>
        public static bool? IsComicSansOverride = null;
        public static bool IsComicSans()
        {
            if (IsComicSansOverride != null) return (bool)IsComicSansOverride;
            return UAGConfig.Data.FavoriteThing.ToLowerInvariant().Trim().StartsWith("comic sans");
        }


        private static Dictionary<Control, Font> oldFontSettings = null;
        private static void RefreshAllButtonsInControl(this Control ctrl, out bool needRefreshForComicSans)
        {
            needRefreshForComicSans = false;
            foreach (Control ctrl2 in ctrl.Controls)
            {
                if (ctrl2 is Button butto)
                {
                    butto.FlatStyle = FlatStyle.Flat;
                    butto.ForeColor = UAGPalette.ForeColor;
                    butto.FlatAppearance.BorderColor = UAGPalette.ForeColor;
                    butto.FlatAppearance.BorderSize = 1;
                    butto.BackColor = UAGPalette.ButtonBackColor;
                    butto.MinimumSize = new Size(0, 10);
                }
                if (ctrl2 is ComboBox combo)
                {
                    combo.ForeColor = UAGPalette.ForeColor;
                    combo.BackColor = UAGPalette.ButtonBackColor;
                    combo.FlatStyle = CurrentTheme == UAGTheme.Light ? FlatStyle.Standard : FlatStyle.Flat;
                }
                if (ctrl2 is GroupBox gp)
                {
                    gp.ForeColor = UAGPalette.ForeColor;
                }

                if (IsComicSans())
                {
                    if (oldFontSettings == null) oldFontSettings = new Dictionary<Control, Font>();
                    if (ctrl2.Font.FontFamily.Name != "Comic Sans MS")
                    {
                        oldFontSettings[ctrl2] = ctrl2.Font;
                        needRefreshForComicSans = true;
                    }
                    ctrl2.Font = new Font("Comic Sans MS", ctrl2.Font.Size, ctrl2.Font.Style);
                }
                else if (oldFontSettings != null)
                {
                    if (oldFontSettings.ContainsKey(ctrl2) && oldFontSettings[ctrl2] != null)
                    {
                        ctrl2.Font = oldFontSettings[ctrl2];
                        needRefreshForComicSans = true;
                    }
                    oldFontSettings[ctrl2] = null;
                }

                RefreshAllButtonsInControl(ctrl2, out bool needRefreshForComicSansInner);
                if (needRefreshForComicSansInner) needRefreshForComicSans = true;
            }
        }

        public static float RecommendedFontSize
        {
            get
            {
                return 8.25f + (float)UAGConfig.Data.DataZoom;
            }
        }

        private static void AdjustDGV(DataGridView dgv)
        {
            Color selectedDGVBackColor = dgv.Columns.Count > 0 ? UAGPalette.DataGridViewActiveColor : UAGPalette.InactiveColor;
            dgv.BackgroundColor = selectedDGVBackColor;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.ColumnHeadersDefaultCellStyle.Font.FontFamily, RecommendedFontSize, dgv.ColumnHeadersDefaultCellStyle.Font.Style);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = UAGPalette.BackColor; // intentional
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = UAGPalette.ForeColor;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = UAGPalette.HighlightBackColor;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = UAGPalette.HighlightForeColor;
            dgv.RowHeadersDefaultCellStyle = dgv.ColumnHeadersDefaultCellStyle;
            dgv.DefaultCellStyle = dgv.ColumnHeadersDefaultCellStyle;

            int defaultNeededRowHeight = (int)(dgv.ColumnHeadersDefaultCellStyle.Font.Height * 1.5f);
            dgv.RowTemplate.MinimumHeight = defaultNeededRowHeight > 22 ? defaultNeededRowHeight : 22;
            dgv.RowTemplate.Height = dgv.RowTemplate.MinimumHeight;
        }

        private static void AdjustTreeView(ColorfulTreeView treeView1)
        {
            Color selectedListViewBackColor = treeView1.Nodes.Count > 0 ? UAGPalette.BackColor : UAGPalette.InactiveColor;
            treeView1.BackColor = selectedListViewBackColor;
            treeView1.ForeColor = UAGPalette.ForeColor;
            treeView1.Font = new Font(treeView1.Font.FontFamily, 8.25f + (float)UAGConfig.Data.DataZoom, treeView1.Font.Style);
        }

        private static void AdjustMenuStrip(MenuStrip menuStrip1)
        {
            menuStrip1.BackColor = UAGPalette.BackColor;
            menuStrip1.ForeColor = UAGPalette.ForeColor;
            foreach (ToolStripItem rootItem in menuStrip1.Items)
            {
                rootItem.BackColor = UAGPalette.BackColor;
                rootItem.ForeColor = UAGPalette.ForeColor;
                if (rootItem is ToolStripMenuItem rootMenuItem)
                {
                    foreach (ToolStripItem childItem in rootMenuItem.DropDownItems)
                    {
                        childItem.BackColor = UAGPalette.BackColor;
                        childItem.ForeColor = UAGPalette.ForeColor;
                    }
                }
            }
        }

        public static readonly int InitialSplitterDistance = 408;
        private static void RefreshThemeInternal(Form frm)
        {
            switch (CurrentTheme)
            {
                case UAGTheme.Light:
                    BackColor = Color.White;
                    ButtonBackColor = Color.FromArgb(240, 240, 240);
                    ForeColor = Color.Black;
                    HighlightBackColor = SystemColors.Highlight;
                    HighlightForeColor = SystemColors.HighlightText;
                    InactiveColor = Color.FromArgb(211, 211, 211);
                    DataGridViewActiveColor = Color.FromArgb(240, 240, 240);
                    LinkColor = Color.Blue;
                    break;
                case UAGTheme.Dark:
                    BackColor = Color.FromArgb(46, 46, 46);
                    ButtonBackColor = Color.FromArgb(61, 61, 61);
                    ForeColor = Color.White;
                    HighlightBackColor = Color.FromArgb(250, 148, 46);
                    HighlightForeColor = BackColor;
                    InactiveColor = Color.FromArgb(45, 45, 45);
                    DataGridViewActiveColor = Color.FromArgb(35, 35, 35);
                    LinkColor = Color.FromArgb(250, 148, 46);
                    break;
            }

            frm.RefreshAllButtonsInControl(out bool needRefreshForComicSans);

            frm.Icon = Properties.Resources.icon;
            frm.BackColor = UAGPalette.BackColor;
            frm.ForeColor = UAGPalette.ForeColor;
            if (frm is Form1 frm1)
            {
                AdjustTreeView(frm1.treeView1);
                AdjustMenuStrip(frm1.menuStrip1);

                frm1.jsonView.ForeColor = UAGPalette.ForeColor;
                frm1.jsonView.BackColor = UAGPalette.BackColor;
                frm1.jsonView.Font = new Font(frm1.jsonView.Font.FontFamily, RecommendedFontSize, frm1.jsonView.Font.Style);

                // ByteViewer has no support for changing the background color
                frm1.byteView1.ForeColor = Color.Black;
                frm1.byteView1.BackColor = Color.White;

                AdjustDGV(frm1.dataGridView1);

                // reset splitter if comic sans
                if (needRefreshForComicSans)
                {
                    frm1.splitContainer1.SplitterDistance = InitialSplitterDistance;
                }

                /*if (frm1.tableEditor != null)
                {
                    frm1.tableEditor.Save(true);
                }*/
            }
            if (frm is FileContainerForm frm3)
            {
                AdjustTreeView(frm3.saveTreeView);
                AdjustTreeView(frm3.loadTreeView);
                AdjustMenuStrip(frm3.menuStrip1);
            }
            if (frm is MapStructTypeOverrideForm frm2)
            {
                AdjustDGV(frm2.mstoDataGridView);
            }

            // fix some strange formatting issues with the comic sans easter egg
            // could just always execute this, but just in case this has other undesired effects, only execute it when needed
            // (not actually that big a deal if the comic sans easter egg breaks something, but a big deal if we break something else just to fix the easter egg...)
            if (frm is SettingsForm frm4 && needRefreshForComicSans)
            {
                frm4.numericUpDown1.Location = new Point(frm4.favoriteThingBox.Location.X, frm4.label3.Location.Y);
                frm4.numericUpDown1.Size = frm4.favoriteThingBox.Size;
                frm4.customSerializationFlagsBox.Location = new Point(frm4.favoriteThingBox.Location.X, frm4.label4.Location.Y);
                frm4.customSerializationFlagsBox.Size = frm4.favoriteThingBox.Size;
            }
        }
    }
}
