using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskScheduler.Infrastructure;

public class CsvIntArrayBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext ctx)
    {
        var raw = ctx.ValueProvider.GetValue(ctx.ModelName).FirstValue;
        if (string.IsNullOrEmpty(raw))
        {
            ctx.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        try
        {
            var arr = raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(s => int.Parse(s.Trim()))
                         .ToArray();
            ctx.Result = ModelBindingResult.Success(arr);
        }
        catch (FormatException)
        {
            ctx.ModelState.AddModelError(ctx.ModelName, "Ожидается список целых чисел через запятую");
            ctx.Result = ModelBindingResult.Failed();
        }

        return Task.CompletedTask;
    }
}
