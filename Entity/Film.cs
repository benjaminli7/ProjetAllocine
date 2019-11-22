using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class Film
    {
        public string codeFilm { get; set; }
        public string nomFilm { get; set; }
        public string imageFilm { get; set; }
        public string genreFilm { get; set; }
        public int nbVotes { get; set; }
        public int totalVotes { get; set; }
    }
}
