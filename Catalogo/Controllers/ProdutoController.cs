using Azure.Core;
using Catalogo.Data;
using Catalogo.Models;
using Catalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProdutoController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProdutoController(IProductRepository repository)
        {
            _repository = repository;
        }


        //comentarios em xml

        /// <summary>
        /// Retorna os itens.
        /// </summary>
        [HttpGet]
        public ActionResult<IQueryable<Produto>> Get()
        {

            var produtos = _repository.Get().ToList();

            return Ok(produtos);

        }

        /// <summary>
        /// retorna elemento pelo id
        /// </summary>
        //metodo que ira retornar pelo Id
        [HttpGet("{id:int}", Name = "obterproduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _repository.GetProduto(id);
            return Ok(produto);
        }

        /// <summary>
        /// adciona um novo elemento
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                return BadRequest();
            }

            var Novoproduto = _repository.Create(produto);
            return new CreatedAtRouteResult("obterproduto",
                new { Id = Novoproduto.ProdutoId }, Novoproduto);
        }

        /// <summary>
        /// altera a informação
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            bool atualizado = _repository.Update(produto);
            if (atualizado)
            {
                return Ok(produto);

            }
            else
            {
                return StatusCode(500,$"Falha ao atualizar o Produto com Id = {id}"); 
            }
        }

        /// <summary>
        /// Deleta o elemento selecionado
        /// </summary>
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
;           bool delete = _repository.Delete(id);

            if (delete)
            {
                return Ok($"produto com id = {id} Excluido!");
            }
            else
            {
                return BadRequest($"Falha ao excluir  Produto com Id = {id}");
            }
        }
    }
}



