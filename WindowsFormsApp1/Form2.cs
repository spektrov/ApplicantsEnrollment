using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        private ListOfApplicants _listOfApplicants;
        private Form1 _form1;
        private int _index;

        public Form2(ListOfApplicants listOfApplicants, Form1 form1, int index = -1)
        {
            _listOfApplicants = listOfApplicants;
            _form1 = form1;
            _index = index;
            FormClosing += Form2_FormClosing;

            InitializeComponent();
        }

        private void Form2_FormClosing(Object sender, FormClosingEventArgs e)
        {
            _form1.Enabled = true;
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var lastName = textBox1.Text;
            var firstName = textBox2.Text;
            var middleName = textBox3.Text;
            var mark1 = decimal.Round(numericUpDown2.Value, 1);
            var mark2 = decimal.Round(numericUpDown3.Value, 1);
            var mark3 = decimal.Round(numericUpDown4.Value, 1);
            var certificate = decimal.Round(numericUpDown1.Value, 1);
            var additionalPoints = numericUpDown5.Value;
            var subject3 = radioButton1.Checked ?
                new Subject(radioButton1.Text, mark3) : new Subject(radioButton2.Text, mark3);
            var rural = checkBox1.Checked;
            var privilege = checkBox1.Checked;
            var onlyBudget = radioButton3.Checked;
            var onlyContract = radioButton5.Checked;

            if (!_listOfApplicants.ValidateNewApplicant(lastName, firstName, middleName))
            {
                MessageBox.Show("Введены неверные или неполные данные",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                return;
            }

            var applicant = new Applicant()
            {
                LastName = lastName,
                FirstName = firstName,
                MiddleName = middleName,
                AdditionalPoint = additionalPoints,
                Certificate = certificate,
                Privilege = privilege,
                RuralCoefficient = rural,
                Subject1 = new Subject(label4.Text, mark1),
                Subject2 = new Subject(label4.Text, mark2),
                Subject3 = subject3,
                OnlyBudget = onlyBudget,
                OnlyContract = onlyContract
            };
                

            if (_index == -1)
            {
                _listOfApplicants.Add(applicant);
            }
            else
            {
                _listOfApplicants.Applicants[_index] = applicant;
            }

            Close();
            _listOfApplicants.SaveData();
            _form1.UpdateData();
        }

        private void Form2_Load_1(object sender, EventArgs e)
        {
            if (_index != -1)
            {
                textBox1.Text = _listOfApplicants.Applicants[_index].LastName;
                textBox2.Text = _listOfApplicants.Applicants[_index].FirstName;
                textBox3.Text = _listOfApplicants.Applicants[_index].MiddleName;

                numericUpDown1.Value = _listOfApplicants.Applicants[_index].Certificate;
                numericUpDown2.Value = _listOfApplicants.Applicants[_index].Subject1.Mark;
                numericUpDown3.Value = _listOfApplicants.Applicants[_index].Subject2.Mark;
                numericUpDown4.Value = _listOfApplicants.Applicants[_index].Subject3.Mark;
                numericUpDown5.Value = _listOfApplicants.Applicants[_index].AdditionalPoint;

                checkBox1.Checked = _listOfApplicants.Applicants[_index].RuralCoefficient;
                checkBox2.Checked = _listOfApplicants.Applicants[_index].Privilege;

                radioButton3.Checked = _listOfApplicants.Applicants[_index].OnlyBudget;
                radioButton5.Checked = _listOfApplicants.Applicants[_index].OnlyContract;
                radioButton4.Checked = _listOfApplicants.Applicants[_index].OnlyBudget == false &&
                                       _listOfApplicants.Applicants[_index].OnlyContract == false;


                var subject3 = _listOfApplicants.Applicants[_index].Subject3.Name;
                if (subject3 == radioButton1.Text)
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }
            }
        }

    }
}
