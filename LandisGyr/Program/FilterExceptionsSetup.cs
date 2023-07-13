using LandisGyr.Exceptions;

namespace LandisGyr.Program
{
    public static class FilterExceptionsSetup
    {
        public static void AddFilterExceptions(this IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new FilterExceptions());
            });
        }
    }
}
