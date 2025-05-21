using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Services.Interfaces;

public class ModuleServiceMapper
{
    private readonly IServiceProvider _serviceProvider;

    public ModuleServiceMapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object? GetServiceByModuleId(int moduleId)
    {
        return moduleId switch
        {
            (int)ModuleEnum.PatientRegistry => _serviceProvider.GetService<IPatientRegistryService>(),
            // add other modules here...
            _ => null
        };
    }
}
