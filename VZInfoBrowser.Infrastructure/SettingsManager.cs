using Newtonsoft.Json;

namespace VZInfoBrowser.Infrastructure
{
    public interface ISettings<T> where T : class
    {
        T LoadSettings();
        void SaveSettings(T _data);
        T Data { get; }
    };

    public class SettingsManager<T> : ISettings<T> where T : class
    {
        public SettingsManager() : this("vzinfobrowser.json")
        {
        }

        public SettingsManager(string fileName)
        {
            filePath = GetLocalFilePath(fileName);
        }

        private readonly string filePath;

        private T data;
        public T Data
        {
            get
            {
                if (data == null)
                {
                    data = LoadSettings();
                    return data;
                }
                return data;
            }
            set { data = value; }
        }

        private string GetLocalFilePath(string fileName)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, fileName);
        }

        public T? LoadSettings() =>
            File.Exists(filePath) ?
            JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath)) :
            null;

        public void SaveSettings(T _data)
        {
            Data = _data;
            // TODO проверка что записываемое по времени > имеющегося 
            string json = JsonConvert.SerializeObject(_data,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
            File.WriteAllText(filePath, json);
        }
    }
}
