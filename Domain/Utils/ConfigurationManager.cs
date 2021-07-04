using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Domain.BaseTypes;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace Domain.Utils
{
    public class ConfigurationManager
    {

        public static ConfigurationManager CreateForIntegrationTest(string rootPath)
        {
            var builder = BuildBaseConfiguration(rootPath, EnvironmentName);

            return new ConfigurationManager(builder.Build());
        }

        public static ConfigurationManager CreateForWebAndService(string rootPath, string environmentName)
        {
            var awsOptions = new AWSOptions
            {
                Region = RegionEndpoint.APSoutheast2
            };

            var builder = BuildBaseConfiguration(rootPath, environmentName);

            return new ConfigurationManager(builder.Build());
        }

        private ConfigurationManager(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }

        private static IConfigurationBuilder BuildBaseConfiguration(string rootPath, string environmentName)
        {
            Log.Information("Building Configuration from Path {rootPath} for environmentName {environmentName}", rootPath, environmentName.ToLower());

            var builder = new ConfigurationBuilder().SetBasePath(rootPath)
                                                    .AddJsonFile("appsettings.json", true, true)
                                                    .AddJsonFile($"appsettings.{environmentName.ToLower()}.json", true, true)
                                                    .AddJsonFile($"appsettings.{environmentName.ToLower()}.private.json", true, true)
                                                    .AddEnvironmentVariables();

            return builder;
        }

        public static string EnvironmentName => GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();
        
        public ConnectionString ConnectionString => GetConfig<ConnectionString>("ConnectionStrings");
        
        public IConfigurationRoot Configuration { get; }

        public static string GetVersionNumber()
        {
            return Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion;
        }

        public T GetConfig<T>(string sectionName) where T : new()
        {
            var config = new T();
            Configuration.GetSection(sectionName).Bind(config);
            return config;
        }

        public static bool IsBuildServer() => EnvironmentName == "circleci";
        public static bool IsDevelopment() => EnvironmentName == "development";
        public static bool IsStaging() => EnvironmentName == "staging";
        public static bool IsProduction() => EnvironmentName == "production";

        private static string GetEnvironmentVariable(string variable)
        {
            var value = Environment.GetEnvironmentVariable(variable);
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException($"Unable to start application. You are missing the {variable} environment variable.");
            }
            return value;
        }

        public static string GetGlobalVersionNumber()
        {
            return $"BOOKREADER-{GetVersionNumber()}";
        }
    }
}
