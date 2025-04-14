using System.Collections.Generic;

namespace RandomStudentPickerApp.Models
{
    public class ClassGroup
    {
        public string Name { get; set; }
        public List<Student> Students { get; set; } = new();
    }
}
