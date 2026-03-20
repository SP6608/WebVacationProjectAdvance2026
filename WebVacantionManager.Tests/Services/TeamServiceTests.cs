using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WebVacantionManager.Data;
using WebVacantionManager.Models;
using WebVacantionManager.Services;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Tests.Services
{
    public class TeamServiceTests
    {
        private ApplicationDbContext context;
        private TeamService service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new ApplicationDbContext(options);
            service = new TeamService(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public async Task CreateAsync_ShouldCreateTeam()
        {
            var model = new TeamCreateViewModel
            {
                TeamName = "Backend Team"
            };

            await service.CreateAsync(model);

            var team = await context.Teams.FirstOrDefaultAsync();

            team.Should().NotBeNull();
            team!.TeamName.Should().Be("Backend Team");
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnTeams()
        {
            context.Teams.Add(new Team { TeamName = "Team1" });
            context.Teams.Add(new Team { TeamName = "Team2" });

            await context.SaveChangesAsync();

            var result = await service.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Test]
        public async Task EditAsync_ShouldEditTeam_WhenExists()
        {
            var team = new Team { TeamName = "Old" };

            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var model = new TeamsEditViewModel
            {
                Id = team.Id,
                TeamName = "New"
            };

            var result = await service.EditAsync(model);

            result.Should().BeTrue();

            var edited = await context.Teams.FindAsync(team.Id);
            edited!.TeamName.Should().Be("New");
        }

        [Test]
        public async Task EditAsync_ShouldReturnFalse_WhenNotExists()
        {
            var model = new TeamsEditViewModel
            {
                Id = 999,
                TeamName = "Test"
            };

            var result = await service.EditAsync(model);

            result.Should().BeFalse();
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteTeam_WhenExists()
        {
            var team = new Team { TeamName = "DeleteMe" };

            context.Teams.Add(team);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(team.Id);

            result.Should().BeTrue();
            context.Teams.Count().Should().Be(0);
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenNotExists()
        {
            var result = await service.DeleteAsync(999);

            result.Should().BeFalse();
        }
    }
}