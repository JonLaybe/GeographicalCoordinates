using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.Model.DataBase.Tables;
using Task1.Model.GoogleMap;

namespace Task1.Model
{
	public class AdapterMarker : Marker
	{
		public AdapterMarker(GMarker gMarker)
		{
			Id = gMarker.Id;
			CoordinatesX = gMarker.CoordinatesX;
			CoordinatesY = gMarker.CoordinatesY;
			MessageTool = gMarker.MessageTool;
		}
		public AdapterMarker(PointLatLng point, GMarker gMarker)
		{
			Id = gMarker.Id;
			CoordinatesX = point.Lat;
			CoordinatesY = point.Lng;
			MessageTool = gMarker.MessageTool;
		}
	}
}
