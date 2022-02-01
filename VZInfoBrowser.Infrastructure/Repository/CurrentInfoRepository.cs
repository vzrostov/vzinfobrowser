using Newtonsoft.Json;
using VZInfoBrowser.ApplicationCore;
using VZInfoBrowser.ApplicationCore.Model;

namespace VZInfoBrowser.Infrastructure
{

    public class CurrentInfoRepository : ICurrentInfoRepository
    {
        public CurrentInfoRepository() : this("vzinfobrowser.json")
        {
        }

        public CurrentInfoRepository(string fileName)
        {
            filePath = GetLocalFilePath(fileName);
        }

        private readonly string filePath;

        public CurrencyRatesInfo? Load() =>
            File.Exists(filePath) ?
            JsonConvert.DeserializeObject<CurrencyRatesInfo>(File.ReadAllText(filePath)) :
            null;

        public bool Save(CurrencyRatesInfo? info)
        {
            if (info == null)
                return false;
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
