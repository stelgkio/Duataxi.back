using System;
using System.Collections.Generic;
using System.Text;

namespace DuaTaxi.Entities.Core.Base
{
    public interface IBaseEntity
    {
        DateTime Created { get; set; }
        long CreatedBy { get; set; }
        DateTime? Deleted { get; set; }
        long DeletedBy { get; set; }
        string ForeignId { get; set; }
        long Id { get; set; }
        long IsDeleted { get; set; }
        bool? IsDirty { get; set; }
        DateTime? LastSync { get; set; }
        DateTime Modified { get; set; }
        long ModifiedBy { get; set; }
        string RowVersion { get; set; }
        long? SyncOrder { get; set; }
        byte[] Version { get; }

      
    }
}
