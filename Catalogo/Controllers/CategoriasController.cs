using AutoMapper;
using Azure.Core;
using Catalogo.Data;
using Catalogo.DTOs;
using Catalogo.DTOs.Mappins;
using Catalogo.Filters;
using Catalogo.Models;
using Catalogo.Paginations;
using Catalogo.Repositories;
using Catalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using X.PagedList;
using Microsoft.Extensions.Logging;

namespace Catalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriasController> _logger;


        //variavel para usar o configuration
        //private readonly IConfiguration _configuration;

        public CategoriasController(IUnitOfWork Iunit, IMapper mapper, ILogger<CategoriasController> logger)
        {
            _uof = Iunit;
            _mapper = mapper;
            _logger = logger;
        }
        /*
        ///<summary>
        ///testando o configuration
        ///</summary>
        [HttpGet("String")]
        public string GetValores()
        {
            var chave1 = _configuration["chave1"];
            return $"{chave1}";
        }
        */
        //teste do get com from service
        /*
        [HttpGet("ComFromService/{nome}")]
        public ActionResult<string> GetComFrom(string nome,[FromServices]IMeuService service)
        {
            return service.BemVindo(nome);
        }

        [HttpGet("SemFromService/{nome}")]
        public ActionResult<string> GetSemFrom(string nome, IMeuService service)
        {
            return service.BemVindo(nome);
        }
          */


        //comentarios em xml
        /// <summary>
        /// Retorna os itens.
        /// </summary>


        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok("Endpoint público funcionando!");
        }

        [Authorize]
        [HttpGet("protected")]
        public IActionResult ProtectedEndpoint()
        {
            var user = HttpContext.User;
            _logger.LogInformation("Usuário autenticado: {Identity}", user.Identity?.Name ?? "Não identificado");
            _logger.LogInformation("Claims: {Claims}", string.Join(", ", user.Claims.Select(c => $"{c.Type}={c.Value}")));
            return Ok("Endpoint protegido funcionando!");
        }




        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoriasDTO>>> Get()
        {
            var categorias = await _uof.CategoriaRepository.GetAllAsync();
            var categoriasDto = _mapper.Map<IEnumerable<CategoriasDTO>>(categorias);
            return Ok(categoriasDto);

        }

        /// <summary>
        /// retorna elemento pelo id
        /// </summary>
        //metodo que ira retornar pelo Id
        [HttpGet("{id:int}", Name = "categoria")]
        public async Task<ActionResult> Get(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetByIdAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound($"Categoria com Id {id} nao encontrado!");

            }
            var categoriaDTO = categoria.TocategoriaDTO();
            return Ok(categoriaDTO);
        }

        /// <summary>
        /// retorna elementos relacionados
        /// </summary>
        //metodo que ira retornar produtos relacionados
        [HttpGet]
        [Route("produtos")]
        [Authorize]

        public async Task<ActionResult<IEnumerable<CategoriasProdutosDTO>>> GetCategoriaProdutos()
        {
            var categoriasProd = await _uof.CategoriaRepository.GetCategoriasProdutosAsync();
            var categoriasDto = _mapper.Map<IEnumerable<CategoriasProdutosDTO>>(categoriasProd);
            return Ok(categoriasDto);


        }


        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriasDTO>>> GetPagination([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetPaginationAsync(categoriasParameters);

            return ObterCategoria(categorias);
        }

        [HttpGet("Categorias/Filtro")]
        public async Task<ActionResult<IEnumerable<CategoriasDTO>>> GetFiltro([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
        {
            var categoraias = await _uof.CategoriaRepository.GetFiltroNomeAsync(categoriasFiltroNome);
            return ObterCategoria(categoraias);
        }

        private ActionResult<IEnumerable<CategoriasDTO>> ObterCategoria(IPagedList<Categorias> categorias)
        {
            var metadados = new
            {

                categorias.Count,
                categorias.PageSize,
                categorias.LastItemOnPage,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-OLHAAAAA", JsonConvert.SerializeObject(metadados));

            var categoriasDto = _mapper.Map<IEnumerable<CategoriasDTO>>(categorias);
            return Ok(categoriasDto);
        }

        [HttpPatch("{id:int}",Name = "PathEdtion")]
        public async Task<ActionResult<CategoriasDTO>> Patch(JsonPatchDocument<CategoriasDTO> jsonPatch,int id)
        {
            var categoria = await _uof.CategoriaRepository.GetByIdAsync(c => c.CategoriaId == id);

            if (categoria is null)return BadRequest();

            var categoriaDto = _mapper.Map<CategoriasDTO>(categoria);
            jsonPatch.ApplyTo(categoriaDto);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _mapper.Map(categoriaDto,categoria);
            _uof.CategoriaRepository.Update(categoria);
            await _uof.Commit();

            return Ok(categoriaDto);
        }
        /// <summary>
        /// adciona um novo elemento
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Categorias>> post(Categorias categoria)
        {

            if (categoria is null)
            {
                return BadRequest("Dados invalidos...");
            }

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
           await _uof.Commit();


            return new CreatedAtRouteResult("categoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);


        }

        /// <summary>
        /// altera a informação
        /// </summary>
        /// <returns></returns>

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Categorias>> Put(int id, Categorias categoria)
        {

            if (id != categoria.CategoriaId)
            {
                return BadRequest("Dados invalidos...");
            }
            var categoriaAtt = _uof.CategoriaRepository.Update(categoria);
          await   _uof.Commit();

            return Ok(categoriaAtt);


        }

        /// <summary>
        /// Deleta o elemento selecionado
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetByIdAsync(c => c.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("produto n encontrado");
            }
            var categoriaDelet = _uof.CategoriaRepository.Delete(categoria);
            if (categoriaDelet is null)
            {
                return BadRequest("Falha ao deletar categoria");
            }
           await _uof.Commit();
            return Ok(categoriaDelet);




        }


    }
}
