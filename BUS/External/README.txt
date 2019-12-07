Questo progetto gestisce la comunicazione con il programma sul mezzo.

Comandi disponibili per External:
time [tempo in secondi] - imposta il tempo di attesa per la generazione di dati su DataReader
serverip [indirizzo ipv4] - imposta l'indirizzo a cui contattare il server influx
serverport [numero di porta] - imposta la porta di ascolto del server influx
apipath [path] - imposta l'url dell'api presso influx a cui inviare i dati
autentication [true/false] - imposta se effettuare l'autenticazione al server (true) o no (false), sia basic che jwt
wait start - DataSender legge dati dalla coda
wait stop - DataSender non legge dati dalla coda
speed start - DataSender non aspetta 1,5 secondi prima di leggere il dato successivo dopo aver compiuto un ciclo
speed stop - DataSender aspetta 1,5 secondi prima di leggere il dato successivo dopo aver compiuto un ciclo
clear server - per simulazione, pulisce la coda sul server RESTITUISCE COMMAND NOT AVAILABLE
clear vehicle - per simulazione, pulisce la coda sul mezzo RESTITUISCE COMMAND NOT AVAILABLE

Per effettuare un comando è sufficiente digitarlo nel prompt di "External",
oppure direttamente nel file "EXTERNALAPP.txt" nella cartella "BUS" e salvarlo.
Per verificare l'inserimento del valore aprire il file "VALUES.txt" nella cartella "BUS".
Potrebbero essere necessari diversi secondi per il file per aggiornarsi.