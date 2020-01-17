﻿  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Collections.ObjectModel;
using Dictionary.ViewModel;

namespace Dictionary.Model
{
    class DataBaseAccess
    {
        private static DataBaseAccess instance;
        private SQLiteConnection myConnection;

        private DataBaseAccess()
        {
            myConnection = new SQLiteConnection("Data Source = wordList.sqlite3");
            if (!File.Exists("./wordList.sqlite3"))
            {
                SQLiteConnection.CreateFile("./wordList.sqlite3");
                CreateTable();
            }

            openConnection();
        }
        
        // realization of singleton because it is a database interface
        public static DataBaseAccess GetInstance()
        {
            if (instance == null)
            {
                return new DataBaseAccess();
            }
            return instance;
        }

        private void CreateTable()
        {
            myConnection.Open();
            string query = @"CREATE TABLE 'Words' (
                        'Id' INTEGER NOT NULL,
	                    'Word' TEXT NOT NULL,
                        'Part' TEXT NOT NULL,
	                    'Definition' TEXT NOT NULL
                        );";
            SQLiteCommand command = new SQLiteCommand(query, myConnection);
            command.ExecuteNonQuery();

        }

        public void SaveWord(WordModel word)
        {
            string query = @"INSERT INTO Words ('Id', 'Word', 'Part', 'Definition')
                            VALUES (@id, @word, @part, @definition);";

            SQLiteCommand command = new SQLiteCommand(query, myConnection);
            command.Parameters.AddWithValue("@id", word.Id);
            command.Parameters.AddWithValue("@word", word.Word);
            command.Parameters.AddWithValue("@part", word.Part.ToString());
            command.Parameters.AddWithValue("@definition", word.Definition);
            command.ExecuteNonQuery();
        }

        public void RemoveWord(WordModel word)
        {
            string query = @"DELETE FROM Words WHERE Id=@id";
            SQLiteCommand command = new SQLiteCommand(query, myConnection);
            command.Parameters.AddWithValue("@id", word.Id);
            command.ExecuteNonQuery();
        }

        // load all words that are in database 
        public ObservableCollection<WordModel> LoadWords()
        {
            ObservableCollection<WordModel> list = new ObservableCollection<WordModel>();
            string query = "SELECT * FROM Words";
            SQLiteCommand command = new SQLiteCommand(query, myConnection);
            using (SQLiteDataReader data = command.ExecuteReader())
            {
                WordModel Word;
                while (data.Read())
                {
                    int id = data.GetInt32(0);
                    string word = data.GetString(1);
                    PartSpeech part;
                    Enum.TryParse<PartSpeech>(data.GetString(2), out part);
                    string definition = data.GetString(3);

                    Word = new WordModel(id, word, definition, part);

                    list.Add(Word);
                }
            }

            return list;
        }

        private void openConnection()
        {
            if (myConnection.State != System.Data.ConnectionState.Open)
            {
                myConnection.Open();
            }
        }

        private void closeConnection()
        {
            if (myConnection.State != System.Data.ConnectionState.Closed)
            {
                myConnection.Close();
            }
        }
    }
}