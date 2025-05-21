using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.BaseModels;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface ITransactionFlowHistoryService : IRepository<TransactionFlowHistory, Guid>
    {
        Task StarterLogAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : BaseTransaction;

        Task UpdateStatusAsync(UpdateStatusDto dto, CancellationToken cancellationToken = default);
    }
}
