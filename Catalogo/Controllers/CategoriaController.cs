using Azure.Core;
using Catalogo.Data;
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
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _repository;
        private readonly ILogger _logger;
        //variavel para usar o configuration
        //private readonly IConfiguration _configuration;

        public CategoriaController(ICategoriaRepository repository, ILogger logger /*IConfiguration configuration*/)
        {
            _logger = logger;
            _repository = repository;
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
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _repository.GetCategorias();
            return Ok(categorias);
            //AsNoTracking evita a sobrecarga, deixando a consulta otimizada
            //Take ira limitar a consulta apenas com os 10 primeiros
        }

        /// <summary>
        /// retorna elemento pelo id
        /// </summary>
        //metodo que ira retornar pelo Id
        [HttpGet("{id:int}", Name = "categoria")]
        public ActionResult Get(int id)
        {
            var categoria = _repository.GetCategoria(id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com Id {id} nao encontrado!");
                return NotFound($"Categoria com Id {id} nao encontrado!");

            }

            return Ok(categoria);
        }

        /// <summary>
        /// retorna elementos relacionados
        /// </summary>
        //metodo que ira retornar produtos relacionados
        [HttpGet]
        [Route("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriaProdutos()
        {
            var categoriasProd = _repository.GetCategoriasProdutos();
            return Ok(categoriasProd);
            //o where limita a consulta para evitar uma grande quantidade de dados retornado.

        }

        /// <summary>
        /// adciona um novo elemento
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Categoria> post(Categoria categoria)
        {

            if (categoria is null)
            {
                _logger.LogWarning("Dados invalidos...");
                return BadRequest("Dados invalidos...");
            }

            var categoriaCriada = _repository.Create(categoria);



            return new CreatedAtRouteResult("categoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);


        }

        /// <summary>
        /// altera a informação
        /// </summary>
        /// <returns></returns>

        [HttpPut("{id:int}")]
        public ActionResult<Categoria> Put(int id, Categoria categoria)
        {

            if (id != categoria.CategoriaId)
            {
                _logger.LogWarning("Dados invalidos...");
                return BadRequest("Dados invalidos...");
            }
            var categoriaAtt = _repository.Update(categoria);

            return Ok(categoriaAtt);


        }

        /// <summary>
        /// Deleta o elemento selecionado
        /// </summary>
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {

            var categoriaDelet = _repository.Delete(id);

            if (categoriaDelet is null)
            {
                _logger.LogWarning("Dados invalidos...");
                return NotFound("produto n encontrado");
            }
            return Ok(categoriaDelet);




        }


    }
}
