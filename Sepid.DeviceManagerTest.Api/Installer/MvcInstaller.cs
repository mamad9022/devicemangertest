using AutoMapper;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Sepid.DeviceManagerTest.Api.Middleware;
using Sepid.DeviceManagerTest.Application.Common.AutoMapper;
using Sepid.DeviceManagerTest.Common.Options;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using System;
using Sepid.DeviceManagerTest.Common.DeviceErrorMessage;
using Sepid.DeviceManagerTest.Common.DeviceServices;
using Sepid.DeviceManagerTest.Common.LanguageService;
using Sepid.DeviceManagerTest.Api.Installer;
using ZK;

namespace Sepid.DeviceManager.Api.Installer
{
    public class MvcInstaller : IInstaller

    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",
                    builder => { builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod(); });
            });

            services.AddControllers(opt => opt.Filters.Add<OnExceptionMiddleware>())
                .AddFluentValidation(mvcConfiguration =>
                    mvcConfiguration.RegisterValidatorsFromAssemblyContaining<MappingProfile>()).AddNewtonsoftJson();

            #region add Dependency

            services.Configure<FileExtensions>(configuration.GetSection("FileExtensions"));
            services.Configure<DataStorage>(configuration.GetSection("DataStorage"));
            services.Configure<RabbitMqConnection>(configuration.GetSection("RabbitMqConnection"));
            services.Configure<HostAddress>(configuration.GetSection("HostAddress"));
            services.Configure<RedisConfiguration>(configuration.GetSection("RedisConfiguration"));
            services.Configure<Setting>(configuration.GetSection("Setting"));
            services.Configure<ErrorCodeDescription>(configuration.GetSection("ErrorCodeDescription"));
            services.Configure<MultiLingualOptions>(configuration.GetSection("MultiLingual"));
            services.Configure<Applications>(configuration.GetSection("Applications"));
            services.Configure<SystemSetting>(configuration.GetSection("SystemSetting"));

            services.TryAddSingleton<IDataStorage>
                (sp => sp.GetRequiredService<IOptions<DataStorage>>().Value);

            services.TryAddSingleton<IRabbitMqConnection>
                (sp => sp.GetRequiredService<IOptions<RabbitMqConnection>>().Value);

            services.AddScoped<IBusPublish, BusPublish>();
            services.AddScoped<IBusSubscribe, BusSubscribe>();

            //services.TryAddEnumerable(new[]
            //{
            //    ServiceDescriptor.Singleton<IInitializedService,ZKServices>(),
            //    //ServiceDescriptor.Singleton<IInitializedService, BioStarV1Services>()
            //});

            services.TryAddEnumerable(new[]
            {
                ServiceDescriptor.Scoped<IDeviceOperationServices,ZKServices>(),
                //ServiceDescriptor.Scoped<IDeviceOperationServices, BioStarV1Services>()
            });


            #endregion add Dependency

            #region Redis

            var redisConfiguration = new RedisConfiguration();
            configuration.Bind(nameof(RedisConfiguration), redisConfiguration);
            services.AddSingleton(redisConfiguration);

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = redisConfiguration.Connection;
            });

            #endregion Redis

            #region HangFire

            services.AddHangfire(conf =>
                {
                    conf
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseSqlServerStorage(configuration.GetConnectionString("HangFireConnection"),
                            new SqlServerStorageOptions
                            {
                                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                QueuePollInterval = TimeSpan.Zero,
                                UseRecommendedIsolationLevel = true,
                                UsePageLocksOnDequeue = true,
                                DisableGlobalLocks = true,
                            });

                    
                }
            );


     
            #endregion HangFire

            #region Automapper

            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion Automapper

            #region HealtCheck

            var rabbitMqConnection = new RabbitMqConnection();
            configuration.Bind(nameof(RabbitMqConnection), rabbitMqConnection);
            services.AddSingleton(rabbitMqConnection);

            services.AddHealthChecks()
                .AddSqlServer(configuration["ConnectionStrings:DeviceManagerContext"], name: "Device Db")
                .AddRedis(configuration["RedisConfiguration:Connection"], " redis db")
                .AddHangfire(config => config.MaximumJobsFailed = 20, "Hang fire")
                .AddRabbitMQ(sp =>
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = rabbitMqConnection.Server,
                        UserName = rabbitMqConnection.UserName,
                        Password = rabbitMqConnection.Password
                    };

                    return factory.CreateConnection();
                });

            #endregion HealtCheck

            #region Swagger

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Device Manager Api",
                    Version = "v1.0",
                    Description = "Device Manager ASP.NET Core Web Api",
                });

                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            #endregion Swagger
        }
    }
}