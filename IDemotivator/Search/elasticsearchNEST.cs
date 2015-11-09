using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nest;
using IDemotivator.Models;
using IDemotivator.App_LocalResources;

namespace IDemotivator.Search
{
    public class elasticsearchNEST : IDisposable
    {
        public elasticsearchNEST()
        {
            uri = new Uri(GlobalRes.ElasticNestUri);
            Settings = new ConnectionSettings(uri).SetDefaultIndex(GlobalRes.ElasticNestSite);
            _context = new ElasticClient(Settings);
        }
        private Uri uri;
        private ConnectionSettings Settings;
        private const string ConnectionString = GlobalRes.ElasticNestUri;
        private readonly ElasticClient _context;

        public IEnumerable<Demotivator> SearchDemotivators(string term)
        {
            term = term.Replace("@", " AND ");
            term = term.Replace(" ", " AND ");
            var result = _context.Search<Demotivator>(q => q
            .Query(f => f
               .QueryString(t => t.Query(term + "*").OnFields(u => u.Name)))
            );
            return result.Hits.Select(t => t.Source);
        }

        public IEnumerable<ApplicationUser> SearchUser(string term)
       {
            term = term.Replace("/[ \t]{ 2,}/ g", " ");
            term = term.Replace(" ", " AND ");
            term = term.Replace("@"," AND ");
            var result = _context.Search<ApplicationUser>(q => q
            .Index(GlobalRes.ElasticNestSite)
            .Size(10)
            .From(0)
            .Query(f => f
               .QueryString(t => t.Query(term+"*").OnFields(u => u.UserName)))
            );
            return result.Hits.Select(t => t.Source);
        }

        public void Adding(Demotivator demotivator)
        {
            _context.Index(demotivator);
            _context.Refresh();
        }

        public void Adding(ApplicationUser user)
        {
            _context.Index(user);
            _context.Refresh();
        }

        public void UpdateSkill()
        {

        }

        public void DeleteDem(Demotivator user)
        {
            _context.Delete(user);
            _context.Refresh();
        }

        private bool isDisposed;
        public void Dispose()
        {
            if (isDisposed)
            {
                isDisposed = true;
                _context.Connection.Delete(uri);
            }
        }
    }
}