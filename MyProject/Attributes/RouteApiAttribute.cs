using Microsoft.AspNetCore.Mvc;

namespace MyProject.Attributes
{
    public class RouteApiAttribute : RouteAttribute
    {
        public RouteApiAttribute(string template): base($"api/{template}") { }
    }
}
