using Newtonsoft.Json;
using VZInfoBrowser.ApplicationCore;
using VZInfoBrowser.ApplicationCore.Model;

namespace VZInfoBrowser.Infrastructure
{

    public class CurrentInfoRepository : ICurrentInfoRepository, ICurrentInfoProvider
    {
        public CurrentInfoRepository() : this("vzinfobrowser.json")
        {
        }

        public CurrentInfoRepository(string fileName)
        {
            filePath = GetLocalFilePath(fileName);
        }

        private readonly string filePath;

        private CurrencyRatesInfo? data;
        public CurrencyRatesInfo? CurrentInfo
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

        public CurrencyRatesInfo? LoadSettings() =>
            File.Exists(filePath) ?
            JsonConvert.DeserializeObject<CurrencyRatesInfo>(File.ReadAllText(filePath)) :
            null;

        public bool Save(CurrencyRatesInfo info)
        {
            CurrentInfo = info;
            try
            {
                // TODO проверка что записываемое по времени > имеющегося 
                string json = JsonConvert.SerializeObject(info,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });
                File.WriteAllText(filePath, json);

            }
            catch (Exception ex)
            {
                // TODO make a logger
                return false;
            }
            return true;
        }

        private static string GetLocalFilePath(string fileName)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, fileName);
        }
    }
}
