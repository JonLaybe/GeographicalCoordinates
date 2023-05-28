using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Linq;
using Task1.Model.DataBase.Tables;

namespace Task1.Model.GoogleMap
{
	public class GMarker : Marker
	{
		public GMapMarker MapMarker { get; private set; } // маркер
		public GMarkerGoogleType MarkerGoogleType { get; private set; }

		public GMarker(Marker marker, GMarkerGoogleType markerGoogleType = GMarkerGoogleType.red)
		{
			Id = marker.Id;
			CoordinatesX = marker.CoordinatesX;
			CoordinatesY = marker.CoordinatesY;
			MessageTool = marker.MessageTool;
			MarkerGoogleType = markerGoogleType;

			CreateMarker();
		}

		public GMarker(double x, double y, string messageTool, GMarkerGoogleType markerGoogleType = GMarkerGoogleType.red)
		{
			CoordinatesX = x;
			CoordinatesY = y;
			MessageTool = messageTool;
			MarkerGoogleType = markerGoogleType;
		}

		private void CreateMarker() // создание маркера
		{
			GMarkerGoogle mapMarker = new GMarkerGoogle(new GMap.NET.PointLatLng(CoordinatesX, CoordinatesY), MarkerGoogleType);
			mapMarker.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(mapMarker);
			mapMarker.ToolTipText = MessageTool;
			mapMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

			MapMarker = mapMarker;
		}
	}
}
