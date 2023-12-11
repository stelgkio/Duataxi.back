using DuaTaxi.Common.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DuaTaxi.Common.WebApiClient
{
    public interface IWebApiClient
    {
        
        Task DownloadFile(string method, Stream stream);

        Task<string> DownloadNamedFile(string method, Stream stream, bool leaveOpen = true, bool usePost = false);

        Task<string> DownloadNamedFile<T>(string method, Stream stream, T postData, bool leaveOpen = true);

        Task DownloadFile<T>(string method, T body, Stream stream);

        Task<int> CountAsync<T>(string method);

        Task<T> LoadAsync<T>(string method, params object[] urlparts);

        Task<T> LoadQAsync<T>(string method, string pname, string pvalue);


        Task<T> LoadQAsync<T>(string method, params Tuple<string, object>[] parameters);

        Task<T> LoadAsync<T>(string method, Dictionary<string, string> parameters);


        Task<T> PostAsync<T>(string method, T body);

        Task<TResult> PostAsync<T, TResult>(string method, T body);

        Task PostAsync(string method);

        Task<TResult> PutAsync<TResult>(string method);

        Task<T> PutAsync<T>(string method, T content);

        Task<TResult> PutAsync<T, TResult>(string method, T content);

        Task<Object> PutAsync(string method, object content, Type type);

        Task SaveBatchAsync<T>(string method, IEnumerable<T> entities) where T : IIdentifiable;

        Task DeleteAsync<T>(string method, T entity) where T : IIdentifiable;

        Task DeleteAsync(string method, long Id) ;
    }
}