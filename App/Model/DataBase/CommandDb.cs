using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using Task1.Model.DataBase.Tables;

namespace Task1.Model.DataBase
{
	public class CommandDb
	{
		private ConnectDb _connectDb;

		public CommandDb(ConnectDb connect)
		{
			_connectDb = connect;
			CreateDataBase();
		}

		private int ExecutionCommand(string query) // выполнение команд
		{
			try
			{
				_connectDb.SqlConnection.Open();
				SqlCommand command = new SqlCommand(query, _connectDb.SqlConnection);
				return command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				_connectDb.SqlConnection.Close();
			}

			return 0;
		}

		private void CreateDataBase() // создание базы данных
		{
			string queryCreateDb = $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{_connectDb.NameDb}') " +
				"BEGIN " +
				$"CREATE DATABASE {_connectDb.NameDb} " +
				"END";

			ExecutionCommand(queryCreateDb);

			_connectDb.ConnactBd();

			CreateTable("TableCoordinates", new Dictionary<string, string>()
			{
				{ "Name", "TEXT" },
				{ "CoordinatesX", "FLOAT" },
				{ "CoordinatesY", "FLOAT" },
			});

			ExecutionCommand("IF NOT EXISTS (SELECT 1 FROM TableCoordinates)" +
				" BEGIN " +
				"INSERT INTO TableCoordinates (Name, CoordinatesX, CoordinatesY) VALUES ('Novosibirsk', '55.0415', '82.9346')," +
				"('Tokyo', '35.6895000', '139.6917100')," +
				"('Helsinki', '60.1695200', '24.9354500')\n" +
				" END");
		}

		public void CreateTable(string tableName, Dictionary<string, string> columns) // создание таблиицы
		{
			string query = $"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{tableName}') CREATE TABLE {tableName} (Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,";

			foreach(var item in columns)
			{
				query += $"{item.Key} {item.Value} NOT NULL,";
			}
			query = query.Remove(query.Length - 1);
			query += ")";

			ExecutionCommand(query);
		}
		public void Insert(string table, Dictionary<string, string> row_values) // добавление данных в таблицу
		{
			string query = $"INSERT INTO {table}(";

			foreach (var cell in row_values)
				query += $"{cell.Key},";
			query = query.Remove(query.Length - 1);
			query += ") VALUES (";
			foreach (var cell in row_values)
				query += "'" + cell.Value + "',";
			query = query.Remove(query.Length - 1);
			query += ")";
			ExecutionCommand(query);
		}

		public List<Marker> GetAllMarker() // получение всех маркеров
		{
			List<Marker> ListMarkers = new List<Marker>();
			string sql = "SELECT * FROM TableCoordinates";

			_connectDb.SqlConnection.Open();

			using (SqlCommand command = new SqlCommand(sql, _connectDb.SqlConnection))
			{
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						ListMarkers.Add(new Marker()
						{
							Id = reader.GetInt32(0),
							MessageTool = reader.GetString(1),
							CoordinatesX = reader.GetDouble(2),
							CoordinatesY = reader.GetDouble(3),
						});
					}
				}
			}

			_connectDb.SqlConnection.Close();

			return ListMarkers;
		}
		public void UpdateCoordinates(Marker marker) // обновление коордитат
		{
			string query = $"UPDATE TableCoordinates SET CoordinatesX = {marker.CoordinatesX.ToString().Replace(',', '.')}, CoordinatesY = {marker.CoordinatesY.ToString().Replace(',', '.')} WHERE Id = {marker.Id}";

			ExecutionCommand(query);
		}
	}
}
