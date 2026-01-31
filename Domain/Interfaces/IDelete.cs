using Domain.Enums;

namespace Domain.Interfaces
{
    internal interface IDelete
    {
        DateTime? DeletadoEm { get; set; }
        public EStatus Status { get; set; }
    }
}
