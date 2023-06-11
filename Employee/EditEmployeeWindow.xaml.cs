using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Employee
{
    /// <summary>
    /// Логика взаимодействия для EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : Window
    {
        private EmployeeClass employee;
        private List<EmployeeClass> employees;

        public EditEmployeeWindow(EmployeeClass employee, List<EmployeeClass> employees)
        {
            InitializeComponent();

            this.employee = employee;
            this.employees = employees;

            FirstNameInput.Text = employee.FirstName;
            LastNameInput.Text = employee.LastName;
            SurnameInput.Text = employee.Surname;
            PhoneInput.Text = employee.Phone;
            EMailInput.Text = employee.EMail;
            PassportInput.Text = employee.PassportData;
            PhotoPathInput.Text = employee.PhotoPath;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            employee.FirstName = FirstNameInput.Text;
            employee.LastName = LastNameInput.Text;
            employee.Surname = SurnameInput.Text;
            employee.Phone = PhoneInput.Text;
            employee.EMail = EMailInput.Text;
            employee.PassportData = PassportInput.Text;
            employee.PhotoPath = PhotoPathInput.Text;

            int index = employees.FindIndex(emp => emp.ID == employee.ID);
            if (index != -1)
            {
                employees[index] = employee;
            }

            this.Close();
        }
    }


}
