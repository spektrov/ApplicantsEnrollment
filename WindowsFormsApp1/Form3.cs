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

            if (!Constants.ValidateCoefficient(coef1, coef2, coef3, coefCertificate))
            {
                MessageBox.Show("Сумма коефіцієнтів повинна дорівнювати 1(одиниці).",
                    "Помилка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                return;
            }

            if (!Constants.ValidatePrivilege(budget, privilege))
            {
                MessageBox.Show("Квотників не може бути більше за бюджетників.",
                        "Помилка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    return;
            }

            Constants.Budget = budget;
            Constants.Contract = contract;
            Constants.Privilege = privilege;
            Constants.Coefficient1 = coef1;
            Constants.Coefficient2 = coef2;
            Constants.Coefficient3 = coef3;
            Constants.Coefficient4 = coefCertificate;
            Constants.RuralCoefficient = rural;


            Close();
            _form1.UpdateData();
            Constants.SaveData();
        }

        private void Form3_FormClosing(Object sender, FormClosingEventArgs e)
        {
            _form1.Enabled = true;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = Constants.Budget;
            numericUpDown2.Value = Constants.Contract;
            numericUpDown3.Value = Constants.Privilege;
            numericUpDown4.Value = Constants.Coefficient1;
            numericUpDown5.Value = Constants.Coefficient2;
            numericUpDown6.Value = Constants.Coefficient3;
            numericUpDown7.Value = Constants.RuralCoefficient;
            numericUpDown8.Value = Constants.Coefficient4;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
        }
    }
}
