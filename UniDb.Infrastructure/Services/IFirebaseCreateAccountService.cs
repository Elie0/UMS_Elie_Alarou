using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using UniDb.Persistence;
using UniDb.Persistence.Models;

namespace UniDb.Infrastructure.Services;

public interface IFirebaseCreateAccountService
{ 
    Task<string> RegisterAsync(string email, string password,string role,string name);
}

public class CreateAccountService : IFirebaseCreateAccountService
{
    private readonly UniversityDbContext _dbContext;
    private static FirebaseApp _app;


    public CreateAccountService(UniversityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> RegisterAsync(string email, string password, string role, string name)
    {
        try
        {
            if (_app == null)
            {
                _app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(
                        @"C:\Users\eliea\RiderProjects\firebaseTest\firebaseTest\admin-98661-firebase-adminsdk-ofiqp-49effdd1f3.json")
                });
            }


            var auth = FirebaseConfig.GetAuthClient();
            var user = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            var claims = new Dictionary<string, object>
            {
                { "role", role }
                //  { "tenant", tenant }
            };

            await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(user.User.Uid, claims);
            Role? existingRole = null;
            while (existingRole == null)
            {
                var roleId = new Random().Next(1000000, 9999999);
                existingRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                if (existingRole != null) continue;
                existingRole = new Role()
                {
                    Name = role,
                    Id = roleId
                };
                _dbContext.Roles.Add(existingRole);
                await _dbContext.SaveChangesAsync();
                break;
            }  
            
            var newUser = new User
            {
                Name = name,
                Email = email,
                RoleId = existingRole.Id,
                FirebaseId = user.User.Uid
            };
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            return "Succesfully registered!";
        }
        catch (Firebase.Auth.FirebaseAuthException e)
        {
            return e.Message;
        }
    }
    
}