using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Esame1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Pubblicazione> pubblicazioni;
            string? risposta = "y";
            int n;

            if (LeggiInStore() != null)
                pubblicazioni = LeggiInStore();
            else
                pubblicazioni = new List<Pubblicazione>();

            foreach (Pubblicazione p in pubblicazioni)
                Console.WriteLine(p);

            while (risposta.Equals("y")) {
                Console.WriteLine("Benvenuto nell'edicola di Enrico!");
                Console.WriteLine("hai una pubblicazione da inserire nel SO y/n");
                risposta = Console.ReadLine();

                if (risposta.Equals("y"))
                {
                    Pubblicazione p = CreatePubblicazione();
                    bool add = true;
                    int indice=0;
                    n = 0;

                    //non possono esserci 2 o più quotidiani con lo stesso nome 
                    //il sistema blocca l'inserimento 
                    while ((n < pubblicazioni.Count) && (add))
                    {
                        if ((pubblicazioni[n].Nome.Equals(p.Nome)) && (pubblicazioni[n].DataUscita.Equals(p.DataUscita)))
                            add = false;
                        indice = n;
                        n++;
                    }
                        
                    if (add)
                    {
                        pubblicazioni.Add(p);
                        AggiungiAlloStore(p);
                    }
                    else
                    {
                        Console.WriteLine("hai già inserito nel sistema questa pubblicazione. Vuoi incrementare lo store? y/n");
                        risposta = Console.ReadLine();

                        if (risposta.Equals("y"))
                        {
                            Console.WriteLine("Di quanto vuoi incrementare?");
                            n = int.Parse(Console.ReadLine());

                            pubblicazioni[indice].NStore = pubblicazioni[indice].NStore + n;
                            AggiornaFilePubblicazioni(pubblicazioni);
                        }

                    }
                }

                Console.WriteLine("hai una pubblicazione da rimuovere nel SO y/n");
                risposta = Console.ReadLine();

                if ((risposta.Equals("y"))&&(pubblicazioni.Count> 0))
                {
                    //qui assumo che per individuare univocamente una pubblicazione ho bisogno della data e del nme
                    Pubblicazione p = RicercaPerNomeDate(pubblicazioni);
                    if(p!= null)
                    {
                        pubblicazioni.Remove(p);
                        AggiornaFilePubblicazioni(pubblicazioni);
                    }
                        
                    else
                        Console.WriteLine("l'elemrno non c'è ERRORE");
                }

                Console.WriteLine("Abbiamo venduto delle pubblicazioni? (ABBONTI O NON ABBONATI) y/n");
                risposta = Console.ReadLine();

                if ((risposta.Equals("y")) && (pubblicazioni.Count > 0))
                {
                    //qui assumo che per individuare univocamente una pubblicazione ho bisogno della data e del nme
                    Pubblicazione p = RicercaPerNomeDate(pubblicazioni);
                    if (p != null)
                    {
                        Console.WriteLine("Quanti ne abbiamo venduti?");
                        n = int.Parse(Console.ReadLine());
                        if(p.NStore - n >= 0)
                        {
                            p.NStore = p.NStore - n;
                            MemorizzaVenditaInStore(p);
                            AggiornaFilePubblicazioni(pubblicazioni);
                        }
                        else
                            Console.WriteLine("ERRORE");
                    }
                    else
                        Console.WriteLine("l'elemrno non c'è ERRORE");
                }

                Console.WriteLine("Continuare? y/n");
                risposta = Console.ReadLine();
            }

            Console.WriteLine("ecco l'elenco delle pubblicazioni");
            foreach (Pubblicazione p in pubblicazioni)
                Console.WriteLine(p);

            Console.WriteLine("Vuoi visualizzare le vendite? y/n");
            risposta = Console.ReadLine();

            if (risposta.Equals("y"))
                LeggiVendite();

            
        }

        public static void LeggiVendite()
        {
            string path = "C:\\Users\\Utente\\Desktop\\Vendite.txt";
            using (StreamReader sr = new StreamReader(path))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                    Console.WriteLine(line);
            }
        }

        public static void AggiornaFilePubblicazioni(List<Pubblicazione> lista)
        {
            string path = "C:\\Users\\Utente\\Desktop\\Pubblicazioni.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (Pubblicazione p in lista)
                    sw.WriteLine(p.CSVString());

                Console.WriteLine("Tutto ok!");
            }
        }

        public static bool MemorizzaVenditaInStore(Pubblicazione p)
        {
            bool risultato = false;
            string risposta;
            string path = "C:\\Users\\Utente\\Desktop\\Vendite.txt";
            string dataStringa;
            Vendita v = new Vendita();
            v.Pubblicazione = p;
            v.Abbonato = null;
            v.DataConsegna = DateTime.Today;

            Console.WriteLine("Il cliente è iun abbonato? (y/n)");
            risposta = Console.ReadLine();
            if (risposta.Equals("y"))
            {
                bool flag = true;
                Abbonato abb = new Abbonato();
                Indirizzo ind = new Indirizzo();
                Console.WriteLine("dammi il nome");
                abb.Nome = Console.ReadLine();
                Console.WriteLine("dammi il cognome");
                abb.Cognome = Console.ReadLine();
                Console.WriteLine("dammi il tipo abbonamento (mensile/annulae)");
                abb.TipoAbbonamento = Console.ReadLine();
                Console.WriteLine("dammi il metodo pagamento (Bancomat/poste/contanti ecc..)");
                abb.MetodoPagamento = Console.ReadLine();
                Console.WriteLine("dammi il nome della strada");
                ind.Via = Console.ReadLine();
                Console.WriteLine("dammi il civico");
                ind.Civico = Console.ReadLine();
                Console.WriteLine("dammi il CAP");
                ind.Cap = Console.ReadLine();
                abb.Indirizzo = ind;
                Console.WriteLine("In che data è prevista la spedizione?");
                Console.WriteLine("inserisci la data nel seguente formato: dd/MM/yyyy");

                while(flag)
                    if ("dd/MM/yyyy".Length == (dataStringa = Console.ReadLine()).Length)
                    {
                        if (DateTime.Compare(DateTime.ParseExact(dataStringa, "dd/MM/yyyy", CultureInfo.InvariantCulture), v.DataConsegna) > 0)
                        {
                            v.DataConsegna = DateTime.ParseExact(dataStringa, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            v.Abbonato = abb;
                            flag = false;
                        }
                        else
                            Console.WriteLine("ERRORE inserire nuovamente la data");
                    }
            }

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(v.CSVString());
                    risultato = true;
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(v.CSVString());
                    risultato = true;
                }
            }

            return (risultato);
        }

        public static List<Pubblicazione> LeggiInStore()
        {
            string path = "C:\\Users\\Utente\\Desktop\\Pubblicazioni.txt";
            List<Pubblicazione> listaRisultato = new List<Pubblicazione>();
            string[] array = new string[4];

            if (!File.Exists(path))
                return (null);
       
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    array = s.Split(',');
                    if (array[2].Equals("giornale"))
                        listaRisultato.Add(new Giornale(array[0], DateTime.ParseExact(array[1], "dd/MM/yyyy", CultureInfo.InvariantCulture), array[2], int.Parse(array[3])));
                
                    else 
                        listaRisultato.Add(new Rivista(array[0], DateTime.ParseExact(array[1], "dd/MM/yyyy", CultureInfo.InvariantCulture), array[2], int.Parse(array[3])));
                }
            }

            return(listaRisultato);
        }

        public static bool AggiungiAlloStore(Pubblicazione p)
        {
            bool risultato = false;
            string path = "C:\\Users\\Utente\\Desktop\\Pubblicazioni.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(p.CSVString());
                    risultato = true;
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(p.CSVString());
                    risultato = true;
                }
            }

            return (risultato);
        }

        public static Pubblicazione RicercaPerNomeDate(List<Pubblicazione> lista)
        {
            string dataStringa;
            DateTime data = DateTime.Today;
            bool val = true;
            int i = 0;

            while (val) 
            {
                Console.WriteLine("inserisci la data di uscita nel seguente formato: dd/MM/yyyy");
                if ("dd/MM/yyyy".Length == (dataStringa = Console.ReadLine()).Length)
                {
                    data = DateTime.ParseExact(dataStringa, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    val = false;
                }
              
            }

            Console.WriteLine("Inserisci nome");
            dataStringa = Console.ReadLine();
            val = true;

            while ((val) && (i < lista.Count))
            { 
                if((lista[i].Nome.Equals(dataStringa)) && (DateTime.Compare(lista[i].DataUscita, data) == 0))
                    val = false;
                i++;
            }

            if (!val)
                return (lista[--i]);
            return (null);
        }

        public static Pubblicazione CreatePubblicazione()
        {
            string nome;
            DateTime dataUscita;
            string categorieStringa = "giornale, riviste moda, riviste tecnologia, rivista medicina, riviste mediche";
            string categoria;
            int nStore;

            Console.WriteLine("Inserisci nome");
            nome = Console.ReadLine();
            dataUscita = DateTime.Today;
            bool val;

            do
            {
                Console.WriteLine("Inserisci la categoria tra " + categorieStringa);
                categoria = Console.ReadLine();
                if (!categorieStringa.ToUpper().Contains(categoria.ToUpper()))
                {
                    val = true;
                    Console.WriteLine("si prega di scegliere una delle categorie elencate");
                }
                else
                    val = false;
                    
            } while (val);

            Console.WriteLine("Inserisci il numero di volumi acquistati da inserire in magazzino");
            nStore = int.Parse(Console.ReadLine());

            if(categoria.Equals("giornale"))
                return (new Giornale(nome, dataUscita, categoria, nStore));
            return (new Rivista(nome, dataUscita, categoria, nStore));

        }
    }
}
