using Math_Solver.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Math_Solver.DAO
{
    public class DatabaseAccess
    {
        static string folder = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public bool CreateFavoriteDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "favorites.db")))
                {
                    connection.CreateTable<Favorites>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                return false;
                throw new Exception("ERRO AO CRIAR BANCO DE DADOS - FAVORITOS");
            }
        }

        public bool CreateRecentDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "recents.db")))
                {
                    connection.CreateTable<Recents>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                return false;
                throw new Exception("ERRO AO CRIAR BANCO DE DADOS - RECENTES");
            }
        }

        public bool TableFavoritesExists()
        {
            string path = Path.Combine(folder, "favorites.db");
            if (File.Exists(path))
                return true;
            else
                return false;
        }

        public void InsertFavorites(int formulaId, int favorite)
        {
            try
            {
                var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "favorites.db"));
                connection.Query<Favorites>("INSERT INTO Favorites" +
                    "(FormulaId, IsFavorited) " +
                    "VALUES (?, ?)", formulaId, favorite);

            }
            catch (SQLiteException ex)
            {
                throw new Exception("ERRO AO INSERIR FAVORITOS NO BANCO DE DADOS");
            }
        }

        public string GetFavorite(int formulaId)
        {
            try
            {
                var connection = new SQLiteConnection(Path.Combine(folder, "favorites.db")); 
                string result = connection.Query<Favorites>("SELECT IsFavorited FROM Favorites" +
                    "WHERE FormulaId = ?", formulaId).ToString();
                return result;
            }
            catch (SQLiteException ex)
            {
                return null;
                throw new Exception("ERRO AO RECEBER FAVORITOS DO BANCO DE DADOS");
            }
        }

        public bool UpdateFavorite(Favorites favorite)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "favorites.db")))
                {
                    connection.Query<Favorites>("UPDATE Favorites set IsFavorited = ? WHERE FormulaID = ?", favorite.IsFavorited, favorite.FormulaId);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                return false;
                throw new Exception("ERRO AO ATUALIZAR FAVORITOS DO BANCO DE DADOS");
            }
        }

        public List<Favorites> GetFavorites()
        {
            try
            {
                var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "favorites.db"));
                
                List<Favorites> result = connection.Query<Favorites>("SELECT * FROM Favorites WHERE IsFavorited = 1");
                if (result == null)
                    return new List<Favorites>();
                else
                    return result;
            }
            catch (SQLiteException ex)
            {
                return null;
                throw new Exception("ERRO AO RECEBER FAVORITOS DO BANCO DE DADOS");
            }
        }


        public bool TableRecentsExists()
        {
            string path = Path.Combine(folder, "recents.db");
            if (File.Exists(path))
                return true;
            else
                return false;
        }

        public void InsertRecents(int formulaId, int recent)
        {
            try
            {
                var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "recents.db"));
                connection.Query<Recents>("INSERT INTO Recents" +
                    "(FormulaId, IsRecent, DateRecent) " +
                    "VALUES (?, ?, CURRENT_TIMESTAMP)", formulaId, recent);

            }
            catch (SQLiteException ex)
            {
                throw new Exception("ERRO AO INSERIR RECENTES NO BANCO DE DADOS");
            }
        }

        public string GetRecent(int formulaId)
        {
            try
            {
                var connection = new SQLiteConnection(Path.Combine(folder, "recents.db"));
                string result = connection.Query<Recents>("SELECT IsRecent FROM Recents" +
                    "WHERE FormulaId = ?", formulaId).ToString();
                return result;
            }
            catch (SQLiteException ex)
            {
                return null;
                throw new Exception("ERRO AO RECEBER RECENTES DO BANCO DE DADOS");
            }
        }

        public List<Recents> UpdateRecent(Recents recent)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "recents.db")))
                {
                    List<Recents> recentList = connection.Query<Recents>("UPDATE Recents set IsRecent = ?, DateRecent = CURRENT_TIMESTAMP WHERE FormulaID = ?", recent.IsRecent, recent.FormulaId);
                    return recentList;
                }
            }
            catch (SQLiteException ex)
            {
                return null;
                throw new Exception("ERRO AO ATUALIZAR RECENTES DO BANCO DE DADOS");
            }
        }

        public List<Recents> GetRecents()
        {
            try
            {
                var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "recents.db"));

                List<Recents> result = connection.Query<Recents>("SELECT * FROM Recents WHERE IsRecent = 1 ORDER BY DateRecent DESC");
                if (result == null)
                    return new List<Recents>();
                else
                    return result;
            }
            catch (SQLiteException ex)
            {
                return null;
                throw new Exception("ERRO AO RECEBER RECENTES DO BANCO DE DADOS");
            }
        }

        //TODO: Verificar
        public bool LimitRecentsTable(List<Recents> recentList)
        {
            try
            {
                int formulaID = recentList.Last().FormulaId;
                var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "recents.db"));
                connection.Query<Recents>("UPDATE Recents set IsRecent = ? WHERE FormulaID = ?", 0, formulaID);
                return true;
            }
            catch (SQLiteException ex)
            {
                return false;
                throw new Exception("ERRO AO LIMITAR RECENTES DO BANCO DE DADOS");
            }

            /*
            try
            {
                var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "recents.db"));
                var cmd = new SQLiteCommand(connection);
                
                cmd.CommandText = "DELETE FROM Clientes Where Id=@Id";
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            */
        }
    }
}
