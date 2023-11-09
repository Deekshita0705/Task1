using Microsoft.AspNetCore.Mvc.Rendering;

namespace Task1.Models
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BranchName { get; set; }
        public string YearName { get; set; }

        public int BranchId { get; set; }
        public List<SelectListItem> Branches { get; set; }
    }
}
