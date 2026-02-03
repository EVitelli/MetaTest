namespace Domain.Converters
{
    public static class ContaConvert
    {
        public static Models.Conta MapToModel(this Entities.Conta entity)
        {
            return new Models.Conta
            {
                Id = entity.Id,
                IdCliente = entity.IdUsuarioCliente,
                IdGerente = entity.IdUsuarioGerente,
                Codigo = entity.Codigo,
                Saldo = entity.Saldo,
                Reservado = entity.Reservado,
                LimiteCredito = entity.LimiteCredito,
                SaldoCredito = entity.SaldoCredito,
                CriadoEm = entity.CriadoEm,
                AtualizadoEm = entity.AtualizadoEm,
                DeletadoEm = entity.DeletadoEm,
                Status = entity.Status,
                AtualizadoPor = entity.AtualizadoPor
            };
        }
    }
}
