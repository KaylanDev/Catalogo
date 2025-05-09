using AutoMapper;
using Catalogo.Data;
using Catalogo.DTOs.Mappins;
using Catalogo.Repositories;
using Catalogo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoxUnitTestes.xUnitTeste;

public class ProdutosTestexUnitController
{
    public IUnitOfWork repository;
    public IMapper mapper;
    public ICacheService _cache;
   
    public static DbContextOptions<AppDbContext> dbContextOptions { get; }

    public static string conectionString = "Server=localhost;Database=apicatalogo;Uid=root;Pwd=Kayzin205;";

    static ProdutosTestexUnitController()
    {
       
        var config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddUserSecrets<ProdutosTestexUnitController>() // pega os segredos
           .Build();

        

        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseMySql(conectionString, ServerVersion.AutoDetect(conectionString)).Options;

    }

    public ProdutosTestexUnitController()
    {
        var configMap = new MapperConfiguration(cfg =>
        cfg.AddProfile(new ProdutosDTOMappingProfile()));
        var memorycache = new MemoryCache(new MemoryCacheOptions());
        mapper = configMap.CreateMapper();
        var context = new AppDbContext(dbContextOptions);
        repository = new UnitOfWork(context);
        _cache = new CacheService(memorycache);
    }
}
