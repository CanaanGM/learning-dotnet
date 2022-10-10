// See https://aka.ms/new-console-template for more information
using Prometheus;

Console.WriteLine("Hello, World!");



using var server = new Prometheus.KestrelMetricServer(port: 9809);
server.Start();


var recordsProcessed = Metrics.CreateCounter("sample_records_processed_total", "Total number of records processed.");
var _somethingGauge = Metrics.CreateGauge("RedGauge", "a helpful message", new GaugeConfiguration { LabelNames = new[] { "ex", "event" } });
var _otherthingGauge = Metrics.CreateGauge("PinkGauge", "a helpful message", new GaugeConfiguration { LabelNames = new[] { "ex", "event" } });

var ex = new List<string> { "red", "sanguine", "crimzon", "scarlet" };
var evnt = new List<string> { "pink", "sakura", "cherry", "rose" };
_ = Task.Run(async delegate
{
    var i = 0;
    while (true)
    {
        // Pretend to process a record approximately every second, just for changing sample data.
        recordsProcessed.Inc();
        _somethingGauge.WithLabels(ex[i], evnt[i]).Set(10.0 * 0.10);
        _otherthingGauge.WithLabels(ex[4 - i], evnt[4 - i]).Set(10.0 * 0.9);
        await Task.Delay(TimeSpan.FromSeconds(1));
        if (i == 4) i = 0;
        else if (i < 4) i++;
    }
});


Console.WriteLine("Open http://localhost:9809/metrics in a web browser.");
Console.WriteLine("Press enter to exit.");
Console.ReadLine();