using RandomStudentPickerApp.Models;

namespace RandomStudentPickerApp.Services
{
    public static class ClassFileService
    {
        private static string folderPath = Path.Combine(FileSystem.AppDataDirectory, "Classes");

        static ClassFileService()
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }

        public static async Task SaveClassAsync(ClassGroup classGroup)
        {
            string filePath = Path.Combine(folderPath, $"{classGroup.Name}.txt");
            var lines = classGroup.Students.Select(s => s.Name);
            await File.WriteAllLinesAsync(filePath, lines);
        }

        public static async Task<ClassGroup> LoadClassAsync(string className)
        {
            string filePath = Path.Combine(folderPath, $"{className}.txt");
            if (!File.Exists(filePath)) return null;

            var lines = await File.ReadAllLinesAsync(filePath);
            return new ClassGroup
            {
                Name = className,
                Students = lines.Select(name => new Student { Name = name }).ToList()
            };
        }

        public static List<string> GetAllClassNames()
        {
            return Directory.GetFiles(folderPath, "*.txt")
                            .Select(f => Path.GetFileNameWithoutExtension(f))
                            .ToList();
        }
    }
}
