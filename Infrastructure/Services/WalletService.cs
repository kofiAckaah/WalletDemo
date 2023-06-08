using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Constants;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.Requests;
using Shared.Responses;

namespace Infrastructure.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public WalletService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        /// <inheritdoc/>
        public async Task<ResponseWrapper<WalletRequest>> CreateWallet(WalletRequest request, string user)
        {
            try
            {
                if (await unitOfWork.Repository<Wallet>().CountAsync() > 5)
                    return WrapResponse.Fail("Wallets limit reached.", request);

                if(!request.Number.VerifyNumber(request.AccountScheme))
                    return WrapResponse.Fail("Account number is invalid.", request);

                if (!unitOfWork.Repository<Wallet>().Entities
                        .Any(w => w.NumberHash == request.Number.HashAccountNumber()))
                    return WrapResponse.Fail("Account already exists", request);

                var dbUser = await userManager.FindByEmailAsync(user);
                if (dbUser == null)
                    return WrapResponse.Fail("Invalid operation.", request);

                if (unitOfWork.Repository<Wallet>().Entities.Any(w => w.Name == request.Name))
                    request.Name = request.Name.ToUniqueWalletName();

                var wallet = new Wallet
                {
                    Name = request.Name,
                    Number = request.Number.HideNumber(request.IsMomo),
                    NumberHash = request.Number.HashAccountNumber(),
                    IsMomo = request.IsMomo,
                    AccountScheme = request.AccountScheme,
                    DateTimeCreated = DateTime.UtcNow,
                    WalletOwner = dbUser
                };
                await unitOfWork.Repository<Wallet>().AddAsync(wallet);
                await unitOfWork.Commit(CancellationToken.None);

                return WrapResponse.Success("Wallet created successfully.", request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return WrapResponse.Fail("Error performing operation.", request);
            }
        }

        /// <inheritdoc/>
        public async Task<ResponseWrapper<List<WalletResponse>>> GetAllWallets(string userName)
        {
            try
            {
                var dbUser = await userManager.FindByEmailAsync(userName);
                if (dbUser == null)
                    return WrapResponse.Fail<List<WalletResponse>>("Invalid operation.");

                var wallets = dbUser.Wallets.ToList();
                var response = wallets.Select(wallet => new WalletResponse
                {
                    Number = wallet.Number,
                    Name = wallet.Name,
                    AccountScheme = wallet.AccountScheme.ToString()
                })
                                      .ToList();

                return WrapResponse.Success("Success", response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return WrapResponse.Fail<List<WalletResponse>>("Error performing operation");
            }
        }

        /// <inheritdoc/>
        public async Task<ResponseWrapper<WalletResponse>> GetSingleWallet(string walletName, string userName)
        {
            try
            {
                var dbUser = await userManager.FindByEmailAsync(userName);
                if (dbUser == null)
                    return WrapResponse.Fail<WalletResponse>("Invalid operation.");
                var wallet = dbUser.Wallets.SingleOrDefault(wallet => wallet.Name == walletName);
                var response = new WalletResponse
                { Name = wallet.Name, Number = wallet.Number, AccountScheme = wallet.AccountScheme.ToString() };

                return WrapResponse.Success("Success.", response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return WrapResponse.Fail<WalletResponse>("Error performing operation");
            }
        }

        /// <inheritdoc/>
        public async Task<ResponseWrapper<List<WalletResponse>>> GetAllUserWallets(string userName)
        {
            try
            {
                var dbUser = await userManager.FindByEmailAsync(userName);
                if (dbUser == null || !await userManager.IsInRoleAsync(dbUser, RoleConstants.AdminRole))
                    return WrapResponse.Fail<List<WalletResponse>>("Invalid operation.");

                var wallets = unitOfWork.Repository<Wallet>().Entities.ToList();
                var response = wallets.Select(wallet => new WalletResponse
                {
                    Number = wallet.Number,
                    Name = wallet.Name,
                    AccountScheme = wallet.AccountScheme.ToString()
                })
                    .ToList();

                return WrapResponse.Success("Success.", response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return WrapResponse.Fail<List<WalletResponse>>("Error performing operation");
            }
        }

        /// <inheritdoc/>
        public async Task<ResponseWrapper<string>> DeleteWallet(string walletName, string userName)
        {
            try
            {
                var dbUser = await userManager.FindByEmailAsync(userName);
                if (dbUser == null)
                    return WrapResponse.Fail<string>("Invalid operation.");

                var wallet = unitOfWork.Repository<Wallet>().Entities.SingleOrDefault(w => w.Name == walletName);
                if (wallet == null || wallet.WalletOwnerId != dbUser.Id)
                    return WrapResponse.Fail<string>("Error performing operation.");

                await unitOfWork.Repository<Wallet>().DeleteAsync(wallet);

                return WrapResponse.Success<string>("Wallet deleted successfully.");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return WrapResponse.Fail<string>("Error performing operation");
            }

        }
    }
}
