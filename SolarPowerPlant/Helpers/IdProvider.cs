﻿using RT.Comb;

namespace SolarPowerPlant.Helpers;

public class IdProvider
{
    public static Guid NewId()
    {
        return Provider.PostgreSql.Create();
    }
}
