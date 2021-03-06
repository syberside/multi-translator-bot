﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EchoBot .NET Template version v4.11.1

using System.IO;
using System.Reflection;
using MultiTranslator.AzureBot.Services;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiTranslator.AzureBot.Services.UsageSamples;
using MultiTranslator.AzureBot.Services.UsageSamples.ContextReverso;
using MultiTranslator.AzureBot.Services.Commands;
using MultiTranslator.AzureBot.Services.Emoji;

namespace MultiTranslator.AzureBot
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
            services.AddControllers().AddNewtonsoftJson();

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, Bots.MultiTranslatorBot>();

            services
                .AddSingleton<TranslationClient>((sp) =>
                {
                    var binDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    //var rootDir = Path.GetFullPath(Path.Combine(binDir, ".."));
                    var configPath = Path.Combine(binDir, "gcp-creds.json");
                    var creds = GoogleCredential.FromFile(configPath);
                    return TranslationClient.Create(creds);
                })
                .AddSingleton<ILanguageDetector, SimpleLanguageDetector>()
                .AddSingleton<ITranslator, GoogleTranslateFacade>()
                .AddSingleton<IUsageSamplesProvider, ContextReversoUsageSamplesAdapter>()
                .AddSingleton<IContextReversoClient, ContextReversoClient>()
                .AddSingleton<ICommandParser, CommandParser>()
                .AddSingleton<ILanguageToEmojiConvertor, LanguageToEmojiConvertor>()
                .AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
    }
}
