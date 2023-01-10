# Identity Server setup and configuration

Note: Duende Identity Server doesn't need a license for development, debugging and testing.
If you intend to use it for commercial solutions have a look at the Community Edition license here: [Community Edition](https://duendesoftware.com/products/communityedition) 
<br/>

### [01 Initial Setup](/Documentation/Code/IdentityServer/01-Initial-Setup.md)
- Add new project
- Install NuGet packages
- Constants
- Data folder
- Connection string and secret code
- Update Program.cs
- Initial database migrations
- Add pages
- Next steps

<br/>

### [02 Add Register Pages](/Documentation/Code/IdentityServer/02-Add-Register-Pages.md)
- Create Register folder and files
- Add link to Login page

<br/>

### [03 Add Confirmation Email](/Documentation/Code/IdentityServer/03-Add-Confirmation-Email.md)
- Create folder
- Add interface
- Add class
- Update User Secrets
- Register dependency injection
- Update Pages

<br/>

### [04 Add Password Reset](/Documentation/Code/IdentityServer/04-Add-Password-Reset.md)
- Update EmailSender
- Add new folder
- Add Index page
- Add reset code sent page
- Add reset password page
- Add password set page
- Update login page

<br/>

### [05 Add Change Password](/Documentation/Code/IdentityServer/05-Add-Change-Password.md)
- Update EmailSender
- Add new folder
- Add Index page
- Add change code sent page
- Add reset password page
- Add password set page
- Add link to \_Layout

<br/>

### [06 Add Delete Account Feature](/Documentation/Code/IdentityServer/06-Add-Delete-Account-Feature.md)
- Update EmailSender
- Add new folder
- Add Index page
- Add delete account email sent page
- Add confirm delete account page
- Add account deleted page
- Add link to delete account

<br/>

### [07 Update Blazor.Server](/Documentation/Code/IdentityServer/07-Update-Blazor-Server.md)
- Add NuGet packages
- Update Program.cs
- Add secret to User Secrets
- Update start up settings
- Remove obsolete code

<br/>

### [08 Update Blazor.Client](/Documentation/Code/IdentityServer/08-Update-Blazor-Client.md)
- Update App.razor
- Update MainLayout.razor
- Add Authentication State Provider
- Add Anti-Forgery handler
- Update Services
- Remove obsolete code

<br/>

## References
Official Duende Identity Server documentation: [Duende Identity Server - Overview](https://docs.duendesoftware.com/identityserver/v6/overview/)
