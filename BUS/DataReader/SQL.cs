using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader
{
    public class SQL
    {
        public class Connection
        {
            private string _IP;
            private string _Port;
            private string _NetLib;
            private string _InitCat;
            private string _User;
            private string _Pass;

            //ConnectionStringBuilder
            public class ConnectionStringBuilder : Connection
            {
                public ConnectionStringBuilder()
                {
                    _IP = String.Empty;
                    _Port = String.Empty;
                    _NetLib = String.Empty;
                    _InitCat = String.Empty;
                    _User = String.Empty;
                    _Pass = String.Empty;
                }

                public ConnectionStringBuilder IP(string ip)
                {
                    _IP = ip;
                    return this;
                }

                public ConnectionStringBuilder Port(string port)
                {
                    _Port = port;
                    return this;
                }

                public ConnectionStringBuilder NetLib(string netlib)
                {
                    _NetLib = netlib;
                    return this;
                }

                public ConnectionStringBuilder InitCat(string initcat)
                {
                    _InitCat = initcat;
                    return this;
                }
                public ConnectionStringBuilder User(string user)
                {
                    _User = user;
                    return this;
                }

                public ConnectionStringBuilder Pass(string pass)
                {
                    _Pass = pass;
                    return this;
                }

                public string Build()
                {
                    return "Data Source=(" + _IP + ")\\SQLEXPRESS" +
                            "," + _Port + ";" +
                            "Network Library=" + _NetLib + ";" +
                            "Initial Catalog=" + _InitCat + ";" +
                            "User ID=" + _User + ";" +
                            "Password=" + _Pass + ";"
                            ;
                }
            }


            //Connect -- perform connection
            public string[] Connect(string connectionstring)
            {
                string[] values = new string[2];

                try
                {
                    SqlConnection sql = new SqlConnection(connectionstring);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                /*
                 * code here
                 */

                return values;
            }
        }
    }
}
