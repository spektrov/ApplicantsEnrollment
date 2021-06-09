using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

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
                                     @"Data (*.xml)|*.xml";
        }

        public void Upload()
        {
            var i = 1;
            foreach (var applicant in _listOfApplicants.Applicants)
            {
                var listViewItem = new ListViewItem(new[]
                {
                    i.ToString(),
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
                i++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = saveFileDialog1.FileName;

            if (filename.Split(".")[^1] == "txt")
            {
                var length = _listOfApplicants.Applicants.Count;
                var str = new string[length];

                for (int i = 0; i < length; i++)
                {
                    str[i] = _listOfApplicants.Applicants[i].ToString();
                }

                try
                {
                    System.IO.File.WriteAllLines(filename, str, Encoding.UTF8);
                    MessageBox.Show("Файл збережен",
                        "Успіх",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Помилка");
                }
            }

            else if (filename.Split(".")[^1] == "xml")
            {
                var xmlFormatter = new XmlSerializer(typeof(List<Applicant>));

                try
                {
                    using (var file = new FileStream(filename, FileMode.Create))
                    {
                        xmlFormatter.Serialize(file, _listOfApplicants.Applicants);
                    }

                    MessageBox.Show("Файл збережен",
                        "Успіх",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    Close();

                }
                catch (Exception)
                {
                    MessageBox.Show("Помилка");
                }
            }
            else
            {
                MessageBox.Show("Виберіть текстовий формат *.txt або\n" +
                                "формат *xml для публікації даних",
                    "Увага",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
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
