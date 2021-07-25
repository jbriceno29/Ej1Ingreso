using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ej1API.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Ej1API.Controllers
{
    public class GpsDataController : ApiController
    {
        string conn = ConfigurationManager.ConnectionStrings["conect_db"].ConnectionString;

        public IHttpActionResult GetGPSData()
        {
            List<GpsDataModel> gpsDatas = new List<GpsDataModel>();

            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                //SqlCommand cmd;
                DataTable data = new DataTable();
                SqlDataAdapter cmd = new SqlDataAdapter("Get_GPSData", sqlConnection);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();

                // SqlDataReader reader = cmd.ExecuteReader();
                cmd.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    GpsDataModel gpsData = new GpsDataModel
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        DateSystem = Convert.ToDateTime(row["DateSystem"]),
                        DateEvent = Convert.ToDateTime(row["DateEvent"]),
                        Latitude = float.Parse(row["Latitude"].ToString()),
                        Longitude = float.Parse(row["Longitude"].ToString()),
                        Battery = Convert.ToInt32(row["Battery"]),
                        Source = Convert.ToInt32(row["Source"]),
                        Type = Convert.ToInt32(row["Type"])
                    };

                    gpsDatas.Add(gpsData);
                }

                sqlConnection.Close();
            }

            return Json(gpsDatas);
        }

        public IHttpActionResult GetGPSData(int Id)
        {
            List<GpsDataModel> gpsDatas = new List<GpsDataModel>();

            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                //SqlCommand cmd;
                DataTable data = new DataTable();
                SqlDataAdapter cmd = new SqlDataAdapter("Get_GPSData", sqlConnection);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.AddWithValue("@Id", Id);
                sqlConnection.Open();

                // SqlDataReader reader = cmd.ExecuteReader();
                cmd.Fill(data);
                foreach (DataRow row in data.Rows)
                {
                    GpsDataModel gpsData = new GpsDataModel
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        DateSystem = Convert.ToDateTime(row["DateSystem"]),
                        DateEvent = Convert.ToDateTime(row["DateEvent"]),
                        Latitude = float.Parse(row["Latitude"].ToString()),
                        Longitude = float.Parse(row["Longitude"].ToString()),
                        Battery = Convert.ToInt32(row["Battery"]),
                        Source = Convert.ToInt32(row["Source"]),
                        Type = Convert.ToInt32(row["Type"])
                    };

                    gpsDatas.Add(gpsData);
                }

                sqlConnection.Close();
            }

            if (gpsDatas.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Json(gpsDatas);
            }
        }

        public IHttpActionResult PostGpsData([FromBody] GpsDataModel gpsData)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("Insert_GPSData", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.Int, 16).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@DateSystem", gpsData.DateSystem);
                    cmd.Parameters.AddWithValue("@DateEvent", gpsData.DateEvent);
                    cmd.Parameters.AddWithValue("@Latitude", gpsData.Latitude);
                    cmd.Parameters.AddWithValue("@Longitude", gpsData.Longitude);
                    cmd.Parameters.AddWithValue("@Battery", gpsData.Battery);
                    cmd.Parameters.AddWithValue("@Source", gpsData.Source);
                    cmd.Parameters.AddWithValue("@Type", gpsData.Type);
                    sqlConnection.Open();

                    cmd.ExecuteNonQuery();
                    gpsData.Id = Convert.ToInt16(cmd.Parameters[0].Value);
                    sqlConnection.Close();
                }
                return Ok(gpsData);
            }
            catch (Exception)
            {
                return BadRequest();
                //throw;
            }
        }

        public IHttpActionResult DeleteGPSData(int Id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("Delete_GPSData", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    sqlConnection.Open();

                    cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
                //throw;
            }
        }

        public IHttpActionResult POSTGPSData(int Id, [FromBody] GpsDataModel gpsData)
        {
            int response;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("Update_GPSData", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Response", SqlDbType.Int, 16).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@DateSystem", gpsData.DateSystem);
                    cmd.Parameters.AddWithValue("@DateEvent", gpsData.DateEvent);
                    cmd.Parameters.AddWithValue("@Latitude", gpsData.Latitude);
                    cmd.Parameters.AddWithValue("@Longitude", gpsData.Longitude);
                    cmd.Parameters.AddWithValue("@Battery", gpsData.Battery);
                    cmd.Parameters.AddWithValue("@Source", gpsData.Source);
                    cmd.Parameters.AddWithValue("@Type", gpsData.Type);
                    sqlConnection.Open();

                    cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    response = Convert.ToInt16(cmd.Parameters[0].Value);
                }

                gpsData.Id = Id;
                if (response == 0)
                {
                    return NotFound();
                }

                return Ok(gpsData);
            }
            catch (Exception ex)
            {
                return BadRequest();
                //throw;
            }
        }
    }
}
