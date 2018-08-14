using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Net.Http.Formatting;
using System.Web.Http;
using Tntp.WebApplication.Models;

namespace Tntp.WebApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            // Web API configuration and services
            var container = new Container();
            container.Register<IRepository, DatabaseRepository>(new AsyncScopedLifestyle());
            container.RegisterWebApiControllers(configuration);
            container.Verify();

            // We want JSON to be the only content/media type.
            // In the future, we might not want to go through content negotiation at all - see https://www.strathweb.com/2013/06/supporting-only-json-in-asp-net-web-api-the-right-way/
            // for implementation details.
            configuration.Formatters.Clear();
            configuration.Formatters.Add(new JsonMediaTypeFormatter());

            // Web API routes
            configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            configuration.MapHttpAttributeRoutes();
        }
    }
}