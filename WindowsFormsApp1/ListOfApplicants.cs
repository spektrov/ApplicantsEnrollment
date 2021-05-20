using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    public class ListOfApplicants
    {
        private List<Applicant> _applicants = new List<Applicant>();

        public List<Applicant> Applicants
        {
            get => _applicants;
            private set => _applicants = value;
        }

        public void Add(Applicant applicant)
        {
            if (applicant != null)
            {
                _applicants.Add(applicant);
            }
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _applicants.Count)
            {
                _applicants.RemoveAt(index);
            }
        }

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
            catch (Exception e)
            {
                return false;
            }
        }

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
            catch (Exception e)
            {
                return null;
            }
        }

        public bool ValidateNewApplicant(string text1, string text2, string text3)
        {
            if (text1.Length < 2 || text2.Length < 2 || text3.Length < 2)
            {
                return false;
            }

            return true;
        }

        public ListOfApplicants SortAlphabet()
        {
            var applicants = new ListOfApplicants();
            applicants.Applicants.AddRange(_applicants.OrderBy(a => a.LastName));

            return applicants;
        }
    }
}
