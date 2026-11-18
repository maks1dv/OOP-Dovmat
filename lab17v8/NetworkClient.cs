using System;
using System.Net.Http;

namespace lab17v8
{
    public class NetworkClient
    {
        private int _attempts = 0;

        public bool PostUserData(string url, string userData)
        {
            _attempts++;
            if (_attempts <= 2)
            {
                throw new HttpRequestException($"Сервер {url} тимчасово недоступний: 503 Service Unavailable (спроба {_attempts}).");
            }

            Console.WriteLine($"[NetworkClient] Дані успішно надіслано на '{url}': {userData}");
            return true;
        }
    }
}
