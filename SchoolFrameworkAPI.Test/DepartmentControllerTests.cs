using DataAccessLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchoolFrameworkAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SchoolFrameworkAPI.Test
{
    [TestClass]
    public class DepartmentControllerTests
    {             
        [TestMethod]
        public void GetDepartments_ShouldReturnAllDepartments()
        {
            // Arrange
            var data = new List<Department>
            {
                new Department { Id = 1, Name = "HR", DateCreated = DateTime.UtcNow },
                new Department { Id = 2, Name = "IT", DateCreated = DateTime.UtcNow }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Department>>();
            mockSet.As<IQueryable<Department>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Department>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Department>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Department>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ScoolFrameworkEntities>();
            mockContext.Setup(c => c.Department).Returns(mockSet.Object);

            // Act
            //var controller = new DepartmentsController();
            //var result = controller.GetDepartments();

            //// Assert
            //Assert.IsNotNull(result);
            //Assert.AreEqual(2, result.Count());
            //Assert.AreEqual("HR", result.ElementAt(0).Name);
            //Assert.AreEqual("IT", result.ElementAt(1).Name);
        }
    }
}
