using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    public class Subject
    {
        public string Name { get; set; }
        public decimal Mark { get; set; }

        public Subject(string name, decimal mark)
        {
            Name = name;
            Mark = mark;
        }

        public Subject()
        {

        }
    }
}
