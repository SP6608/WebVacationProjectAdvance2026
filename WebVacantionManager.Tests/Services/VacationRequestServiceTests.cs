using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WebVacationManager.Models;
using WebVacantionManager.Cammon;
using WebVacantionManager.Data;
using WebVacantionManager.Services;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Tests.Services
{
    public class VacationRequestServiceTests
    {
        private ApplicationDbContext context;
        private VacationRequestService service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new ApplicationDbContext(options);
            service = new VacationRequestService(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public async Task CreateRequestAsync_ShouldCreateRequest()
        {
            var model = new VacationRequestCreateViewModel
            {
                DateFrom = DateTime.Today,
                DateTo = DateTime.Today.AddDays(2),
                VacationType = VacationType.PaidLeave
            };

            var result = await service.CreateRequestAsync(model, "user1");

            result.Should().BeTrue();
            context.VacationRequests.Count().Should().Be(1);
        }

        [Test]
        public async Task CreateRequestAsync_ShouldReturnFalse_WhenInvalidDates()
        {
            var model = new VacationRequestCreateViewModel
            {
                DateFrom = DateTime.Today,
                DateTo = DateTime.Today.AddDays(-1),
                VacationType = VacationType.PaidLeave
            };

            var result = await service.CreateRequestAsync(model, "user1");

            result.Should().BeFalse();
        }

        [Test]
        public async Task DeleteRequestAsync_ShouldReturnNotFound_WhenMissing()
        {
            var result = await service.DeleteRequestAsync(999, "user1", false, false);

            result.Should().Be(VacationRequestOperationResult.NotFound);
        }

        [Test]
        public async Task DeleteRequestAsync_ShouldDeleteRequest_WhenExists()
        {
            var request = new VacationRequest
            {
                DateFrom = DateTime.Today,
                DateTo = DateTime.Today.AddDays(1),
                ApplicantId = "user1",
                VacationType = VacationType.PaidLeave,
                Status = RequestStatus.Pending
            };

            context.VacationRequests.Add(request);
            await context.SaveChangesAsync();

            var result = await service.DeleteRequestAsync(request.Id, "user1", false, false);

            result.Should().Be(VacationRequestOperationResult.Success);
            context.VacationRequests.Count().Should().Be(0);
        }
    }
}