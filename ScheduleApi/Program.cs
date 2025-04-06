using ScheduleApi.Middlewares;
using ScheduleApi.ServiceRegistrator;
using ScheduleCore.ApiClient;
using ScheduleCore.MadiSiteApiHelpers.Parsers;
using ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces;

namespace ScheduleApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddInjectables();

            builder.Services.AddSingleton<IParser, HtmlHardcoreParser>();

            //builder.Services.AddHttpContextAccessor();
            //builder.Services.AddTransient<AuthHeaderHandler>();

            builder.Services.Configure<UniversityApiSettings>(builder.Configuration.GetSection(nameof(UniversityApiSettings)));
            builder.Services.AddHttpClient<UniversityApiClient>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseMiddleware<RequestLoggingMiddleware>();

            //app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}