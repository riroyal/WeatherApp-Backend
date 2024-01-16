using System.Reflection;

namespace Weather.API.Test
{
    public static class Utils
    {
        public static string GetJsonFromTestDataFile()
        {
            string result;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = Service.Common.Constants.TestDataResource;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
