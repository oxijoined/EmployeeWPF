using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Employee
{
    public class EmployeeClass
    {

        public EmployeeClass(int iD, string firstName, string lastName, string surname, string phone, string email, string phoneNumber, string passportData, string photoUrl)
        {
            ID = iD;
            FirstName = firstName;
            LastName = lastName;
            Surname = surname;
            Phone = phone;
            EMail = email;
            PhoneNumber = phoneNumber;
            PassportData = passportData;
            PhotoPath = photoUrl;


        }

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }
        public string PhoneNumber { get; set; }
        public string PassportData { get; set; }
        public string PhotoPath { get; set; }


    }
}
