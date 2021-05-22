using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MoreLinq.Extensions;
using Obscured.DynDNS.Provider;
using Quartz;
using Serilog;

namespace Obscured.DynDNS.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class ZoneJob : laget.Quartz.Job, laget.Quartz.IJob
    {
        private readonly IEnumerable<Client.Options> _options;
        private readonly IEnumerable<Resolver> _resolvers;

        // We need this empty constructor as it's used when scheduling the job
        public ZoneJob()
        {
        }

        public ZoneJob(IEnumerable<Client.Options> options, IEnumerable<Resolver> resolvers)
        {
            _options = options;
            _resolvers = resolvers;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Log.Information($"Executing '{nameof(ZoneJob)}' (Reason='Trigger fired at {context.FireTimeUtc.LocalDateTime}', Id='{context.FireInstanceId}')");

            var result = _resolvers
                .Select(x => x.Resolve())
                .GroupBy(x => x.ToString())
                .OrderByDescending(x => x.Key.ToString())
                .First()
                .Key;

            Parallel.ForEach(_options, async (option) =>
            {
                Log.Information(option.ZoneName);
            });

            Log.Information($"The next occurrence of the '{nameof(ZoneJob)}' schedule (Constant='{Interval}') will be='{context.NextFireTimeUtc?.DateTime.ToLocalTime().ToString(CultureInfo.CurrentCulture) ?? string.Empty}'");
        }

        public ITrigger Trigger => TriggerBuilder
            .Create()
            .WithIdentity($"{typeof(ZoneJob).FullName}-Trigger")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithInterval(Interval)
                .WithMisfireHandlingInstructionIgnoreMisfires()
                .RepeatForever()
            )
            .Build();
    }
}
