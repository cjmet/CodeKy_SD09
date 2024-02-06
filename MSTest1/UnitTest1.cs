using Moq;
using DataLibrary;
using CodeKY_SD01.Logic;

namespace MSTest1
{
    [TestClass]
    public class ProductLogicTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly ProductLogic _productLogic;

        public ProductLogicTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _productLogic = new ProductLogic(_productRepositoryMock.Object, _orderRepositoryMock.Object);
        }

        [TestMethod]
        public void GetProductById_CallsRepo()
        {
            //Arrange
            _productRepositoryMock.Setup(x => x.GetProductById(1)).Returns(new ProductEntity { Id = 1, Name = "test product" });

            //Act
            _productLogic.GetProductById(1);

            //Assert
            _productRepositoryMock.Verify(x => x.GetProductById(1), Times.Once);
        }
    }
}