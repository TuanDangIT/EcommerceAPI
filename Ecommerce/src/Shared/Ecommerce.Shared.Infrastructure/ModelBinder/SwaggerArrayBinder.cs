using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.ModelBinder
{
    public class SwaggerArrayBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var values = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (values.Length == 0)
                return Task.CompletedTask;
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            if (values.FirstValue is null)
                return Task.CompletedTask;
            var deserialized = JsonSerializer.Deserialize(values.FirstValue, bindingContext.ModelType, options);

            bindingContext.Result = ModelBindingResult.Success(deserialized);
            return Task.CompletedTask;
        }
    }
}
