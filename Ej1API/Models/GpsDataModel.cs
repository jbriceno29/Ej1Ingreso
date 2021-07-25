using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ej1API.Models
{
    public class GpsDataModel
    {
        public int Id { get; set; }
        public DateTime DateSystem { get; set; }
        public DateTime DateEvent { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int Battery { get; set; }
        public int Source { get; set; }
        public int Type { get; set; }
    }
}