using DuaTaxi.Common.RestEase;
using DuaTaxi.Common.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DuaTaxi.Common.WebApiClient
{
    public class WebApiClient : IWebApiClient
    {


        private static Uri baseUri;

        public static string ApiRoot = "api";

        private readonly IOptions<RestEaseOptions> config;


        public WebApiClient(IOptions<RestEaseOptions> config, string serviceName)
        {
            this.config = config;  
        
            var service = this.config.Value.Services.SingleOrDefault(s => s.Name.Equals(serviceName,StringComparison.InvariantCultureIgnoreCase));
            if (service == null)
            {
                throw new RestEaseServiceNotFoundException($"RestEase service: '{serviceName}' was not found.",
                    serviceName);
            }

            baseUri = new UriBuilder
            {
                Scheme = service.Scheme,
                Host = service.Host,
                Port = service.Port
            }.Uri;

        }


        private MediaTypeFormatter JSONFormatter = null;

        private HttpClient PrepareClient()
        {

            HttpClientHandler handler = new HttpClientHandler()
            {
                CookieContainer = new CookieContainer(),
                UseCookies = true
            };

            HttpClient client = new HttpClient(handler)
            {
                BaseAddress = baseUri
            };

            if (JSONFormatter == null)
            {
                JSONFormatter = new JsonMediaTypeFormatter()
                {
                    SerializerSettings = new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                        NullValueHandling = NullValueHandling.Include,
                        PreserveReferencesHandling = PreserveReferencesHandling.All
                    }
                };
            }

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            client.Timeout = new TimeSpan(0, 10, 0);

            return client;
        }

        private HttpClient _client = null;

        private HttpClient Client
        {
            get
            {

                if (_client == null)
                {
                    _client = PrepareClient();
                }

                //if (string.IsNullOrEmpty(AccessToken))
                //{
                //    throw new Exception("No authentication token found", TokenException);
                //}

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                return _client;
            }
        }                                                                     





        public async Task DownloadFile(string method, Stream stream)
        {

            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));
            HttpResponseMessage response = await Client.GetAsync(ApiRoot + "/" + method);


#if DEBUG
            if (response.StatusCode != HttpStatusCode.OK)
            {

                try
                {
                    using (FileStream fs = new FileStream("C:/temp/error.html", FileMode.Create))
                    {
                        var content = await response.Content.ReadAsStreamAsync();
                        await content.CopyToAsync(fs);
                    }
                }
                finally
                {

                }

            }
#endif

            response.EnsureSuccessStatusCode();


            using (Stream s = await response.Content.ReadAsStreamAsync())
            {
                s.CopyTo(stream);
                s.Close();
            }

        }


        public async Task<string> DownloadNamedFile(string method, Stream stream, bool leaveOpen = true, bool usePost = false)
        {


            HttpResponseMessage response;

            if (usePost)
            {
                response = await Client.PostAsync(ApiRoot + "/" + method, null);
            }
            else
            {
                response = await Client.GetAsync(ApiRoot + "/" + method);
            }

#if DEBUG
            if (response.StatusCode != HttpStatusCode.OK)
            {

                using (FileStream fs = new FileStream("C:/temp/error.html", FileMode.Create))
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    await content.CopyToAsync(fs);
                }

            }
#endif

            response.EnsureSuccessStatusCode();

            string name = response.Content?.Headers?.ContentDisposition?.FileName ?? "";
            name = HttpUtility.UrlDecode(name);
            if (name.StartsWith("\"")) name = name.Substring(1);
            if (name.EndsWith("\"")) name = name.Substring(0, name.Length - 1);

            using (Stream s = await response.Content.ReadAsStreamAsync())
            {
                s.CopyTo(stream);
                if (!leaveOpen)
                {
                    s.Close();
                }
            }

            return name;

        }


        public async Task<string> DownloadNamedFile<T>(string method, Stream stream, T postData, bool leaveOpen = true)
        {


            HttpResponseMessage response;
            Client.DefaultRequestHeaders.Accept.Clear();

            response = await Client.PostAsync(ApiRoot + "/" + method, postData, JSONFormatter);


#if DEBUG
            if (response.StatusCode != HttpStatusCode.OK)
            {

                using (FileStream fs = new FileStream("C:/temp/error.html", FileMode.Create))
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    await content.CopyToAsync(fs);
                }

            }
#endif

            response.EnsureSuccessStatusCode();

            string name = response.Content?.Headers?.ContentDisposition?.FileName ?? "";
            name = HttpUtility.UrlDecode(name);
            if (name.StartsWith("\"")) name = name.Substring(1);
            if (name.EndsWith("\"")) name = name.Substring(0, name.Length - 1);

            using (Stream s = await response.Content.ReadAsStreamAsync())
            {
                s.CopyTo(stream);
                if (!leaveOpen)
                {
                    s.Close();
                }
            }

            return name;

        }

        public async Task DownloadFile<T>(string method, T body, Stream stream)
        {

            _client = null;

            Client.Timeout = new TimeSpan(0, 30, 0);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));



            try
            {


                HttpResponseMessage response = await Client.PostAsJsonAsync<T>(ApiRoot + "/" + method, body);

                response.EnsureSuccessStatusCode();

                using (Stream s = await response.Content.ReadAsStreamAsync())
                {
                    s.CopyTo(stream);
                    s.Close();
                }

            }
            finally
            {
                PrepareClient();
            }

        }


//        public async Task<object> LoadByTypeAsync(Type t, string method)
//        {

//            HttpResponseMessage response = await Client.GetAsync(ApiRoot + "/" + method);

//#if DEBUG
//            if (response.StatusCode == HttpStatusCode.InternalServerError)
//            {

//                using (FileStream fs = new FileStream("C:/temp/error.html", FileMode.Create))
//                {
//                    var content = await response.Content.ReadAsStreamAsync();
//                    await content.CopyToAsync(fs);
//                }

//            }
//#endif
//            response.EnsureSuccessStatusCode();

//            Type rtype = typeof(t).MakeGenericType(t);

//            object o;

//            try
//            {
//                o = await response.Content.ReadAsAsync(rtype);
//            }
//            catch (Exception ex)
//            {
//                // FireTimerNow();
//                throw new Exception("Error while retrieving data - could not read response", ex);
//            }

//            PropertyInfo pi = rtype.GetProperty("Success");

//            bool success = (bool)pi.GetValue(o);

//            if (success)
//            {

//                PropertyInfo cpi = rtype.GetProperty("Content");

//                return cpi.GetValue(o);

//            }
//            else
//            {

//                PropertyInfo epi = rtype.GetProperty("ErrorMessage");

//                throw new Exception("Error while retrieving data: " + (epi.GetValue(o) ?? "no error message"));
//            }



//        }

        public async Task<T> LoadAsync<T>(string method)
        {

            HttpResponseMessage response = await Client.GetAsync(ApiRoot + "/" + method);

//#if DEBUG
//            if (response.StatusCode == HttpStatusCode.InternalServerError)
//            {

//                using (FileStream fs = new FileStream("C:/temp/error.html", FileMode.Create))
//                {
//                    var content = await response.Content.ReadAsStreamAsync();
//                    await content.CopyToAsync(fs);
//                }

//            }
//#endif
            response.EnsureSuccessStatusCode();

            //SrvResp<T> r;
            //try
            //{
            //    r = await response.Content.ReadAsAsync<SrvResp<T>>();
            //}
            return await response.Content.ReadAsAsync<T>();
            //try
            //{
               
            //}
            //catch (Exception ex)
            //{
            //    // FireTimerNow();
            //    throw new Exception("Error while retrieving data - could not read response", ex);
            //}

            //if (r.Success)
            //{
            //    return r.Content;
            //}
            //else
            //{
            //    throw new Exception(r.ErrorMessage);
            //}


        }

        /// <summary>
        /// Φέρνει entities με paging βάσει Id
        /// Προσοχή η μέθοδος που καλούμε πρέπει να δέχεται στα δύο τελευταία ορίσματα Id και pagesize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        //public async Task<IEnumerable<T>> LoadImportsPagedAsync<T>(string method, int pageSize, params object[] urlparts) where T : IBaseEntityImport
        //{

        //    List<T> result = new List<T>();

        //    long lastId = -1;

        //    int retries = 0;

        //    int maxRetries = 3;


        //    while (true)
        //    {

        //        var allParts = urlparts.ToList();
        //        allParts.Add(lastId);
        //        allParts.Add(pageSize);

        //        IEnumerable<T> more;

        //        try
        //        {
        //            more = await LoadAsync<IEnumerable<T>>(method, allParts.ToArray());
        //        }
        //        catch
        //        {
        //            if (retries < maxRetries)
        //            {
        //                retries++;
        //                continue;
        //            }
        //            else
        //            {
        //                throw;
        //            }

        //        }

        //        if (!more.Any())
        //        {
        //            break;
        //        }

        //        lastId = more.Cast<IBaseEntityImport>().Select(x => x.Id).Max();

        //        result.AddRange(more);

        //    }

        //    return result;

        //}



        private string ToQueryParameter(object param)
        {
            if (param == null)
            {
                return null;
            }
            switch (param.GetType().Name)
            {
                case ("DateTime"):
                    return ((DateTime)param).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                case ("Decimal"):
                    return ((decimal)param).ToString(System.Globalization.CultureInfo.InvariantCulture);
                default:
                    string s = HttpUtility.UrlEncode(param.ToString());
                    if (s.Contains("/"))
                    {
                        throw new Exception("Invalid query parameter " + param.ToString());
                    }
                    return s;
            }
        }       


        public async Task<int> CountAsync<T>(string method)
        {

            HttpResponseMessage response = await Client.GetAsync(ApiRoot + "/" + method);

#if DEBUG
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {

                using (FileStream fs = new FileStream("C:/temp/error.html", FileMode.Create))
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    await content.CopyToAsync(fs);
                }

            }
#endif
            response.EnsureSuccessStatusCode();

            SrvResp<IEnumerable<T>> r;
            try
            {
                r = await response.Content.ReadAsAsync<SrvResp<IEnumerable<T>>>();
            }
            catch (Exception ex)
            {
                // FireTimerNow();
                throw new Exception("Error while retrieving data - could not read response", ex);
            }

            if (r.Success)
            {
                return r.TotalCount;
            }
            else
            {
                throw new Exception(r.ErrorMessage);
            }


        }


        /// <summary>
        /// ΠΡΟΣΟΧΗ τα urlparts πρέπει να είναι όμορφα, π.χ. αν περιέχουν / θα καταλήξουμε σε λάθος σημείο
        /// Αν καλείς αυτή τη μέθοδο με urlparts από τον έξω κόσμο κάνεις λάθος
        /// Δες τη LoadQAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="urlparts"></param>
        /// <returns></returns>
        public async Task<T> LoadAsync<T>(string method, params object[] urlparts)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append(method);

            foreach (object param in urlparts)
            {
                if (param == null)
                {
                    continue;
                }
                sb.Append("/");
                sb.Append(ToQueryParameter(param));
            }

            return await LoadAsync<T>(sb.ToString());
        }

        public async Task<T> LoadQAsync<T>(string method, string pname, string pvalue)
        {
            return await LoadQAsync<T>(method, Tuple.Create(pname, (object)pvalue));
        }

        public async Task<T> LoadQAsync<T>(string method, params Tuple<string, object>[] parameters)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(method);


            if (parameters.Any())
            {
                sb.Append("?");
                string sep = "";
                foreach (Tuple<string, object> p in parameters)
                {
                    sb.Append(sep);
                    sb.Append(p.Item1);
                    string pvalue = ToQueryParameter(p.Item2);
                    if (!string.IsNullOrEmpty(pvalue))
                    {
                        sb.Append("=");
                        sb.Append(pvalue);
                    }
                    sep = "&";
                }
            }

            return await LoadAsync<T>(sb.ToString());


        }



        public async Task<T> LoadAsync<T>(string method, Dictionary<string, string> parameters)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(method);


            if (parameters.Any())
            {
                sb.Append("?");
                string sep = "";
                foreach (string pname in parameters.Keys)
                {
                    sb.Append(sep);
                    sb.Append(pname);
                    string pvalue = ToQueryParameter(parameters[pname]);
                    if (!string.IsNullOrEmpty(pvalue))
                    {
                        sb.Append("=");
                        sb.Append(pvalue);
                    }
                    sep = "&";
                }
            }

            return await LoadAsync<T>(sb.ToString());


        }

        public async Task<T> PostAsync<T>(string method, T body)
        {
            return await PostAsync<T, T>(method, body);
        }


        public async Task<TResult> PostAsync<T, TResult>(string method, T body)
        {

            string endpoint = ApiRoot + "/" + method;

            var response = await Client.PostAsync(endpoint, body, JSONFormatter);
            response.EnsureSuccessStatusCode();
            SrvResp<TResult> r = await response.Content.ReadAsAsync<SrvResp<TResult>>();

            if (r.Success)
            {
                return r.Content;
            }
            else
            {
                throw new Exception(r.ErrorMessage);
            }
        }

        public async Task PostAsync(string method)
        {

            string endpoint = ApiRoot + "/" + method;

            var response = await Client.PostAsync<string>(endpoint, null, JSONFormatter);
            response.EnsureSuccessStatusCode();
            SrvResp r = await response.Content.ReadAsAsync<SrvResp>();

            if (!r.Success)
            {
                throw new Exception(r.ErrorMessage);
            }
        }

        public async Task<TResult> PutAsync<TResult>(string method)
        {

            string endpoint = ApiRoot + "/" + method;

            var response = await Client.PutAsync(endpoint, null);
            response.EnsureSuccessStatusCode();
            SrvResp<TResult> r = await response.Content.ReadAsAsync<SrvResp<TResult>>();

            if (r.Success)
            {
                return r.Content;
            }
            else
            {
                throw new Exception(r.ErrorMessage);
            }
        }

        public async Task<T> PutAsync<T>(string method, T content)
        {

            string endpoint = ApiRoot + "/" + method;

            var response = await Client.PutAsync(endpoint, content, JSONFormatter);

            response.EnsureSuccessStatusCode();

            SrvResp<T> r = await response.Content.ReadAsAsync<SrvResp<T>>();

            if (r.Success)
            {
                return r.Content;
            }
            else
            {
                throw new Exception(r.ErrorMessage);
            }
        }

        public async Task<TResult> PutAsync<T, TResult>(string method, T content)
        {

            string endpoint = ApiRoot + "/" + method;

            var response = await Client.PutAsync(endpoint, content, JSONFormatter);

            response.EnsureSuccessStatusCode();

            SrvResp<TResult> r = await response.Content.ReadAsAsync<SrvResp<TResult>>();

            if (r.Success)
            {
                return r.Content;
            }
            else
            {
                throw new Exception(r.ErrorMessage);
            }
        }

        public async Task<Object> PutAsync(string method, object content, Type type)
        {

            string endpoint = ApiRoot + "/" + method;

            var response = await Client.PutAsync(endpoint, content, JSONFormatter);

            response.EnsureSuccessStatusCode();

            Type srType = typeof(SrvResp<>).MakeGenericType(type);

            dynamic r = await response.Content.ReadAsAsync(srType);

            if (r.Success)
            {
                return r.Content;
            }
            else
            {
                throw new Exception(r.ErrorMessage);
            }
        }

        public async Task SaveBatchAsync<T>(string method, IEnumerable<T> entities) where T : IIdentifiable
        {

            string endpoint = ApiRoot + "/" + method;

            var response = await Client.PostAsync(endpoint, entities, JSONFormatter);
            response.EnsureSuccessStatusCode();
            SrvResp r = await response.Content.ReadAsAsync<SrvResp>();

            if (!r.Success)
            {
                throw new Exception(r.ErrorMessage);
            }

        }

        //public async Task<T> SaveAsync<T>(string method, T entity) where T : BaseEntity
        //{

        //    string endpoint = ApiRoot + "/" + method;

        //    if (entity.Id == 0)
        //    {
        //        var response = await Client.PostAsync(endpoint, entity, JSONFormatter);
        //        response.EnsureSuccessStatusCode();
        //        SrvResp<T> r = await response.Content.ReadAsAsync<SrvResp<T>>();
        //        if (r.Success)
        //        {
        //            entity.Id = r.Content.Id;
        //            entity.Created = r.Content.Created;
        //            entity.CreatedBy = r.Content.CreatedBy;
        //            entity.Modified = r.Content.Modified;
        //            entity.RowVersion = r.Content.RowVersion;
        //            return r.Content;
        //        }
        //        else
        //        {
        //            throw new Exception(r.ErrorMessage);
        //        }
        //    }
        //    else
        //    {
        //        var response = await Client.PutAsync(endpoint, entity, JSONFormatter);
        //        response.EnsureSuccessStatusCode();
        //        SrvResp<T> r = await response.Content.ReadAsAsync<SrvResp<T>>();
        //        if (r.Success)
        //        {
        //            entity.Modified = r.Content.Modified;
        //            entity.ModifiedBy = r.Content.ModifiedBy;
        //            entity.RowVersion = r.Content.RowVersion;
        //            return r.Content;
        //        }
        //        else
        //        {
        //            throw new Exception(r.ErrorMessage);
        //        }
        //    }

        //}


        public async Task DeleteAsync<T>(string method, T entity) where T : IIdentifiable
        {

            string endpoint = ApiRoot + "/" + method;

            if (entity.Id ==Guid.Empty.ToString())
            {
                return;
            }
            else
            {
                var response = await Client.DeleteAsync(endpoint + "/ById/" + entity.Id.ToString());

                response.EnsureSuccessStatusCode();

                SrvResp<bool> r = await response.Content.ReadAsAsync<SrvResp<bool>>();

                if (r.Success)
                {
                    if (r.Content)
                    {
                        return;
                    }
                    else
                    {
                        throw new Exception("Could not delete entity");
                    }
                }
                else
                {
                    throw new Exception("Error while deleting entity: " + r.ErrorMessage);
                }
            }

        }

        public async Task DeleteAsync(string method, long Id)
        {

            string endpoint = ApiRoot + "/" + method;

            if (Id == 0)
            {
                return;
            }


            var response = await Client.DeleteAsync(endpoint + "/ById/" + Id.ToString());

            response.EnsureSuccessStatusCode();

            SrvResp<bool> r = await response.Content.ReadAsAsync<SrvResp<bool>>();

            if (r.Success)
            {
                if (r.Content)
                {
                    return;
                }
                else
                {
                    throw new Exception("Could not delete entity");
                }
            }
            else
            {
                throw new Exception("Error while deleting entity: " + r.ErrorMessage);
            }


        }

       


        #region Authorization 

        private static string _ClientSettings;

        public static string ClientSettings
        {
            get
            {
                return _ClientSettings;
            }
        }

        private static GenericPrincipal _Principal = new GenericPrincipal(new GenericIdentity("Unauthenitcated"), null);

        public static GenericPrincipal Principal
        {
            get
            {
                return _Principal;
            }
        }

        private static bool _ServiceAccount = false;

        public static bool ServiceAccount
        {
            get
            {
                return _ServiceAccount;
            }
        }



        //public static async Task LoadRoles()
        //{

        //    WebApiClient wac = new WebApiClient();

        //    ApplicationUser user = await wac.LoadAsync<ApplicationUser>("ApplicationUser/CurrentUser");

        //    if (user != null)
        //    {

        //        List<string> roles = user.UserRoleGrants.Where(urg => urg.Permission != "None").Select(urg => urg.PermissionKey + urg.Permission).ToList();
        //        if (user.IsAdmin)
        //        {
        //            roles.Add("Administrator");
        //        }

        //        if (user.ApplicationRoles != null)
        //        {
        //            foreach (var role in user.ApplicationRoles.Select(x => x.Name))
        //            {
        //                roles.Add(role);
        //            }
        //        }


        //        _Principal = new GenericPrincipal(new GenericIdentity(user.UserName), roles.ToArray());
        //        _ServiceAccount = user.IsSystemUser;

        //        _ClientSettings = user.ClientSettings;
        //    }


        //}

        #endregion


        #region Authentication and token management






        private static Timer TokenExpiryTimer;

        //private static TokenResponse TokenResponse {
        //    get;
        //    set;
        //}

        private static string AccessToken;

        //private static OAuth2Client OAClient;
        private static HttpClient _HttpClient;

        private static string Username;
        private static string Password;

        private static Exception TokenException;
     


       


        //public static async Task AuthenticateAsync(string siteUrl, string clientKey, string clientSecret, string username, string password)
        //{

        //    baseUri = new Uri(siteUrl);

        //    Username = username;
        //    Password = password;



        //    //try {
        //    //   //AClient = new OAuth2Client(new Uri(siteUrl + "/oauth/token"), clientKey, clientSecret);
        //    //} catch (Exception ex) {
        //    //    throw new WebApiException("Failed to create authentication client", ex);
        //    //}

        //    _HttpClient = new HttpClient()
        //    {
        //        BaseAddress = new Uri(siteUrl + "/oauth/token"),
        //    };

        //    Encoding encoding = Encoding.UTF8;
        //    string credential = String.Format("{0}:{1}", clientKey, clientSecret);


        //    _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(encoding.GetBytes(credential)));

        //    AccessToken = await GetTokenAsync();

        //    await LoadRoles();

        //}

        #endregion


        //public async Task<Dictionary<long, T>> LoadReference<T>(string method) where T : BaseEntity
        //{

        //    Dictionary<long, T> result = new Dictionary<long, T>();

        //    foreach (var item in await LoadAsync<IEnumerable<T>>(method))
        //    {
        //        result[item.Id] = item;
        //    }

        //    return result;

        //}

    }

    public class WebApiException : Exception
    {
        public WebApiException(string message, Exception inner) : base(message, inner)
        {

        }
    }


}
