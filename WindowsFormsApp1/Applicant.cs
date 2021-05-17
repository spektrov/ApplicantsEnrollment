using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApp1
{
    class Applicant
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public Subject Subject1 { get; set; }
        public Subject Subject2 { get; set; }
        public Subject Subject3 { get; set; }

        public double Certificate { get; set; }

        public Applicant(
            string lastName,
            string firstName,
            string middleName,
            Subject subject1,
            Subject subject2,
            Subject subject3, 
            double certificate)
        {
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            Subject1 = subject1;
            Subject2 = subject2;
            Subject3 = subject3;
            Certificate = certificate;
        }


    }
}
