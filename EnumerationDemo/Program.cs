using System;
using System.Collections.Generic;

namespace EnumerationDemo
{
    class Program
    {
        public enum Couleurs
        {
            Coeur,//rouge
            Carreau,//rouge
            Pique,//noire
            Trefle//noire
        }
        public enum Valeurs
        {
            Deux = 2,
            Trois,
            Quatre,
            Cinq,
            Six,
            Sept,
            Huit,
            Neuf,
            Dix,
            Valais,
            Reine,
            Roi,
            As//14
        }
        #region Théorie
        //public enum Right 
        //{ 
        //    Execute = 1,
        //    Write = 2,
        //    Read = 4,
        //}
        //public enum TypeCarburant { Essence = 2, Diesel = 4, Gaz = 3 }
        #endregion
        static void Main(string[] args)
        {
            #region Théorie
            /*
             * Les énumerations permettent de développer une structure complexe de données (valeur)
             * contenant des données constantes liés logiquement les uns aux autres
             * Syntaxe :
             * public enum TypeCarburant { Essence = 2, Diesel = 4, Gaz = 3 }.**/
            //TypeCarburant carburant = TypeCarburant.Essence;
            //Console.WriteLine(carburant.ToString());
            //Console.WriteLine((int)carburant);

            //Right right = Right.Execute | Right.Read; //(int)Right = 5. Cette valeur n'existe pas dans l'énum, il faut donc utiliser [Flags]
            //Couleurs c =  Couleurs.Coeur;
            //Console.WriteLine(c.ToString());//affiche Coeur
            //Console.WriteLine((int)c);//affiche 2
            //Valeurs v = Valeurs.As;
            //Console.WriteLine((int)v);//affiche 14
            #endregion
            //Console.WriteLine("-- Version Non Optimisée --");
            #region Version_N_Optimise
            const int NB_CARTE = 52;
            Carte[] cartes = new Carte[NB_CARTE];
            int count = 2;
            for (int carte = 0; carte < NB_CARTE; carte++)
            {
                if (carte <= 12)
                {
                    cartes[carte].couleurs = Couleurs.Coeur;
                    cartes[carte].valeurs = (Valeurs)count;
                    count += 1;
                }
                else if (carte <= 25)
                {
                    if (count > (int)Valeurs.As)
                    {
                        count = 2;
                    }
                    cartes[carte].couleurs = Couleurs.Carreau;
                    cartes[carte].valeurs = (Valeurs)count;
                    count += 1;
                }
                else if (carte <= 38)
                {
                    if (count > (int)Valeurs.As)
                    {
                        count = 2;
                    }
                    cartes[carte].couleurs = Couleurs.Pique;
                    cartes[carte].valeurs = (Valeurs)count;
                    count += 1;
                }
                else
                {
                    if (count > (int)Valeurs.As)
                    {
                        count = 2;
                    }
                    cartes[carte].couleurs = Couleurs.Trefle;
                    cartes[carte].valeurs = (Valeurs)count;
                    count += 1;
                }
            }
            //for (int i = 0; i < NB_CARTE; i++)
            //{
            //    Console.WriteLine($"{cartes[i].couleurs} - {cartes[i].valeurs}");
            //}
            #endregion
            //Console.WriteLine("-- Version Optimisée --");
            #region VERSION_Optimise
            Carte[] jeu = new Carte[52];

            for (int i = 0, couleur = 0, valeur = 2; couleur < 4; valeur++, i++)
            {
                jeu[i].couleurs = (Couleurs)couleur;
                jeu[i].valeurs = (Valeurs)valeur;
                
                if (valeur == 14)
                {
                    valeur = 1;// Car on incrémente valeur de 1 à l'entrée de la boucle
                    couleur++;
                }
            }
            //foreach (Carte carte in jeu)
            //{
            //    Console.WriteLine($"{carte.couleurs} - {carte.valeurs}");
            //}
            #endregion
            #region MelangerCarte
            const int repete = 1000;
            const int LOWER_BOUND = 0;
            const int HIGHER_BOUND = 52;
            
            Random rnd = new Random();            

            for (int i = 0; i < repete; i++)
            {
                Carte temp;
                int rdnIndexOne = rnd.Next(LOWER_BOUND, HIGHER_BOUND);
                int rdnIndexTwo = rnd.Next(LOWER_BOUND, HIGHER_BOUND);

                temp = jeu[rdnIndexOne];
                jeu[rdnIndexOne] = jeu[rdnIndexTwo];
                jeu[rdnIndexTwo] = temp;
            }
            
            Queue<Carte> q1 = new Queue<Carte>();
            Queue<Carte> q2 = new Queue<Carte>(); // CTRL D = duppliquer la ligne courrante

            for (int i = 0; i < jeu.Length; i++)
            {
                if (i <= 25)
                {
                    q1.Enqueue(jeu[i]);
                }
                else 
                {
                    q2.Enqueue(jeu[i]);
                }
            }
            //affichage mélange
            //foreach (Carte carte in jeu)
            //{
            //    Console.WriteLine($"{carte.couleurs} - {carte.valeurs}");
            //}

            bool winGame = false;
            bool winFight = false;
            string? msg = null;
            
            do
            {
                Carte temp;
                if (q1.Count == 0 || q2.Count == 0)
                {
                    msg = q1.Count == 0 ? "Joueur 2 a gagné" : "Joueur 1 a gagné";
                    winGame = true; 
                }
                else if ((int)q1.Peek().valeurs > (int)q2.Peek().valeurs)//Scénario : J1 gagne
                {
                    q1.Enqueue(q2.Dequeue());
                    temp = q1.Dequeue();// permet de placer la carte "gagnante" à la fin du paquet de J1
                    q1.Enqueue(temp);// raison : sinon J1 sortira constamment cette carte tant qu'il ne perd pas
                }
                else if ((int)q1.Peek().valeurs < (int)q2.Peek().valeurs)//Scénario : J2 gagne
                {
                    q2.Enqueue(q1.Dequeue());
                    temp = q2.Dequeue();// permet de placer la carte "gagnante" à la fin du paquet de J2
                    q2.Enqueue(temp);// raison : idem que ligne 182
                }
                else //Scénario : Bataille/Équalité
                {
                    Stack<Carte> s1 = new Stack<Carte>();
                    Stack<Carte> s2 = new Stack<Carte>();
                    do
                    {
                        s1.Push(q1.Dequeue());
                        s2.Push(q2.Dequeue());                        
                       
                        if ((int)q1.Peek().valeurs > (int)q2.Peek().valeurs)//Scénario : J1 gagne la bataille
                        {
                            q1.Enqueue(s1.Pop());
                            q1.Enqueue(s2.Pop());
                            winFight = true;
                        }
                        else if ((int)q1.Peek().valeurs < (int)q2.Peek().valeurs)//Scénario : J2 gagne la bataille
                        {
                            q2.Enqueue(s1.Pop());
                            q2.Enqueue(s2.Pop());
                            winFight = true;
                        }                      
                    } while (!winFight); // si plusieurs bataille s'enchaine de manière fortuite, on répète les instructions                          
                }
            } while (!winGame);           
            

            foreach (Carte item in q1)// affichage q1
            {
                Console.WriteLine($"Paquet de carte : J1 {item.couleurs} - {item.valeurs}");
            }

            foreach (Carte item in q2)// affichage q2
            {
                Console.WriteLine($"Paquet de carte : J2 {item.couleurs} - {item.valeurs}");
            }

            Console.WriteLine($"q1 : {q1.Count} - q2 : {q2.Count}");// 26 et 26 avant partie

            Console.WriteLine(msg);
            #endregion
        }

        public struct Carte
        {
            public Couleurs couleurs;
            public Valeurs valeurs;
        }
    }
    /*
     * Avec votre jeu de cartes,
     * Mettez en place un jeu de bataille
     * 1)Mélanger les cartes (utiliser la classe random)
     *      1.1)Inverser les cartes sur base de deux indices OK
     *      1.1)Répéter mille fois OK
     * 2)Distribuer les cartes équitablement (queue) OK
     * 3)Partie
     *      3.1) Egalité ? =>Utiliser des stacks pour les joueurs en cas d'égalité
     *          3.1.1)Egalité encore ? Répéter
     *      3.2) Ajouter les cartes dans le pot du joueur gagnant à la fin
     *      3.3) Celui qui n'a plus de carte => Perdue :(
     *  **/
}

