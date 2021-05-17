using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApp1
{
    class Subject
    {
        public string Name { get; set; }
        public double Mark { get; set; }

        public Subject(string name, double mark)
        {
            Name = name;
            Mark = mark;
        }
    }
}
