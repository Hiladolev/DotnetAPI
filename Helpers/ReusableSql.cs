using DotnetAPI.Data;

namespace DotnetAPI.Helpers
{
    public class ReusableSql
    {
        private readonly DataContextDapper _dapper;

        public ReusableSql(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }
    
    }
}