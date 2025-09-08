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

namespace Tests.ServiceTest
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountModel, AccountDTO>()
                .ForMember(dest => dest.AccountTypeName, opt => opt.MapFrom(src => src.AccountType.AccountType))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
        }
    }

    [TestFixture]
    public class AccountServiceTest
    {
        private MyAppDbContext _context;
        private IMapper _mapper;
        private AccountService _accountService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MyAppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique DB per test
                .Options;

            _context = new MyAppDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();

            _accountService = new AccountService(_context, _mapper);

            _context.DbUsers.Add(new UsersModel { UserId = 1, UserName = "John" });
            _context.DbAccountType.Add(new MasterAccountTypeModel { AccountTypeID = 1, AccountType = "Savings" });
            _context.DbAccountType.Add(new MasterAccountTypeModel { AccountTypeID = 2, AccountType = "Current" });
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateAccount_ShouldReturnSuccess_WhenValid()
        {
            var dto = new AccountCreationDTO
            {
                UserId = 1,
                AccountType = "Savings",
                OpeningBalance = 1000
            };

            var result = await _accountService.CreateAccountAsync(dto);

            Assert.That(result, Is.EqualTo("Success"));
            Assert.That(_context.DbAccount.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task CreateAccount_ShouldReturnUserNotFound_WhenInvalidUser()
        {
            var dto = new AccountCreationDTO
            {
                UserId = 999,
                AccountType = "Savings",
                OpeningBalance = 1000
            };

            var result = await _accountService.CreateAccountAsync(dto);

            Assert.That(result, Is.EqualTo("User not found."));
        }

        [Test]
        public async Task CreateAccount_ShouldReturnInvalidAccountType_WhenWrongType()
        {
            var dto = new AccountCreationDTO
            {
                UserId = 1,
                AccountType = "Gold",
                OpeningBalance = 1000
            };

            var result = await _accountService.CreateAccountAsync(dto);

            Assert.That(result, Is.EqualTo("Invalid account type."));
        }

        [Test]
        public async Task CloseAccount_ShouldReturnSuccess_WhenValid()
        {
            var account = new AccountModel
            {
                UserId = 1,
                AccountTypeId = 1,
                Balance = 500,
                IsActive = true,
                IsAccountClosed = false
            };
            _context.DbAccount.Add(account);
            _context.SaveChanges();

            var result = await _accountService.CloseAccountAsync(account.AccountNumber);

            Assert.That(result, Is.EqualTo("Account closed successfully."));
            Assert.That(account.IsAccountClosed, Is.True);
        }

        [Test]
        public async Task CloseAccount_ShouldReturnNotFound_WhenInvalidAccount()
        {
            var result = await _accountService.CloseAccountAsync(9999);

            Assert.That(result, Is.EqualTo("Account not found."));
        }

        [Test]
        public async Task CloseAccount_ShouldReturnAlreadyClosed_WhenClosed()
        {
            var account = new AccountModel
            {
                UserId = 1,
                AccountTypeId = 1,
                Balance = 500,
                IsActive = false,
                IsAccountClosed = true
            };
            _context.DbAccount.Add(account);
            _context.SaveChanges();

            var result = await _accountService.CloseAccountAsync(account.AccountNumber);

            Assert.That(result, Is.EqualTo("Account is already closed."));
        }

        [Test]
        public async Task RequestChange_ShouldReturnSuccess_WhenValid()
        {
            var account = new AccountModel
            {
                UserId = 1,
                AccountTypeId = 1,
                Balance = 500,
                IsActive = true,
                IsAccountClosed = false
            };
            _context.DbAccount.Add(account);
            _context.SaveChanges();

            var result = await _accountService.RequestAccountTypeChangeAsync(account.AccountNumber, "Current", 1);

            Assert.That(result, Is.EqualTo("Account update request submitted. Awaiting staff approval."));
        }

        [Test]
        public async Task RequestChange_ShouldReturnNotFound_WhenInvalidAccount()
        {
            var result = await _accountService.RequestAccountTypeChangeAsync(999, "Current", 1);

            Assert.That(result, Is.EqualTo("Account not found."));
        }

        [Test]
        public async Task RequestChange_ShouldReturnInvalidType_WhenWrongType()
        {
            var account = new AccountModel
            {
                UserId = 1,
                AccountTypeId = 1,
                Balance = 500,
                IsActive = true,
                IsAccountClosed = false
            };
            _context.DbAccount.Add(account);
            _context.SaveChanges();

            var result = await _accountService.RequestAccountTypeChangeAsync(account.AccountNumber, "Gold", 1);

            Assert.That(result, Is.EqualTo("Invalid account type."));
        }

        [Test]
        public async Task RequestChange_ShouldReturnClosedAccount_WhenClosed()
        {
            var account = new AccountModel
            {
                UserId = 1,
                AccountTypeId = 1,
                Balance = 500,
                IsActive = false,
                IsAccountClosed = true
            };
            _context.DbAccount.Add(account);
            _context.SaveChanges();

            var result = await _accountService.RequestAccountTypeChangeAsync(account.AccountNumber, "Current", 1);

            Assert.That(result, Is.EqualTo("Cannot update a closed account."));
        }

        [Test]
        public async Task GetAllAccounts_ShouldReturnAccounts_WhenExist()
        {
            _context.DbAccount.Add(new AccountModel
            {
                UserId = 1,
                AccountTypeId = 1,
                Balance = 1000,
                IsActive = true,
                IsAccountClosed = false
            });
            _context.SaveChanges();

            var accounts = await _accountService.GetAllAccountsAsync();

            Assert.That(accounts.Count, Is.EqualTo(1));
            Assert.That(accounts.First().UserId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAccountsByUserId_ShouldReturnAccounts_WhenExist()
        {
            _context.DbAccount.Add(new AccountModel
            {
                UserId = 1,
                AccountTypeId = 1,
                Balance = 2000,
                IsActive = true,
                IsAccountClosed = false
            });
            _context.SaveChanges();

            var accounts = await _accountService.GetAccountsByUserIdAsync(1);

            Assert.That(accounts.Count, Is.EqualTo(1));
            Assert.That(accounts.First().Balance, Is.EqualTo(2000));
        }

        [Test]
        public async Task GetAccountsByUserId_ShouldReturnEmpty_WhenNoneExist()
        {
            var accounts = await _accountService.GetAccountsByUserIdAsync(999);

            Assert.That(accounts.Count, Is.EqualTo(0));
        }
    }
}
