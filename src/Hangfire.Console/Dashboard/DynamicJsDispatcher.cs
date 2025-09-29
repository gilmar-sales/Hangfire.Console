using System;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Dashboard;

namespace Hangfire.Console.Dashboard;

/// <summary>
///     Dispatcher for configured script
/// </summary>
internal class DynamicJsDispatcher : IDashboardDispatcher
{
    private readonly ConsoleOptions _options;

    public DynamicJsDispatcher(ConsoleOptions? options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public Task Dispatch(DashboardContext context)
    {
        var builder = new StringBuilder();
        
        var consolePollUrl = Path.Combine(context.Options.PrefixPath, context.Request.PathBase, "console/");

        builder.Append("(function (hangfire) {")
            .Append("hangfire.config = hangfire.config || {};")
            .AppendFormat("hangfire.config.consolePollInterval = {0};", _options.PollInterval)
            .AppendFormat("hangfire.config.consolePollUrl = '{0}';", consolePollUrl)
            .Append("})(window.Hangfire = window.Hangfire || {});")
            .AppendLine();

        return context.Response.WriteAsync(builder.ToString());
    }
}
