using System.Text.Json;

namespace web.reunion.Interfaces
{
    public static class MyJsonOptions
    {
        /// <summary>
        /// Propriedades para a Serialização de Jsons
        /// </summary>
        /// <returns></returns>
        public static readonly JsonSerializerOptions Default = new()
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
