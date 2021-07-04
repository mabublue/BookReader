using System.Runtime.CompilerServices;

namespace Domain.Services.Telemetry
{
    public static class MetricNames
    {
        #region Namespaces

        public static string LibraryNamespace(string environmentName, MetricNameSpace @namespace) => $"BookReader-{environmentName}-{@namespace}";

        public const string DeprecatedFunctionalityUsage = "DeprecatedFunctionalityUsage";

        #endregion

        #region Events

        public static string DeprecatedControllersUsed(string controller, [CallerMemberName] string method = "") => $"DeprecatedControllersUsed-{controller}-{method}";
        public static string DeprecatedWorkerProcessJob(string jobName) => $"DeprecatedWorkerProcessJob-{jobName}";

        #endregion
    }
}
