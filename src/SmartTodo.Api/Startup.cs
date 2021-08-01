using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SmartTodo.Business;
using SmartTodo.Business.Models;
using SmartTodo.Business.Validators;
using SmartTodo.Data;

namespace SmartTodo.Api
{
    public class Startup
    {
        readonly string UiCorsPolicy = "_myAllowSpecificOrigins";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: UiCorsPolicy, builder =>
                {
                    builder.WithOrigins("http://localhost:3000");
                    builder.WithMethods("GET", "POST", "PUT", "DELETE");
                    builder.WithHeaders("*");
                });
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartTodo.Api", Version = "v1" });
            });

            services.AddDbContext<SmartTodoDbContext>();
            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<IValidator<CreateTodoItemRequest>, CreateTodoItemValidator>();
            services.AddScoped<IValidator<UpdateTodoItemRequest>, UpdateTodoItemValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartTodo.Api v1"));
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors(UiCorsPolicy);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
