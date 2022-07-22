using System;
using System.Collections.Generic;
using System.IO;
using AsciiArt;

namespace jeu_pendu
{
    class Program
    {
        static void AfficherMot(string mot, List<char> lettres)
        {
            for (int i = 0; i< mot.Length; i++)
            {
                char lettre = mot[i];
                if (lettres.Contains(lettre))
                {
                    Console.Write(lettre + " ");
                }
                else
                {
                    Console.Write("_ ");
                }
               
            }
            Console.WriteLine();
        }
        static bool TouteslesLettresDevinees(string mot, List<char> lettres)
        { 
        //true toutes les lettres ont été trouvé 
            foreach(var lettre in lettres) 
            {
                mot = mot.Replace(lettre.ToString(), "");
            }
            if(mot.Length == 0)
            {
                return true;
            }
            return false;
        }
        static char DemanderUneLettre(string message = "Rentrez une lettre : ")
        {
            while(true)
            {
                Console.Write(message);
                string reponse = Console.ReadLine();
                if (reponse.Length == 1)
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }
                Console.WriteLine(" ERREUR , vous devez rentrer une lettre");
            }
        }

        static void DevinerMot(string mot)
        {
            var LettresDeviner = new List<char>();
            var LettresExclues = new List<char>();
            const int NB_VIES = 6;
            int viesRestantes = NB_VIES;

            while (viesRestantes > 0)
            {
                Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);
                Console.WriteLine();
                AfficherMot(mot, LettresDeviner);
                Console.WriteLine();
                var lettre = DemanderUneLettre();
                Console.Clear();
                if (mot.Contains(lettre))
                {
                    Console.WriteLine("Cette lettre est dans le mot");
                    LettresDeviner.Add(lettre);
                    if(TouteslesLettresDevinees(mot, LettresDeviner))
                    {
                        //Console.WriteLine("GAGNE !!");
                        //return;
                        break;
                    }
                }
                else
                {
                  if (!LettresExclues.Contains(lettre))
                    {
                       viesRestantes--;
                       LettresExclues.Add(lettre);
                    }
                   
                    Console.WriteLine("vie restantes : " + viesRestantes );
                }
                if (LettresExclues.Count > 0)
                {
                    Console.WriteLine("le mot ne contient pas lettres : " + String.Join(",", LettresExclues));
                }
                Console.WriteLine();
               
            }
            Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);
            if (viesRestantes == 0)
            {
                Console.WriteLine("PERDU !!! le mot était : " + mot);
            }
            else
            {
                AfficherMot(mot, LettresDeviner);
                Console.WriteLine();
                Console.WriteLine("GAGNE");
            }
        }
        static string[]Chargerlesmots(string nomdufichier)
        {
            try { 
                return File.ReadAllLines(nomdufichier);
            }catch(Exception ex)
            {
                Console.WriteLine("erreur du lecture du fichier : " + nomdufichier + "(" + ex.Message +")");
            }return null;
        }

        static bool demanderDeRejouer()
        {
            
            char reponse = DemanderUneLettre("voulez-vous rejouer ? (o/n) : ");
            if ((reponse=='o')||(reponse == 'O'))
            {
                return true;
            }
            else if ((reponse == 'n') || (reponse == 'N'))
            {
                return false;
            }
            else
            {
                Console.WriteLine("Erreur : vous devez repondre avec O ou N");
                return demanderDeRejouer();
            }
        }
        static void Main(string[] args)
        {
            var mots = Chargerlesmots("mots.txt");
            if ((mots == null) || (mots.Length == 0))
            {
                Console.WriteLine("la liste de mots est vide");
            }
            else
            {
                while (true)
                {
                    Random r = new Random();
                    int i = r.Next(mots.Length);
                    string mot = mots[i].Trim().ToUpper();
                    DevinerMot(mot);
                    if (!demanderDeRejouer())
                    {
                        break;
                    }
                    Console.Clear();
                }
                Console.WriteLine("Merci et à bientôt");
              
                
                
            }
        }
    }
}
