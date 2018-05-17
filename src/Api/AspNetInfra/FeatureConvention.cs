using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Api.AspNetInfra
{
    public class FeatureConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.Properties.Add("module",
                GetFeatureName(controller.ControllerType));
        }

        string GetFeatureName(TypeInfo controllerType)
        {
            var tokens = controllerType.FullName.Split('.');
            if (tokens.All(t => t != "Modules")) return "";
            var featureName = tokens
                .SkipWhile(t => !t.Equals("modules",
                    StringComparison.CurrentCultureIgnoreCase))
                .Skip(1)
                .Take(1)
                .FirstOrDefault();
            return featureName;
        }
    }
}