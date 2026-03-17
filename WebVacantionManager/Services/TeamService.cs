using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebVacantionManager.Data;
using WebVacantionManager.Models;
using WebVacantionManager.Services.Contracts;
using WebVacantionManager.ViewModels;

namespace WebVacantionManager.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext context;

        public TeamService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<TeamIndexViewModel>> GetAllAsync()
        {
            return await context.Teams
                .AsNoTracking()
                .Include(t => t.Project)
                .Include(t => t.TeamLeader)
                .Include(t => t.Developers)
                .Select(t => new TeamIndexViewModel
                {
                    Id = t.Id,
                    TeamName = t.TeamName,
                    ProjectName = t.Project != null ? t.Project.ProjectName : null,
                    TeamLeaderName = t.TeamLeader != null ? t.TeamLeader.UserName : null,
                    DevelopersCount = t.Developers.Count
                })
                .ToListAsync();
        }

        public async Task<TeamsDetailsViewModel?> GetByIdAsync(int id)
        {
            Team? team = await context.Teams
                .AsNoTracking()
                .Include(t => t.Project)
                .Include(t => t.TeamLeader)
                .Include(t => t.Developers)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
            {
                return null;
            }

            return new TeamsDetailsViewModel
            {
                Id = team.Id,
                TeamName = team.TeamName,
                ProjectName = team.Project != null ? team.Project.ProjectName : null,
                TeamLeaderName = team.TeamLeader != null ? team.TeamLeader.UserName : null,
                Developers = team.Developers.ToList()
            };
        }

        public async Task<TeamCreateViewModel> GetCreateModelAsync()
        {
            return new TeamCreateViewModel
            {
                Projects = await GetProjectsAsync(),
                TeamLeaders = await GetTeamLeadersAsync()
            };
        }

        public async Task PopulateCreateModelAsync(TeamCreateViewModel model)
        {
            model.Projects = await GetProjectsAsync();
            model.TeamLeaders = await GetTeamLeadersAsync();
        }

        public async Task CreateAsync(TeamCreateViewModel model)
        {
            Team team = new Team
            {
                TeamName = model.TeamName,
                ProjectId = model.ProjectId,
                TeamLeaderId = model.TeamLeaderId
            };

            await context.Teams.AddAsync(team);
            await context.SaveChangesAsync();
        }

        public async Task<TeamsEditViewModel?> GetForEditAsync(int id)
        {
            Team? team = await context.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
            {
                return null;
            }

            return new TeamsEditViewModel
            {
                Id = team.Id,
                TeamName = team.TeamName,
                ProjectId = team.ProjectId,
                TeamLeaderId = team.TeamLeaderId,
                Projects = await GetProjectsAsync(),
                TeamLeaders = await GetTeamLeadersAsync()
            };
        }

        public async Task PopulateEditModelAsync(TeamsEditViewModel model)
        {
            model.Projects = await GetProjectsAsync();
            model.TeamLeaders = await GetTeamLeadersAsync();
        }

        public async Task<bool> EditAsync(TeamsEditViewModel model)
        {
            Team? team = await context.Teams.FindAsync(model.Id);

            if (team == null)
            {
                return false;
            }

            team.TeamName = model.TeamName;
            team.ProjectId = model.ProjectId;
            team.TeamLeaderId = model.TeamLeaderId;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<TeamsDeleteViewModel?> GetForDeleteAsync(int id)
        {
            Team? team = await context.Teams
                .AsNoTracking()
                .Include(t => t.Project)
                .Include(t => t.TeamLeader)
                .Include(t => t.Developers)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
            {
                return null;
            }

            return new TeamsDeleteViewModel
            {
                Id = team.Id,
                TeamName = team.TeamName,
                ProjectName = team.Project != null ? team.Project.ProjectName : null,
                TeamLeaderName = team.TeamLeader != null
                    ? $"{team.TeamLeader.FirstName} {team.TeamLeader.LastName}"
                    : null,
                Developers = team.Developers.ToList()
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Team? team = await context.Teams.FindAsync(id);

            if (team == null)
            {
                return false;
            }

            context.Teams.Remove(team);
            await context.SaveChangesAsync();

            return true;
        }

        private async Task<List<SelectListItem>> GetProjectsAsync()
        {
            return await context.Projects
                .AsNoTracking()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.ProjectName
                })
                .ToListAsync();
        }

        private async Task<List<SelectListItem>> GetTeamLeadersAsync()
        {
            return await context.Users
                .AsNoTracking()
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName!
                })
                .ToListAsync();
        }
    }
}