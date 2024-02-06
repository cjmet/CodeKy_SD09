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
        


        [Test]
        public void GetProductById_CallsRepo()
        {
            //Arrange
            _productRepositoryMock.Setup(x => x.GetProductById(1)).Returns(new ProductEntity { Id = 1, Name = "test product" });
            _productRepositoryMock.Setup(x => x.GetProductById(1)).Returns(new ProductLogic { Id = 1, Name = "test product" });

            //Act
            _productLogic.GetProductById(1);


            // Arrange
            Mock<IProductRepository> _productRepositoryMock = new Mock<IProductRepository>();
            Mock<IOrderRepository> _orderRepositoryMock = new Mock<IOrderRepository>();
            var _productLogic = new ProductLogic(_productRepositoryMock.Object);
            var _orderLogic = new ProductLogic(_orderRepositoryMock.Object);
            _productRepositoryMock.Setup(x => x.GetProductById(10)).Returns(new ProductEntity { Id = 10, Name = "test product" });

            //Act
            _productRepositoryMock.Verify(x => x.GetProductById(10), Times.Once);

        }

    }
}
