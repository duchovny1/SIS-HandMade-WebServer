using System;
using System.Collections.Generic;
using Xunit;

namespace SIS.MvcFramework.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void Test1(string testName)
        {
            var viewModel = new TestViewModel
            {
                Name = "Niki",
                Year = 2020,
                Numbers = new List<int> { 10, 100, 1000, 10000 }
            };

        }
       
      
    }
}
