using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hantek_UI
{
    public partial class FrmSettings : Form
    {
        public FrmSettings()
        {
            InitializeComponent();
        }

        public Color? WindowColor { get; set; }
        public Color? TextColor { get; set; }
        public Color? UnitColor { get; set; }
        public Color? ModeColor { get; set; }
        public int? Precision { get; set; }
        public bool? WindowBorder { get; set; }
        public decimal? Interval { get; set; }

        private bool initializing;

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            initializing = true;
            BtnTextColor.BackColor = Settings.Default.TextColor;
            BtnWindowColor.BackColor = Settings.Default.WindowColor;
            BtnUnitColor.BackColor = Settings.Default.UnitColor;
            BtnModeColor.BackColor = Settings.Default.ModeColor;
            ChkTransparent.Checked = Settings.Default.WindowColor == Color.Transparent;
            ChkWindowBorder.Checked = Settings.Default.HasBorder;
            CboPrecision.Text = Settings.Default.Precision.ToString();
            NumInterval.Value = Settings.Default.Interval;
            initializing = false;
        }

        private Color? SelectColor(Button colorBtn)
        {
            Color color = colorBtn.BackColor;
            using ColorDialog dlgColor = new();
            dlgColor.AnyColor = true;
            if (color == Color.Transparent)
                dlgColor.Color = Color.FromArgb(96, 64, 32);
            else
                dlgColor.Color = color;
            DialogResult rslt = dlgColor.ShowDialog(this);
            if ((rslt == DialogResult.OK) && (color != dlgColor.Color))
                return dlgColor.Color;
            else
                return null;
        }

        private void BtnTextColor_Click(object sender, EventArgs e)
        {
            TextColor = SelectColor(BtnTextColor);
            if (TextColor != null)
                BtnTextColor.BackColor = TextColor.Value;
        }

        private void BtnWindowColor_Click(object sender, EventArgs e)
        {
            Color? tmpColor =  SelectColor(BtnWindowColor);
            if (tmpColor != null)
            {
                if (ChkTransparent.Checked) ChkTransparent.Checked = false;
                WindowColor = tmpColor;
                BtnWindowColor.BackColor = WindowColor.Value;
            }
        }

        private void BtnUnitColor_Click(object sender, EventArgs e)
        {
            UnitColor = SelectColor(BtnUnitColor);
            if (UnitColor != null)
                BtnUnitColor.BackColor = UnitColor.Value;
        }

        private void BtnModeColor_Click(object sender, EventArgs e)
        {
            ModeColor = SelectColor(BtnModeColor);
            if (ModeColor != null)
                BtnModeColor.BackColor = ModeColor.Value;
        }

        private void ChkTransparent_CheckedChanged(object sender, EventArgs e)
        {
            if (!initializing)
            {
                if (ChkTransparent.Checked)
                {
                    WindowColor = Color.Transparent;
                    BtnWindowColor.BackColor = Color.Transparent;
                    ChkWindowBorder.Checked = false;
                }
                else
                {
                    WindowColor = Color.Black;
                    BtnWindowColor.BackColor = Color.Black;
                }
            }
        }

        private void CboPrecision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!initializing)
            {
                var item = CboPrecision.SelectedItem as String;  //CboPrecision.Items[CboPrecision.SelectedIndex];
                if (item != null)
                {
                    if (int.TryParse(item, out int val))
                    {
                        Precision = val;
                    }
                }
            }
        }

        private void ChkWindowBorder_CheckedChanged(object sender, EventArgs e)
        {
            if (!initializing)
            {
                WindowBorder = ChkWindowBorder.Checked;
                if (ChkWindowBorder.Checked) ChkTransparent.Checked = false;
            }
        }

        private void NumInterval_ValueChanged(object sender, EventArgs e)
        {
            if (!initializing)
            {
                Interval = NumInterval.Value;
            }
        }
    }
}
