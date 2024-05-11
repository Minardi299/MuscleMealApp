// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Conventions;
// using MuscleMeal;
// using Moq;

// namespace ProjectTests;

// [TestClass]
// public class UserServiceTest
// {
//     [TestMethod]
//     public void SubmitRecipeTest()
//     {
//         //Arrange
//         byte [] hash1=new byte[] {1,2,3};
//         byte [] hash2=new byte[] {1,3,5};
//         List<string> ingredients = new List<string> {"beef","seasoning"};
//         User user = new User("TestUser", "Fitness Enthusiast",hash1,hash2);
//         Recipe r1 = new Recipe("steak","a steak",user,ingredients);

//         var mockSet = new Mock<DbSet<Recipe>>();
//         var mockContext = new Mock<MyDbContext>();
//         mockContext.Setup(m => m.Recipe).Returns(mockSet.Object);
//         var userService = new UserService(mockContext.Object,user);

//         //Act
//         userService.SubmitRecipe(r1);

//         //Assert
//         mockSet.Verify(m => m.Add(It.IsAny<Recipe>()), Times.Once());
//         mockContext.Verify(m => m.SaveChanges(), Times.Once());
//         // Assert.AreEqual(r1.Name, user.Recipes[0].Name); 
//         // mockContext.Verify(u => u.SaveChanges(), Times.Once());
//     }
// }
