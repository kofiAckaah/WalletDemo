using Shared.Requests;
using Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IWalletService
    {
        /// <summary>
        /// Method to get a wallet of a user by an admin.
        /// </summary>
        /// <param name="walletName">Name of wallet</param>
        /// <param name="userName">The email of the user</param>
        /// <returns>A wallet as a <see cref="ResponseWrapper{T}"/></returns>
        Task<ResponseWrapper<WalletResponse>> AdminGetSingleWallet(string walletName, string userName);

        /// <summary>
        /// Method to create wallet.
        /// </summary>
        /// <param name="request">The wallet request with the information to create the wallet.</param>
        /// <param name="userName">The email of the user.</param>
        /// <returns>A <see cref="ResponseWrapper{T}"/> of type <seealso cref="WalletRequest"/>.</returns>
        Task<ResponseWrapper<WalletRequest>> CreateWallet(WalletRequest request, string userName);

        /// <summary>
        /// Method to delete wallet.
        /// </summary>
        /// <param name="walletName">The unique name of the wallet.</param>
        /// <param name="userName">The email of the user.</param>
        /// <returns>A <see cref="ResponseWrapper{T}"/> message.</returns>
        Task<ResponseWrapper<string>> DeleteWallet(string walletName, string userName);

        /// <summary>
        /// Method to get all wallets by admin.
        /// </summary>
        /// <param name="userName">The email of the admin user.</param>
        /// <returns>A <see cref="List{T}"/> of wallets as <seealso cref="WalletResponse"/>.</returns>
        Task<ResponseWrapper<List<WalletResponse>>> GetAllUserWallets(string userName);

        /// <summary>
        /// Method to get all wallets of a user.
        /// </summary>
        /// <param name="userName">The email of the user.</param>
        /// <returns>A <see cref="List{T}"/> of wallets as <seealso cref="WalletResponse"/>.</returns>
        Task<ResponseWrapper<List<WalletResponse>>> GetAllWallets(string userName);

        /// <summary>
        /// Method to get a wallet of a user.
        /// </summary>
        /// <param name="walletName">Name of wallet.</param>
        /// <param name="userName">The email of the user.</param>
        /// <returns>A wallet as a <see cref="ResponseWrapper{T}"/>.</returns>
        Task<ResponseWrapper<WalletResponse>> GetSingleWallet(string walletName, string userName);
    }
}
