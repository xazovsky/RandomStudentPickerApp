using RandomStudentPickerApp.Models;

namespace RandomStudentPickerApp.Services
{
    public static class RandomStudentPicker
    {
        private static Random _random = new();

        public static Student PickRandomStudent(ClassGroup classGroup)
        {
            if (classGroup.Students.Count == 0) return null;
            int index = _random.Next(classGroup.Students.Count);
            return classGroup.Students[index];
        }
    }
}
