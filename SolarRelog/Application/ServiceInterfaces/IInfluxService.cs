using SolarRelog.Domain.Entities;

namespace SolarRelog.Application.ServiceInterfaces;

public interface IInfluxService
{
    Task Send(LogDataEntity entity);
}