//using Microsoft.AspNetCore.Identity;
//using ProjectBackend.Models;

//namespace ProjectBackend.ExtraTools;

//public class UserEmailStoreHelper
//{
//    private readonly UserManager<ApplicationUser> _userManager;
//    private readonly IUserStore<ApplicationUser> _userStore;

//    public UserEmailStoreHelper(
//        UserManager<ApplicationUser> userManager,
//        IUserStore<ApplicationUser> userStore)
//    {
//        _userManager = userManager;
//        _userStore = userStore;
//    }

//    public IUserEmailStore<ApplicationUser> GetEmailStore()
//    {
//        if (!_userManager.SupportsUserEmail)
//        {
//            throw new NotSupportedException(
//                "The default Identity implementation requires a user store with email support.");
//        }

//        return (IUserEmailStore<ApplicationUser>)_userStore;
//    }
//}
