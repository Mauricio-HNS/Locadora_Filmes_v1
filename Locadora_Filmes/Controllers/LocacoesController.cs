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
    [Route("v1/Locacoes")]
    public class LocacoesController: ControllerBase
    {
        ///<summary>
        ///Busca todas os locações com Ids do cliente e do filme
        ///</summary>						
        ///<response code = "200" > Retorna a lista de locações com ids </response>

        [HttpGet]        
        public async Task<ActionResult<List<Locacao>>> Obter([FromServices] DataContext context)
        {
            var locacoes = await context.Locacoes.ToListAsync();

            if (locacoes.Count() == 0)
            {
                return Content("Não Existem Locações para Exibição");
            }
            else
            {
                return locacoes;
            }   
        }

        ///<summary>
        ///Alugar um Filme
        ///</summary>
        ///<remarks>
        ///Situação 1 - Para Locação Informar: Id da Locação, Id do cliente, Id do filme, Manter as datas do formulario        
        ///</remarks>
        ///<remarks>
        ///Situação 2 - Testar nova locação para mesmo Cliente ou Filme Informar: Id da Locação, Id do cliente que possui locação ativa ou Id do filme já alugado, Manter as datas do formulario        
        ///</remarks>
        ///<remarks>
        ///Situação 3- Testar devolução com Atraso, Informar: Id da Locação, Id do cliente, Id do filme,
        ///Alterar as datas para um dia anterior ao atual: data_Locação , data_Entrega e data_Devolvido       
        ///</remarks>	
        ///<response code = "200" > Retorna o cadastro com os dados da locação</response>	

        [HttpPost]
        [Route("Alugar")]
        public async Task<ActionResult<Locacao>> Alugar([FromServices] DataContext context, [FromBody] Locacao locacao)
        {
            if (locacao.ClienteId != 0 && locacao.FilmeId != 0 && locacao.Data_Entrega != DateTime.MinValue)
            {
                var filme = context.Filmes.FirstOrDefault(a => a.Id == locacao.FilmeId);
                var cliente = context.Clientes.FirstOrDefault(a => a.Id == locacao.ClienteId);

                if (filme == null)
                {
                    return Content("Filme não Encontrado !");

                }
                else if (!filme.Disponivel)
                {
                    return Content("'" + filme.Nome + "' não esta Disponível !");
                }
                else if (!filme.Ativo)
                {
                    return Content("Filme '" + filme.Nome + "' Inativo !");
                }
                else if (cliente == null)
                {
                    return Content("Cliente não Encontrado !");
                }
                else if (!cliente.Disponivel)
                {
                    return Content("'" + cliente.Nome + "' não esta Disponível !");
                }
                else if (!cliente.Ativo)
                {
                    return Content("Cliente '" + cliente.Nome + "' Inativo !");
                }
                else
                {
                    filme.Disponivel = false;
                    context.Filmes.Update(filme);

                    cliente.Disponivel = false;
                    context.Clientes.Update(cliente);

                    locacao.Data_locacao = DateTime.Now;
                    context.Locacoes.Add(locacao);

                    await context.SaveChangesAsync();
                    return Content("'" + filme.Nome + "' alugado com Sucesso !");
                }
            }
            else
            {
                return Content("Informe o Id do Cliente, do Filme e a Data da Entrega yyyy-MM-dd!");
            }
        }

        ///<summary>
        ///Devolver um filme
        ///</summary>
        ///<remarks>
        ///Informar :  Id do cliente e Id do filme
        ///</remarks>	
        ///<response code = "200" > Retorna filme devolvido </response>

        [HttpPost]
        [Route("Devolver")]
        public async Task<ActionResult<Locacao>> Devolver([FromServices] DataContext context, Locacao locacao)
        {
            if (locacao.ClienteId != 0 && locacao.FilmeId != 0)
            {
                var cliente = context.Clientes.FirstOrDefault(a => a.Id == locacao.ClienteId);
                var filme = context.Filmes.FirstOrDefault(a => a.Id == locacao.FilmeId);
                
                if (cliente == null)
                {
                    return Content("Cliente não Encontrado !");
                }
                else if (filme == null)
                {
                    return Content("Filme não Encontrado !");
                }
                else if (filme.Disponivel)
                {
                    return Content("'" + filme.Nome + "' esta disponível ou já foi devolvido !");
                }

                var obj_locacao = await context.Locacoes.FirstOrDefaultAsync(a => a.ClienteId == locacao.ClienteId && a.FilmeId == locacao.FilmeId && locacao.Data_Entrega != DateTime.MinValue);

                if (obj_locacao == null)
                {
                    return Content("O Filme não foi alugado por esse cliente !");
                }   

                filme.Disponivel = true;
                locacao.Data_Devolvido = DateTime.Now;

                context.Filmes.Update(filme);
                context.Locacoes.Update(obj_locacao);
                await context.SaveChangesAsync();

                if (DateTime.Now > obj_locacao.Data_Entrega)
                {
                    return Content("Filme Devolvido apos Data de Entrega Prevista, Cobrar Taxa de Atraso!");
                }
                else
                {
                    return Content("Filme Devolvido com Sucesso !");
                }
            }
            else
            {
                return Content("Informe o Id do Cliente e do Filme");
            }
        }
    }
}
