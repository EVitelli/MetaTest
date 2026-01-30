using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime CriadoEm { get; set; }
        DateTime? AtualizadoEm { get; set; }
    }
}
