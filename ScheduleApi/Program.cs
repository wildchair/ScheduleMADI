using Microsoft.EntityFrameworkCore;
using ScheduleApi.Repository;
using ScheduleApi.ServiceRegistrator;
using ScheduleCore.ApiClient;
using ScheduleCore.MadiSiteApiHelpers.Parsers;
using ScheduleCore.MadiSiteApiHelpers.Parsers.Interfaces;
using System.Text.Json.Serialization;

namespace ScheduleApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            builder.Services.AddInjectables();

            builder.Services.AddSingleton<IParser, HtmlHardcoreParser>();

            //builder.Services.AddHttpContextAccessor();
            //builder.Services.AddTransient<AuthHeaderHandler>();

            builder.Services.Configure<UniversityApiSettings>(builder.Configuration.GetSection(nameof(UniversityApiSettings)));
            builder.Services.AddHttpClient<UniversityApiClient>();

            builder.Services.AddDbContext<InMemoryDbContext>(options => options.UseInMemoryDatabase("Schedule"));
            builder.Services.AddDbContext<InMemoryDbMadiContext>(options => { options.UseInMemoryDatabase("ScheduleMadi"); options.EnableSensitiveDataLogging(); });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            //app.UseMiddleware<RequestLoggingMiddleware>();

            //app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}