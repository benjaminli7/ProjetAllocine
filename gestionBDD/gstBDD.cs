using System;
using MySql.Data.MySqlClient;
using Entity;
using System.Collections.Generic;

namespace gestionBDD
{
    public class gstBDD
    {
        private MySqlConnection conn;
        private MySqlCommand cmd;
        private MySqlDataReader dr;

        public gstBDD()
        {
            string driver = "server=localhost; user id=root; password=;database=cine";
            conn = new MySqlConnection(driver);
            conn.Open();
        }

        public List<Cinema> getListCinema()
        {
            List<Cinema> lstCinema = new List<Cinema>();
            cmd = new MySqlCommand("select * from cinema", conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Cinema c = new Cinema()
                {
           
                    codeCine = dr[0].ToString(),
                    nomCine = dr[1].ToString(),
                    adresseCine = dr[2].ToString(),
                    codePostalCine = Convert.ToInt32(dr[3]),
                    villeCine = dr[4].ToString(),
                    latitudeCine = Convert.ToSingle((dr[5])),
                    longitudeCine = Convert.ToSingle(dr[6]),
                    
                    imageCine = dr[7].ToString()
                };
                lstCinema.Add(c);
            }
            dr.Close();
            return lstCinema;
        }

        public List<Film> getListFilmsByIdCinema(string codeCinema)
        {
            List<string> lstStr = new List<string>();
            List<Film> lstFilms = new List<Film>();
            cmd = new MySqlCommand("select * from cinema inner join projeter on cinema.codeCine = projeter.numCinema where codeCine = '" + codeCinema + "'", conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string s = dr[9].ToString();
                lstStr.Add(s);
            }
            dr.Close();

            foreach (string s in lstStr)
            {
                cmd = new MySqlCommand("select * from film where codeFilm = '" + s + "'", conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                   Film f = new Film()
                    {
                        codeFilm = dr[0].ToString(),
                        nomFilm = dr[1].ToString(),
                        imageFilm = dr[2].ToString(),
                        genreFilm = dr[3].ToString(),
                        nbVotes = Convert.ToInt32(dr[4]),
                        totalVotes = Convert.ToInt32(dr[5])
                    };
                    lstFilms.Add(f);
                }
                dr.Close();
            }
            return lstFilms;
        }

        public List<Acteur> getListActeurByFilm(string numFilm)
        {
            List<Acteur> lstActeur = new List<Acteur>();
            cmd = new MySqlCommand("select codeActeur, nomActeur, imageActeur from film inner join jouer on codeFilm = numFilm inner join acteur on numActeur = codeActeur where codeFilm = '" + numFilm + "'", conn);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Acteur a = new Acteur()
                {
                    codeActeur = Convert.ToInt32(dr[0]),
                    nomActeur = dr[1].ToString(),
                    imageActeur = dr[2].ToString()
                };
                lstActeur.Add(a);
            }
            dr.Close();
            return lstActeur;
        }

        public void updateVoteByFilm(string numFilm, float sldValue)
        {
            cmd = new MySqlCommand("update film SET nbVotes = nbVotes + 1, totalVotes = totalVotes + " + sldValue + " where codeFilm = '" + numFilm + "'", conn);
            cmd.ExecuteNonQuery();
        }

        public int getNbrVote(string numFilm)
        {
            int nbrVote;
            cmd = new MySqlCommand("select nbVotes from film where codeFilm = '" + numFilm + "'", conn);
            dr = cmd.ExecuteReader();
            dr.Read();
            nbrVote = Convert.ToInt32(dr[0]);
            dr.Close();
            return nbrVote;
        }

        public float getTotalVote(string numFilm) {
            float totalVote;
            cmd = new MySqlCommand("select totalVotes from film where codeFilm = '" + numFilm + "'", conn);
            dr = cmd.ExecuteReader();
            dr.Read();
            totalVote = Convert.ToSingle(dr[0]);
            dr.Close();
            return totalVote;
        }
    }
}
