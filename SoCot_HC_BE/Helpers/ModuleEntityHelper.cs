using System;
using System.Collections.Generic;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;

namespace SoCot_HC_BE.Helpers
{
    public static class ModuleEntityTypeHelper
    {
        private static readonly Dictionary<int, Type> _moduleEntityMap = new()
        {
            { (int)ModuleEnum.PatientRegistry, typeof(PatientRegistry) },
            // Add other mappings here
            // { (int)ModuleEnum.RequestSlip, typeof(RequestSlip) },
        };

        public static Type? GetEntityTypeByModuleId(int moduleId)
        {
            _moduleEntityMap.TryGetValue(moduleId, out var type);
            return type;
        }
    }
}
