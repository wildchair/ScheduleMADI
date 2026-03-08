using Microsoft.EntityFrameworkCore;
using ScheduleMadi.Api.Repository;
using ScheduleMadi.Api.ServiceRegistrator;
using ScheduleMadi.Core.ApiClient;
using ScheduleMadi.Core.MadiSiteApiHelpers.Parsers;
using ScheduleMadi.Core.MadiSiteApiHelpers.Parsers.Interfaces;
using System.Text.Json.Serialization;

namespace ScheduleMadi.Api
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
            builder.Services.AddDbContext<InMemoryDbMadiContext>(options => options.UseInMemoryDatabase("ScheduleMadi"));

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