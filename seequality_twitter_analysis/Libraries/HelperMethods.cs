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

        public static int GetTextMiningMethodIDFromDatabase(string targetSQLConnectionString, string TextMiningMethodName)
        {
            int TextMiningMethodID = 0;

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

                cmd = new SqlCommand("sp_GetTextMiningMethodID", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@TextMiningMethodName", SqlDbType.VarChar, 100).Value = TextMiningMethodName;
                cmd.Parameters.Add("@TextMiningMethodID", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                try
                {
                    cmd.ExecuteNonQuery();
                    TextMiningMethodID = Convert.ToInt32(cmd.Parameters["@TextMiningMethodID"].Value);
                }
                catch (Exception exc)
                {
                    logger.Error(exc);
                }
            }
            
            return TextMiningMethodID;
        }
    }
}
