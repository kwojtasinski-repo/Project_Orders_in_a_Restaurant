﻿using Castle.Windsor;
using Restaurant.UI;
using System;
using System.Configuration;
using System.Linq;

namespace Restaurant.IntegrationTests.Common
{
    internal class TestApplicationFactory
    {
        public static string DB_FILE_NAME => GetDatabaseFileName();

        public IWindsorContainer StartApplication()
        {
            var container = SetupApplication.Create();
            DataSeed.AddData(container);
            return container;
        }

        private static string GetDatabaseFileName()
        {
            var connectionSplited = ConfigurationManager.ConnectionStrings["RestaurantDb"].ToString()
               .Split(';').AsEnumerable();
            var dataSource = connectionSplited.Where(s => s.Contains("Data Source=")).FirstOrDefault();

            if (dataSource is null)
            {
                throw new InvalidOperationException("Invalid string, there is no 'Data Source='");
            }
            
            return dataSource.Split('=')[1];
        }
    }
}
