using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {
        private ListOfApplicants _listOfApplicants ;

        private Form1 _form1;

        public Form4(ListOfApplicants listOfApplicants, Form1 form1)
        {
            _listOfApplicants = listOfApplicants;
            _form1 = form1;

            InitializeComponent();

            Upload();
            FormClosing += Form4_FormClosing;

            saveFileDialog1.Filter = @"Text files(*.txt)|*.txt|" +
                                     @"Data (*.xml)|*.xml|" +
                                     @"All files(*.*)|*.*";
        }

        public void Upload()
        {
            var i = 0;
            foreach (var applicant in _listOfApplicants.Applicants)
            {
                var listViewItem = new ListViewItem(new string[]
                {
                    i.ToString(),
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

                listView11.Items.Add(listViewItem);
                i++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл

            var length = _listOfApplicants.Applicants.Count;
            var str = new string[length];

            for (int i = 0; i < length; i++)
            {
                str[i] = _listOfApplicants.Applicants[i].ToString();
            }

            System.IO.File.WriteAllLines(filename, str, Encoding.UTF8);
            MessageBox.Show("Файл сохранен");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form4_FormClosing(Object sender, FormClosingEventArgs e)
        {
            _form1.Enabled = true;
        }
    }
}
