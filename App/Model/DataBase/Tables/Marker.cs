using System;

namespace Task1.Model.DataBase.Tables
{
	public class Marker // таблица маркер
	{
		public int Id { get; set; }
		public string MessageTool { get; set; }
		public double CoordinatesX { get; set; }
		public double CoordinatesY { get; set; }
	}
}
