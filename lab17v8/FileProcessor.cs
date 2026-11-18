using System;
using System.IO;

namespace lab17v8
{
    public class FileProcessor
    {
        private int _attempts = 0;

        public void SaveUserData(string path, string userData)
        {
            _attempts++;
            if (_attempts <= 4)
            {
                throw new IOException($"Файл '{path}' тимчасово зайнятий іншим процесом (спроба {_attempts}).");
            }

            Console.WriteLine($"[FileProcessor] Дані успішно збережено у файл '{path}': {userData}");
        }
    }
}
