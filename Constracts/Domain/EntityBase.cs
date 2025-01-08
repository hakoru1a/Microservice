using Constracts.Domain.Interface;
namespace Constracts.Domain
{
    public abstract class EntityBase<T> : IEnityBase<T>
    {
        public T Id { get; set; }
    }
}
