using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Service;
using DTO;
using Model;
using MyDbContext;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using Model.DTOs;

namespace Tests.ServiceTest
{
    public class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
            CreateMap<TransactionModel, TransactionDTO>()
                .ForMember(dest => dest.FromAccount, opt => opt.MapFrom(src => src.FromAccountNumber))
                .ForMember(dest => dest.ToAccount, opt => opt.MapFrom(src => src.ToAccountNumber))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType.TransactionType))
                .ForMember(dest => dest.FromUser, opt => opt.MapFrom(src => src.FromAccount.User.UserName))
                .ForMember(dest => dest.ToUser, opt => opt.MapFrom(src => src.ToAccount.User.UserName))
                .ForMember(dest => dest.FromEmail, opt => opt.MapFrom(src => src.FromAccount.User.Email))
                .ForMember(dest => dest.ToEmail, opt => opt.MapFrom(src => src.ToAccount.User.Email));
        }
    }

    [TestFixture]
    public class TransactionServiceTest
    {
        private MyAppDbContext _context;
        private IMapper _mapper;
        private TransactionService _transactionService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MyAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new MyAppDbContext(options);

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new TransactionMappingProfile()));
            _mapper = config.CreateMapper();

            _transactionService = new TransactionService(_context, _mapper);

            _context.DbUsers.Add(new UsersModel { UserId = 1, UserName = "John", Email = "john@example.com" });
            _context.DbUsers.Add(new UsersModel { UserId = 2, UserName = "Alice", Email = "alice@example.com" });

            _context.DbAccountType.Add(new MasterAccountTypeModel { AccountTypeID = 1, AccountType = "Savings" });
            _context.DbAccountType.Add(new MasterAccountTypeModel { AccountTypeID = 2, AccountType = "Current" });

            _context.DbAccount.Add(new AccountModel
            {
                AccountNumber = 1001,
                UserId = 1,
                AccountTypeId = 1,
                Balance = 5000,
                IsActive = true,
                IsAccountClosed = false
            });

            _context.DbAccount.Add(new AccountModel
            {
                AccountNumber = 1002,
                UserId = 2,
                AccountTypeId = 2,
                Balance = 3000,
                IsActive = true,
                IsAccountClosed = false
            });

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task MakeTransaction_ShouldSucceed_WhenValid()
        {
            var dto = new MakeTransactionDTO
            {
                FromAccount = 1001,
                ToAccount = 1002,
                Amount = 1000,
                TransactionTypeId = 1
            };

            var result = await _transactionService.MakeTransactionAsync(dto);

            Assert.That(result, Is.EqualTo("Transaction successful."));

            var fromAccount = await _context.DbAccount.FindAsync(1001);
            var toAccount = await _context.DbAccount.FindAsync(1002);

            Assert.That(fromAccount.Balance, Is.EqualTo(4000));
            Assert.That(toAccount.Balance, Is.EqualTo(4000));
        }

        [Test]
        public async Task MakeTransaction_ShouldFail_WhenInsufficientBalance()
        {
            var dto = new MakeTransactionDTO
            {
                FromAccount = 1002,
                ToAccount = 1001,
                Amount = 5000,
                TransactionTypeId = 1
            };

            var result = await _transactionService.MakeTransactionAsync(dto);

            Assert.That(result, Is.EqualTo("Insufficient balance."));
        }

        [Test]
        public async Task MakeTransaction_ShouldRequireVerification_WhenHighValue()
        {
            var dto = new MakeTransactionDTO
            {
                FromAccount = 1001,
                ToAccount = 1002,
                Amount = 15000,
                TransactionTypeId = 3
            };

            var result = await _transactionService.MakeTransactionAsync(dto);

            Assert.That(result, Is.EqualTo("Pending staff approval for high value transaction."));
        }

        [Test]
        public async Task ApproveTransaction_ShouldApproveSuccessfully()
        {
            var txn = new TransactionModel
            {
                FromAccountNumber = 1001,
                ToAccountNumber = 1002,
                Amount = 2000,
                TransactionTypeID = 3,
                IsVerificationRequired = true,
                IsSuccess = false,
                TransactionDate = IndianTime.GetIndianTime()
            };

            _context.DbTransactions.Add(txn);
            await _context.SaveChangesAsync();

            var result = await _transactionService.ApproveTransactionAsync(txn.TransactionId, "", 1, true);

            Assert.That(result, Is.EqualTo("Transaction approved successfully."));

            var from = await _context.DbAccount.FindAsync(1001);
            var to = await _context.DbAccount.FindAsync(1002);

            Assert.That(from.Balance, Is.EqualTo(3000));
            Assert.That(to.Balance, Is.EqualTo(5000));
        }

        [Test]
        public async Task ApproveTransaction_ShouldRejectTransaction()
        {
            var txn = new TransactionModel
            {
                FromAccountNumber = 1001,
                ToAccountNumber = 1002,
                Amount = 2000,
                TransactionTypeID = 3,
                IsVerificationRequired = true,
                IsSuccess = false,
                TransactionDate = IndianTime.GetIndianTime()
            };

            _context.DbTransactions.Add(txn);
            await _context.SaveChangesAsync();

            var result = await _transactionService.ApproveTransactionAsync(txn.TransactionId, "Rejected by staff", 1, false);

            Assert.That(result, Is.EqualTo("Transaction rejected."));
        }

        [Test]
        public async Task GetAllTransactionsToApprove_ShouldReturnPendingTransactions()
        {
            _context.DbTransactions.Add(new TransactionModel
            {
                FromAccountNumber = 1001,
                ToAccountNumber = 1002,
                Amount = 15000,
                TransactionTypeID = 3,
                IsVerificationRequired = true,
                IsSuccess = false,
                TransactionDate = IndianTime.GetIndianTime()
            });
            await _context.SaveChangesAsync();

            var transactions = await _transactionService.GetAllTransactionsToApproveAsync();

            Assert.That(transactions.Count, Is.EqualTo(1));
            Assert.That(transactions.First().IsVerificationRequired, Is.True);
        }

        [Test]
        public async Task GetAllTransactionByAccount_ShouldReturnTransactions()
        {
            _context.DbTransactions.Add(new TransactionModel
            {
                FromAccountNumber = 1001,
                ToAccountNumber = 1002,
                Amount = 1000,
                TransactionTypeID = 1,
                IsVerificationRequired = false,
                IsSuccess = true,
                TransactionDate = IndianTime.GetIndianTime()
            });
            await _context.SaveChangesAsync();

            var transactions = await _transactionService.GetAllTransactionByAccountAsync(1001);

            Assert.That(transactions.Count, Is.EqualTo(1));
        }
    }
}
