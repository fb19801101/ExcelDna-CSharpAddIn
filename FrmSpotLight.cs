using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpAddIn
{
    public partial class FrmSpotLight : Form
    {
        public FrmSpotLight()
        {
            InitializeComponent();

            btnRow.BackColor = Color.FromArgb(RibbonController.row_clrIndex & 0x0000ff, (RibbonController.row_clrIndex & 0x00ff00) >> 8, (RibbonController.row_clrIndex & 0xff0000) >> 16);
            btnCol.BackColor = Color.FromArgb(RibbonController.col_clrIndex & 0x0000ff, (RibbonController.col_clrIndex & 0x00ff00) >> 8, (RibbonController.col_clrIndex & 0xff0000) >> 16);
            nudTransparent.Value = Convert.ToDecimal(RibbonController.clr_transparent) * 100;
            rdGraLine.Checked = RibbonController.type_mask == 1;
            rdShapes.Checked = RibbonController.type_mask == 2;
            rdGraMask.Checked = RibbonController.type_mask == 3;
            ckLineMask.Checked = RibbonController.line_mask;
        }

        private void btnRow_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.FullOpen = true;
            colorDialog.ShowHelp = true;
            colorDialog.Color = Color.Black;//初始化颜色
            colorDialog.ShowDialog();
            Color clr = colorDialog.Color;
            btnRow.BackColor = clr;
            RibbonController.row_clrColor = clr;
            RibbonController.row_clrIndex = (int)(((uint)clr.B << 16) | (ushort)(((ushort)clr.G << 8) | clr.R));
        }

        private void btnCol_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.FullOpen = true;
            colorDialog.ShowHelp = true;
            colorDialog.Color = Color.Black;//初始化颜色
            colorDialog.ShowDialog();
            Color clr = colorDialog.Color;
            btnCol.BackColor = clr;
            RibbonController.col_clrColor = clr;
            RibbonController.col_clrIndex = (int)(((uint)clr.B << 16) | (ushort)(((ushort)clr.G << 8) | clr.R));
        }

        private void nudTransparent_ValueChanged(object sender, EventArgs e)
        {
            RibbonController.clr_transparent = Decimal.ToSingle(nudTransparent.Value)/100.0f;
        }

        private void FrmSpotLight_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (rdGraLine.Checked) RibbonController.type_mask = 1;
            if (rdShapes.Checked) RibbonController.type_mask = 2;
            if (rdGraMask.Checked) RibbonController.type_mask = 3;
            RibbonController.line_mask = ckLineMask.Checked;
        }
    }
}
