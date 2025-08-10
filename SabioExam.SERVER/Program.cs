
using SabioExam.DATA;
using SabioExam.SERVER.Services;
using SabioExam.SERVER.Utilities.Classes;

namespace SabioExam.SERVER
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<DataContext, DataContext>();
            builder.Services.AddTransient<IDiscountService, DiscountService>();
            var app = builder.Build();

            var hubClient = builder.Configuration.GetValue<string>("AllowedHubClientOrigin") ?? "";
            app.UseCors(builder =>
            {
                builder
                    .WithOrigins(hubClient)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHub<DiscountHub>("hub/discount");
            app.Run();
        }
    }
}
