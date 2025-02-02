using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using CardValidation.Core.Services;
using CardValidation.Core.Services.Interfaces;
using CardValidation.Infrustructure;
using System.Linq;

namespace UnitTests.CardValidation
{
    [TestFixture]
    public class StartupTests
    {
        private ServiceProvider _serviceProvider;
        private IServiceCollection _services;

        [SetUp]
        public void Setup()
        {
            var builder = WebApplication.CreateBuilder();
            _services = builder.Services;

            ConfigureServices(_services);

            _serviceProvider = _services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Register dependencies
            services.AddTransient<ICardValidationService, CardValidationService>();
            services.AddTransient<CreditCardValidationFilter>();

            services.AddMvc(options =>
            {
                options.Filters.AddService<CreditCardValidationFilter>();
            });
        }

        [TearDown]
        public void TearDown()
        {
            _serviceProvider?.Dispose();
        }

        [Test]
        public void ShouldRegisterCardValidationService()
        {
            // Act
            var service = _serviceProvider.GetService<ICardValidationService>();

            // Assert
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<CardValidationService>(service);
        }

        [Test]
        public void ShouldRegisterCreditCardValidationFilter()
        {
            // Act
            var mvcOptions = _serviceProvider.GetService<IOptions<MvcOptions>>();

            // Assert
            Assert.IsNotNull(mvcOptions, "MvcOptions should not be null");

            // Create a new instance of MvcOptions to verify the filters
            var options = mvcOptions.Value;

            var filter = options.Filters.OfType<ServiceFilterAttribute>().FirstOrDefault(f => f.ServiceType == typeof(CreditCardValidationFilter));
            Assert.IsNotNull(filter, "CreditCardValidationFilter should be registered");
        }

        [Test]
        public void ShouldConfigureMvcOptions()
        {
            // Act
            var mvcOptions = _serviceProvider.GetService<IOptions<MvcOptions>>();

            // Assert
            Assert.IsNotNull(mvcOptions, "MvcOptions should not be null");

            // Print all registered filters for diagnostic purposes
            var options = mvcOptions.Value;
            var filters = options.Filters.Select(f => f.GetType().Name).ToList();

            TestContext.WriteLine("Registered filters:");
            foreach (var filter in filters)
            {
                TestContext.WriteLine(filter);
            }
        }
    }
}
