using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SIS.MvcFramework.Tests
{
    public class ViewEngineTests
    {
        [Theory]
        [InlineData("OnlyHtmlView")]
        [InlineData("ForForeachIfView")]
        [InlineData("ViewModelView")]
        public void GetHtmlTest(string testName)
        {
            var viewModel = new TestViewModel
            {
                Name = "Niki",
                Year = 2020,
                Numbers = new List<int> { 10, 100, 1000, 10000 }
            };

            var viewContent = File.ReadAllText($"ViewTests/{testName}.html");
            var expectedViewContent = File.ReadAllText($"ViewTests/{testName}Expected.html");

            IViewEngine viewEngine = new ViewEngine();
            var actualResult = viewEngine.GetHtml(viewContent, viewModel);

            Assert.Equal(expectedViewContent, actualResult);

        }
       
      
    }
}
