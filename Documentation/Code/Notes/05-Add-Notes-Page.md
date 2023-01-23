# Add Notes Page

<br/>

### Add NoteDetails component
If it doesn't exist, create a new folder in Examplium.Client/Shared/, name it "Components".

In this folder add a new folder again and name this "Notes".

Add a new Razor component with the name "NoteDetails.razor" and replace the content of the file with this code:
```
@using Examplium.Shared.Models.Domain
@using Examplium.Client.Services.Notes
@inject INotesService NotesService
@implements IDisposable

@if (_editNote && NoteItem != null)
{
    <EditForm Model="NoteItem" OnSubmit="SaveNote">
        <div class="card card bg-notes rounded">
            <div class="card-header bg-success">
                <label class="text-white-50" for="titleInput">Title</label>
                <InputText id="titleInput" class="form-control" @bind-Value="NoteItem.Title" DisplayName="Title"></InputText>
            </div>
            <div class="card-body">
                <label class="text-white-50" for="contentInput">Content</label>
                <InputTextArea id="contentInput" class="form-control" @bind-Value="NoteItem.Content" DisplayName="Content"></InputTextArea>
            </div>
            <div class="card-footer">
                <div class="float-start">
                    <div>
                        <label class="text-white-50" for="categoryInput">Category</label>
                        <InputText id="categoryInput" class="form-control" @bind-Value="NoteItem.Category" DisplayName="Category"></InputText>
                    </div>
                    <div>
                        <label class="text-white-50" for="tagsInput">Tags</label>
                        <InputText id="tagsInput" class="form-control" @bind-Value="NoteItem.Tags" DisplayName="Tags"></InputText>
                    </div>
                </div>
                <div class="float-end">
                    <div class="text-white-50">
                        Created: @NoteItem?.Created
                    </div>
                    <div class="text-white-50">
                        Updated: @NoteItem?.Updated
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <div class="mt-3 w-100">
                    <div class="float-end">
                        <button class="btn btn-primary" type="submit">Save note</button>
                        <button class="btn btn-secondary" @onclick="CancelEdit">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </EditForm>
}
else
{
    <div class="card bg-notes rounded">
        <div class="card-header">
            @if (_showDeleteWarning)
            {
                <div class="float-end alert alert-warning">
                    <span class="text-danger">
                        Are you sure want to delete this note?
                    </span>
                    <button class="btn btn-success ms-2" @onclick="DeleteNote"><span class="oi oi-check me-2" aria-hidden="true"></span>Yes</button>
                    <button class="btn btn-danger ms-2" @onclick="CancelDelete"><span class="oi oi-x me-2" aria-hidden="true"></span>No</button>
                </div>
               
                
            }
            else
            {
                <div class="float-end">
                    <button class="btn btn-primary ms-2" @onclick="EditNote"><span class="oi oi-pencil" aria-hidden="true"></span></button>
                    <button class="btn btn-danger ms-2" @onclick="ConfirmDeleteNote"><span class="oi oi-trash" aria-hidden="true"></span></button>
                </div>
            }

            <div class="h3 text-white mt-2 float-start">
                <span class="oi oi-document" aria-hidden="true"></span> @NoteItem?.Title
            </div>
        </div>
        <div class="card-body bg-white m-2 rounded">
            <div class="space-20"></div>
            @NoteItem?.Content
            <div class="space-20"></div>
            
        </div>
        <div class="card-footer">
            <div class="float-start">
                <div>
                    Category: @NoteItem?.Category
                </div>
                <div>
                    Tags: @NoteItem?.Tags
                </div>
            </div>
            <div class="float-end">
                <div class="text-white-50">
                    Created: @NoteItem?.Created
                </div>
                <div class="text-white-50">
                    Updated: @NoteItem?.Updated
                </div>
            </div>
        </div>
    </div>
}

@code {
    [Parameter]
    public Note? NoteItem { get; set; }

    private bool _editNote = false;
    private bool _showDeleteWarning = false;

    protected override async Task OnInitializedAsync()
    {
        NotesService.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        NotesService.OnChange -= StateHasChanged;
    }

    private void EditNote()
    {
        _editNote = true;
    }

    private async Task SaveNote()
    {
        if (NoteItem != null)
        {
            await NotesService.UpdateNote(NoteItem);
        }

        _editNote = false;
    }

    private void ConfirmDeleteNote()
    {
        _showDeleteWarning = true;
    }

    private async Task DeleteNote()
    {
        if (NoteItem != null)
        {
            await NotesService.DeleteNote(NoteItem);
        }
    }

    private async Task CancelEdit()
    {
        if (NoteItem != null)
        {
            NoteItem = await NotesService.GetNoteById(NoteItem.Id);
        }
    }

    private void CancelDelete()
    {
        _showDeleteWarning = false;
    }
}
```

<br/>

### Add Notes folder and Index page

In the Examplium.Client/Pages folder add a new folder with the name "Notes".

In the new Notes folder add a new Razor component file and name it "Index.razor".

Update the file contents with the following code:
```
@page "/Notes"
@using Examplium.Client.Shared.Components.Notes
@using Examplium.Client.Services.Notes
@using Examplium.Shared.Models.Domain
@using Microsoft.AspNetCore.Authorization
@inject INotesService NotesService
@attribute [Authorize]
@implements IDisposable

<PageTitle>My Notes</PageTitle>
@if (!_showAddNoteForm)
{
    <button class="btn btn-primary float-end" @onclick="AddNote">Add note</button>
}
<h3>My Notes</h3>
<div class="space-50"></div>
@if (_showAddNoteForm)
{
    <EditForm Model="_editingNote" OnSubmit="SaveNewNote">
        <div>Add Note</div>
        <div class="space-20"></div>
        <div class="row">
            <label for="titleInput">Title</label>
            <InputText id="titleInput" class="form-control" @bind-Value="_editingNote.Title" DisplayName="Title"></InputText>
        </div>
        <div class="row">
            <label for="contentInput">Content</label>
            <InputTextArea id="contentInput" class="form-control" @bind-Value="_editingNote.Content" DisplayName="Content"></InputTextArea>
        </div>
        <div class="row">
            <label for="categoryInput">Category</label>
            <InputText id="categoryInput" class="form-control" @bind-Value="_editingNote.Category" DisplayName="Category"></InputText>
        </div>
        <div class="row">
            <label for="tagsInput">Tags</label>
            <InputText id="tagsInput" class="form-control" @bind-Value="_editingNote.Tags" DisplayName="Tags"></InputText>
        </div>
        <div class="mt-3 w-100">
            <div class="float-end">
                <button class="btn btn-primary" type="submit">Save note</button>
                <button class="btn btn-secondary" @onclick="CancelAdd">Cancel</button>
            </div>
        </div>
    </EditForm>
    <div class="space-50"></div>
}
@if (!_showAddNoteForm && NotesService.MyNotes.Any())
{
    foreach (Note note in NotesService.MyNotes)
    {
        <div class="space-50"></div>
        <NoteDetails NoteItem="note"></NoteDetails>
        
    }
}


@code 
{
    private bool _showAddNoteForm = false;

    private Note _editingNote = new Note();

    protected override async Task OnInitializedAsync()
    {
        await NotesService.GetMyNotes();
        NotesService.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        NotesService.OnChange -= StateHasChanged;
    }

    private void AddNote()
    {
        _editingNote = new Note();
        _showAddNoteForm = true;
    }

    private async Task SaveNewNote()
    {
        if (!string.IsNullOrEmpty(_editingNote.Title))
        {
            await NotesService.CreateNote(_editingNote);
            _showAddNoteForm = false;
        }
    }

    private void CancelAdd()
    {
        _showAddNoteForm = false;
    }
}
```

<br/>

### Add CSS classes 

Open the Examplium.Client/wwwroot/css/app.css file.

Add these classes:
```
.space-20 {
    height: 20px;
    display: block;
}

.space-50 {
    height: 50px;
    display: block;
}

.bg-notes {
    background: linear-gradient(120deg, #1c8850, #2c9950);
}
```

<br/>

### Update Navigation Menu

Open the Examplium.Client/Shared/NavMenu.razor file.

After the `<div class="nav-item px-3">... </div>` element add this:
```
<AuthorizeView>
    <Authorized>
        <div class="nav-item px-3">
            <NavLink class="nav-link" href="/Notes" Match="NavLinkMatch.All">
                <span class="oi oi-document" aria-hidden="true"></span> Notes
             </NavLink>
         </div>
    </Authorized>
</AuthorizeView>
```

<br/>
