using AutoMapper;
using Azure.Core;
using Catalogo.Data;
using Catalogo.DTOs;
using Catalogo.Migrations;
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
        private readonly IMapper _mapper;

        public ProdutoController(IUnitOfWork uof, ILogger<ProdutoController> logger,IMapper mapper)
        {
            _uof = uof;
            _logger = logger;   
            _mapper = mapper;
        }


        //comentarios em xml

        /// <summary>
        /// Retorna os itens.
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<ProdutosDTO>> Get()
        {

            var produtos = _uof.ProductRepository.GetAll().ToList();
            var produtosDto = _mapper.Map<IEnumerable<ProdutosDTO>>(produtos);

            return Ok(produtosDto);

        }
        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<ProdutosDTO>> GetProdutosCategoria(int id)
        {
            var produtos = _uof.ProductRepository.GetProdutosPorCategoria(id);
            if (produtos is null) return BadRequest();
            //var destino = _mapper.Map<Destino>(origem)
            var produtosDTO = _mapper.Map<IEnumerable<ProdutosDTO>>(produtos);
            return Ok(produtos);
        }

        /// <summary>
        /// retorna elemento pelo id
        /// </summary>
        //metodo que ira retornar pelo Id
        [HttpGet("{id:int}", Name = "ProdutoporID")]
        public ActionResult<ProdutosDTO> GetById(int id)
        {
            var produto = _uof.ProductRepository.GetById(p => p.ProdutoId == id);
            var produtoDto = _mapper.Map<ProdutosDTO>(produto);

            return Ok(produtoDto);
        }

        /// <summary>
        /// adciona um novo elemento
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult<ProdutosDTO> Post(ProdutosDTO produtoDto)
        {
            if (produtoDto is null)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produtos>(produtoDto); 
            _uof.ProductRepository.Create(produto);
            _uof.Commit();
            var produtoDTo = _mapper.Map<ProdutosDTO>(produto);
            return new CreatedAtRouteResult("obterproduto",
                new { Id = produtoDTo.ProdutoId }, produtoDTo);
        }

        /// <summary>
        /// altera a informação
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public ActionResult<ProdutosDTO> Put(int id, ProdutosDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
            {
                return BadRequest("Id informado é diferente");
            }
            if (produtoDto is null)
            {
                return BadRequest();
            }
            var produto = _mapper.Map<Produtos>(produtoDto);
              _uof.ProductRepository.Update(produto);
            _uof.Commit();
            var produtoAttDto = _mapper.Map<ProdutosDTO>(produto);
            return Ok(produtoDto);
        }

        /// <summary>
        /// Deleta o elemento selecionado
        /// </summary>
        [HttpDelete("{id:int}")]
        public ActionResult<ProdutosDTO> Delete(int id)
        {
            var produto = _uof.ProductRepository.GetById(p => p.ProdutoId == id)
;           _uof.ProductRepository.Delete(produto);
            _uof.Commit();
            var ProdutoDto = _mapper.Map<ProdutosDTO>(produto);

            return Ok(ProdutoDto);
        }
    }
}



