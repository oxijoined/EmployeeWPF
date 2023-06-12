using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Employee
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик события загрузки окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReadJsonAndAppendDataGrid("export.json");
        }

        // Запрет редактирования ячеек в DataGrid
        private void employeesGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        // Обработчик нажатия кнопки "Добавить"
        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            // Получение списка сотрудников из DataGrid
            List<EmployeeClass> employees = employeesGrid.Items.OfType<EmployeeClass>().ToList();

            // Генерация нового идентификатора
            int newId = employees.Count + 1;

            // Создание нового экземпляра класса EmployeeClass
            EmployeeClass newEmployee = new EmployeeClass(newId, "", "", "", "", "", "", "", "");

            // Открытие окна редактирования с передачей нового сотрудника и списка всех сотрудников
            EditEmployeeWindow editWindow = new EditEmployeeWindow(newEmployee, employees);
            editWindow.ShowDialog();

            // Добавление нового сотрудника в список и обновление DataGrid
            employees.Add(newEmployee);
            employeesGrid.ItemsSource = employees;
            employeesGrid.Items.Refresh();
        }

        // Обработчик нажатия кнопки "Редактировать"
        private void EditRecordButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение выбранного сотрудника из DataContext кнопки
            Button button = (Button)sender;
            EmployeeClass employee = (EmployeeClass)button.DataContext;

            // Получение списка сотрудников из DataGrid
            List<EmployeeClass> employees = employeesGrid.Items.OfType<EmployeeClass>().ToList();

            // Открытие окна редактирования с передачей выбранного сотрудника и списка всех сотрудников
            EditEmployeeWindow editWindow = new EditEmployeeWindow(employee, employees);
            editWindow.ShowDialog();

            // Обновление DataGrid
            employeesGrid.Items.Refresh();
        }

        // Обработчик нажатия кнопки "Удалить"
        private void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение выбранного сотрудника из DataContext кнопки
            Button button = (Button)sender;
            EmployeeClass employee = (EmployeeClass)button.DataContext;

            // Получение списка сотрудников из DataGrid
            if (employeesGrid.ItemsSource is List<EmployeeClass> employees)
            {
                // Удаление выбранного сотрудника из списка и обновление DataGrid
                employees.Remove(employee);
                employeesGrid.Items.Refresh();
            }
        }

        // Обработчик нажатия кнопки "Импорт"
        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            // Открытие диалогового окна для выбора файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON Files (*.json)|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                // Получение пути выбранного JSON-файла и чтение данных из него
                string jsonFilePath = openFileDialog.FileName;
                ReadJsonAndAppendDataGrid(jsonFilePath);
            }
        }

        // Обработчик нажатия кнопки "Экспорт и сохранение"
        private void ExportSaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение списка сотрудников из DataGrid
            List<EmployeeClass> employees = employeesGrid.Items.OfType<EmployeeClass>().ToList();

            // Сериализация списка сотрудников в формате JSON
            string json = JsonConvert.SerializeObject(employees, Formatting.Indented);

            // Сохранение JSON-данных в файл "export.json"
            File.WriteAllText("export.json", json);

            // Вывод сообщения об успешном сохранении
            MessageBox.Show($"Успешно сохранено {employees.Count} записей");
        }

        // Обработчик нажатия кнопки "Сбросить фильтр"
        private void CancelFilter_Click(object sender, RoutedEventArgs e)
        {
            // Сброс фильтрации и восстановление видимости всех строк в DataGrid
            for (int i = 0; i < employeesGrid.Items.Count; i++)
            {
                DataGridRow row = employeesGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;

                if (row != null)
                {
                    foreach (DataGridColumn column in employeesGrid.Columns)
                    {
                        if (column is DataGridTextColumn textColumn)
                        {
                            var cellContent = column.GetCellContent(row);
                            if (cellContent is TextBlock textBlock)
                            {
                                // Сброс цвета фона ячейки
                                textBlock.Background = Brushes.White;
                            }
                        }
                    }

                    // Восстановление видимости строки
                    row.Visibility = Visibility.Visible;
                }
            }
        }

        // Обработчик нажатия кнопки "Поиск"
        private void SearchSelection_Click(object sender, RoutedEventArgs e)
        {
            // Проверка наличия фильтра
            if (FilterInput.Text == "")
            {
                MessageBox.Show("Необходимо ввести фильтр");
                return;
            }

            // Поиск и подсветка ячеек, содержащих текст фильтра
            for (int i = 0; i < employeesGrid.Items.Count; i++)
            {
                DataGridRow? row = employeesGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;

                if (row != null)
                {
                    foreach (DataGridColumn column in employeesGrid.Columns)
                    {
                        if (column is DataGridTextColumn textColumn)
                        {
                            var cellContent = column.GetCellContent(row);
                            if (cellContent is TextBlock textBlock)
                            {
                                string cellText = textBlock.Text;
                                if (cellText.Contains(FilterInput.Text))
                                {
                                    // Подсветка ячейки совпадающего текста
                                    textBlock.Background = Brushes.Yellow;
                                }
                                else
                                {
                                    // Восстановление цвета фона ячейки
                                    textBlock.Background = Brushes.White;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Обработчик кнопки "Фильтр"
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilterInput.Text == "")
            {
                MessageBox.Show("Необходимо ввести фильтр");
                return;
            }

            for (int i = 0; i < employeesGrid.Items.Count; i++)
            {
                DataGridRow row = employeesGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;

                if (row != null)
                {
                    foreach (DataGridColumn column in employeesGrid.Columns)
                    {
                        if (column is DataGridTextColumn textColumn)
                        {
                            var cellContent = column.GetCellContent(row);
                            if (cellContent is TextBlock textBlock)
                            {
                                string cellText = textBlock.Text;
                                if (cellText.Contains(FilterInput.Text))
                                {
                                    row.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    row.Visibility = Visibility.Collapsed;
                                }
                            }
                        }
                    }
                }
            }
        }


        // Чтение данных из JSON-файла и добавление их в DataGrid
        private void ReadJsonAndAppendDataGrid(string filePath)
        {
            if (File.Exists(filePath))
            {
                // Чтение JSON-данных из файла
                string json = File.ReadAllText(filePath);

                // Десериализация JSON-данных в список сотрудников
                List<EmployeeClass> employees = JsonConvert.DeserializeObject<List<EmployeeClass>>(json);

                // Установка списка сотрудников в источник данных для DataGrid
                employeesGrid.ItemsSource = employees;
            }
            else
            {
                MessageBox.Show("Файл export.json не найден в корневой папке.");
            }
        }
    }
}
