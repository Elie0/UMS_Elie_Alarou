namespace UniDb.Infrastructure.Services;

public interface IFirebaseLoginServer
{
    Task<string> LoginAsync(string email, string password);
}
public class FirebaseLoginServer:IFirebaseLoginServer
{
    public async Task<string> LoginAsync(string email, string password)
    {
        try
        {
            var auth = FirebaseConfig.GetAuthClient();
            var authLink = await auth.SignInWithEmailAndPasswordAsync(email, password);
            var token = await authLink.User.GetIdTokenAsync();
            return token;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error during login: {ex.Message}");
        }
    }
}