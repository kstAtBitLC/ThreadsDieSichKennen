using System;
using System.Threading;

/* Einbrecher-Simulation
 Zwei Einbrecher (Personen) lernen sich durch Kommunikation kennen.
 und rauben kontinuierlich ein Behältnis aus. Dabei entnehmen sie 10 Geldeinheiten
 und zahlen diesen auf ihr eigenes Konto ein, während der Kontostand des Behältnisses
 abnimmt. Sollte das Behältnis leer sein,
 warten beide geduldig bis wieder etwas zu holen ist. Sollte einer der beiden Einbrecher
 mehr als 200 Geldeinheiten auf dem Konto haben, spricht dieser den anderen an, um 
 miteinander zu feiern. Dabei reduziert sich der Kontoinhalt um 200 Geldeinheiten ...
     */
namespace ThreadsDieSichKennen {
    class Program {
        static void Main ( string [] args ) {

            Console.WriteLine ( "Einbrecher-Impression ..." );
            Console.WriteLine ( "=========================" );

            Behältnis behältnis = new Behältnis () { BehältnisID = 1, Inhalt = 500 };
            Person person1 = new Person () { ID = 1, Name = "Hugo", AnderePerson = null, Inhalt = 0, Behältnis = behältnis };
            Person person2 = new Person () { ID = 2, Name = "Boss", AnderePerson = person1, Inhalt = 0, Behältnis = behältnis };

            Thread b1 = new Thread ( new ThreadStart ( behältnis.Anfangen ) );
            Thread p1 = new Thread ( new ThreadStart ( person1.Einbrechen ) );
            Thread p2 = new Thread ( new ThreadStart ( person2.Einbrechen ) );

            b1.Start ();
            p1.Start ();
            p2.Start ();

        }
    }

    class Person {
        public int Inhalt { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public Person AnderePerson { get; set; }
        public Behältnis Behältnis { get; set; }
        public bool HatEinenKumpel { get; set; } = false;

        public void Einbrechen () {
            while ( true ) {
                Thread.Sleep ( 1000 );

                try {
                    if ( this.AnderePerson != null && !this.HatEinenKumpel ) {

                        Console.WriteLine ( "Schön dich kennen zu lernen ... " );
                        Console.WriteLine ( "Ich bin {0}", this.Name );

                        this.AnderePerson.AnderePerson = this;
                        Console.WriteLine ( "Du bist {0}", this.AnderePerson.Name );
                        this.HatEinenKumpel = true;
                    }
                }
                catch ( Exception ) {
                    Console.WriteLine ( "Ich kenne Dich nicht" );
                }

                if ( Behältnis.Inhalt >= 10 ) {
                    Behältnis.Inhalt -= 10;
                    this.Inhalt += 10;
                }
                else {
                    Thread.Sleep ( 5000 );
                    Behältnis.Inhalt = 300;
                }


                if ( this.Inhalt > 200 ) {
                    Console.WriteLine ( "Einbrecher {0} sagt zu Kumpel {1} Komm lass uns feiern", this.Name, this.AnderePerson.Name );
                    this.Inhalt -= 200;
                }


                Console.WriteLine ( "Name: {0} Inhalt: {1}  InhaltBehältnis: {2}", this.Name, this.Inhalt, this.Behältnis.Inhalt );

            }

        }
    }

    class Behältnis {
        public int BehältnisID { get; set; }
        public int Inhalt { get; set; } = 4711;

        public void Anfangen () {
            while ( true ) {
                Thread.Sleep ( 1000 );
                Console.WriteLine ( "Behältnis - Info: {0}", this.Inhalt );
            }
        }
    }
}
