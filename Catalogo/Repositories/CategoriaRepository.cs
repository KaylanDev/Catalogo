using Catalogo.Data;
using Catalogo.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Catalogo.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public Categoria GetCategoria(int id)
        {
            return _context.Categorias.FirstOrDefault(c=> c.CategoriaId == id);
        }
        public IEnumerable<Categoria> GetCategorias()
        {
            return _context.Categorias.Take(10).ToList();
        }
        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return _context.Categorias.Include(p => p.Produtos).Where(p => p.CategoriaId < 5).ToList();
        }
        public Categoria Create(Categoria categoria)
        {
            if (categoria is null) throw new ArgumentNullException("Object is null"); 

            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return categoria;
        }

        public Categoria Delete(int id)
        {
            var Categoria = _context.Categorias.Find(id);
            if (Categoria is null) throw new ArgumentNullException("Object is null");
            _context.Categorias.Remove(Categoria);
            _context.SaveChanges();
            return Categoria;
        }



        public Categoria Update(Categoria categoria)
        {
            if (categoria is null) throw new ArgumentNullException("Object is null");
            //entry modifica o elemento selecionado e o state recebe o modo modified q avisa q esta sendo modificado

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return categoria;
        }


    }
}
