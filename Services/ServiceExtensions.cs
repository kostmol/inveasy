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
    }
}
