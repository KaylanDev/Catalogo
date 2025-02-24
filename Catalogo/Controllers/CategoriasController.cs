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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using System.Collections;
using System.Reflection.Metadata.Ecma335;

namespace Catalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;


        //variavel para usar o configuration
        //private readonly IConfiguration _configuration;

        public CategoriasController(IUnitOfWork Iunit,IMapper mapper)
        {
            _uof = Iunit;
           _mapper = mapper;
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

        [HttpGet]
        public ActionResult<IEnumerable<CategoriasDTO>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            return Ok(categorias);
           
        }

        /// <summary>
        /// retorna elemento pelo id
        /// </summary>
        //metodo que ira retornar pelo Id
        [HttpGet("{id:int}", Name = "categoria")]
        public ActionResult Get(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

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
        public ActionResult<IEnumerable<Categorias>> GetCategoriaProdutos()
        {
            var categoriasProd = _uof.CategoriaRepository.GetCategoriasProdutos();
            return Ok(categoriasProd);
           

        }


        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoriasDTO>> GetPagination([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = _uof.CategoriaRepository.GetPagination(categoriasParameters);

            return ObterCategoria(categorias);
        }

        [HttpGet("Categorias/Filtro")]
        public ActionResult<IEnumerable<CategoriasDTO>> GetFiltro([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
        {
            var categoraias = _uof.CategoriaRepository.GetFiltroNome(categoriasFiltroNome);
            return ObterCategoria(categoraias);
        }

        private ActionResult<IEnumerable<CategoriasDTO>> ObterCategoria(PagedList<Categorias> categorias)
        {
            var metadados = new
            {

                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPage,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Append("X-OLHAAAAA", JsonConvert.SerializeObject(metadados));

            var categoriasDto = _mapper.Map<IEnumerable<CategoriasDTO>>(categorias);
            return Ok(categoriasDto);
        }


        /// <summary>
        /// adciona um novo elemento
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Categorias> post(Categorias categoria)
        {

            if (categoria is null)
            {
                return BadRequest("Dados invalidos...");
            }

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();


            return new CreatedAtRouteResult("categoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);


        }

        /// <summary>
        /// altera a informação
        /// </summary>
        /// <returns></returns>

        [HttpPut("{id:int}")]
        public ActionResult<Categorias> Put(int id, Categorias categoria)
        {

            if (id != categoria.CategoriaId)
            {
                return BadRequest("Dados invalidos...");
            }
            var categoriaAtt = _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            return Ok(categoriaAtt);


        }

        /// <summary>
        /// Deleta o elemento selecionado
        /// </summary>
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("produto n encontrado");
            }
            var categoriaDelet = _uof.CategoriaRepository.Delete(categoria);
            if (categoriaDelet is null)
            {
                return BadRequest("Falha ao deletar categoria");
            }
            _uof.Commit();
            return Ok(categoriaDelet);




        }


    }
}
