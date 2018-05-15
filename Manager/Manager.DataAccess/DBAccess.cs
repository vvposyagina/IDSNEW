using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.DataAccess
{
    public class DBAccess
    {
        SQLiteConnection connection;

        public DBAccess(string address)
        {
            connection = new SQLiteConnection(String.Format("DataSource={0};", address));
        }

        public string[][] GetAllNetEntrys()
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT * FROM networklog"), connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<string[]> result = new List<string[]>();

            foreach (DbDataRecord entry in reader)
            {
                List<string> newEntry = new List<string>();
                newEntry.Add(entry["id"].ToString());
                newEntry.Add(entry["date"].ToString());
                newEntry.Add(entry["src"].ToString());
                newEntry.Add(entry["dst"].ToString());
                newEntry.Add(entry["length"].ToString());
                newEntry.Add(entry["data"].ToString());
                newEntry.Add(entry["threat"].ToString());
                result.Add(newEntry.ToArray());
            }

            connection.Close();
            return result.ToArray();                   
        }

        public void AddNetLog(string[] data)
        {
            connection.Open();
            SQLiteCommand commandInsert = new SQLiteCommand(String.Format("INSERT INTO networklog (date, src, dst, length, data, threat) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", data[0], data[1], data[2], data[3], data[4], data[5]), connection);
            SQLiteDataReader reader = commandInsert.ExecuteReader();
            connection.Close();
        }

        public string[][] GetAllHostEntrys()
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT * FROM hostlog"), connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<string[]> result = new List<string[]>();

            foreach (DbDataRecord entry in reader)
            {
                List<string> newEntry = new List<string>();
                newEntry.Add(entry["id"].ToString());
                newEntry.Add(entry["date"].ToString());
                newEntry.Add(entry["event_id"].ToString());
                newEntry.Add(entry["provider"].ToString());
                newEntry.Add(entry["data"].ToString());
                newEntry.Add(entry["threat"].ToString());
                result.Add(newEntry.ToArray());
            }

            connection.Close();
            return result.ToArray();
        }

        public void AddHostLog(string[] data)
        {
            connection.Open();
            SQLiteCommand commandInsert = new SQLiteCommand(String.Format("INSERT INTO hostlog (date, event_id, provider, data, threat) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", data[0], data[1], data[2], data[3], data[4]), connection);
            SQLiteDataReader reader = commandInsert.ExecuteReader();
            connection.Close();
        }

        public string[][] GetAllNNEntrys()
        {
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(String.Format("SELECT * FROM neural_network"), connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<string[]> result = new List<string[]>();

            foreach (DbDataRecord entry in reader)
            {
                List<string> newEntry = new List<string>();
                newEntry.Add(entry["id"].ToString());
                newEntry.Add(entry["date"].ToString());
                newEntry.Add(entry["goal"].ToString());
                newEntry.Add(entry["samples"].ToString());
                newEntry.Add(entry["sgood"].ToString());
                newEntry.Add(entry["sbad"].ToString());
                newEntry.Add(entry["epochcount"].ToString());
                newEntry.Add(entry["test"].ToString());
                newEntry.Add(entry["accuracy"].ToString());
                newEntry.Add(entry["firstmistake"].ToString());
                newEntry.Add(entry["secondmistake"].ToString());
                result.Add(newEntry.ToArray());
            }

            connection.Close();
            return result.ToArray();
        }

        public void AddNNEntry(string[] data)
        {
            connection.Open();
            SQLiteCommand commandInsert = new SQLiteCommand(String.Format("INSERT INTO neural_network (date, goal, samples, sgood, sbad, epochcount, test, accuracy, firstmistake, secondmistake) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')", data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9]), connection);
            SQLiteDataReader reader = commandInsert.ExecuteReader();
            connection.Close();
        }
    }
}
