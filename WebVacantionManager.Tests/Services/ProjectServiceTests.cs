using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WebVacantionManager.Data;
using WebVacantionManager.Models;
using WebVacantionManager.Services;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Tests.Services
{
    public class ProjectServiceTests
    {
        private ApplicationDbContext context;
        private ProjectService service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new ApplicationDbContext(options);
            service = new ProjectService(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        [Test]
        public async Task CreateAsync_ShouldCreateProject()
        {
            var model = new ProjectsCreateViewModel
            {
                Projectname = "Test Project",
                Description = "Test Description"
            };

            await service.CreateAsync(model);

            var project = await context.Projects.FirstOrDefaultAsync();

            project.Should().NotBeNull();
            project!.ProjectName.Should().Be("Test Project");
            project.Description.Should().Be("Test Description");
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAllProjects()
        {
            context.Projects.Add(new Project
            {
                ProjectName = "Project A",
                Description = "Desc A"
            });

            context.Projects.Add(new Project
            {
                ProjectName = "Project B",
                Description = "Desc B"
            });

            await context.SaveChangesAsync();

            var result = await service.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnProject_WhenExists()
        {
            var project = new Project
            {
                ProjectName = "Existing Project",
                Description = "Existing Description"
            };

            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var result = await service.GetByIdAsync(project.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(project.Id);
            result.ProjectName.Should().Be("Existing Project");
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnNull_WhenProjectDoesNotExist()
        {
            var result = await service.GetByIdAsync(999);

            result.Should().BeNull();
        }

        [Test]
        public async Task EditAsync_ShouldReturnFalse_WhenProjectDoesNotExist()
        {
            var model = new ProjectEditViewModel
            {
                Id = 999,
                ProjectName = "Edited Name",
                Description = "Edited Description"
            };

            var result = await service.EditAsync(model);

            result.Should().BeFalse();
        }

        [Test]
        public async Task EditAsync_ShouldEditProject_WhenProjectExists()
        {
            var project = new Project
            {
                ProjectName = "Old Name",
                Description = "Old Description"
            };

            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var model = new ProjectEditViewModel
            {
                Id = project.Id,
                ProjectName = "New Name",
                Description = "New Description"
            };

            var result = await service.EditAsync(model);

            result.Should().BeTrue();

            var editedProject = await context.Projects.FindAsync(project.Id);
            editedProject!.ProjectName.Should().Be("New Name");
            editedProject.Description.Should().Be("New Description");
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnFalse_WhenProjectDoesNotExist()
        {
            var result = await service.DeleteAsync(999);

            result.Should().BeFalse();
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteProject_WhenProjectExists()
        {
            var project = new Project
            {
                ProjectName = "To Delete",
                Description = "Delete Description"
            };

            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(project.Id);

            result.Should().BeTrue();
            context.Projects.Count().Should().Be(0);
        }
    }
}