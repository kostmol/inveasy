using Inveasy.Services.CommentServices;
using Inveasy.Services.DonationServices;
using Inveasy.Services.ProjectServices;
using Inveasy.Services.UserServices;
using Inveasy.Services.ViewServices;

namespace Inveasy.Services
{
    public static class ServiceExtensions
    {
        public static void AddDatabaseServices(this IServiceCollection services) 
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();            

            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IDonationService, DonationService>();
            services.AddScoped<IViewService, ViewService>();
            services.AddScoped<ICommentService, CommentService>();            
        }

        public static void AddStatusServices(this IServiceCollection services)
        {
            services.AddScoped(_ => new ServiceStatus(""));
            services.AddScoped<ServiceStatus.UserStatus>();
            services.AddScoped<ServiceStatus.RoleStatus>();
            services.AddScoped<ServiceStatus.ProjectStatus>();
            services.AddScoped<ServiceStatus.CategoryStatus>();
            services.AddScoped<ServiceStatus.DonationStatus>();
            services.AddScoped<ServiceStatus.ViewStatus>();
            services.AddScoped<ServiceStatus.CommentStatus>();
        }
    }
}
