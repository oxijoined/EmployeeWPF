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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReadJsonAndAppendDataGrid("export.json");
        }

        private void employeesGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            List<EmployeeClass> employees = employeesGrid.Items.OfType<EmployeeClass>().ToList();
            int newId = employees.Max(emp => emp.ID) + 1;
            EmployeeClass newEmployee = new EmployeeClass(newId, "", "", "", "", "", "", "", "");

            EditEmployeeWindow editWindow = new EditEmployeeWindow(newEmployee, employees);
            editWindow.ShowDialog();

            employees.Add(newEmployee);
            employeesGrid.ItemsSource = employees;
            employeesGrid.Items.Refresh();
        }

        private void EditRecordButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            EmployeeClass employee = (EmployeeClass)button.DataContext;

            List<EmployeeClass> employees = employeesGrid.Items.OfType<EmployeeClass>().ToList();

            EditEmployeeWindow editWindow = new EditEmployeeWindow(employee, employees);
            editWindow.ShowDialog();
            employeesGrid.Items.Refresh();
        }

        private void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            EmployeeClass employee = (EmployeeClass)button.DataContext;

            if (employeesGrid.ItemsSource is List<EmployeeClass> employees)
            {
                employees.Remove(employee);
                employeesGrid.Items.Refresh();
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON Files (*.json)|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                string jsonFilePath = openFileDialog.FileName;
                ReadJsonAndAppendDataGrid(jsonFilePath);
            }
        }

        private void ExportSaveButton_Click(object sender, RoutedEventArgs e)
        {
            List<EmployeeClass> employees = employeesGrid.Items.OfType<EmployeeClass>().ToList();
            string json = JsonConvert.SerializeObject(employees, Formatting.Indented);
            File.WriteAllText("export.json", json);
            MessageBox.Show($"Успешно сохранено {employees.Count} записей");
        }

        private void CancelFilter_Click(object sender, RoutedEventArgs e)
        {
            foreach (EmployeeClass employee in employeesGrid.Items)
            {
                DataGridRow row = (DataGridRow)employeesGrid.ItemContainerGenerator.ContainerFromItem(employee);

                if (row != null)
                {
                    foreach (DataGridColumn column in employeesGrid.Columns)
                    {
                        if (column is DataGridTextColumn textColumn)
                        {
                            var cellContent = column.GetCellContent(row);
                            if (cellContent is TextBlock textBlock)
                            {
                                textBlock.Background = Brushes.White;
                                row.Visibility = Visibility.Visible;
                            }
                        }
                    }
                }
            }
        }

        private void SearchSelection_Click(object sender, RoutedEventArgs e)
        {
            if (FilterInput.Text == "")
            {
                MessageBox.Show("Необходимо ввести фильтр");
                return;
            }

            foreach (EmployeeClass employee in employeesGrid.Items)
            {
                DataGridRow row = (DataGridRow)employeesGrid.ItemContainerGenerator.ContainerFromItem(employee);

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
                                    textBlock.Background = Brushes.Yellow;
                                }
                                else
                                {
                                    textBlock.Background = Brushes.White;
                                }
                            }
                        }
                    }
                }
            }
        }


        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (EmployeeClass employee in employeesGrid.Items)
            {
                DataGridRow row = (DataGridRow)employeesGrid.ItemContainerGenerator.ContainerFromItem(employee);

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

        private void ReadJsonAndAppendDataGrid(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<EmployeeClass> employees = JsonConvert.DeserializeObject<List<EmployeeClass>>(json);
                employeesGrid.ItemsSource = employees;
            }
            else
            {
                MessageBox.Show("Файл export.json не найден в корневой папке.");
            }
        }
    }
}
