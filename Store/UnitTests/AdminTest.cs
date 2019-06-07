using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;

namespace UnitTests
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void Index_Contains_All_Games()
        {
            Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
            mock.Setup(m => m.Goods).Returns(new List<Good>
                {
                new Good { Id=1,Name="Good1"},
                new Good { Id=2,Name="Good2"},
                new Good { Id=3,Name="Good3"},
                new Good { Id=4,Name="Good4"},
                new Good { Id=5,Name="Good5"},
            });
            AdminController controller = new AdminController(mock.Object);

            //Act
            List<Good> result = ((IEnumerable<Good>)controller.Index().ViewData.Model).ToList();
            //Assert
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Good1", result[0].Name);
            Assert.AreEqual("Good2", result[1].Name);
            Assert.AreEqual("Good3", result[2].Name);
        }
    
        //[TestMethod]
        //public void CanEditGame()
        //{
        //    Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
        //    mock.Setup(m => m.Goods).Returns(new List<Good>
        //    {
        //        new Good{Id=1,Name="Good1"},
        //        new Good{Id=2,Name="Good2"},
        //        new Good{Id=3,Name="Good3"},
        //        new Good{Id=4,Name="Good4"},
        //        new Good{Id=5,Name="Good5"},
        //    });
        //    AdminController controller = new AdminController(mock.Object);
        //    //Act
        //    Good good1 = controller.Edit(1).ViewData.Model as Good;
        //    Good good2 = controller.Edit(2).ViewData.Model as Good;
        //    Good good3 = controller.Edit(3).ViewData.Model as Good;
        //    //Assert
        //    Assert.AreEqual(1, good1.Id);
        //    Assert.AreEqual(2, good2.Id);
        //    Assert.AreEqual(3, good3.Id);
        //}

    //    [TestMethod]
    //    public void Cannot_Edit_Nonexistent_Game()
    //    {
    //        // Организация - создание имитированного хранилища данных
    //        Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
    //        mock.Setup(m => m.Goods).Returns(new List<Good>
    //{
    //    new Good { Id = 1, Name = "Good1"},
    //    new Good { Id = 2, Name = "Good2"},
    //    new Good { Id = 3, Name = "Good3"},
    //    new Good { Id = 4, Name = "Good4"},
    //    new Good { Id = 5, Name = "Good5"}
    //});

    //        // Организация - создание контроллера
    //        AdminController controller = new AdminController(mock.Object);

    //        // Действие
    //        Good result = controller.Edit(6).ViewData.Model as Good;

    //        // Assert
    //    }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IGoodRepository> mock = new Mock<IGoodRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Good good = new Good { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(good);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveGood(good));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IGoodRepository> mock = new Mock<IGoodRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            Good good = new Good { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(good);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveGood(It.IsAny<Good>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

       [ TestMethod]
public void Can_Delete_Valid_Goods()
        {
            // Организация - создание объекта Game
            Good good = new Good { Id = 2, Name = "Good2" };

            // Организация - создание имитированного хранилища данных
            Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
            mock.Setup(m => m.Goods).Returns(new List<Good>
    {
        new Good {Id = 1, Name = "Good1"},
        new Good {Id = 2, Name = "Good2"},
        new Good {Id = 3, Name = "Good3"},
        new Good {Id = 4, Name = "Good4"},
        new Good {Id = 5, Name = "Good5"}
    });     

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие - удаление игры
            controller.Delete(good.Id);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Game
            mock.Verify(m => m.DeleteGood(good.Id));
        }
    }

   
}
