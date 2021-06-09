using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form5 : Form
    {
        private ListOfApplicants _listOfApplicants;

        private Form1 _form1;

        private int _maxAmount;


        public ListOfApplicants CheckedItems = new ListOfApplicants();


        public Form5(int maxAmount, ListOfApplicants listOfApplicants, Form1 form1)
        {
            _maxAmount = maxAmount;
            _listOfApplicants = listOfApplicants;
            _form1 = form1;

            foreach (var applicant in _listOfApplicants.Applicants)
            {
                applicant.Budget = false;
                applicant.Contract = false;
            }

            InitializeComponent();
            Upload();

            label2.Text = _maxAmount.ToString();

            FormClosing += Form5_FormClosing;
        }

        public void Upload()
        {
            foreach (var applicant in _listOfApplicants.Applicants)
            {
                var listViewItem = new ListViewItem(new[]
                {
                    "",
                    $"{applicant.LastName} {applicant.FirstName[0]}.{applicant.MiddleName[0]}.",
                    applicant.Subject1.Mark.ToString(),
                    applicant.Subject2.Mark.ToString(),
                    applicant.Subject3.Mark.ToString(),
                    applicant.Certificate.ToString(),
                    applicant.AdditionalPoint.ToString(),
                    applicant.RuralCoefficient ? "+" : "",
                    applicant.Privilege ? "+" : "",
                    applicant.TotalMark.ToString(),
                    applicant.Contract ? "Контракт" : applicant.Budget ? "Бюджет" : ""
                });

                listView11.Items.Add(listViewItem);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResolvedConflict();
            if (CheckedItems.Applicants.Count != 0)
                Close();
        }

        public void ResolvedConflict()
        {
            if (listView11.CheckedItems.Count != _maxAmount)
            {
                MessageBox.Show($"Виберіть {_maxAmount} абітурієнтів",
                    "Попередження",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            foreach (var index in listView11.CheckedIndices)
            {
                var applicant = _listOfApplicants.Applicants[(int)index];
                CheckedItems.Add(applicant);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form5_FormClosing(Object sender, FormClosingEventArgs e)
        {
            _form1.Enabled = true;
        }
    }
}
