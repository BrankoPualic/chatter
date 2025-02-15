using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Chatter.Web.Api.Binders;

public class JsonModelBinder : IModelBinder
{
	private static JsonSerializerSettings DeserializationSettings { get; } = new()
	{
		ContractResolver = new ApplicationContractResolver(),
		ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
		DefaultValueHandling = DefaultValueHandling.Ignore,
		NullValueHandling = NullValueHandling.Ignore,
	};

	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		Argument.AssertNotNull(bindingContext);

		// Check the value sent in
		var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
		if (valueProviderResult != ValueProviderResult.None)
		{
			bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

			// Attempt to convert the input value
			var valueAsString = valueProviderResult.FirstValue;

			var result = JsonConvert.DeserializeObject(valueAsString, bindingContext.ModelType, DeserializationSettings);
			if (result != null)
			{
				bindingContext.Result = ModelBindingResult.Success(result);
				return Task.CompletedTask;
			}
		}

		return Task.CompletedTask;
	}
}