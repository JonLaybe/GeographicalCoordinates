using System.Data.SqlClient;

namespace Task1.Model.DataBase
{
	public class ConnectDb // подключение к базе данных
	{
		public string NameDb { get; private set; }
		public SqlConnection SqlConnection { get; private set; }
		private string _connectionString;

		public ConnectDb(string serverName, string dataBaseName)
		{
			_connectionString = $"Server={serverName};Database={dataBaseName};Trusted_Connection=True;Integrated Security=SSPI";
			Init(dataBaseName, $"Server={serverName};Trusted_Connection=True;Integrated Security=SSPI");
		}

		private void Init(string nameDb, string connectionStringNoDb) // подключение к серверу mssql
		{
			NameDb = nameDb;
			SqlConnection = new SqlConnection(connectionStringNoDb);
		}

		public void ConnactBd() // подключение к базе данных на сервере
		{
			SqlConnection = new SqlConnection(_connectionString);
		}
	}
}
