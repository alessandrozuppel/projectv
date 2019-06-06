AGGIUNTE:

Creato file DataReader.
Creato file DataSender.
Creata API per invio dati.
Creato database con INFLUX.

Funzionamento: 

1.	DataReader: 
•	Program.cs  si apre un client redis in locale che memorizza i dati inviati dai sensori, si inizializza la variabile “data” che richiama la funzione dei sensori ToJson, in VirtualSensor.cs.
In VirtualSensor.cs sono presenti funzioni per ogni tipo di dato, le quali li generano e li formattano in una stringa Json. 
Tale stringa quindi viene restituita in program.cs diventando valore di “data”.

Questo data viene inviato in una coda di messaggi  grazie alla funzione LPush, contenuta nella libreria CSRedis di C#.

Dopodichè DataReader si interrompe per 10 secondi.

2.	DataSender: 
Il DataSender recupera, con la funzione BLPop, un elemento della coda in Redis. 
Con la libreria HttpWebRequest si genera una richiesta di tipo POST all’indirizzo ip dell’API esterna.
L’API esterna poi si occuperà di salvare i dati letti sul database: InfluxDB. 
Influx permette di memorizzare moltissimi dati e accedervi in fretta, attraverso esso si possono formulare query grazie alle quali si visualizzeranno le tabelle create al momento. 

MODIFICHE: 

[22-05-2019]
Cambiata in DataSender la libreria HttpWebRequest con la libreria system.net.
Usato metodo webclient, per la risoluzione di un problema sorto nell'invio dati all'API.

[06-06-2019]
Implementate nuovi funzioni random per rendere più verosimili i dati delle tratte del bus. 
