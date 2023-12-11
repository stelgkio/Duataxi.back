using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace DuaTaxi.Entities.Core.Base
{
    public class BaseEntity : IBaseEntity
    {       
        public long Id { get; set; }
        public long? ImportId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime? Deleted { get; set; }
        public long DeletedBy { get; set; }
        public long IsDeleted { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(40)]
        public string ForeignId { get; set; }
        public bool? IsDirty { get; set; }
        public DateTime? LastSync { get; set; }
        public long? SyncOrder { get; set; }
        [Timestamp]
        public byte[] Version { get; internal set; }

        [NotMapped]
        public string RowVersion
        {
            get
            {
                if (this.Version != null)
                    return Convert.ToBase64String(this.Version);

                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || (string)value == "0")
                    this.Version = null;
                else
                    this.Version = Convert.FromBase64String(value);
            }

        }
    }
}
