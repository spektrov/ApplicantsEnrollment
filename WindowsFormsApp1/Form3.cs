using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {

        private Form1 _form1;

        public Form3(Form1 form1)
        {

            _form1 = form1;

            FormClosing += Form3_FormClosing;
            InitializeComponent();
        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var budget = (int) numericUpDown1.Value;
            var contract = (int) numericUpDown2.Value;
            var privilege = (int) numericUpDown3.Value;
            var coef1 = numericUpDown4.Value;
            var coef2 = numericUpDown5.Value;
            var coef3 = numericUpDown6.Value;
            var rural  = numericUpDown7.Value;
            var coefCertificate = numericUpDown8.Value;

            if (!Logic.ValidateCoefficient(coef1, coef2, coef3, coefCertificate))
            {
                MessageBox.Show("Сумма коефіцієнтів повинна дорівнювати 1(одиниці).",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                return;
            }

            Logic.Budget = budget;
            Logic.Contract = contract;
            Logic.Privilege = privilege;
            Logic.Coefficient1 = coef1;
            Logic.Coefficient2 = coef2;
            Logic.Coefficient3 = coef3;
            Logic.Coefficient4 = coefCertificate;
            Logic.RuralCoefficient = rural;


            Close();
            _form1.UpdateData();
            Logic.SaveData();
        }

        private void Form3_FormClosing(Object sender, FormClosingEventArgs e)
        {
            _form1.Enabled = true;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = Logic.Budget;
            numericUpDown2.Value = Logic.Contract;
            numericUpDown3.Value = Logic.Privilege;
            numericUpDown4.Value = Logic.Coefficient1;
            numericUpDown5.Value = Logic.Coefficient2;
            numericUpDown6.Value = Logic.Coefficient3;
            numericUpDown7.Value = Logic.RuralCoefficient;
            numericUpDown8.Value = Logic.Coefficient4;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
