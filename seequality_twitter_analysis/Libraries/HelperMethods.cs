using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries
{
    public static class HelperMethods
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static DateTime UnixTimeStampToDateTime(double unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
        }

        public static void CleanDatabase(string targetSQLConnectionString, bool cleanSource)
        {
            SqlConnection conn = new SqlConnection(targetSQLConnectionString);
            SqlCommand cmd;

            try
            {
                conn.Open();
            }
            catch (Exception exc)
            {
                logger.Error(exc);
            }

            if (conn.State == ConnectionState.Open)
            {

                cmd = new SqlCommand("sp_CleanTables", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CleanSource", SqlDbType.Bit).Value = cleanSource;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    logger.Error(exc);
                }
            }
        }
    }
}
