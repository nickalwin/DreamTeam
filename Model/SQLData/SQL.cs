using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace Model
{
    public static class SQL
    {
        /**
         * <summary>Maakt de verbinding met de database</summary>
         * <returns>SqlConnection connection</returns>
         */
        public static MySqlConnection OpenConnection()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "web0084.zxcs.nl",
                UserID = "u27608p21502_DreamTeam",
                Password = "oQyLbemB",
                Database = "u27608p21502_DreamTeam",
                ConvertZeroDateTime = true,

            };

            MySqlConnection connection = new MySqlConnection(builder.ConnectionString);
            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                throw;
            }

            return connection;
        }
        /**
         * <summary>Sluit de verbinding met de database</summary>
         */
        public static void CloseConnection(MySqlConnection connection)
        {
            connection.Close();
        }
        /**
         * <summary>Maakt verbinding met de database en haalt de data op</summary>
         * <returns>SqlDataReader reader</returns>
         */
        public static MySqlDataReader ReadQuery(MySqlConnection connection, string query)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;

            MySqlDataReader reader = command.ExecuteReader();
            //CloseConnection(connection); als je dit aan laat gaan de readquery kapot :)
            return reader;
        }
        /**
         * <summary>
         * Maakt verbinding met de database en haalt de data op.
         * Met parameter binding, gaat via de Dictionary. Net zoals bij php met ?. \n
         * Nu werkt het via @(varname) -> value
         * Update profiel set id = @id;
         * @id = 1
         * 
         * </summary>
         * <returns><c>SqlDataReader reader</c></returns>
         */
        public static MySqlDataReader ReadQuery(MySqlConnection connection, string query, Dictionary<string, string> parameters)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;

            foreach (KeyValuePair<string, string> pair in parameters)
            {
                command.Parameters.AddWithValue(pair.Key, pair.Value);
            }

            MySqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        /**
         * <summary>Maakt verbinding met de database en voert de meegegeven query uit</summary>
         * <returns>int RowsAffected</returns>
         */
        public static int WriteQuery(MySqlConnection connection, string query)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;

            int rowsAffected = command.ExecuteNonQuery();
            CloseConnection(connection);
            return rowsAffected;
        }
        /**
         * <summary>
         * Maakt verbinding met de database en voert query uit.
         * Met parameter binding, gaat via de Dictionary. Net zoals bij php met ?. \n
         * Nu werkt het via @(varname) -> value
         * </summary>
         * <returns>int RowsAffected</returns>
         */
        public static int WriteQuery(MySqlConnection connection, string query, Dictionary<string, string> parameters)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;

            foreach (KeyValuePair<string, string> pair in parameters)
            {
                command.Parameters.AddWithValue(pair.Key, pair.Value);
            }
            int rowsAffected = 0;
            try
            {
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                rowsAffected = -1;
            }
            CloseConnection(connection);
            return rowsAffected;
        }
    }
}
