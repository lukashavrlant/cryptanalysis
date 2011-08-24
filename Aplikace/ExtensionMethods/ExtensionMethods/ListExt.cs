using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
    public static class ListExt
    {
        /// <summary>
        /// Vrací tzv. ocas seznamu -- vše krom prvního prvku
        /// Pokud zavoláme funkci s prázdným seznamem, vrátí se prázdný seznam
        /// </summary>
        /// <typeparam name="T">Datový typ prvků seznamu</typeparam>
        /// <param name="list"></param>
        /// <returns>Ocas seznamu</returns>
        public static List<T> Rest<T>(this List<T> list)
        {
            if (list.Count == 0)
                return list;

            List<T> newList = new List<T>();

            for (int i = 1; i < list.Count; i++)
                newList.Add(list[i]);

            return newList;
        }


        /// <summary>
        /// Mapovací funkce - aplikuje předanou funkci na všechny prvky seznamu
        /// a tento modifikovaný seznam následně vrátí.
        /// Nezmění hodnoty v současném seznamu, metoda není destruktivní.
        /// </summary>
        /// <typeparam name="T">Typ vstupního seznamu</typeparam>
        /// <typeparam name="R">Typ výstupního seznamu</typeparam>
        /// <param name="List"></param>
        /// <param name="Action">Funkce, která má být aplikována na prvky seznamu</param>
        /// <returns>Modifikovaný seznam</returns>
        public static List<R> Mapcar<T, R>(this List<T> List, Func<T, R> Action)
        {
            List<R> NewList = new List<R>(); 

            foreach (T item in List)
                NewList.Add(Action(item));

            return NewList;
        }

        /// <summary>
        /// Mapovací funkce - aplikuje předanou funkci na všechny prvky seznamu.
        /// Metoda je destruktivní.
        /// </summary>
        /// <typeparam name="T">Typ seznamu</typeparam>
        /// <param name="List"></param>
        /// <param name="Action">Funkce, která má být aplikována na prvky seznamu</param>
        public static void MapcarDes<T>(this List<T> List, Func<T, T> Action)
        {
            for (int i = 0; i < List.Count; i++)
                List[i] = Action(List[i]);
        }

        /// <summary>
        /// Zjistí, jestli se v seznamu vyskytuje nějaký prvek vícekrát. 
        /// </summary>
        /// <typeparam name="T">Typ seznamu</typeparam>
        /// <param name="List"></param>
        /// <returns>True v případě, že seznam</returns>
        public static bool WithoutDuplicates<T>(this List<T> List)
        {
            return List.TrueForAll(item => List.Count(y => y.Equals(item)) == 1);
        }

        /// <summary>
        /// Prohází náhodně prvky v seznamu
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public static List<T> Shuffle<T>(this List<T> inputList)
        {
            List<T> randomList = new List<T>();
            if (inputList.Count == 0)
                return randomList;

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list<
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            //clean up
            inputList.Clear();
            inputList = null;
            r = null;

            return randomList; //return the new random list
        }

        public static List<T> Extract<T>(this List<T[]> list)
        {
            List<T> l = new List<T>();

            foreach (T[] item in list)
            {
                foreach (T arrayItem in item)
                {
                    l.Add(arrayItem);
                }
            }

            return l;
        }

        public static bool Find<T>(this List<T> list, T item)
        {
            return list.IndexOf(item) != -1;
        }
    }
}
