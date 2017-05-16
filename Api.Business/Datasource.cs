using Business.DataStore;

namespace Business
{
    public class Datasource
    {
        protected readonly IDatabase DB;

        public Datasource(IDatabase db)
        {
            DB = db;
        }
    }
}
