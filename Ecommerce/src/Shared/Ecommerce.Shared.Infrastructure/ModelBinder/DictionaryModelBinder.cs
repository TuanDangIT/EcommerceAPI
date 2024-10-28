using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.ModelBinder
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Threading.Tasks;

    public class DictionaryModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(Dictionary<string, string>))
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            var result = new Dictionary<string, string>();

            foreach (var kvp in bindingContext.ActionContext.HttpContext.Request.Query)
            {
                var modelStateList = bindingContext.ModelState.Select(ms => ms.Key);
                if (modelStateList.Contains(kvp.Key))
                    continue;

                result[kvp.Key] = kvp.Value.ToString();
            }

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
