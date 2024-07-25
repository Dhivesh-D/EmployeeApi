using System.Xml.Serialization;

namespace EmployeeAPI.Models
{

    [XmlRoot("Employee")]
    public class Employee
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("FirstName")]
        public string FirstName { get; set; } =string.Empty;

        [XmlElement("LastName")]
        public string LastName { get; set; } = string.Empty;

        [XmlElement("Designation")]
        public string Designation { get; set; } = string.Empty;

        [XmlElement("Email")]
        public string Email { get; set; } = string.Empty;
    }

    public class EmployeeWrapper
    {
        public required Employee Employee { get; set; }
    }

}
