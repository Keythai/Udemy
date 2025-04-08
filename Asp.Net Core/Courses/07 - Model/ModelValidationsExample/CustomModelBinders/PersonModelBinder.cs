using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelValidationsExample.Models;

namespace ModelValidationsExample.CustomModelBinders
{
    // Bind first name and last name to PersonName
    public class PersonModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Person person = new Person
            {
                Email = bindingContext.ValueProvider.GetValue("Email").FirstValue ?? null,
                Phone = bindingContext.ValueProvider.GetValue("Phone").FirstValue ?? null,
                Password = bindingContext.ValueProvider.GetValue("Password").FirstValue ?? null,
                ConfirmPassword = bindingContext.ValueProvider.GetValue("ConfirmPassword").FirstValue ?? null,
                Price = double.TryParse(bindingContext.ValueProvider.GetValue("Price").FirstValue, out double price) ? price : 0,
                DateOfBirth = System.DateTime.TryParse(bindingContext.ValueProvider.GetValue("DateOfBirth").FirstValue, out DateTime dateOfBirth) ? dateOfBirth : default,
                FromDate = System.DateTime.TryParse(bindingContext.ValueProvider.GetValue("FromDate").FirstValue, out DateTime fromDate) ? fromDate : default,
                ToDate = System.DateTime.TryParse(bindingContext.ValueProvider.GetValue("ToDate").FirstValue, out DateTime toDate) ? toDate : default,
                Age = int.TryParse(bindingContext.ValueProvider.GetValue("Age").FirstValue, out int age) ? age : 0,
                Tags = bindingContext.ValueProvider.GetValue("Tags").ToArray().ToList() ?? new List<string>(),
            };

            // FirstName and LastName
            if (bindingContext.ValueProvider.GetValue("FirstName").Length > 0)
            {
                string firstName = bindingContext.ValueProvider.GetValue("FirstName").FirstValue;
                if (bindingContext.ValueProvider.GetValue("LastName").Length > 0)
                {
                    string lastName = bindingContext.ValueProvider.GetValue("LastName").FirstValue;
                    person.PersonName = $"{firstName} {lastName}";
                }
            }

            bindingContext.Result = ModelBindingResult.Success(person);
            return Task.CompletedTask;
        }
    }
}
