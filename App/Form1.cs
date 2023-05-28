using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task1.Model.GoogleMap;
using Task1.Model.DataBase;
using Task1.Model.DataBase.Tables;
using Task1.Model;

namespace Task1
{
	public partial class Form1 : Form
	{
		Map map;
		GMapMarker _selectedMarker;
		CommandDb CommandDb;
		public Form1()
		{
			InitializeComponent();
			Connect();
		}
		private void Connect()
		{
			ConnectDb connectDb = new ConnectDb("GLEB-PC\\SQLEXPRESS", "BdCoordinates");
			CommandDb = new CommandDb(connectDb);
		}
		private void gMapControl1_Load(object sender, EventArgs e) // загрузка карты
		{
			map = new Map(gMapControl1);

			map.MainMap.MouseUp += _gMapControl_MouseUp;
			map.MainMap.MouseDown += _gMapControl_MouseDown;

			map.AddOverlays(new GMapOverlay("First"));

			Task.Run(() => // загрузка координат на карту
			{
				List<Marker> listMarkers = new List<Marker>();

				Task taskGetMarkers = Task.Run(() => listMarkers = CommandDb.GetAllMarker());

				taskGetMarkers.Wait();

				foreach (var item in listMarkers)
				{
					map.AddMarker(new GMarker(item), "First");
				}
			});
		}
		private void _gMapControl_MouseDown(object sender, MouseEventArgs e) // передвижение маркеров
		{
			_selectedMarker = map.MainMap.Overlays
				.SelectMany(m => m.Markers)
				.FirstOrDefault(m => m.IsMouseOver == true);
		}

		private void _gMapControl_MouseUp(object sender, MouseEventArgs e) // передвижение маркеров
		{
			if (_selectedMarker is null)
				return;
			var latlng = map.MainMap.FromLocalToLatLng(e.X, e.Y);

			_selectedMarker.Position = latlng;

			GMarker gSelectedMarker = (from item in map.ListMarkers where item.MapMarker.Position == _selectedMarker.Position select item).FirstOrDefault();

			if (gSelectedMarker != null)
			{
				Marker marker = new AdapterMarker(_selectedMarker.Position, gSelectedMarker);
				Task.Run(() =>
				{
					CommandDb.UpdateCoordinates(marker);
				});
			}
			_selectedMarker = null;
		}
	}
}
