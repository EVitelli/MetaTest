namespace Domain.Interfaces
{
    public interface IEntity
    {
        uint Id { get; set; }
        DateTime CriadoEm { get; set; }
        DateTime AtualizadoEm { get; set; }
    }
}
