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

        private Form5 _form5B;

        private Form5 _form5C;

        public Form1()
        {
            InitializeComponent();

            _listOfApplicants = new ListOfApplicants();
            Constants.ReadData();
            UpdateData();
            FormClosing += Form_Closing;
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
                    applicant.RuralCoefficient ? "+" : "",
                    applicant.Privilege ? "+" : "",
                    applicant.TotalMark.ToString(),
                    applicant.OnlyBudget ? "Б" : applicant.OnlyContract ? "К" : "Б/К"
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
            ListOfApplicants list;

            if (radioButton1.Checked)
            {
                list = _listOfApplicants.Subject1Sort();
            }
            else if (radioButton2.Checked)
            {
                list = _listOfApplicants.Subject2Sort();
            }
            else if (radioButton3.Checked)
            {
                list = _listOfApplicants.Subject3Sort(radioButton3.Text);
            }
            else
            {
                list = _listOfApplicants.Subject3Sort(radioButton4.Text);
            }

            Form4 form4 = new Form4(list, this);
            form4.Show();
            Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var list = _listOfApplicants.SortAlphabet();

            Form4 form4 = new Form4(list, this);
            form4.Show();
            Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var list = _listOfApplicants.SortTotalMark();

            Form4 form4 = new Form4(list, this);
            form4.Show();
            Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {

            var l = _listOfApplicants.Applicants.OrderByDescending(a => a.TotalMark).ToList();
            if (l[Constants.Budget - Constants.Privilege - 1].TotalMark ==
                l[Constants.Budget - Constants.Privilege].TotalMark)
                //Constants.Budget - withoutConflict.Applicants.Count < conflictList.Applicants.Count)
            {
                var conflictList = _listOfApplicants.ConflictBudget();

                var withoutConflict = _listOfApplicants.WithoutConflictBudget();

                if (Constants.Budget - withoutConflict.Applicants.Count > 0)
                {
                    _form5B = new Form5(Constants.Budget - withoutConflict.Applicants.Count,
                        conflictList, this);
                    _form5B.Show();
                    Enabled = false;

                    _form5B.FormClosing += f5_FormClosingBudget;
                    return;
                }

            }

            var list = new ListOfApplicants();
            list.Applicants.AddRange(_listOfApplicants.Budget().Applicants);
            Form4 form4 = new Form4(list, this);
            form4.Show();
            Enabled = false;
            _listOfApplicants.SaveData();
        }

        private void button9_Click(object sender, EventArgs e)
        {

            var l1 = _listOfApplicants.Applicants.OrderByDescending(a => a.TotalMark).ToList();
            
            if (l1[Constants.Budget - Constants.Privilege - 1].TotalMark ==
                l1[Constants.Budget - Constants.Privilege].TotalMark)
            {
                var conflictList = _listOfApplicants.ConflictBudget();

                var withoutConflict = _listOfApplicants.WithoutConflictBudget();

                _form5B = new Form5(Constants.Budget - withoutConflict.Applicants.Count,
                    conflictList, this);
                _form5B.Show();
                Enabled = false;

                _form5B.FormClosing += f5_FormClosing;
                _listOfApplicants.SaveData();
            }

            var l2 = _listOfApplicants.AllContract();
            if (Constants.Contract < l2.Count &&
                l2[Constants.Contract - 1].TotalMark == l2[Constants.Contract].TotalMark)
            {
                var conflictList = _listOfApplicants.ConflictContract();
                var withoutConflict = _listOfApplicants.WithoutConflictContract();

                _form5C = new Form5(Constants.Contract - withoutConflict.Applicants.Count, conflictList, this);
                _form5C.Show();
                Enabled = false;

                _form5C.FormClosing += f5_FormClosing;
                _listOfApplicants.SaveData();
            }

            if (l1[Constants.Budget - Constants.Privilege - 1].TotalMark !=
                l1[Constants.Budget - Constants.Privilege].TotalMark &&
                (Constants.Contract <= l2.Count &&
                 l2[Constants.Contract - 1].TotalMark != l2[Constants.Contract].TotalMark))
            {
                var list = _listOfApplicants.AllApplicants();

                Form4 form4 = new Form4(list, this);
                form4.Show();
                Enabled = false;
            }

        }


        private void button8_Click(object sender, EventArgs e)
            {

                var l = _listOfApplicants.AllContract();
                if (Constants.Contract < l.Count &&
                    l[Constants.Contract - 1].TotalMark == l[Constants.Contract].TotalMark)
                    //Constants.Contract - withoutConflict.Applicants.Count < conflictList.Applicants.Count)
                {
                    var conflictList = _listOfApplicants.ConflictContract();
                    var withoutConflict = _listOfApplicants.WithoutConflictContract();

                    _form5C = new Form5(Constants.Contract - withoutConflict.Applicants.Count, conflictList, this);
                    _form5C.Show();
                    Enabled = false;

                    _form5C.FormClosing += f5_FormClosingContract;
                }

                else
                {
                    var list = _listOfApplicants.Contract();
                    Form4 form4 = new Form4(list, this);
                    form4.Show();
                    Enabled = false;
                }

                _listOfApplicants.SaveData();

            }

            private void button10_Click(object sender, EventArgs e)
            {
                Form3 form3 = new Form3(this);
                form3.Show();
                Enabled = false;
            }

            private void f5_FormClosingContract(object sender, FormClosingEventArgs e)
            {
                var list = FormClosingContract();

                if (list == null) return;

                Form4 form4 = new Form4(list, this);
                form4.Show();
            }

            private ListOfApplicants FormClosingContract()
            {
                var conflictCount = _listOfApplicants.ConflictContract().Applicants.Count;
                var list = new ListOfApplicants();


                if (conflictCount > 0)
                {
                    var withoutConflict = _listOfApplicants.WithoutConflictContract();

                    foreach (var applicant in withoutConflict.Applicants)
                    {
                        list.Applicants.Add(applicant);
                    }

                    if (_form5C?.CheckedItems.Applicants.Count == 0)
                    {
                        MessageBox.Show("Конфлікт не опрацьовано");
                        return null;
                    }

                    if (_form5C?.CheckedItems.Applicants != null)
                        foreach (var applicant in _form5C.CheckedItems.Applicants)
                        {
                            applicant.Contract = true;
                            applicant.Budget = false;
                            list.Applicants.Add(applicant);
                        }
                }
                else
                {
                    list = _listOfApplicants.Contract();
                }

                return list;
            }

            private ListOfApplicants FormClosingBudget()
            {
                var conflictCount = _listOfApplicants.ConflictBudget().Applicants.Count;
                var list = new ListOfApplicants();

                if (conflictCount > 0)
                {
                    var withoutConflict = _listOfApplicants.WithoutConflictBudget();

                    foreach (var applicant in withoutConflict.Applicants)
                    {
                        list.Applicants.Add(applicant);
                    }

                    if (_form5B.CheckedItems.Applicants.Count == 0)
                    {
                        MessageBox.Show("Конфлікт не опрацьовано");
                        return null;
                    }

                    foreach (var applicant in _form5B.CheckedItems.Applicants)
                    {
                        applicant.Budget = true;
                        applicant.Contract = false;
                        list.Applicants.Add(applicant);
                    }
                }
                else
                {
                    list = _listOfApplicants.Budget();
                }

                return list;
            }

            private void f5_FormClosingBudget(object sender, FormClosingEventArgs e)
            {
                var listAppl = FormClosingBudget();
                if (listAppl == null) return;

                Form4 form4 = new Form4(listAppl, this);
                form4.Show();
            }

            private void f5_FormClosing(object sender, FormClosingEventArgs e)
            {
                if (Application.OpenForms.OfType<Form5>().Count() > 1)
                {
                    return;
                }

                var budget = FormClosingBudget();
                var contract = FormClosingContract();

                if (budget == null || contract == null)
                    return;

                var list = new ListOfApplicants();
                list.Applicants.AddRange(budget.Applicants);
                list.Applicants.AddRange(contract.Applicants);

                Form4 form4 = new Form4(list, this);
                form4.Show();
            }

            private void Form_Closing(object sender, FormClosingEventArgs e)
            {
                _listOfApplicants.SaveData();
            }

        }
    }
