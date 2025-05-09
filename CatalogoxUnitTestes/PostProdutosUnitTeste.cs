using Catalogo.Controllers;
using Catalogo.DTOs;
using CatalogoxUnitTestes.xUnitTeste;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoxUnitTestes
{
   public class PostProdutosUnitTeste : IClassFixture<ProdutosTestexUnitController>
    {
        private readonly ProdutoController _controller;

        public PostProdutosUnitTeste(ProdutosTestexUnitController controller)
        {


            _controller = new ProdutoController(controller.repository, controller.mapper, controller._cache);
        }




        [Fact]
        public async Task PostProduto_Return_CreatAtResult()
        {
            //
            var produto = new ProdutosDTO
            {
                CategoriaId = 2,
                Descricao = "ficssdadsds",
                ImagemUrl = "idsdjskdaskjd",
                Nome = "pirudklsd",
                Preco = 55,
                ProdutoId = 455
            };

            //Act
            var data = await _controller.Post(produto);

            ////Assert
            //var okResult = Assert.IsType<OkObjectResult>(data.Result);
            //Assert.Equal(200, okResult.StatusCode);

            //Assert(fluent)
            var creatResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
            creatResult.Subject.StatusCode.Should().Be(201);

        }

        [Fact]
        public async Task PostProduto_Return_BadRequest()
        {
            //
            ProdutosDTO produto = null;

            //Act
            var data = await _controller.Post(produto);

            ////Assert
            //var okResult = Assert.IsType<OkObjectResult>(data.Result);
            //Assert.Equal(200, okResult.StatusCode);

            //Assert(fluent)
            var creatResult = data.Result.Should().BeOfType<BadRequestResult>();
            creatResult.Subject.StatusCode.Should().Be(400);

        }

    }
}
