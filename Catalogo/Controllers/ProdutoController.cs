using Azure.Core;
using Catalogo.Data;
using Catalogo.Models;
using Catalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Catalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProdutoController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        //ao adicionar o ILogger, lembre de colocar a class
        private readonly ILogger<ProdutoController> _logger;

        public ProdutoController(IUnitOfWork uof, ILogger<ProdutoController> logger)
        {
            _uof = uof;
            _logger = logger;   
        }


        //comentarios em xml

        /// <summary>
        /// Retorna os itens.
        /// </summary>
        [HttpGet]
        public ActionResult<IQueryable<Produto>> Get()
        {

            var produtos = _uof.ProductRepository.GetAll().ToList();

            return Ok(produtos);

        }

        /// <summary>
        /// retorna elemento pelo id
        /// </summary>
        //metodo que ira retornar pelo Id
        [HttpGet("{id:int}", Name = "obterproduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _uof.ProductRepository.GetById(p => p.ProdutoId == id);

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

            var Novoproduto = _uof.ProductRepository.Create(produto);
            _uof.Commit();
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
                return BadRequest("Id informado é diferente");
            }
            if (produto is null)
            {
                return BadRequest();
            }
              _uof.ProductRepository.Update(produto);
            _uof.Commit(); 
            return Ok(produto);
        }

        /// <summary>
        /// Deleta o elemento selecionado
        /// </summary>
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProductRepository.GetById(p => p.ProdutoId == id)
;           _uof.ProductRepository.Delete(produto);
            _uof.Commit();

            return Ok(produto);
        }
    }
}



