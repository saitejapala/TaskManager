using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.CacheService
{
    public interface IRedisCacheService
    {
        public bool SetString(string key, string value, int expireinSec = 0);
        public string? GetString(string key);
        public void RemoveKey(string key);
    }
}
