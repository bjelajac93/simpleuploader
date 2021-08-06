using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using ApplicantsApi.Data.Database;
using Microsoft.EntityFrameworkCore;
using ApplicantsApi.Data.Repository;
using SimpleUploaderAPI.Data.Repository;
using MediatR;
using System.Collections.Generic;
using SimpleUploaderAPI.Service.UploadDownloadService.Query;
using SimpleUploaderAPI.Domain.Entities;
using SimpleUploaderAPI.Service.UploadDownloadService.Command;
using System.IO;
using System.Reflection;
using System;

namespace SimpleUploaderAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContextPool<ApplicantContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy(name: "MyAllowSpecificOrigins",
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200");
                                  });
            });
            services.AddControllers();

            // configure DI for application services
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUploadDownloadRepository, UploadDownloadRepository>();

            services.AddTransient<IRequestHandler<GetFilesQuery, List<FileData>>, GetFilesQueryHandler>();
            services.AddTransient<IRequestHandler<CreateFileCommand, FileData>, CreateFileCommandHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.WebRootPath, "uploads")),
                RequestPath = "/UploadedFiles"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.WebRootPath, "uploads")),
                RequestPath = "/UploadedFiles"
            });

            app.UseRouting();

            app.UseCors("MyAllowSpecificOrigins");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}