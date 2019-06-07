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
    public class ImageTests
    {

        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            
            Good good = new Good
            {
                Id = 2,
                Name = "Good2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            
            Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
            mock.Setup(m => m.Goods).Returns(new List<Good> {
                new Good {Id = 1, Name = "Good1"},
                good,
                new Good {Id = 3, Name = "Good3"}
            }.AsQueryable());
            
            GoodController controller = new GoodController(mock.Object);
            
            ActionResult result = controller.GetImage(2);
          
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(good.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            Mock<IGoodRepository> mock = new Mock<IGoodRepository>();
            mock.Setup(m => m.Goods).Returns(new List<Good> {
                new Good {Id = 1, Name = "Игра1"},
                new Good {Id = 2, Name = "Игра2"}
            }.AsQueryable());
            
            GoodController controller = new GoodController(mock.Object);
            
            ActionResult result = controller.GetImage(10);
            
            Assert.IsNull(result);
        }
    }
}


