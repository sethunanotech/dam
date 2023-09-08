using DAM.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAM.Application
{
    public static class ServiceExtension 
    {
        public static void ConfigureApplication(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<OrganizationRequestValidator>();
        }
    }
}
