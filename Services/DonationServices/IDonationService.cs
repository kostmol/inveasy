using Inveasy.Models;

namespace Inveasy.Services.DonationServices
{
    public interface IDonationService
    {
        public interface IDonationService
        {
            string StatusMessage { get; }

            Task<Donation> GetDonationAsync(int donationId);
            Task<List<Donation>> GetUserDonationsAsync(string username);
            Task<List<Donation>> GetUserDonationsAsync(int userId);
            Task<List<Donation>> GetUserDonationsAsync(User user);
            Task<List<Donation>> GetDonationsAsync();
            Task<bool> AddDonationAsync(int projectId, string username, double donationAmount);
            Task<bool> AddDonationAsync(Project project, string username, double donationAmount);
            Task<bool> AddDonationAsync(Project project, int userId, double donationAmount);
            Task<bool> AddDonationAsync(int projectId, int userId, double donationAmount);
            Task<bool> AddDonationAsync(Project project, User user, double donationAmount);
            Task<bool> AddDonationAsync(Donation donation);
        }
    }
}
