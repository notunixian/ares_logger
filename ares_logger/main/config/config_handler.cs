using System;
using System.IO;
using Newtonsoft.Json;

namespace ares_logger.main.config
{
    // thx to _1234/hacker for this config system, 1000x better than what i had before
    public class config_handler<T> where T : class
    {
        private string FilePath { get; }
        public T Config { get; private set; }
        public event Action PreOnConfigUpdate;
        public event Action OnConfigUpdate;

        public config_handler(string Path)
        {
            FilePath = Path;

            var Watch = System.IO.Path.GetDirectoryName(FilePath);

            if (Watch != null)
            {
                var watcher = new FileSystemWatcher(Watch, System.IO.Path.GetFileName(FilePath))
                {
                    NotifyFilter = NotifyFilters.LastWrite,
                    EnableRaisingEvents = true
                };
                watcher.Changed += UpdateConfig;
            }
            CheckConfig();

            Config = JsonConvert.DeserializeObject<T>(File.ReadAllText(FilePath));
        }

        private void CheckConfig()
        {
            if (!File.Exists(FilePath) || new System.IO.FileInfo(FilePath).Length < 2)
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(Activator.CreateInstance(typeof(T)), Formatting.Indented, new JsonSerializerSettings()));
        }

        private void UpdateConfig(object obj, FileSystemEventArgs args)
        {
            try
            {
                var UpdatedObject = JsonConvert.DeserializeObject<T>(File.ReadAllText(FilePath));

                if (UpdatedObject != null)
                    foreach (var Prop in UpdatedObject.GetType()?.GetProperties())
                    {
                        var Original = Config.GetType().GetProperty(Prop?.Name);

                        if (Original != null
                            && Prop.GetValue(UpdatedObject) != Original.GetValue(Config))
                        {
                            PreOnConfigUpdate?.Invoke();
                            Config = UpdatedObject;

                            OnConfigUpdate?.Invoke();
                            break;
                        }
                    }
            }
            catch
            { // Throw Error
            }
        }

        public void Save() =>
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(Config, Formatting.Indented, new JsonSerializerSettings()));
    }
}
