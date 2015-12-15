using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalcFileResourceInfo
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            String filePath = txtChooseFile.Text;

            CalcFileInfo(filePath);
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            var items = e.Data.GetData(DataFormats.FileDrop) as String[];

            if (items != null && items.Length > 0)
            {
                txtChooseFile.Text = items[0];

                CalcFileInfo(items[0]);
            }
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void CalcFileInfo(String filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath)) return;

            if (!File.Exists(filePath))
            {
                MessageBox.Show(String.Format("文件绝对路径:\"{0}\"不存在!", filePath));
                return;
            }

            try
            {
                var bytes = File.ReadAllBytes(filePath);

                txtSize.Text = bytes.Length.ToString();
                txtMD5.Text = Moqikaka.Util.Security.MD5Util.MD5(bytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("文件计算失败, 错误信息: {0}", ex.Message + Environment.NewLine + ex.StackTrace));
            }
        }

        private void txtChooseFile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.V || e.Modifiers != Keys.Control) return;

            IDataObject iData = Clipboard.GetDataObject();

            if (iData.GetDataPresent(DataFormats.FileDrop))
            {
                var items = iData.GetData(DataFormats.FileDrop) as String[];

                if (items != null && items.Length > 0)
                {
                    txtChooseFile.Text = items[0];

                    CalcFileInfo(items[0]);
                }
            }
        }
    }
}
