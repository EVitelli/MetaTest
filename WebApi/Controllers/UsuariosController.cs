using Domain.Enums;
using Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsuariosController(IUsuarioService service) : ControllerBase
    {
        /*  Cadastro de usuários
         *  Caso usuário esteja logado
         *      ADM - Pode criar ADM e Gerente
         *      Gerente - Pode Criar Cliente
         *      Cliente - não pode criar conta já tendo uma
         *  Caso o usuário não esteja logado
         *      A criação do usuário é do tipo cliente
         */
        [HttpPost]
        public async Task<IActionResult> CriarUsuarioAsync(CriarUsuarioRequest usuario)
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                switch (this.GetClaimRoleValue(User))
                {
                    case ETipoUsuario.Administrador:
                        if (usuario.Tipo != ETipoUsuario.Administrador || usuario.Tipo != ETipoUsuario.Gerente)
                            return Forbid();
                        break;
                    case ETipoUsuario.Gerente:
                        if (usuario.Tipo != ETipoUsuario.Cliente)
                            return Forbid();
                        break;
                    case ETipoUsuario.Cliente:
                        return Forbid();
                    default:
                        usuario.Tipo = ETipoUsuario.Cliente;
                        break;
                }

                usuario.Operador = this.GetClaimEmailValue(User);
            }
            else
            {
                usuario.Tipo = ETipoUsuario.Cliente;
                usuario.Operador = "Sistema";
            }

            CriarUsuarioResponse response = await service.CriarUsuarioAsync(usuario);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarUsuarioAsync(uint id)
        {
            UsuarioResponse? response = await service.BuscarUsuarioAsync(id);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarUsuarioAsync(uint id)
        {
            UsuarioResponse? usuario = await service.BuscarUsuarioAsync(id);

            if (usuario is null)
                return NoContent();

            switch (this.GetClaimRoleValue(User))
            {
                case ETipoUsuario.Administrador:
                    break;
                case ETipoUsuario.Gerente:
                    if(usuario.Tipo != ETipoUsuario.Cliente)
                        return Forbid();
                    break;
                case ETipoUsuario.Cliente:
                    if (id != this.GetClaimIdValue(User))
                        return Forbid();
                    break;
                default:
                    break;
            }

            DeletarUsuarioRequest request = new()
            {
                Id = id,
                Operador = this.GetClaimEmailValue(User)
            };

            DeletarUsuarioResponse? response = await service.DeletarUsuarioAsync(request);

            return Ok(response);
        }
    }
}
