﻿using Hangfire.Dashboard;

namespace ServerMonitor.Startup;

public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext dashboardContext)
    {
        return true;
    }
}