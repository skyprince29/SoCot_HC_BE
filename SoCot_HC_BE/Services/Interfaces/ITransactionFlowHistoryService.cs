using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.BaseModels;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface ITransactionFlowHistoryService : IRepository<TransactionFlowHistory, Guid>
    {
        Task StarterLogAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : IBaseTransaction;

        Task UpdateStatusAsync(UpdateStatusDto dto, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(UpdateStatusDto dto, bool isSave = false, CancellationToken cancellationToken = default);
        Task UpdateStatusEntityAsync(PatientDepartmentTransaction entity, UpdateStatusDto dto, bool isSave = false, CancellationToken cancellationToken = default);
    }
}
