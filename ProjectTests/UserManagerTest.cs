using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MuscleMeal;
using System.Security.Cryptography;
using System.Text;
namespace musclemeals;

[TestClass]
public class UserManagerTest
{
    private Mock<MyDbContext>? mockContext;
    private UserManager? userManager;
    private Mock<DbSet<User>>? mockSet;
    private List<User>? userList;

    [TestInitialize]
    public void Initialize()
    {
        try
        {
            mockSet = new Mock<DbSet<User>>();
            mockContext = new Mock<MyDbContext>();

            userList = new List<User> {
            new User("User1", "Bio1", new byte[] { 0x01, 0x02 }, new byte[] { 0x01, 0x02 }),
            new User("User2", "Bio2", new byte[] { 0x02, 0x03 }, new byte[] { 0x02, 0x03 })
        };

            var userQueryable = userList.AsQueryable();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(userQueryable.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userQueryable.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userQueryable.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(userQueryable.GetEnumerator());
            mockContext.Setup(m => m.User).Returns(mockSet.Object);

            userManager = new UserManager(mockContext.Object);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during initialization: " + ex.Message);
            throw;  // Re-throw the exception to fail the test if initialization fails
        }
    }

    [TestMethod]
    public void Register_Authenticate_ShouldReturnTrue()
    {
        // Act
        var result = userManager.Register("NewUser", "New bio", "newpassword123");

        // Assert
        Assert.IsTrue(result);
        mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
        mockContext.Verify(m => m.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void Registering_ExistingUser_ShouldReturnFalse()
    {
        // Act
        var result = userManager.Register("User1", "Bio1", "password");

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Login_ValidCredentials_ReturnsTrue()
    {
        // Arrange
        userManager.CreatePasswordHash("password", out byte[] hash, out byte[] salt);
        userList[0].Password = hash;
        userList[0].PasswordSalt = salt;

        // Act
        var result = userManager.Login("User1", "password");

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(userManager.CurrentUser, userList[0]);
    }

    [TestMethod]
    public void Login_InvalidCredentials_ReturnsFalse()
    {
        // Act
        var result = userManager.Login("User1", "wrongpassword");

        // Assert
        Assert.IsFalse(result);
        Assert.IsNull(userManager.CurrentUser);
    }

    [TestMethod]
    public void Logout_ClearsCurrentUser()
    {
        // Arrange
        userManager.Login("User1", "password");  // Assume this logs in correctly

        // Act
        userManager.Logout();

        // Assert
        Assert.IsNull(userManager.CurrentUser);
    }

    // [TestMethod]
    // public void UpdateUser_UpdatesUser()
    // {
    //     // Arrange
    //     var user = new User("UpdateUser", "Bio", new byte[] { 0x01 }, new byte[] { 0x02 });
    //     mockSet.Setup(m => m.Update(It.IsAny<User>())).Verifiable();

    //     // Act
    //     userManager.UpdateUser(user);

    //     // Assert
    //     mockSet.Verify(m => m.Update(It.IsAny<User>()), Times.Once());
    //     mockContext.Verify(m => m.SaveChanges(), Times.Once());
    // }

    // [TestMethod]
    // public void DeleteUserAccount_UserExists_AccountDeleted()
    // {
    //     // Arrange
    //     var user = new User("ToDelete", "Bio", new byte[] { 0x01 }, new byte[] { 0x02 }) { ID = 1 };
    //     mockSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockSet.Object);
    //     mockSet.Setup(m => m.FirstOrDefault(It.IsAny<Func<User, bool>>())).Returns(user);

    //     // Act
    //     var result = userManager.DeleteUserAccount(1);

    //     // Assert
    //     Assert.IsTrue(result);
    //     mockSet.Verify(m => m.Remove(It.IsAny<User>()), Times.Once());
    //     mockContext.Verify(m => m.SaveChanges(), Times.Once());
    // }

    [TestMethod]
    public void GetAllUsers_ReturnsAllUsers()
    {
        // Act
        var result = userManager.GetAllUsers();

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.Contains(userList[0]) && result.Contains(userList[1]));
    }


    [TestMethod]
    public void RegisterUserTest()
    {
        //Arrange
        var mockSet = new Mock<DbSet<User>>();
        var mockContext = new Mock<MyDbContext>();
        mockContext.Setup(m => m.User).Returns(mockSet.Object);
        var manager = new UserManager(mockContext.Object);

        //Act
        manager.Register("nameof user", "bio of user", "password");

        //Assert
        mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
        mockContext.Verify(m => m.SaveChanges(), Times.Once());
    }

    [TestMethod]
    public void GetAllUsersTest()
    {
        //Arrange
        byte[] hash1 = new byte[] { 1, 2, 3 };
        byte[] hash2 = new byte[] { 1, 3, 5 };
        List<User> usersList = new List<User>();
        User newUser = new User("John", "Fitness Enthusiast", hash1, hash2);
        User newUser2 = new User("Jack", "Fitness Enthusiast", hash2, hash1);
        usersList.Add(newUser);
        usersList.Add(newUser2);
        var data = usersList.AsQueryable();
        var mockSet = new Mock<DbSet<User>>();
        mockSet.As<IQueryable<User>>().Setup(x => x.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<User>>().Setup(x => x.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<User>>().Setup(x => x.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<User>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());
        var mockContext = new Mock<MyDbContext>();
        mockContext.Setup(c => c.User).Returns(mockSet.Object);
        var manager = new UserManager(mockContext.Object);

        //Act
        List<User> allUsers = manager.GetAllUsers();
        Assert.AreEqual(2, allUsers.Count);
        Assert.AreEqual("John", allUsers[0].Username);
        Assert.AreEqual("Jack", allUsers[1].Username);

        //Assert
        Assert.AreEqual(2, allUsers.Count);
        Assert.AreEqual("John", allUsers[0].Username);
        Assert.AreEqual("Jack", allUsers[1].Username);
    }
}
