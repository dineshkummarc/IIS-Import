namespace MaestroPanelIisImport
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Web.Administration;

    public class IIsReader
    {
        public List<IIsSite> GetAllSites()
        {
            var _tmp_list = new List<IIsSite>();

            using(ServerManager _server = new ServerManager())
            {
                _tmp_list.AddRange(_server.Sites.Select(m => new IIsSite() { Name = m.Name }));
            }

            return _tmp_list;
        }
    }

    public struct IIsSite
    {
        public string Name { get; set; }  
    }
}
