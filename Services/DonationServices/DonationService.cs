using Inveasy.Data;
using Inveasy.Models;
using Inveasy.Services;
using Microsoft.EntityFrameworkCore;
using static Inveasy.Services.ServiceStatus;
using Inveasy.Services.UserServices;
using Inveasy.Services.ProjectServices;

namespace Inveasy.Services.DonationServices
{
    public class DonationService : IDonationService
    {
        private readonly InveasyContext _context;
        private readonly DonationStatus _status;

        private readonly IUserService _userService;
        private readonly IProjectService _projectService;

        public string StatusMessage { get; private set; } = "DonationService initialized";

        public DonationService(InveasyContext context, DonationStatus serviceStatus, IUserService userService, IProjectService projectService)
        {
            _context = context;
            _status = serviceStatus;
            _userService = userService;
            _projectService = projectService;
        }

        #region Donation GET services
        // Method that gets donations with given donation id
        public async Task<Donation> GetDonationAsync(int donationId)
        {
            var donations = await GetDonationsAsync();
            return donations?.FirstOrDefault(d => d.Id == donationId);
        }

        // Method that gets donations made by a user with given username
        public async Task<List<Donation>> GetUserDonationsAsync(string username) => await GetUserDonationsAsync(await _userService.GetUserAsync(username));

        // Method that gets donations made by a user with given user id
        public async Task<List<Donation>> GetUserDonationsAsync(int userId) => await GetUserDonationsAsync(await _userService.GetUserAsync(userId));

        // Method that gets donations made by a user with given user
        public async Task<List<Donation>> GetUserDonationsAsync(User user)
        {
            // If user doesn't exist, do nothing
            if (user == null)
            {
                StatusMessage = _status.ErrorGetStatus("Null user");
                return null;
            }

            var donations = await GetDonationsAsync();
            return donations?.Where(d => d.User.Id == user.Id).ToList();
        }

        public async Task<List<Donation>> GetDonationsAsync()
        {
            try
            {
                return await _context.Donation
                    ?.Include(p => p.Project)
                    ?.Include(s => s.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionGetStatus(ex.Message);
                return null;
            }
        }
        #endregion

        #region Donation ADD services
        public async Task<bool> AddDonationAsync(int projectId, string username, double donationAmount)
            => await AddDonationAsync(await _projectService.GetProjectAsync(projectId), await _userService.GetUserAsync(username), donationAmount);

        public async Task<bool> AddDonationAsync(Project project, string username, double donationAmount)
            => await AddDonationAsync(project, await _userService.GetUserAsync(username), donationAmount);

        public async Task<bool> AddDonationAsync(Project project, int userId, double donationAmount)
            => await AddDonationAsync(project, await _userService.GetUserAsync(userId), donationAmount);

        public async Task<bool> AddDonationAsync(int projectId, int userId, double donationAmount)
            => await AddDonationAsync(await _projectService.GetProjectAsync(projectId), await _userService.GetUserAsync(userId), donationAmount);

        public async Task<bool> AddDonationAsync(Project project, User user, double donationAmount)
        {
            if (project == null)
            {
                StatusMessage = _status.ErrorAddStatus("Null project");
                return false;
            }

            if (user == null)
            {
                StatusMessage = _status.ErrorAddStatus("Null user");
                return false;
            }

            // Create Donation
            Donation donation = new Donation
            {
                Amount = Math.Round(donationAmount, 2),
                Date = DateTime.Now,
                User = user,
                Project = project
            };

            var donationAdded = await AddDonationAsync(donation);

            if (!donationAdded)
                return false;

            // Add donation amount to project
            project.FundAmount += donationAmount;

            return true;
        }

        public async Task<bool> AddDonationAsync(Donation donation)
        {
            if (donation == null)
            {
                StatusMessage = _status.ErrorAddStatus("Null donation", "");
                return false;
            }

            // Don't allow empty or negative donations
            if (donation.Amount <= 0)
            {
                StatusMessage = _status.ErrorAddStatus("Donation amount equal to or less than zero", donation.Amount.ToString());
                return false;
            }

            try
            {
                // Store Donation
                _context.Donation.AddAsync(donation);
                await _context.SaveChangesAsync();
                StatusMessage = _status.SuccessAddStatus(donation.Id.ToString());
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionAddStatus(ex.Message, donation.Amount.ToString());
                return false;
            }
        }
        #endregion

    }
}
