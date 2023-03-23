using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace UniDb.Domain.Services;

public interface IFirebaseCreateAccountService
{ 
    Task<string> RegisterAsync(string email, string password,string role);
}

public class CreateAccountService : IFirebaseCreateAccountService
{
    public async Task<string> RegisterAsync(string email, string password,string role)
    {
        try
        {
            FirebaseApp app = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(@"C:\Users\eliea\RiderProjects\firebaseTest\firebaseTest\admin-98661-firebase-adminsdk-ofiqp-49effdd1f3.json")
            });

            
            var auth = FirebaseConfig.GetAuthClient();
            var user = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            var claims = new Dictionary<string, object> { { "role", role } };
            await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(user.User.Uid, claims);
            return "Account Succesfully Created";
        }
        catch (Firebase.Auth.FirebaseAuthException e)
        {
            return e.Message;
        }
    }

   
}
