using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebVacationManager.Models;

namespace WebVacantionManager.ViewModels
{
    public class VacationRequestCreateViewModel
    {
        [Required]
        [Display(Name = "Date From")]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }

        [Display(Name = "Half Day")]
        public bool IsHalfDay { get; set; }

        [Required]
        [Display(Name = "Vacation Type")]
        public VacationType VacationType { get; set; }

        public IEnumerable<SelectListItem> VacationTypes { get; set; }
            = new List<SelectListItem>();
    }
}
