using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Abstract;
using Moq;
using GameStore.WebUI.Controllers;
using System.Web.Mvc;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void Can_Add_New_Line()
        {
            Good good1 = new Good { Id = 1, Name = "Good1" };
            Good good2 = new Good { Id = 2, Name = "Good2" };

            Cart cart = new Cart();

            cart.AddItem(good1, 1);
            cart.AddItem(good2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Good, good1);
            Assert.AreEqual(results[1].Good, good2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание нескольких тестовых игр
            Good good1 = new Good { Id = 1, Name = "Good1" };
            Good good2 = new Good { Id = 2, Name = "Good2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(good1, 1);
            cart.AddItem(good2, 1);
            cart.AddItem(good1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Good.Id).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых игр
            Good good1 = new Good { Id = 1, Name = "Good1" };
            Good good2 = new Good { Id = 2, Name = "Good2" };
            Good good3 = new Good { Id = 3, Name = "Good3" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - добавление нескольких игр в корзину
            cart.AddItem(good1, 1);
            cart.AddItem(good2, 4);
            cart.AddItem(good3, 2);
            cart.AddItem(good2, 1);

            // Действие
            cart.RemoveLine(good2);

            // Утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Good == good2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых игр
            Good good1 = new Good { Id = 1, Name = "Good1", Price = 100 };
            Good good2 = new Good { Id = 2, Name = "Good2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(good1, 1);
            cart.AddItem(good2, 1);
            cart.AddItem(good1, 5);
            decimal result = cart.ComputeTotalValue();

            // Утверждение
            Assert.AreEqual(result, 655);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых игр
            Good good1 = new Good { Id = 1, Name = "Good1", Price = 100 };
            Good good2 = new Good { Id = 2, Name = "Good2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(good1, 1);
            cart.AddItem(good2, 1);
            cart.AddItem(good1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        //[TestMethod]
        //public void CanAddToCart()
        //{
        //    //Arrange
        //    Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
        //    mock.Setup(m => m.Goods).Returns(new List <Good>{ new Good { Id = 1, Name = "Good1", Category = "Category1" },}.AsQueryable());

        //    Cart cart = new Cart();
        //    CartController cartController = new CartController(mock.Object);
        //    //Act
        //    cartController.AddToCart(cart, 1, null);
        //    //Assert
        //    Assert.AreEqual(cart.Lines.Count(), 1);
        //    Assert.AreEqual(cart.Lines.ToList()[0].Good.Id, 1);
        //}

        //[TestMethod]
        //public void AddingGoodToCaartGoToCartScreen()
        //{
        //    Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
        //    mock.Setup(m => m.Goods).Returns(new List<Good> { new Good { Id = 1, Name = "Good1", Category = "Category1" }, }.AsQueryable());

        //    Cart cart = new Cart();
        //    CartController cartController = new CartController(mock.Object);

        //    //Act
        //    RedirectToRouteResult result = cartController.AddToCart(cart, 2, "myUrl");
        //    //Assert
        //    Assert.AreEqual(result.RouteValues["action"], "Index");
        //    Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        //}

        //[TestMethod]
        //public void Can_View_Cart_Contents()
        //{
        //    // Организация - создание корзины
        //    Cart cart = new Cart();

        //    // Организация - создание контроллера
        //    CartController target = new CartController(null);

        //    // Действие - вызов метода действия Index()
        //    CartIndexViewModel result
        //        = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

        //    // Утверждение
        //    Assert.AreSame(result.Cart, cart);
        //    Assert.AreEqual(result.ReturnUrl, "myUrl");
        //}

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            Cart cart = new Cart();

            // Организация - создание деталей о доставке
            ShippingDetails shippingDetails = new ShippingDetails();

            // Организация - создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Good(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Good(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.ProcessorOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            Assert.AreEqual("Completed", result.ViewName);

            // Утверждение - проверка, что представлению передается допустимая модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}

