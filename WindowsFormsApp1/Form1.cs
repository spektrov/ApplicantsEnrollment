using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private ListOfApplicants _listOfApplicants;

        public Form1()
        {
            InitializeComponent();

            _listOfApplicants = new ListOfApplicants();
            Logic.ReadData();
            UpdateData();
        }

        public void UpdateData()
        {
            var applicants = _listOfApplicants.ReadData();
            if (applicants == null)
            {
                return;
            }

            listView1.Items.Clear();
            foreach (var applicant in _listOfApplicants.Applicants)
            {
                var listViewItem = new ListViewItem(new string[]
                {
                    $"{applicant.LastName} {applicant.FirstName[0]}.{applicant.MiddleName[0]}.",
                    applicant.Subject1.Mark.ToString(),
                    applicant.Subject2.Mark.ToString(),
                    applicant.Subject3.Mark.ToString(),
                    applicant.Certificate.ToString(),
                    applicant.AdditionalPoint.ToString(),
                    applicant.RuralCoefficient ? "+" : "-",
                    applicant.Privilege ? "+" : "-",
                    applicant.TotalMark.ToString(),
                    applicant.OnlyBudget ? "Бюджет" : applicant.OnlyContract ? "Контракт" : "Б або К"
                });

                listView1.Items.Add(listViewItem);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(_listOfApplicants, this);
            form2.Show();
            Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                return;
            }

            Form2 form2 = new Form2(_listOfApplicants, this, listView1.SelectedIndices[0]);
            form2.Show();
            Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                return;
            }

            var answer = MessageBox.Show(
                $"Ви впевнені, що бажаєте видалити дані?\nПовернути їх буде неможливо!",
                "Видалення",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.DefaultDesktopOnly);

            var index = listView1.SelectedIndices[0];

            if (answer == DialogResult.Yes)
            {
                _listOfApplicants.RemoveAt(index);
                _listOfApplicants.SaveData();
                UpdateData();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var list = _listOfApplicants.SortAlphabet();

            Form4 form4 = new Form4(list, this);
            form4.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(this);
            form3.Show();
            Enabled = false;
        }
    }
}
