﻿using SolarRelog.Domain.Entities;

namespace SolarRelog.Domain.Dto;

public record SettingsModel(AppLogSettings LogSettings, DataLogSettings DataLogSettings, InfluxSettings InfluxSettings);