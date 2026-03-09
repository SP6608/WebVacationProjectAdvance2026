using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebVacantionManager.Models;

namespace WebVacationManager.Models
{
    public class VacationRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public bool IsHalfDay { get; set; }

        public bool IsApproved { get; set; } = false;

        [Required]
        public VacationType VacationType { get; set; }

        // Navigation
        public string ApplicantId { get; set; } = null!;

        [ForeignKey(nameof(ApplicantId))]
        public AppUser Applicant { get; set; } = null!;
    }

    public enum VacationType
    {
        PaidLeave,
        UnpaidLeave,
        SickLeave
    }
}