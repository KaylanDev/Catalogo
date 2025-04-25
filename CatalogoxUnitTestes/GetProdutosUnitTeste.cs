using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Catalogo.Controllers;
using CatalogoxUnitTestes.xUnitTeste;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit.Sdk;

namespace CatalogoxUnitTestes;

public class GetProdutosxUnitTeste : IClassFixture<ProdutosTestexUnitController>
{
    private readonly ProdutoController _controller;

    public GetProdutosxUnitTeste(ProdutosTestexUnitController controller)
    {
       
 
            _controller = new ProdutoController(controller.repository, controller.mapper);
        }

    [Fact]
    public async Task GetProdutoById_Return_OkResult()
    {
        //Arrange
        var PId = 2;

        //Act
        var data = await _controller.GetById(PId);

        ////Assert
        //var okResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okResult.StatusCode);

        //Assert(fluent)
        data.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);

    }
    [Fact]
    public async Task GetProdutoById_Return_NotFound()
    {
        //Arrange
        var PId = 999;

        //Act
        var data = await _controller.GetById(PId);

        ////Assert
        //var okResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okResult.StatusCode);

        //Assert(fluent)
        data.Result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(404);

    }

    [Fact]
    public async Task GetProdutoById_Return_BadRequest()
    {
        //Arrange
        var PId = 0;

        //Act
        var data = await _controller.GetById(PId);

        ////Assert
        //var okResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okResult.StatusCode);

        //Assert(fluent)
        data.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be(400);

    }


    //GET
    [Fact]
    public async Task GetProduto_Return_OkResult()
    {
      

        //Act
        var data = await _controller.Get();

        ////Assert
        //var okResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okResult.StatusCode);

        //Assert(fluent)
        data.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);

    }

    [Fact]
    public async Task GetProduto_Return_NotFound()
    {


        //Act
        var data = await _controller.Get();

        ////Assert
        //var okResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okResult.StatusCode);

        //Assert(fluent)
        data.Result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(404);

    }

    [Fact]
    public async Task GetProduto_Return_BadRequest()
    {


        //Act
        var data = await _controller.Get();

        ////Assert
        //var okResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okResult.StatusCode);

        //Assert(fluent)
        data.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);

    }




}
