using CodeKY_SD01.Logic;
using DataLibrary;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeKY_SD01.Tests
{
    [TestFixture]
    public class ProductLogicTests
    {
        private Mock<ProductContext> _mockContext;
        private ProductLogic _productLogic;

        [SetUp]
        public void SetUp()
        {
            //_mockContext = new Mock<ProductContext>();
            //_productLogic = new ProductLogic(_mockContext.Object);
        }

        [Test]
        public void GetProductById_CallsRepo()
        {
            // Arrange
            // _mockContext.Setup(m => m.Products).Returns((DbSet<ProductEntity>)new List<ProductEntity>().AsQueryable());
            // Act
            // _productLogic.GetProductById(1);
            // Assert

            //// Arrange
            //Mock<IProductRepository> _productRepositoryMock = new Mock<IProductRepository>();
            //Mock<IOrderRepository> _orderRepositoryMock = new Mock<IOrderRepository>();
            //var _productLogic = new ProductLogic(_productRepositoryMock.Object);
            //var _orderLogic = new ProductLogic(_orderRepositoryMock.Object);
            //_productRepositoryMock.Setup(x => x.GetProductById(10)).Returns(new ProductEntity { Id = 10, Name = "test product" });

            ////Act
            //_productRepositoryMock.Verify(x => x.GetProductById(10), Times.Once);

        }


        //[Test]
        //public void AddProduct_ValidProduct_AddsProductToRepository()
        //{
        //    // Arrange
        //    var product = new ProductEntity("Test Product", "Test Category", "Test Description", 10.0m, 10);
        //    _mockContext.Setup(m => m.Add(It.IsAny<ProductEntity>())).Verifiable();

        //    // Act
        //    _productLogic.AddProduct(product);

        //    // Assert
        //    _mockContext.Verify(m => m.Add(It.IsAny<ProductEntity>()), Times.Once);
        //}

        //[Test]
        //public void GetProductByName_ValidName_ReturnsProduct()
        //{
        //    // Arrange
        //    var product = new ProductEntity("Test Product", "Test Category", "Test Description", 10.0m, 10);
        //    _mockContext.Setup(m => m.Products).Returns((DbSet<ProductEntity>)new List<ProductEntity> { product }.AsQueryable());

        //    // Act
        //    var result = _productLogic.GetProductByName("Test Product");

        //    // Assert
        //    Assert.AreEqual(product, result);
        //}

        // Add similar tests for other methods in the ProductLogic class
    }
}
