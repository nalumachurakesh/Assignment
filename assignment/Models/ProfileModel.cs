using Microsoft.AspNetCore.Mvc.Rendering;

namespace assignment.Models
{
    public class ProfileModel
    {
        public string Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Gender { get; set; }

        public List<SelectListItem> Cities { get; set; }
        public List<SelectListItem> States { get; set; }
    }
}
