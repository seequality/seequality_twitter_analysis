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

        public static List<KeyValuePair<string, string>> GetCountryCodesAndNames()
        {
            List<KeyValuePair<string, string>> allLanguages = new List<KeyValuePair<string, string>>();

            allLanguages.Add(new KeyValuePair<string, string>("",""));

            allLanguages.Add(new KeyValuePair<string, string>("en","English (default)"));
            allLanguages.Add(new KeyValuePair<string, string>("ar","Arabic"));
            allLanguages.Add(new KeyValuePair<string, string>("bn","Bengali"));
            allLanguages.Add(new KeyValuePair<string, string>("cs","Czech"));
            allLanguages.Add(new KeyValuePair<string, string>("da","Danish"));
            allLanguages.Add(new KeyValuePair<string, string>("de","German"));
            allLanguages.Add(new KeyValuePair<string, string>("el","Greek"));
            allLanguages.Add(new KeyValuePair<string, string>("es","Spanish"));
            allLanguages.Add(new KeyValuePair<string, string>("fa","Persian"));
            allLanguages.Add(new KeyValuePair<string, string>("fi","Finnish"));
            allLanguages.Add(new KeyValuePair<string, string>("fr","French"));
            allLanguages.Add(new KeyValuePair<string, string>("fil","Filipino"));
            allLanguages.Add(new KeyValuePair<string, string>("he","Hebrew"));
            allLanguages.Add(new KeyValuePair<string, string>("hi","Hindi"));
            allLanguages.Add(new KeyValuePair<string, string>("hu","Hungarian"));
            allLanguages.Add(new KeyValuePair<string, string>("id","Indonesian"));
            allLanguages.Add(new KeyValuePair<string, string>("it","Italian"));
            allLanguages.Add(new KeyValuePair<string, string>("ja","Japanese"));
            allLanguages.Add(new KeyValuePair<string, string>("ko","Korean"));
            allLanguages.Add(new KeyValuePair<string, string>("msa","Malay"));
            allLanguages.Add(new KeyValuePair<string, string>("nl","Dutch"));
            allLanguages.Add(new KeyValuePair<string, string>("no","Norwegian"));
            allLanguages.Add(new KeyValuePair<string, string>("pl","Polish"));
            allLanguages.Add(new KeyValuePair<string, string>("pt","Portuguese"));
            allLanguages.Add(new KeyValuePair<string, string>("ro","Romanian"));
            allLanguages.Add(new KeyValuePair<string, string>("ru","Russian"));
            allLanguages.Add(new KeyValuePair<string, string>("sv","Swedish"));
            allLanguages.Add(new KeyValuePair<string, string>("th","Thai"));
            allLanguages.Add(new KeyValuePair<string, string>("tr","Turkish"));
            allLanguages.Add(new KeyValuePair<string, string>("uk","Ukrainian"));
            allLanguages.Add(new KeyValuePair<string, string>("ur","Urdu"));
            allLanguages.Add(new KeyValuePair<string, string>("vi","Vietnamese"));
            allLanguages.Add(new KeyValuePair<string, string>("zh - cn","Chinese(Simplified)"));
            allLanguages.Add(new KeyValuePair<string, string>("zh - tw","Chinese(Traditional)"));
            allLanguages.Add(new KeyValuePair<string, string>("und","Undefined"));

            // not provided by twtitter officialy
            allLanguages.Add(new KeyValuePair<string, string>("in","Indonesian"));
            allLanguages.Add(new KeyValuePair<string, string>("ht","Haitian"));
            allLanguages.Add(new KeyValuePair<string, string>("sl","Slovenian"));
            allLanguages.Add(new KeyValuePair<string, string>("tl","Tagalog"));

            return allLanguages;
        }
    }
}
