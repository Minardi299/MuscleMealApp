using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Moq;
using MuscleMeal;

namespace ProjectTests;

[TestClass]
public class RecipeManagerTest
{
    [TestMethod]
    public void GetAllRecipeTest()
    {
        //Arrange
        byte[] hash1 = new byte[] { 1, 2, 3 };
        byte[] hash2 = new byte[] { 1, 3, 5 };
        List<Recipe> recipeList = new List<Recipe>();
        List<string> ingredients = new List<string> { "beef", "seasoning" };
        List<string> ingredients2 = new List<string> { "pasta", "seasoning" };
        User user = new User("Jack", "Fitness Enthusiast", hash2, hash1);
        Recipe r1 = new Recipe("steak", "a steak", user, ingredients);
        Recipe r2 = new Recipe("pasta", "spaghetti pasta", user, ingredients2);
        recipeList.Add(r1);
        recipeList.Add(r2);
        var data = recipeList.AsQueryable();
        var mockSet = new Mock<DbSet<Recipe>>();
        mockSet.As<IQueryable<Recipe>>().Setup(x => x.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Recipe>>().Setup(x => x.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Recipe>>().Setup(x => x.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Recipe>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());
        var mockContext = new Mock<MyDbContext>();
        mockContext.Setup(r => r.Recipe).Returns(mockSet.Object);
        var recipeService = new RecipeService(mockContext.Object);
        //Act
        List<Recipe> result = recipeService.GetAllRecipes();

        //Assert
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("steak", result[0].Name);
        Assert.AreEqual("pasta", result[1].Name);
    }
}