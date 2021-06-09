using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    /// <summary>
    /// Holds list of applicants and make changes in it.
    /// </summary>
    public class ListOfApplicants
    {
        private List<Applicant> _applicants = new List<Applicant>();

        public List<Applicant> Applicants
        {
            get => _applicants;
            set => _applicants = value;
        }

        /// <summary>
        /// Adding new applicant to list.
        /// </summary>
        /// <param name="applicant"></param>
        public void Add(Applicant applicant)
        {
            if (applicant != null)
            {
                _applicants.Add(applicant);
            }

            _applicants = SortAlphabet().Applicants;
        }

        /// <summary>
        /// Removing applicant from position in list.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _applicants.Count)
            {
                _applicants.RemoveAt(index);
            }
        }

        /// <summary>
        /// Save list into XML file
        /// </summary>
        /// <returns></returns>
        public bool SaveData()
        {
            var xmlFormatter = new XmlSerializer(typeof(List<Applicant>));

            try
            {
                using (var file = new FileStream("applicants.xml", FileMode.Create))
                {
                    xmlFormatter.Serialize(file, _applicants);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Read data from XML file and save to list.
        /// </summary>
        /// <returns></returns>
        public List<Applicant> ReadData()
        {
            var xmlFormatter = new XmlSerializer(typeof(List<Applicant>));
            try
            {
                using (var file = new FileStream("applicants.xml", FileMode.OpenOrCreate))
                {
                    if (xmlFormatter.Deserialize(file) is List<Applicant> applicants)
                    {
                        _applicants = applicants;
                        return applicants;
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Check if FirstName, LastName and MiddleName are correct.
        /// </summary>
        public bool ValidateNewApplicant(string text1, string text2, string text3)
        {
            if (text1.Length < 2 || text2.Length < 2 || text3.Length < 2)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Find all applicants who will be on budget
        /// it is no conflicts.
        /// </summary>
        public ListOfApplicants Budget()
        {
            var applicants = new ListOfApplicants();

            applicants.Applicants.AddRange(FindPrivilege().Applicants);

            var nonContract = WithoutContract();

            var nonPrivilege = nonContract.Applicants.Except(FindPrivilege().Applicants);

            var budget = new List<Applicant>();
            budget.
                AddRange(nonPrivilege.
                OrderByDescending(a=>a.TotalMark).
                Take(Constants.Budget - Constants.Privilege));

            applicants.Applicants.AddRange(budget);

            var nonBudget = _applicants.Except(applicants.Applicants);
            foreach (var applicant in nonBudget)
            {
                applicant.Budget = false;
            }

            foreach (var applicant in applicants.Applicants)
            {
                applicant.Budget = true;
                applicant.Contract = false;
            }

            return applicants;
        }

        /// <summary>
        /// If conflict, find index of first element in conflict.
        /// </summary>
        public int StartConflictIndex(List<Applicant> checkConflicts, int constant)
        {
            var i = checkConflicts.Count - 1;
            var startConflictIndex = i;
            while (i > 0)
            {
                if (checkConflicts[i].TotalMark == checkConflicts[i - 1].TotalMark)
                {
                    startConflictIndex = i-1;
                }
                else
                {
                    if (i < constant && startConflictIndex < constant)
                    {
                        break;
                    }
                }

                i--;
            }

            if (startConflictIndex >= constant)
            {
                return checkConflicts.Count - 1;
            }

            return startConflictIndex;
        }

        /// <summary>
        ///  If conflict, find index of last element in conflict.
        /// </summary>
        public int EndConflictIndex(List<Applicant> checkConflicts, int startConflictIndex, int constant)
        {
            var i = startConflictIndex;
            var endConflictIndex = 0;
            var check = 0;

            if (i >= checkConflicts.Count - 1)
            {
                return 0;
            }

            while (check != -1)
            {
                if (checkConflicts[i].TotalMark == checkConflicts[i + 1].TotalMark)
                {
                    endConflictIndex = i + 1;
                }
                else
                {
                    check = -1;
                }
                i++;
            }

            return endConflictIndex < constant ? 0 : endConflictIndex;
        }


        /// <summary>
        /// Return conflict students on budget.
        /// </summary>
        public ListOfApplicants ConflictBudget()
        {
            var checkConflicts = new List<Applicant>();
            checkConflicts.AddRange(FindPrivilege().Applicants);

            var nonPrivilege = WithoutContract().Applicants.Except(FindPrivilege().Applicants);

            checkConflicts.AddRange(nonPrivilege.
                OrderByDescending(a => a.TotalMark).ToList());

            var startConflictIndex = StartConflictIndex(checkConflicts, Constants.Budget);
            var endConflictIndex = EndConflictIndex(checkConflicts, startConflictIndex, Constants.Budget);

            var list = FindConflict(checkConflicts, startConflictIndex, endConflictIndex);

            return list;
        }

        /// <summary>
        /// All non-conflict budgets.
        /// </summary>
        public ListOfApplicants WithoutConflictBudget()
        {
            var checkConflicts = new List<Applicant>();
            checkConflicts.AddRange(FindPrivilege().Applicants);

            var nonPrivilege = WithoutContract().Applicants.Except(FindPrivilege().Applicants);

            checkConflicts.AddRange(nonPrivilege.
                OrderByDescending(a => a.TotalMark).ToList());

            var startConflictIndex = StartConflictIndex(checkConflicts, Constants.Budget);

            var list = new ListOfApplicants();

            for (int i = 0; i < startConflictIndex; i++)
            {
                checkConflicts[i].Budget = true;
                checkConflicts[i].Contract = false;
                list.Applicants.Add(checkConflicts[i]);
            }

            var others = _applicants.Except(list.Applicants);
            foreach (var applicant in others)
            {
                applicant.Budget = false;
            }

            return list;
        }

        /// <summary>
        /// Find if conflict in contract.
        /// </summary>
        /// <returns></returns>
        public ListOfApplicants ConflictContract()
        {
            var checkConflicts = WithoutBudget().Applicants.
                Except(FindPrivilege().Applicants).
                Except(_applicants.Where(a => a.Budget)).
                OrderByDescending(a => a.TotalMark).ToList();

            var startConflictIndex = StartConflictIndex(checkConflicts, Constants.Contract);
            var endConflictIndex = EndConflictIndex(checkConflicts, startConflictIndex, Constants.Contract);

            var list = FindConflict(checkConflicts, startConflictIndex, endConflictIndex);

            return list;
        } 

        /// <summary>
        /// All contract without conflict.
        /// </summary>
        /// <returns></returns>
        public ListOfApplicants WithoutConflictContract()
        {
            var checkConflicts = WithoutBudget().Applicants.
                Except(FindPrivilege().Applicants).
                Except(_applicants.Where(a => a.Budget)).
                OrderByDescending(a => a.TotalMark).ToList();

            var startConflictIndex = StartConflictIndex(checkConflicts, Constants.Contract);

            var list = new ListOfApplicants();

            for (int i = 0; i < startConflictIndex; i++)
            {
                checkConflicts[i].Contract = true;
                checkConflicts[i].Budget = false;
                list.Applicants.Add(checkConflicts[i]);
            }

            var others = _applicants.Except(list.Applicants);
            foreach (var applicant in others)
            {
                applicant.Contract = false;
            }

            return list;
        }

        /// <summary>
        /// All applicants that are not on budget.
        /// </summary>
        public List<Applicant> AllContract()
        {
            var applicants = new List<Applicant>();
            applicants.AddRange(WithoutBudget().Applicants.
                Except(Budget().Applicants).
                OrderByDescending(a => a.TotalMark));

            return applicants;
        }

        /// <summary>
        /// Contract list if it is no conflict.
        /// </summary>
        public ListOfApplicants Contract()
        {
            var applicants = new ListOfApplicants();

            var withoutAllBudgets = WithoutBudget().Applicants.
                Except(FindPrivilege().Applicants).
                Except(_applicants.Where(a => a.Budget));

            withoutAllBudgets =  withoutAllBudgets.
            OrderByDescending(a => a.TotalMark).
            Take(Constants.Contract);
            var withoutAllBudgetList = withoutAllBudgets.ToList();
            foreach (var applicant in withoutAllBudgetList)
            {
                applicant.Contract = true;
                applicant.Budget = false;
            }

            var others = _applicants.Except(withoutAllBudgetList);
            foreach (var applicant in others)
            {
                applicant.Contract = false;
            }
            
            applicants.Applicants.AddRange(withoutAllBudgetList);

            return applicants;
        }

        /// <summary>
        /// Privilege list.
        /// </summary>
        public ListOfApplicants FindPrivilege()
        {
            var applicants = new ListOfApplicants();
            applicants.Applicants.AddRange(_applicants.
                Where(a => a.Privilege).
                OrderByDescending(a => a.TotalMark).
                Take(Constants.Privilege));

            return applicants;
        }

        /// <summary>
        /// Applicants on budget and contract both without conflicts.
        /// </summary>
        /// <returns></returns>
        public ListOfApplicants AllApplicants()
        {
            var applicants = new ListOfApplicants();

            applicants.Applicants.AddRange(Budget().Applicants);
            applicants.Applicants.AddRange(Contract().Applicants);

            return applicants;
        }

        /// <summary>
        /// Sorting applicants by math mark.
        /// </summary>
        public ListOfApplicants Subject1Sort()
        {
            var applicants = new ListOfApplicants();

            applicants.Applicants.AddRange(AllApplicants().Applicants.
                OrderByDescending(a => a.Subject1.Mark));

            return applicants;
        }

        /// <summary>
        /// Sorting applicants by ukrainian mark.
        /// </summary>
        public ListOfApplicants Subject2Sort()
        {
            var applicants = new ListOfApplicants();

            applicants.Applicants.AddRange(AllApplicants().Applicants.
                OrderByDescending(a => a.Subject2.Mark));

            return applicants;
        }


        /// <summary>
        /// Sorting applicants by english or physics mark.
        /// </summary>
        public ListOfApplicants Subject3Sort(string name)
        {
            var applicants = new ListOfApplicants();

            applicants.Applicants.AddRange(AllApplicants().Applicants.
                Where(a => a.Subject3.Name[0].ToString().ToLower() == name[0].ToString().ToLower()).
                OrderByDescending(a => a.Subject3.Mark));

            return applicants;
        }

        /// <summary>
        /// Sorting applicants by LastName.
        /// </summary>
        public ListOfApplicants SortAlphabet()
        {
            var applicants = new ListOfApplicants();
            applicants.Applicants.AddRange(_applicants.OrderBy(a => a.LastName));

            return applicants;
        }

        /// <summary>
        /// Sorting applicants by total mark.
        /// </summary>
        public ListOfApplicants SortTotalMark()
        {
            var applicants = new ListOfApplicants();
            applicants.Applicants.AddRange(_applicants.
                OrderByDescending(a => a.TotalMark));

            return applicants;
        }

        private ListOfApplicants WithoutContract()
        {
            var applicants = new ListOfApplicants();
            applicants.Applicants.AddRange(_applicants.
                Where(a => !a.OnlyContract));

            return applicants;
        }

        private ListOfApplicants WithoutBudget()
        {
            var applicants = new ListOfApplicants();
            applicants.Applicants.AddRange(_applicants.
                Where(a => !a.OnlyBudget));

            return applicants;
        }

        private ListOfApplicants FindConflict(List<Applicant> checkConflicts,
            int startConflictIndex, int endConflictIndex)
        {
            var list = new ListOfApplicants();

            if (startConflictIndex >= endConflictIndex)
            {
                return list;
            }

            for (int i = startConflictIndex; i <= endConflictIndex; i++)
            {
                list.Add(checkConflicts[i]);
            }

            return list;
        }
    }
}
