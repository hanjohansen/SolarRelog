using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using SolarRelog.Application.ServiceInterfaces;
using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.Services;

public class InfluxService : IInfluxService
{
    private readonly SettingsService _settings;

    public InfluxService(SettingsService settings)
    {
        _settings = settings;
    }

    private bool InfluxIsConfigured(InfluxSettings settings)
    {
        return !string.IsNullOrEmpty(settings.Url) 
               && !string.IsNullOrEmpty(settings.Organization) 
               && !string.IsNullOrEmpty(settings.Bucket) 
               && !string.IsNullOrEmpty(settings.ApiToken);
    }

    public async Task Send(LogDataEntity entity)
    {
        var settings = await _settings.GetSettings();
        var influxSettings = settings.InfluxSettings;
        
        if (!InfluxIsConfigured(influxSettings))
            return;
        
        var points = new List<PointData>
        {
            PointData.Measurement("solarlogbase")
                .Tag("Device", entity.Device.Name)
                .Field("Pdc", Convert.ToDouble(entity.Pdc))
                .Field("Pac", Convert.ToDouble(entity.Pac))
                .Field("ConsPac", Convert.ToDouble(entity.ConsPac))
                .Timestamp(entity.LoggedDate, WritePrecision.S)
        };

        foreach (var consumer in entity.ConsumerData)
            points.Add(PointData.Measurement("consumer")
                .Tag("Consumer", consumer.ConsumerIndex)
                .Field("Consumption", Convert.ToDouble(consumer.Consumption))
                .Timestamp(entity.LoggedDate, WritePrecision.S));
        
        using var client = new InfluxDBClient(influxSettings.Url, influxSettings.ApiToken);
        var writeApi = client.GetWriteApiAsync();

        if (points.Any())
            await writeApi.WritePointsAsync(points, influxSettings.Bucket, influxSettings.Organization);
    }
}