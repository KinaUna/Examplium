# First steps

This document describes what you need to get up and running with the code in this repository.

At this point, the code only has configuration settings for debugging.

<br/>

### Prerequisites:
- Visual Studio 2022 17.4.3
  - Visual Studio Workloads/Components installed:
    - ASP.NET and web development
    - .NET Multi-platform App UI development
    - Azure Development
    - Data storage and processing
- Entity Framework Core - version 7.0.0


Todo: Instructions for installing Visual Studio, Entity Framework Core.

<br/>

## Clone or download the code.

Todo: Instructions for cloning code.

<br/>

## Update Constants

If you want to change the name of the organization and/or app: 

- Update values in Examplium.Shared/Constants/ExampliumCoreConstants.cs

<br/>

Verify that Examplium.IdentityServer and Examplium.Server addresses are correct:

- Look in Examplium.IdentityServer/Properties/launchSettings.json
- Look in Examplium.Server/Properties/launchSettings.json
- Make sure that the values in Examplium.Shared/Constants/ExampliumAuthServerConstants.cs are the same.
- If you changed the name of the app or organization, you should change the CoreApiName too.

<br/>

## Configure Examplium.IdentityServer

Right click on the Examplium.IdentityServer project, select "Manage User Secrets...".

For more information about configuration and secrets in ASP.NET Core see https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0

Copy the contents of Examplium.IdentityServer/SecretsTemplate.txt to the secrets.json file.

Update the values for 
- CoreApiSecret (pick your own code, any strong password should do)
- EmailServer (the server for your email service, i.e. for GMail see https://developers.google.com/gmail/imap/imap-smtp)
- EmailPort (The port number to use for sending emails, usually 465 or 587)
- EmailUserName (Your email account's user name, usually your email address)
- EmailPassword (Your email account's password)
- EmailFrom (The email address you want the recepient to see in the "From" field)

<br/>

## Apply database migrations

Delete all the in the the Migrations folders in the Examplium.IdentityServer and Examplium.Server.

In the Package Manager Console run these commands:

```
add-migration InitialIdentityServerApplicationDbMigration -Project Examplium.IdentityServer -Context ApplicationDbContext
```
```
update-database -Project Examplium.IdentityServer -Context ApplicationDbContext
```

```
add-migration InitialIdentityServerPersistedGrantDbMigration -Project Examplium.IdentityServer -Context PersistedGrantDbContext
```

```
update-database -Project Examplium.IdentityServer -Context PersistedGrantDbContext
```

```
add-migration InitialIdentityServerConfigurationDbMigration -Project Examplium.IdentityServer -Context ConfigurationDbContext
```

```
update-database -Project Examplium.IdentityServer -Context ConfigurationDbContext
```

```
add-migration InitialServerMigration -Project Examplium.Server -Context ApplicationDbContext
```

```
update-database -Project Examplium.Server -Context ApplicationDbContext
```

<br/>

## Update "Startup Project" settings

In the solution explorer right click on "Solution 'Examplium'", then select "Select Startup Projects...".

Select the option "Multiple startup projects".

Move Examplium.IdentityServer to the top, so it starts first, and set the action to "Start".

Set Examplium.Server's action to "Start" as well.

<br/>
