namespace WebVacantionManager.ViewModels
{
    public class VacationRequestDetailsViewModel
    {
        public int Id { get; set; }
        public string ApplicantName { get; set; } = null!;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime CreatedOn { get; set; }
        public string VacationType { get; set; } = null!;
        public bool IsHalfDay { get; set; }
        public string Status { get; set; } = null!;
    }
}