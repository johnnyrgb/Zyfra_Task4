
using Microsoft.EntityFrameworkCore;
using Zyfra_Task4.BusinessLogic.Interfaces;
using Zyfra_Task4.BusinessLogic.Services;
using Zyfra_Task4.DataAccess;
using Zyfra_Task4.DataAccess.Interfaces;
using Zyfra_Task4.DataAccess.Repositories;

namespace Zyfra_Task4.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // ��������� ������ ����������� �� appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            // ����������� DbContext � �������������� SQLite
            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlite(connectionString);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            // ����������� ������ ��������
            builder.Services.AddTransient<DatabaseSeed>();
            builder.Services.AddTransient<IDataEntryService, DataEntryService>();
            builder.Services.AddTransient<IDbRepository, DbRepository>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            // ����������� ���� ������
            using (var scope = app.Services.CreateScope())
            {
                var seed = scope.ServiceProvider.GetRequiredService<DatabaseSeed>();
                seed.SeedAsync().Wait();
            }
            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
