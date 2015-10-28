using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLite.Net;
using SQLite.Net.Async;

namespace MapXamarinForm
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetConnection();
    }
}
