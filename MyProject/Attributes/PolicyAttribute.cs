using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace MyProject.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PolicyAttribute : AuthorizeAttribute
    {
        public PolicyAttribute(params UserRole[] roles) {

            Policy = string.Join(",", roles.Select(x => $"{x}"));
        }
    }
}
