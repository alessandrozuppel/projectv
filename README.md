# projectv

1.	DataReader: 
•	Program.cs  si apre un client redis in locale che memorizza i dati inviati dai sensori, si inizializza la variabile “data” che richiama la funzione dei sensori ToJson, in VirtualSensor.cs.
In VirtualSensor.cs sono presenti funzioni per ogni tipo di dato, le quali li generano e li formattano in una stringa Json. 
Tale stringa quindi viene restituita in program.cs diventando valore di “data”.

Questo data viene inviato in una coda di messaggi  grazie alla funzione LPush, contenuta nella libreria CSRedis di C#.

Dopo di che DataReader si interrompe per 10 secondi.

2.	DataSender: 
Il DataSender recupera, con la funzione BLPop, un elemento della coda in Redis. 
Con la libreria HttpWebRequest si genera una richiesta di tipo POST all’indirizzo ip dell’API esterna.
L’API esterna poi si occuperà di salvare i dati letti sul database: InfluxDB. 
Influx permette di memorizzare moltissimi dati e accedervi in fretta, attraverso esso si possono formulare query grazie alle quali si visualizzeranno le tabelle create al momento. 


ISTRUZIONI: 
•	Eseguire il codice di Data Reader e Data Sender su Visual Studio 2019. 
•	Avviare l’Influx DB attraverso gli eseguibili della cartella “influx”. 
•	Eseguire il main.js su Visual Studio Code aprendo una chiamata al server su porta 3000, e mediante la POST inviare i dati all’influx db che li registra. 

	Bisogna installare le seguenti librerie: influx, fastify, fastifycors. 
	Programmi usati Visual Studio 2019 e Visual Studio Code.



