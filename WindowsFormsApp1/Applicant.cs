using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    public class Applicant
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public Subject Subject1 { get; set; }

        public Subject Subject2 { get; set; }

        public Subject Subject3 { get; set; }

        public decimal Certificate { get; set; }

        public decimal AdditionalPoint { get; set; }

        public bool RuralCoefficient { get; set; }

        public bool Privilege { get; set; }

        public bool OnlyBudget { get; set; }

        public bool OnlyContract { get; set; }

        public decimal TotalMark => (decimal.Round((Subject1.Mark * Logic.Coefficient1 +
                                                     Subject2.Mark * Logic.Coefficient2 +
                                                     Subject3.Mark * Logic.Coefficient3 +
                                                     (Certificate > 2 ? (100 + 10 * (Certificate - 2)) * Logic.Coefficient4 : 100) +
                                                     AdditionalPoint) *
                                                     (RuralCoefficient ? Logic.RuralCoefficient : 1), 3));

        public override string ToString()
        {
            return $"{LastName} {FirstName[0]}.{MiddleName[0]}. : {TotalMark}";
        }
    }
}
