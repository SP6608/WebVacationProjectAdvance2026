using Microsoft.EntityFrameworkCore;
using WebVacantionManager.Data;
using WebVacantionManager.Models;
using WebVacantionManager.Services.Contracts;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext context;

        public ProjectService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<ProjectDetailsViewModel>> GetAllAsync()
        {
            return await context
                .Projects
                .AsNoTracking()
                .Select(p => new ProjectDetailsViewModel
                {
                    Id = p.Id,
                    ProjectName = p.ProjectName,
                    Description = p.Description,
                })
                .OrderBy(p => p.ProjectName)
                .ToListAsync();
        }

        public async Task<ICollection<ProjectDetailsViewModel>> GetPagedAsync(int page, int pageSize)
        {
            return await context
                .Projects
                .AsNoTracking()
                .OrderBy(p => p.ProjectName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProjectDetailsViewModel
                {
                    Id = p.Id,
                    ProjectName = p.ProjectName,
                    Description = p.Description
                })
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await context.Projects.CountAsync();
        }

        public async Task<ProjectDetailsViewModel?> GetByIdAsync(int id)
        {
            return await context
                .Projects
                .AsNoTracking()
                .Include(p => p.Teams)
                .Select(p => new ProjectDetailsViewModel
                {
                    Id = p.Id,
                    ProjectName = p.ProjectName,
                    Description = p.Description,
                    Teams = p.Teams.ToList()
                })
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateAsync(ProjectsCreateViewModel model)
        {
            Project project = new Project
            {
                ProjectName = model.Projectname,
                Description = model.Description
            };

            await context.Projects.AddAsync(project);
            await context.SaveChangesAsync();
        }

        public async Task<ProjectEditViewModel?> GetForEditAsync(int id)
        {
            return await context
                .Projects
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProjectEditViewModel
                {
                    Id = p.Id,
                    ProjectName = p.ProjectName,
                    Description = p.Description
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> EditAsync(ProjectEditViewModel model)
        {
            Project? project = await context.Projects.FindAsync(model.Id);

            if (project == null)
            {
                return false;
            }

            project.ProjectName = model.ProjectName;
            project.Description = model.Description;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<ProjectDeleteViewModel?> GetForDeleteAsync(int id)
        {
            return await context
                .Projects
                .AsNoTracking()
                .Include(p => p.Teams)
                .Where(p => p.Id == id)
                .Select(p => new ProjectDeleteViewModel
                {
                    Id = p.Id,
                    ProjectName = p.ProjectName,
                    Description = p.Description,
                    Teams = p.Teams
                        .Select(t => new TeamSimpleViewModel
                        {
                            Id = t.Id,
                            TeamName = t.TeamName
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Project? project = await context.Projects.FindAsync(id);

            if (project == null)
            {
                return false;
            }

            context.Projects.Remove(project);
            await context.SaveChangesAsync();

            return true;
        }
    }
}