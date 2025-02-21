using AutoMapper;
using Azure;
using Azure.Core;
using Catalogo.Data;
using Catalogo.DTOs;
using Catalogo.Migrations;
using Catalogo.Models;
using Catalogo.Paginations;
using Catalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using JsonConverter = Newtonsoft.Json.JsonConverter;


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

        public ProdutoController(IUnitOfWork uof, ILogger<ProdutoController> logger, IMapper mapper)
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
        [HttpGet("produtosCategoria/{id}")]
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
        /// 
        [HttpGet("{id:int}", Name = "ProdutoporID")]
        public ActionResult<ProdutosDTO> GetById(int id)

        {
            var produto = _uof.ProductRepository.GetById(p => p.ProdutoId == id);
            var produtoDto = _mapper.Map<ProdutosDTO>(produto);

            return Ok(produtoDto);
        }

        [HttpGet("paramans")]
        public ActionResult<IEnumerable<ProdutosDTO>> GetParamns([FromQuery]ProdutosParameters produtosParameters)
        {
            var produtos = _uof.ProductRepository.GetPagination(produtosParameters);
            return ObterProduto(produtos);
        }
        [HttpGet("Produtos/Filtros")]
        public ActionResult<IEnumerable<ProdutosDTO>> GetFiltro([FromQuery] ProdutosFiltroPrecos produtosFiltroPrecos)
        {
            var produtos = _uof.ProductRepository.GetProdutosFiltro(produtosFiltroPrecos);
            return ObterProduto(produtos);
        }

        private ActionResult<IEnumerable<ProdutosDTO>> ObterProduto(PagedList<Produtos> produtos)
        {
            var metaDados = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPage,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Append("X-Append", JsonConvert.SerializeObject(metaDados));

            var produtosDto = _mapper.Map<IEnumerable<ProdutosDTO>>(produtos);
            return Ok(produtosDto);
        }

        [HttpPatch("{id:int}/UpdatePartial")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id,JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
        {
            if (patchProdutoDto is null || id == 0) return BadRequest();

            var produto = _uof.ProductRepository.GetById(c => c.ProdutoId == id);

            if (produto is null)
            {
                return NotFound();
            }

            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);
            patchProdutoDto.ApplyTo(produtoUpdateRequest);
            if (!ModelState.IsValid || TryValidateModel(produtoUpdateRequest)) return BadRequest(ModelState);
            

            _mapper.Map(produtoUpdateRequest, produto);
            _uof.ProductRepository.Update(produto);
            _uof.Commit();
            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
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
            return new CreatedAtRouteResult("ProdutoporID",
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



