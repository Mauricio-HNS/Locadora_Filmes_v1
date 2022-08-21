using Locadora_De_Filmes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Locadora_De_Filmes.Controllers
{
    [ApiController]
    [Route("v1/Clientes")]
    public class ClientesController : ControllerBase
    {
        /// <summary>
        /// Busca todos os clientes
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        ///<response code = "200" > Retorna a lista de clientes cadastrados</response>

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> Obter([FromServices] DataContext context)
        {
            var clientes = await context.Clientes.ToListAsync();

            if (clientes.Count() == 0)
            {
                return Content("Não Existem Clientes para Exibição");
            }
            else
            {
                return clientes;
            }
        }

        /// <summary>
        /// Cadastrar clientes na locadora
        /// <remarks>
        /// Informar Id e Nome do cliente
        /// </remarks>
        ///</summary>			
        ///<response code = "200" > retorna cadastro realizado</response>        

        [HttpPost]
        [Route("Cadastrar")]
        public async Task<ActionResult<Cliente>> Cadastrar([FromServices] DataContext context, [FromBody] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var obj_cliente = await context.Clientes.FirstOrDefaultAsync(a => a.Nome == cliente.Nome.Trim());

                if (obj_cliente == null)
                {
                    cliente.Nome = cliente.Nome.Trim();
                    cliente.Ativo = true;
                    cliente.Data_Cadastro = DateTime.Now;
                    context.Clientes.Add(cliente);
                    await context.SaveChangesAsync();
                    return cliente;
                }
                else
                {
                    return Content("Cliente já Cadastrado !");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        ///<summary>
	    ///Inativar um cliente
        ///</summary>
	    ///<remarks>
	    ///Informar Id do cliente
        ///</remarks>	
	    ///<response code = "200" > Retorna do cadastro Inativado</response>	

        [HttpPost]
        [Route("Inativar/{id:int?}")]
        public async Task<ActionResult<Cliente>> Inativar([FromServices] DataContext context, int id)
        {
            if (id != 0)
            {
                var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                {
                    return Content("Cliente não Encontrado !");
                }

                cliente.Ativo = false;
                context.Clientes.Update(cliente);
                await context.SaveChangesAsync();
                return cliente;
            }
            else
            {
                return Content("Informe o Id do Cliente !");
            }
        }

        ///<summary>
	    ///Ativar um cliente
        ///</summary>
        ///<remarks>
	    ///Informar Id do cliente
        ///</remarks>
        ///<response code = "200" > Retorna do cadastro Ativado</response>

        [HttpPost]
        [Route("Ativar/{id:int?}")]
        public async Task<ActionResult<Cliente>> Ativar([FromServices] DataContext context, int id)
        {
            if (id != 0)
            {
                var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                {
                    return Content("Cliente não Encontrado !");
                }

                cliente.Ativo = true;
                context.Clientes.Update(cliente);
                await context.SaveChangesAsync();
                return cliente;
            }
            else
            {
                return Content("Informe o Id do Cliente !");
            }
        }

    }
}
