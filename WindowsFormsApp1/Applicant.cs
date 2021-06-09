using System;

namespace WindowsFormsApp1
{
    /// <summary>
    /// This class represents one applicant.
    /// </summary>
    public class Applicant
    {
        /// <summary>
        /// Surname.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Father's name.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Math.
        /// </summary>
        public Subject Subject1 { get; set; }

        /// <summary>
        /// Ukrainian.
        /// </summary>
        public Subject Subject2 { get; set; }

        /// <summary>
        /// English or Physics.
        /// </summary>
        public Subject Subject3 { get; set; }

        /// <summary>
        /// Certificate point.
        /// </summary>
        public decimal Certificate { get; set; }

        /// <summary>
        /// Points for additional activities.
        /// </summary>
        public decimal AdditionalPoint { get; set; }

        /// <summary>
        /// Is the applicant from countryside.
        /// </summary>
        public bool RuralCoefficient { get; set; }

        /// <summary>
        /// Has the applicant privilege.
        /// </summary>
        public bool Privilege { get; set; }

        /// <summary>
        /// Want to study only on budget.
        /// </summary>
        public bool OnlyBudget { get; set; }

        /// <summary>
        /// Want to study only on contract.
        /// </summary>
        public bool OnlyContract { get; set; }

        /// <summary>
        /// Entered the contract.
        /// </summary>
        public bool Contract { get; set; }

        /// <summary>
        /// Entered the budget.
        /// </summary>
        public bool Budget { get; set; }

        /// <summary>
        /// Result mark. Need to define place in rating.
        /// </summary>
        public decimal TotalMark {
            get =>
                Math.Min((decimal.Round((Subject1.Mark * Constants.Coefficient1 +
                                         Subject2.Mark * Constants.Coefficient2 +
                                         Subject3.Mark * Constants.Coefficient3 +
                                         (Certificate > 2 ? (100 + 10 * (Certificate - 2)) * Constants.Coefficient4 : 100) +
                                         AdditionalPoint) *
                                        (RuralCoefficient ? Constants.RuralCoefficient : 1), 3)), 
                    decimal.Round((decimal)200.0001, 3));
            // For serialization.
            set { }
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName[0]}.{MiddleName[0]}. : {TotalMark}";
        }
    }
}
