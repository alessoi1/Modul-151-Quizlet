using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text.Json;

namespace Quizleter.Services.Session.Impl
{
    public class SessionService : ISessionService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public TResult GetValue<TResult>(string key)
        {
            _httpContextAccessor.HttpContext.Session.TryGetValue(key, out byte[] byteResult);
            var result = JsonSerializer.Deserialize<TResult>(byteResult);
            return result;
        }

        public bool StoreValue<TValue>(string key, TValue value)
        {
            try
            {
                var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
                _httpContextAccessor.HttpContext.Session.Set(key, bytes);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool KeyExists(string key)
        {
            return _httpContextAccessor.HttpContext.Session.Keys.Contains(key);
        }

        public void ClearSession()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

    }
}
