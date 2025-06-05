/*
using System.Data;
using System.Data.SQLite;
using UnityEngine;

namespace Model
{
    public class DatabaseManager : MonoBehaviour
    {
        private string dbPath;
        private IDbConnection dbConnection;

        private void Awake()
        {
            string dbName = "DungeonAdventure.db";
            dbPath = "URI=file:" + System.IO.Path.Combine(Application.persistentDataPath, dbName);

            dbConnection = new SqliteConnection(dbPath);
            dbConnection.Open();

            CreateCharacterTable();
        }

        private void CreateCharacterTable()
        {
            IDbCommand command = dbConnection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Characters (
                    Name TEXT PRIMARY KEY,
                    Type TEXT NOT NULL,
                    HitPoints INTEGER,
                    DamageMin INTEGER,
                    DamageMax INTEGER,
                    AttackSpeed INTEGER,
                    MoveSpeed INTEGER,
                    ChanceToCrit REAL,
                    Level INTEGER,
                    ExperiencePoints INTEGER
                )";
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void SaveCharacter(DungeonCharacter character)
        {
            string characterType = character is Knight ? "Knight" : "Unknown";
            int level = 0, experiencePoints = 0;

            if (character is Hero hero)
            {
                level = hero.Level;
                experiencePoints = hero.ExperiencePoints;
            }

            IDbCommand command = dbConnection.CreateCommand();
            command.CommandText = @"
                INSERT OR REPLACE INTO Characters (Name, Type, HitPoints, DamageMin, DamageMax, 
                    AttackSpeed, MoveSpeed, ChanceToCrit, Level, ExperiencePoints)
                VALUES (@name, @type, @hitPoints, @damageMin, @damageMax, 
                    @attackSpeed, @moveSpeed, @chanceToCrit, @level, @experiencePoints)";
            
            command.Parameters.Add(new SqliteParameter("@name", character.Name));
            command.Parameters.Add(new SqliteParameter("@type", characterType));
            command.Parameters.Add(new SqliteParameter("@hitPoints", character.HitPoints));
            command.Parameters.Add(new SqliteParameter("@damageMin", character.DamageMin));
            command.Parameters.Add(new SqliteParameter("@damageMax", character.DamageMax));
            command.Parameters.Add(new SqliteParameter("@attackSpeed", character.AttackSpeed));
            command.Parameters.Add(new SqliteParameter("@moveSpeed", character.MoveSpeed));
            command.Parameters.Add(new SqliteParameter("@chanceToCrit", character.ChanceToCrit));
            command.Parameters.Add(new SqliteParameter("@level", level));
            command.Parameters.Add(new SqliteParameter("@experiencePoints", experiencePoints));
            
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public DungeonCharacter LoadCharacter(string name)
        {
            IDbCommand command = dbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM Characters WHERE Name = @name";
            command.Parameters.Add(new SqliteParameter("@name", name));
            
            IDataReader reader = command.ExecuteReader();
            DungeonCharacter character = null;

            if (reader.Read())
            {
                string type = reader.GetString(1);
                if (type == "Knight")
                {
                    character = new Knight(reader.GetString(0))
                    {
                        HitPoints = reader.GetInt32(2),
                        Level = reader.GetInt32(8),
                        ExperiencePoints = reader.GetInt32(9)
                    };
                }
            }

            reader.Close();
            command.Dispose();
            return character;
        }

        private void OnDestroy()
        {
            dbConnection.Close();
        }
    }
}
*/