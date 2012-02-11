namespace MaestroPanelIisImport
{
    using System;
    using System.Linq;
    using System.Text;
    using System.IO;

    class Program
    {
        static MaestroApi _api;

        static string api_key;
        static string api_host_name;
        static string api_plan_alias;
        static int api_port_number;
        static bool use_ssl;
        static bool commit;

        static void Main(string[] args)
        {
            commit = true;
            SetArgs("-key", "API Key cannot be null", out api_key);
            SetArgs("-host", "Hostname cannot be null", out api_host_name);
            SetArgs("-plan", "Plan Alias cannot be null", out api_plan_alias);
            SetArgs("-port", "Port cannot be null", out api_port_number);
            SetArgs("-ssl", "Hostname cannot be null", out use_ssl);

            Import();
        }

        static void SetArgs<T>(string argumentName, string errorMessage, out T TValue)
        {            
            TValue = default(T);

            var _args = Environment.GetCommandLineArgs().ToList();
            var _arg = _args.Where(m => m.Equals(argumentName)).FirstOrDefault();            

            if (_arg != null)
            {
                var _index = _args.IndexOf(_arg);

                try
                {
                    TValue = (T)Convert.ChangeType(_args[_index + 1], typeof(T));
                }
                catch (Exception ex)
                {                    
                    ShowUsage();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: {1} {0}",ex.Message, argumentName);
                    Console.ResetColor();                    
                }
            }
            else
            {
                ShowUsage();
            }
        }

        static void ShowUsage()
        {
            commit = false;
            var _sb = new StringBuilder();
            _sb.AppendLine("IIS 7.5 to MaestroPanel Import Tool 1.0");            
            _sb.AppendLine();
            _sb.AppendLine("Parameters:");
            _sb.AppendLine("-key\tMaestroPanel API key");
            _sb.AppendLine("-host\tMaestroPanel Web Management Service Name");
            _sb.AppendLine("-plan\tMaestroPanel Domain Plan Alias Name");
            _sb.AppendLine("-port\tMaestroPanel Web Management Service Port Number. Default: 9715");
            _sb.AppendLine("-ssl\tUse https protocol true or false");
            _sb.AppendLine();
            _sb.AppendLine("Usage:");
            _sb.AppendLine("iisimport.exe -key 1_1e9f1131d7cc44fd8c5df1aca0155937 -host contoso.com -plan GOLDPLAN -port 9715 -ssl false");

            Console.WriteLine(_sb.ToString());
            Environment.Exit(0);
        }

        static void Import()
        {
            if (commit)
            {
                var _sb = new StringBuilder();
                var _iis = new IIsReader();
                var _password = String.Empty;
                ApiResult _result = null;

                _sb.AppendLine("domain\tplan\tusername\tpassword\tcode\tmessage");
                _api = new MaestroApi(api_key, api_host_name, port: api_port_number, ssl: use_ssl);                                

                foreach (var item in _iis.GetAllSites())
                {
                    _password = _api.GeneratePassword(8);
                    _result = _api.DomainCreate(item.Name, api_plan_alias, item.Name, _password, false);
                    _sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}{6}", item.Name, api_plan_alias, item.Name, _password, _result.Code, _result.Message, Environment.NewLine);

                    Console.WriteLine("{0}\t{1}",item.Name, _result.Message);
                    _result = null;
                    _password = null;
                }

                SaveLogFile(_sb.ToString());
            }
        }

        static void SaveLogFile(string _text)
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "iisimport.log"), _text);
        }
    }
}
