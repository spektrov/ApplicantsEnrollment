namespace WindowsFormsApp1
{
    /// <summary>
    /// Class represents one subject
    /// that can be used as a applicant mark.
    /// </summary>
    public class Subject
    {

        public Subject(string name, decimal mark)
        {
            Name = name;
            Mark = mark;
        }

        // For serialization.
        public Subject() { }

        /// <summary>
        /// Name of the subject.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mark received on this subject.
        /// </summary>
        public decimal Mark { get; set; }
    }
}
