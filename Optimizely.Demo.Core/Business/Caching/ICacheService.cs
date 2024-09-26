using EPiServer.Framework.Cache;

namespace Optimizely.Demo.Core.Business.Caching;

public interface ICacheService : IObjectInstanceCache
{
    /// <summary>
    /// Gets or sets the value in cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="getT"></param>
    /// <returns></returns>
    T GetOrSet<T>(string key, Func<T> getT) where T : class;

    /// <summary>
    /// Gets or sets the value in cache. Use expiresBy to add max duration in cache.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="getT"></param>
    /// <param name="expiresBy"></param>
    /// <returns></returns>
    T GetOrSet<T>(string key, Func<T> getT, TimeSpan expiresBy) where T : class;

    /// <summary>
    /// Gets the value from cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    T Get<T>(string key) where T : class;

    /// <summary>
    /// Inserts an item to cache with expiresBy
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <param name="expiresBy"></param>
    void Insert<T>(string key, T item, TimeSpan expiresBy) where T : class;

    /// <summary>
    /// Inserts an item to cache with expiresBy for up to 20 minutes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="item"></param>
    void Insert<T>(string key, T item) where T : class;

    /// <summary>
    /// Remove the cached items beginning with the key
    /// </summary>
    /// <param name="startsWith"></param>
    void RemoveStartsWith(string startsWith);
}
