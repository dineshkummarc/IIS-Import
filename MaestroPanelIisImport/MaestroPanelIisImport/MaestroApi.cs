namespace MaestroPanelIisImport
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public class MaestroApi
    {
        private string _apiKey;
        private string _apiUri;

        public MaestroApi(string ApiKey, string apiHostdomain, int port=9715, bool ssl = false)
        {
            _apiKey = ApiKey;
            _apiUri = String.Format("{2}://{0}:{1}/Api", apiHostdomain, port, ssl ? "https" : "http");
        }

        public ApiResult DomainDelete(string name)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);

            return SendApi("Domain/Delete", "DELETE", _args);
        }
        

        public ApiResult DomainStart(string name)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);

            return SendApi("Domain/Start", "POST", _args);
        }

        public ApiResult DomainStop(string name)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);

            return SendApi("Domain/Stop", "POST", _args);
        }

        //from http://www.c-sharpcorner.com/uploadfile/niradhip/automatic-password-generator-in-C-Sharp/
        public string GeneratePassword(int Length)
        {
            Random random = new Random();
            string password = HashData(random.Next().ToString()).Substring(0, Length);
            string newPass = "";

            // Uppercase at random
            random = new Random();
            for (int i = 0; i < password.Length; i++)
            {
                if (random.Next(0, 2) == 1)
                    newPass += password.Substring(i, 1).ToUpper();
                else
                    newPass += password.Substring(i, 1);
            }
            return newPass;
        }

        private string HashData(string Data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(Data));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        public ApiResult DomainCreate(string name, string planAlias, string username, string password, bool activedomainuser)
        {            
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name",name);
            _args.Add("planAlias", planAlias);
            _args.Add("username",username);
            _args.Add("password", password);
            _args.Add("activedomainuser", activedomainuser.ToString());

            return SendApi("Domain/Create", "POST", _args);
        }
        
        private ApiResult SendApi(string action, string method, NameValueCollection _parameters)
        {
            var _result = new ApiResult();
            var _uri = new Uri(String.Format("{0}/{1}",_apiUri, action));            

            try
            {
                HttpWebRequest request = WebRequest.Create(_uri) as HttpWebRequest;
                request.Method = method;
                request.Timeout = 8 * 1000; //8 seconds                
                request.ContentType = "application/x-www-form-urlencoded";

                WriteData(ref request, _parameters);
                var _responseText = GetData(request);

                _result = ApiResult.DeSerializeObject<ApiResult>(_responseText);
            }
            catch (Exception ex)
            {
                _result.Code = -1;
                _result.Message = ex.Message;
                _result.OperationResultString = ex.StackTrace;
            }

            return _result;
        }

        private void WriteData(ref HttpWebRequest _request, NameValueCollection _parameters)
        {
            byte[] byteData = CreateParameters(_parameters);
            _request.ContentLength = byteData.Length;

            using (Stream postStream = _request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
        }

        private string GetData(HttpWebRequest _request)
        {
            var _response = String.Empty;
            using (HttpWebResponse response = _request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                _response = reader.ReadToEnd();
            }

            return _response;
        }

        private byte[] CreateParameters(NameValueCollection _parameters)
        {
            var _sb = new StringBuilder(_parameters.Count);

            foreach (var item in _parameters.AllKeys)
                _sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(item), HttpUtility.UrlEncode(_parameters[item]));

            _sb.Length -= 1;

            return UTF8Encoding.UTF8.GetBytes(_sb.ToString());
        }
    }
}
