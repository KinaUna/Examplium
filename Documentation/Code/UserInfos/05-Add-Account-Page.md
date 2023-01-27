# Add Notes Page

<br/>

### Add NoteDetails component
If it doesn't exist, create a new folder in Examplium.Client/Shared/, name it "Components".

In this folder add a new folder again and name this "UserInfos".

Add a new Razor component with the name "UserInfoDetails.razor" and replace the content of the file with this code:
```
@using Examplium.Client.Services.UserInfos
@using Microsoft.AspNetCore.Authorization
@attribute [Authorize]
@inject IUserInfosService UserInfosService
@implements IDisposable
@if (_editUserInfo && UserInfosService.CurrentUser != null)
{
    <EditForm Model="UserInfosService.CurrentUser" OnSubmit="UpdateUserInfo">
        <div class="card card bg-userinfo rounded">
            <div class="card-header bg-userinfo-header text-white">
                @UserInfosService.CurrentUser.Email
            </div>
            <div class="card-body">
                <label class="text-white-50" for="firstNameInput">First name</label>
                <InputText id="firstNameInput" class="form-control mb-3" @bind-Value="UserInfosService.CurrentUser.FirstName" DisplayName="First name"></InputText>
                <label class="text-white-50" for="middleNameInput">Middle name</label>
                <InputText id="middleNameInput" class="form-control mb-3" @bind-Value="UserInfosService.CurrentUser.MiddleName" DisplayName="Middle name"></InputText>
                <label class="text-white-50" for="lastNameInput">Last name</label>
                <InputText id="lastNameInput" class="form-control mb-3" @bind-Value="UserInfosService.CurrentUser.LastName" DisplayName="Last name"></InputText>
            <div class="card-footer">
                <div class="mt-3 w-100">
                    <div class="float-end">
                        <button class="btn btn-primary" type="submit">Save</button>
                        <button class="btn btn-secondary" @onclick="CancelEdit">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </EditForm>
}
else
{
    <div class="card card bg-userinfo rounded">
        <div class="card-header bg-userinfo-header">
            <div class="float-end">
                <button class="btn btn-primary ms-2" @onclick="EditUserInfo"><span class="oi oi-pencil" aria-hidden="true"></span></button>
            </div>
            <h4 class="text-white">@UserInfosService.CurrentUser?.Email</h4>
        </div>
        <div class="card-body text-white">
            <div class="row mb-3">
                <div class="col-3 col-lg-2 text-white-50">First name:</div>
                <div class="col-8"> @UserInfosService.CurrentUser?.FirstName</div>
            </div>
            <div class="row mb-3">
                <div class="col-3 col-lg-2 text-white-50">Middle name: </div>
                <div class="col-8">@UserInfosService.CurrentUser?.MiddleName</div>
            </div>
            <div class="row mb-3">
                <div class="col-3 col-lg-2 text-white-50">Last name:</div>
                <div class="col-8">@UserInfosService.CurrentUser?.LastName</div>
            </div>
        </div>
    </div>
}
@code {
    private bool _editUserInfo = false;

    protected override async Task OnInitializedAsync()
    {
        await UserInfosService.GetCurrentUserInfo();
        UserInfosService.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        UserInfosService.OnChange -= StateHasChanged;
    }

    private async Task EditUserInfo()
    {
        _editUserInfo = true;
    }

    private async Task CancelEdit()
    {
        _editUserInfo = false;
        await UserInfosService.GetCurrentUserInfo();
    }

    private async Task UpdateUserInfo()
    {
        if (UserInfosService.CurrentUser != null)
        {
            await UserInfosService.UpdateCurrentUser();
        }

        _editUserInfo = false;
    }
}

```

<br/>

### Add Account folder and Index page

In the Examplium.Client/Pages folder add a new folder with the name "Account".

In the new Notes folder add a new Razor component file and name it "Index.razor".

Update the file contents with the following code:
```
@page "/Account"
@using Examplium.Client.Shared.Components.UserInfos
@using Microsoft.AspNetCore.Authorization
@attribute [Authorize]
<PageTitle>My Account</PageTitle>

<UserInfoDetails></UserInfoDetails>
@code {

}
```

<br/>

### Add CSS classes 

Open the Examplium.Client/wwwroot/css/app.css file.

If they don't exist, add these classes:
```
.space-20 {
    height: 20px;
    display: block;
}

.space-50 {
    height: 50px;
    display: block;
}

.bg-userinfo {
    background: linear-gradient(120deg, #1c5088, #2c5099);
}

.bg-userinfo-header {
    background: linear-gradient(120deg, #1c4077, #2c4088);
}
```

<br/>

### Update Navigation Menu

Open the Examplium.Client/Shared/NavMenu.razor file.

After the `<div class="nav-item px-3">... </div>` element update or add the AuthorizeView component like this:
```
<AuthorizeView>
    <Authorized>
        <div class="nav-item px-3">
            <NavLink class="nav-link" href="/Notes" Match="NavLinkMatch.All">
                <span class="oi oi-document" aria-hidden="true"></span> Notes
            </NavLink>
        </div>
        <div class="nav-item px-3">
            <NavLink class="nav-link" href="/Account" Match="NavLinkMatch.All">
                <span class="oi oi-person" aria-hidden="true"></span> My Account
            </NavLink>
        </div>
    </Authorized>
</AuthorizeView>
```

<br/>
