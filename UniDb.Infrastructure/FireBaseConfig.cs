using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;

namespace UniDb.Infrastructure;
public static class FirebaseConfig
{
    private static FirebaseAuthClient _auth;

    static FirebaseConfig()
    {
        
        var config = new FirebaseAuthConfig
        {
            ApiKey = "AIzaSyC2-WqjJYWvw621T022i6eD4tQrx5V0QSk",
            AuthDomain = "admin-98661.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]
            {
                new GoogleProvider().AddScopes("email"),
                new EmailProvider()
            },
            UserRepository = new FileUserRepository("FirebaseSample")
        };
        _auth = new FirebaseAuthClient(config);
    }

    public static FirebaseAuthClient GetAuthClient()
    {
        return _auth;
    }
}  
