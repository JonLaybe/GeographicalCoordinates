using GMap.NET.WindowsForms;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Task1.Model.GoogleMap
{
	public class Map
	{
		public GMapControl MainMap { get; private set; } // карты
		public List<GMarker> ListMarkers { get; private set; } // все маркеры

		public Map(GMapControl map)
		{
			MainMap = map;
			ListMarkers = new List<GMarker>();
			CreateMap();
		}

		public List<GMapOverlay> gMapOverlays // все слои
		{
			get { return MainMap.Overlays.ToList(); }
		}

		private void CreateMap() // Создание карты
		{
			GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
			MainMap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
			MainMap.MinZoom = 2;
			MainMap.MaxZoom = 20;
			MainMap.Zoom = 4;
			MainMap.Position = new GMap.NET.PointLatLng(66.4169575018027, 94.25025752215694);
			MainMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
			MainMap.CanDragMap = true;
			MainMap.DragButton = MouseButtons.Left;
			MainMap.ShowCenter = false;
			MainMap.ShowTileGridLines = false;
		}

		public void AddOverlays(GMapOverlay overlay) => MainMap.Overlays.Add(overlay); // добавление слоя

		public void AddMarker(GMarker marker, string nameOverlay) // добавление маркера на определенный слой
		{
			GMapOverlay mapOverlay = (from item in MainMap.Overlays where item.Id == nameOverlay select item).FirstOrDefault();

			if (mapOverlay != null)
			{
				mapOverlay.Markers.Add(marker.MapMarker);
				ListMarkers.Add(marker);
			}
		}
	}
}
