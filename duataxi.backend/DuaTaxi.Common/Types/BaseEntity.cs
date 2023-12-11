using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace DuaTaxi.Common.Types
{
    public abstract class BaseEntity : IIdentifiable
    {

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))] 
        public string Id { get; set; }
        public DateTime CreatedDate { get; protected set; }          
        public DateTime UpdatedDate { get; protected set; }         
        public DateTime? DeletedDate { get; protected set; }         
        public bool IsDeleted { get; protected set; } = false;  
         
        public BaseEntity(string id)
        {
            Id = id;         
            CreatedDate = DateTime.UtcNow;
            SetUpdatedDate();
        }
        public BaseEntity()
        {
            SetId();
            CreatedDate = DateTime.UtcNow;
            SetUpdatedDate();
        }

        protected virtual void SetUpdatedDate()
            => UpdatedDate = DateTime.UtcNow;
        protected virtual void SetDeleted()
        {
            UpdatedDate = DateTime.UtcNow;
            IsDeleted = true;
        }

        protected virtual void SetId()
            => Id = Guid.NewGuid().ToString();

        public string Error
        {
            get; protected set;
        }

    }
}