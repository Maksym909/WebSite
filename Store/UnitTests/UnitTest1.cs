using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.HtmlHelpers;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckIsFourGoodsOnPage()
        {
            //Arrange
            Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
            mock.Setup(m => m.Goods).Returns(new List<Good>         // We add 5 items for check is 4 stuff on a page
            {
                new Good{ Id=1,Name="Good1"},
                new Good{ Id=2,Name="Good2"},
                new Good{ Id=3,Name="Good3"},
                new Good{ Id=4,Name="Good4"},
                new Good{ Id=5,Name="Good5"},
            });
            GoodController controller = new GoodController(mock.Object);
            controller.pageSize = 3;
            //Act
           // IEnumerable<Good> result = (IEnumerable<Good>)controller.List(2).Model;  //Мы обращаемся к свойству Model объекта результата,
                                                                                     //чтобы получить последовательность IEnumerable<Good> 
                                                                                    //сгенерированную методом List().

             GoodsListViewModel result = (GoodsListViewModel)controller.List(null, 2).Model;
            //Assert
            List<Good> goods = result.Goods.ToList();
            Assert.IsTrue(goods.Count==2);
            Assert.AreEqual(goods[0].Name, "Good4");
            Assert.AreEqual(goods[1].Name, "Good5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange
            HtmlHelper myhelper = null;
            PageInfo pageInfo = new PageInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;
            //Act
            MvcHtmlString result = myhelper.PageLinks(pageInfo, pageUrlDelegate);
            //Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
               + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
               + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
               result.ToString());
        }

        [TestMethod]
        public void Can_Send_Paginating_View_Model()
        {
            //Arrange
            Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
            mock.Setup(m => m.Goods).Returns(new List<Good>
            {
                new Good{Id=1, Name="Good1"},
                new Good{Id=2, Name="Good2"},
                new Good{Id=3, Name="Good3"},
                new Good{Id=4, Name="Good4"},
                new Good{Id=5, Name="Good5"},
            });
            GoodController controller = new GoodController(mock.Object);
            controller.pageSize = 3;
            //Act
            GoodsListViewModel result = (GoodsListViewModel)controller.List(null, 2).Model;
            //Assert
            PageInfo pageInfo = result.PageInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);

        }

        [TestMethod]
        public void Can_Filter_Goods()
        {
            //Arr
            Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
            mock.Setup(m => m.Goods).Returns(new List<Good>
            {
                new Good{Id=1,Name="Good1",Category="Category1"},
                new Good{Id=2,Name="Good2",Category="Category2"},
                new Good{Id=3,Name="Good3",Category="Category1"},
                new Good{Id=4,Name="Good4",Category="Category2"},
                new Good{Id=5,Name="Good5",Category="Category3"},
            });
            GoodController controller = new GoodController(mock.Object);
            controller.pageSize = 3;
            //Act
            List<Good> result = ((GoodsListViewModel)controller.List("Category2", 1).Model).Goods.ToList();
            //Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Good2" && result[0].Category == "Category2");
            Assert.IsTrue(result[1].Name == "Good4" && result[1].Category == "Category2");
        }

        [TestMethod]
        public void Cab_Create_Categories()
        {
            Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
            mock.Setup(m => m.Goods).Returns(new List<Good>
            {
                new Good{Id=1,Name="Good1",Category="Симулятор"},
                new Good{Id=2,Name="Good2",Category="Симулятор"},
                new Good{Id=3,Name="Good3",Category="Шутер"},
                new Good{Id=4,Name="Good4",Category="Шутер"},
                new Good{Id=5,Name="Good5",Category="RPG"}
            });
            NavController target = new NavController(mock.Object);

            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "RPG");
            Assert.AreEqual(results[1], "Симулятор");
            Assert.AreEqual(results[2], "Шутер");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
            mock.Setup(m => m.Goods).Returns(new Good[]
            {
                new Good{Id=1,Name="Good1",Category="Симулятор"},
                new Good{Id=2,Name="Good2",Category="Шутер"}
            });
            NavController target = new NavController(mock.Object);
            string categoryToSelect = "Шутер";
            //Act
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            Assert.AreEqual(categoryToSelect, result);
            
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            /// Организация (arrange)
            Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
            mock.Setup(m => m.Goods).Returns(new List<Good>
    {
        new Good { Id = 1, Name = "Good1", Category="Cat1"},
        new Good { Id = 3, Name = "Good3", Category="Cat1"},
        new Good { Id = 4, Name = "Good4", Category="Cat2"},
        new Good { Id = 2, Name = "Good2", Category="Cat2"},
        new Good { Id = 5, Name = "Good5", Category="Cat3"}
    });
            GoodController controller = new GoodController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((GoodsListViewModel)controller.List("Cat1").Model).PageInfo.TotalItems;
            int res2 = ((GoodsListViewModel)controller.List("Cat2").Model).PageInfo.TotalItems;
            int res3 = ((GoodsListViewModel)controller.List("Cat3").Model).PageInfo.TotalItems;
            int resAll = ((GoodsListViewModel)controller.List(null).Model).PageInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
