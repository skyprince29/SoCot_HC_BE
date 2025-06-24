using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Services;
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
            (int)ModuleEnum.PatientDepartmentTransaction => _serviceProvider.GetService<IPatientDepartmentTransactionService>(),
            (int)ModuleEnum.Referral => _serviceProvider.GetService<IPatientDepartmentTransactionService>(),
            // add other modules here...
            _ => null
        };
    }
}
