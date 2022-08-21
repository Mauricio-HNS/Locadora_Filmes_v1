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
    [Route("v1/filmes")]
    public class FilmesController : ControllerBase
    {
        ///<summary>
        ///Busca todos os filmes
        ///</summary>						
        ///<response code = "200" > Retorna a lista de filmes cadastrados</response>

        [HttpGet]
        public async Task<ActionResult<List<Filme>>> Obter([FromServices] DataContext context)
        {
            var filmes = await context.Filmes.ToListAsync();

            if (filmes.Count() == 0)
            {
                return Content("Não Existem Filmes para Exibição");
            }
            else
            {
                return filmes;
            }
        }

        /// <summary>
        /// Cadastrar filmes na locadora
        /// <remarks>
        /// Informar Id e Nome do filme
        /// </remarks>
        ///</summary>			
        ///<response code = "200" > retorna cadastro realizado</response> 

        [HttpPost]
        [Route("cadastrar")]
        public async Task<ActionResult<Filme>> Cadastrar([FromServices] DataContext context, [FromBody] Filme filme)
        {
            if (ModelState.IsValid)
            {
                var obj_filme = await context.Filmes.FirstOrDefaultAsync(a => a.Nome == filme.Nome.Trim());

                if (obj_filme == null)
                {
                    filme.Nome = filme.Nome.Trim();
                    filme.Ativo = true;
                    filme.Disponivel = true;
                    filme.Data_Cadastro = DateTime.Now;
                    context.Filmes.Add(filme);
                    await context.SaveChangesAsync();
                    return filme;
                }
                else
                {
                    return Content("Filme já Cadastrado !");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        ///<summary>
        ///Inativar um filme
        ///</summary>
        ///<remarks>
        ///Informar Id do Filme
        ///</remarks>	
        ///<response code = "200" > Retorna cadastro Inativado</response>	

        [HttpPost]
        [Route("inativar/{id:int?}")]
        public async Task<ActionResult<Filme>> Inativar([FromServices] DataContext context, int id)
        {
            if (id != 0)
            {
                var filme = await context.Filmes.FirstOrDefaultAsync(c => c.Id == id);

                if (filme == null)
                {
                    return Content("Filme não Encontrado !");
                }

                filme.Ativo = false;
                context.Filmes.Update(filme);
                await context.SaveChangesAsync();
                return filme;
            }
            else
            {
                return Content("Informe o Id do Filme !");
            }
        }

        ///<summary>
        ///Ativar um filme
        ///</summary>
        ///<remarks>
        ///Informar Id do filme
        ///</remarks>
        ///<response code = "200" > Retorna cadastro Ativado</response>

        [HttpPost]
        [Route("ativar/{id:int?}")]
        public async Task<ActionResult<Filme>> Ativar([FromServices] DataContext context, int id)
        {
            if (id != 0)
            {
                var filme = await context.Filmes.FirstOrDefaultAsync(c => c.Id == id);

                if (filme == null)
                {
                    return Content("Filme não Encontrado !");
                }

                filme.Ativo = true;
                context.Filmes.Update(filme);
                await context.SaveChangesAsync();
                return filme;
            }
            else
            {
                return Content("Por Favor, Informe o Id do Filme!");
            }
        }

    }
}
