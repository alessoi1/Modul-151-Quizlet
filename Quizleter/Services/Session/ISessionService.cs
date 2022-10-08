namespace Quizleter.Services.Session
{
    public interface ISessionService
    {
        TResult GetValue<TResult>(string key);
        bool KeyExists(string key);
        bool StoreValue<TValue>(string key, TValue value);
        void ClearSession();
    }
}