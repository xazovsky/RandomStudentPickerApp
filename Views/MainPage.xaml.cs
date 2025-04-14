using RandomStudentPickerApp.Models;
using RandomStudentPickerApp.Services;

namespace RandomStudentPickerApp.Views
{
    public partial class MainPage : ContentPage
    {
        private ClassGroup _currentClass = new();

        public MainPage()
        {
            InitializeComponent();
            LoadClassNames();
        }

        private void LoadClassNames()
        {
            classPicker.ItemsSource = ClassFileService.GetAllClassNames();
        }

        private async void OnClassSelected(object sender, EventArgs e)
        {
            if (classPicker.SelectedItem is string className)
            {
                _currentClass = await ClassFileService.LoadClassAsync(className);
                studentsList.ItemsSource = _currentClass.Students;
                resultLabel.Text = $"Wczytano klasę: {className}";
            }
        }

        private async void OnCreateNewClassClicked(object sender, EventArgs e)
        {
            string className = await DisplayPromptAsync("Nowa klasa", "Podaj nazwę klasy:");
            if (!string.IsNullOrWhiteSpace(className))
            {
                _currentClass = new ClassGroup { Name = className };
                studentsList.ItemsSource = null;
                studentsList.ItemsSource = _currentClass.Students;
                resultLabel.Text = $"Utworzono nową klasę: {className}";
                if (!string.IsNullOrWhiteSpace(_currentClass.Name))
                {
                    await ClassFileService.SaveClassAsync(_currentClass);
                    LoadClassNames();
                    resultLabel.Text = $"Dodano ucznia i zapisano klasę: {_currentClass.Name}";
                }
            }
        }

        private void OnPickStudentClicked(object sender, EventArgs e)
        {
            var student = RandomStudentPicker.PickRandomStudent(_currentClass);
            resultLabel.Text = student != null
                ? $"Wylosowano: {student.Name}"
                : "Brak uczniów do wylosowania.";
        }

        private async void OnAddStudentClicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Nowy uczeń", "Podaj imię i nazwisko:");
            if (!string.IsNullOrWhiteSpace(name))
            {
                _currentClass.Students.Add(new Student { Name = name });
                studentsList.ItemsSource = null;
                studentsList.ItemsSource = _currentClass.Students;

                if (!string.IsNullOrWhiteSpace(_currentClass.Name))
                {
                    await ClassFileService.SaveClassAsync(_currentClass);
                    LoadClassNames();
                    resultLabel.Text = $"Dodano ucznia i zapisano klasę: {_currentClass.Name}";
                }
            }
        }


        private async void OnSaveClassClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_currentClass.Name))
            {
                string name = await DisplayPromptAsync("Zapisz klasę", "Podaj nazwę klasy:");
                _currentClass.Name = name;
            }

            await ClassFileService.SaveClassAsync(_currentClass);
            LoadClassNames();

            resultLabel.Text = $"Zapisano klasę: {_currentClass.Name}";
        }
    }
}
