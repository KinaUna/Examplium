# ServiceResponse

Actions sometimes fail, this class will be used to wrap domain model objects to provide more information about the result of operatons performed.

<br/>

### Add Services folder

If it doesn't exist, add a new folder to Examplium.Shared/Models/ and name it "Services".

<br/>

### Add ServiceResponse class

In the Services folder add a new class with the name "ServiceReponse.cs" and update the content like this:

```
namespace Examplium.Shared.Models.Services
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}

```
