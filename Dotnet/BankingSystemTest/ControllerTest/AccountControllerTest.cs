using System.Threading.Tasks;
using DTO;
using interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Controllers;

namespace BankingSystemTest.ControllerTest
{
    [TestFixture]
    public class AccountControllerTest
    {
        private Mock<IAccountService> _mockService;
        private AccountController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IAccountService>();
            _controller = new AccountController(_mockService.Object);
        }

        [Test]
        public async Task CreateAccount_ShouldReturnOk_WhenSuccess()
        {
            var dto = new AccountCreationDTO { UserId = 1, AccountType = "Savings", OpeningBalance = 1000 };
            _mockService.Setup(s => s.CreateAccountAsync(dto)).ReturnsAsync("Success");

            var result = await _controller.CreateAccount(dto);

            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value.ToString(), Does.Contain("Account created successfully"));
        }

        [Test]
        public async Task CreateAccount_ShouldReturnBadRequest_WhenFailed()
        {
            var dto = new AccountCreationDTO { UserId = 2, AccountType = "Current", OpeningBalance = 500 };
            _mockService.Setup(s => s.CreateAccountAsync(dto)).ReturnsAsync("Account type not allowed");

            
            var result = await _controller.CreateAccount(dto);

            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            var badResult = result as BadRequestObjectResult;
            Assert.That(badResult, Is.Not.Null);
            Assert.That(badResult.Value.ToString(), Does.Contain("Account type not allowed"));
        }
    }
}
