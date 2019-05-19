#r "Newtonsoft.Json"
#r "Microsoft.ApplicationInsights"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    string requestHeaders = JsonConvert.SerializeObject(req.Headers);

    Dictionary<string,string> properties = new Dictionary<string,string>();
    properties["body"] = requestBody;
    properties["headers"] = requestHeaders;

    string telemetryKey = TelemetryConfiguration.Active.InstrumentationKey = System.Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);
    TelemetryClient telemetry = new TelemetryClient() { InstrumentationKey = telemetryKey };
    telemetry.TrackEvent("Csp", properties);

    return new OkResult();
}
