using Azure.Core;
using Catalogo.Data;
using Catalogo.DTOs;
using Catalogo.DTOs.Mappins;
using Catalogo.Filters;
using Catalogo.Models;
using Catalogo.Repositories;
using Catalogo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Collections;
using System.Reflection.Metadata.Ecma335;

namespace Catalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        //variavel para usar o configuration
        //private readonly IConfiguration _configuration;

        public CategoriasController(IUnitOfWork Iunit/*IConfiguration configuration*/)
        {
            _uof = Iunit;
            /*_configuration = configuration;*/
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
