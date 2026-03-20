using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebVacantionManager.Models;
using WebVacationManager.Models;

namespace WebVacantionManager.Cammon
{
    public class VacationRequestsTypeConfiguration : IEntityTypeConfiguration<VacationRequest>
    {
        private readonly ICollection<VacationRequest> requests =
            new HashSet<VacationRequest>()
            {
                new VacationRequest
                {
                    Id = 1,
                    DateFrom = new DateTime(2026, 6, 1),
                    DateTo = new DateTime(2026, 6, 5),
                    CreatedOn = new DateTime(2026, 5, 20),
                    IsHalfDay = false,
                  Status = RequestStatus.Pending,
                    VacationType = VacationType.PaidLeave,
                    ApplicantId = "3f3c3333-3333-3333-3333-333333333333"
                },
                new VacationRequest
                {
                    Id = 2,
                    DateFrom = new DateTime(2026, 7, 10),
                    DateTo = new DateTime(2026, 7, 12),
                    CreatedOn = new DateTime(2026, 6, 1),
                    IsHalfDay = true,
                Status = RequestStatus.Pending,
                    VacationType = VacationType.UnpaidLeave,
                    ApplicantId = "2f2b2222-2222-2222-2222-222222222222"
                },
                new VacationRequest
                {
                    Id = 3,
                    DateFrom = new DateTime(2026, 8, 3),
                    DateTo = new DateTime(2026, 8, 8),
                    CreatedOn = new DateTime(2026, 7, 15),
                    IsHalfDay = false,
                    Status = RequestStatus.Pending,
                    VacationType = VacationType.SickLeave,
                    ApplicantId = "3f3c3333-3333-3333-3333-333333333333"
                },
                new VacationRequest
                {
                    Id = 4,
                    DateFrom = new DateTime(2026, 3, 14),
                    DateTo = new DateTime(2026, 3, 17),
                    CreatedOn = new DateTime(2026, 3, 10),
                    IsHalfDay = false,
                    Status = RequestStatus.Pending,
                    VacationType = VacationType.SickLeave,
                    ApplicantId = "1f1a1111-1111-1111-1111-111111111111"
                }
            };

        public void Configure(EntityTypeBuilder<VacationRequest> builder)
        {
            builder.HasData(requests);
        }
    }
}