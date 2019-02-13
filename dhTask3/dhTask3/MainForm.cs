using Core.Interfaces;
using System;
using System.IO;
using System.Windows.Forms;

namespace dhTask3
{
    public partial class MainForm : Form
    {
        MetroFactory.Factory Factory { get; set; }

        public MainForm()
        {
            InitializeComponent();
            Factory = MetroFactory.Factory.CreateFactory();
            Initialize();
        }

        private void Run()
        {
            IStation start = Factory.Metro.GetStationByName(cbEnter.SelectedItem.ToString());
            IStation end = Factory.Metro.GetStationByName(cbExit.SelectedItem.ToString());

            if (start == null || end == null) return;

            try
            {
                tbRes.Text = Factory.GetRoute(start, end);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, $"Невозможно рассчитать маршрут.\r\n{ex.Message}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void CbEnter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEnter.SelectedIndex <= -1) return;
            if (cbExit.SelectedIndex <= -1) return;
            Run();
        }

        private void CbExit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEnter.SelectedIndex <= -1) return;
            if (cbExit.SelectedIndex <= -1) return;
            Run();
        }

        private void BtLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPath.Text))
            {
                MessageBox.Show($"Выберите файл Метрополитена");
                BtPath_Click(sender, e);
            }
            if (string.IsNullOrWhiteSpace(tbPath.Text))
            {
                return;
            }

            if (!File.Exists(tbPath.Text))
            {
                MessageBox.Show($"Файл Метрополитена '{tbPath.Text}' не найден");
            }
            Factory.CreateMetro(tbPath.Text);
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                this.Text = Factory.Metro.Name;
                string[] names = Factory.GetStationArray();
                cbEnter.Items.Clear();
                cbEnter.Items.AddRange(names);
                cbEnter.Enabled = true;
                cbExit.Items.Clear();
                cbExit.Items.AddRange(names);
                cbExit.Enabled = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, $"Невозможно инициализировать окно, возможно файл метрополитена повреждён.\r\n{ex.Message}", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Text = "Метро";
                cbEnter.Enabled = false;
                cbExit.Enabled = false;
            }
        }

        private void BtPath_Click(object sender, EventArgs e)
        {
            if (MetroOpenFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            tbPath.Text = MetroOpenFileDialog.FileName;
            Factory.SaveMetro(MetroOpenFileDialog.FileName);
        }
    }
}