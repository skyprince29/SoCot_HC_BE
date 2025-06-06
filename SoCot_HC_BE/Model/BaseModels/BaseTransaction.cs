using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model.BaseModels
{

    // The corrected IBaseTransaction interface (from above)
    public interface IBaseTransaction
    {
        byte StatusId { get; set; }
        Guid TransactionId { get; }
        int ModuleId { get; set; }
        string? CurrentRemarks { get; set; }
    }

    public abstract class BaseTransaction :  IBaseTransaction
    {
        // Status of the transaction (e.g., Created, Approved, etc.)
        public byte StatusId { get; set; }

        /// <summary>
        /// Use this to return the main ID (e.g., PatientRegistryId) as TransactionId.
        /// Should be overridden in the derived class if needed.
        /// </summary>
        [NotMapped]
        public virtual Guid TransactionId => Guid.Empty;

        /// <summary>
        /// Module identifier for status flow (should be set by each concrete entity).
        /// </summary>
        [NotMapped]
        public virtual int ModuleId { get; set; }

        /// <summary>
        /// Temporary remarks added during status updates.
        /// </summary>
        [NotMapped]
        public virtual string? CurrentRemarks { get; set; }
    }


    // Base class for transactions that DO require AuditInfo
    public abstract class BaseTransactionWithAudit : AuditInfo, IBaseTransaction // Inherits core properties and implements IBaseTransaction
    {
        // Status of the transaction (e.g., Created, Approved, etc.)
        public byte StatusId { get; set; }

        /// <summary>
        /// Use this to return the main ID (e.g., PatientRegistryId) as TransactionId.
        /// Should be overridden in the derived class if needed.
        /// </summary>
        [NotMapped]
        public virtual Guid TransactionId => Guid.Empty;

        /// <summary>
        /// Module identifier for status flow (should be set by each concrete entity).
        /// </summary>
        [NotMapped]
        public virtual int ModuleId { get; set; }

        /// <summary>
        /// Temporary remarks added during status updates.
        /// </summary>
        [NotMapped]
        public virtual string? CurrentRemarks { get; set; }
    }

}
