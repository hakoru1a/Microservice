using Constracts.Domain.Interface;

namespace Constracts.Domain
{
    public abstract class EntityAuditBase<T> : EntityBase<T>, IAuditable
    {
        public DateTimeOffset CreatedDate {  get; set; }
        public DateTimeOffset? LastModifiedDate {  get; set; }
    }
}
