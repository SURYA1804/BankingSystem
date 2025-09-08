using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers;
using Service;
using DTO;
using interfaces;
using Model.DTOs;

namespace BankingSystemTest.ControllerTest
{
    [TestFixture]
    public class TransactionControllerTest
    {
        private Mock<ITransactionService> _mockService;
        private TransactionController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ITransactionService>();
            _controller = new TransactionController(_mockService.Object);
        }

        [Test]
        public async Task MakeTransaction_ShouldReturnOk_WhenSuccessful()
        {
            var makeTransactionDto = new MakeTransactionDTO
            {
                FromAccount = 123,
                ToAccount = 456,
                Amount = 1000
            };

            _mockService.Setup(s => s.MakeTransactionAsync(makeTransactionDto))
                        .ReturnsAsync("Transaction completed successfully.");

            var result = await _controller.MakeTransaction(makeTransactionDto);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo("Transaction completed successfully."));
        }

        [Test]
        public async Task MakeTransaction_ShouldReturnBadRequest_WhenFailed()
        {
            var makeTransactionDto = new MakeTransactionDTO
            {
                FromAccount = 123,
                ToAccount = 456,
                Amount = 5000
            };

            _mockService.Setup(s => s.MakeTransactionAsync(makeTransactionDto))
                        .ReturnsAsync("Insufficient balance");

            var result = await _controller.MakeTransaction(makeTransactionDto);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest.Value, Is.EqualTo("Insufficient balance"));
        }

        [Test]
        public async Task GetAllTransactionsToApprove_ShouldReturnListOfTransactions()
        {
            var transactions = new List<TransactionDTO>
            {
                new TransactionDTO { TransactionId = 1, FromAccount = 123, ToAccount = 456, Amount = 100 },
                new TransactionDTO { TransactionId = 2, FromAccount = 789, ToAccount = 101, Amount = 200 }
            };

            _mockService.Setup(s => s.GetAllTransactionsToApproveAsync())
                        .ReturnsAsync(transactions);

            var result = await _controller.GetAllTransactionsToApprove();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(transactions));
        }

        [Test]
        public async Task GetAllTransactionsByAccount_ShouldReturnTransactionsForAccount()
        {
            long accountNumber = 123;
            var transactions = new List<TransactionDTO>
            {
                new TransactionDTO { TransactionId = 1, FromAccount = 123, ToAccount = 456, Amount = 100 }
            };

            _mockService.Setup(s => s.GetAllTransactionByAccountAsync(accountNumber))
                        .ReturnsAsync(transactions);

            var result = await _controller.GetAllTransactionsByAccount(accountNumber);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(transactions));
        }

        [Test]
        public async Task ApproveTransaction_ShouldReturnOk_WhenApproved()
        {
            int transactionId = 1;
            _mockService.Setup(s => s.ApproveTransactionAsync(transactionId, "Approved", 1, true))
                        .ReturnsAsync("Transaction approved successfully.");

            var result = await _controller.ApproveTransaction(transactionId, "Approved", 1, true);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo("Transaction approved successfully."));
        }

        [Test]
        public async Task ApproveTransaction_ShouldReturnOk_WhenRejected()
        {
            int transactionId = 2;
            _mockService.Setup(s => s.ApproveTransactionAsync(transactionId, "Rejected", 1, false))
                        .ReturnsAsync("Transaction rejected.");

            var result = await _controller.ApproveTransaction(transactionId, "Rejected", 1, false);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo("Transaction rejected."));
        }

        [Test]
        public async Task ApproveTransaction_ShouldReturnBadRequest_WhenError()
        {
            int transactionId = 3;
            _mockService.Setup(s => s.ApproveTransactionAsync(transactionId, "Error", 1, true))
                        .ReturnsAsync("Some error occurred");

            var result = await _controller.ApproveTransaction(transactionId, "Error", 1, true);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest.Value, Is.EqualTo("Some error occurred"));
        }
    }
}
