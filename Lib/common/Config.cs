using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Lib.common
{
    internal class Config
    {
        public string Language { get; set; } = "en";

        public static Config Load()
        {
            try
            {
                string path = Path.Combine(AppContext.BaseDirectory, "config.json");
                if (!File.Exists(path))
                {
                    return new Config();
                }

                string json = File.ReadAllText(path);
                var cfg = JsonSerializer.Deserialize<Config>(json);
                return cfg ?? new Config();
            }
            catch
            {
                return new Config();
            }
        }
    }
}
