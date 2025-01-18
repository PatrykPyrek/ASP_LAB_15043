using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using Data;
using Task_Manager.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace TaskManagerTestProject
{
    [TestFixture]
    public class ProjectControllerTests
    {
       

        [Test]
        public void Create_AddsProjectAndRedirects()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Create")
                .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockUserManager = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("user1");

            var controller = new ProjectController(dbContext, mockUserManager.Object);

            // Act
            var result = controller.Create("New Project", "Description");

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult!.ActionName, Is.EqualTo("Index"));

            var project = dbContext.Projects.FirstOrDefault(p => p.Name == "New Project");
            Assert.That(project, Is.Not.Null);
            Assert.That(project!.Description, Is.EqualTo("Description"));
            Assert.That(project.UserId, Is.EqualTo("user1"));
        }
    }
}
