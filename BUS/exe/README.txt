La cartella contiene gli eseguibili necessari per il sistema installato sul mezzo, quindi:
-simulatori dati
-DataReader
-DataSender
-server redis 2.2
-External (programma per gestire le variabili anche da programmi avviati)

Questi programmi usano .Net 4.0 in quanto sono progettati per funzionare
su Windows XP Embedded SP3.

Per eseguire il sistema basta eseguire "starter.bat", si apriranno quattro
schermate per i relativi programmi. Il server è attivato da DataReader.

DataSender è impostato per attedere 1,5 secondi dopo ogni ciclo prima
di cominciarne un altro. Questa impostazione può essere tolta tramite
il comando "speed stop" su External.

L'indirizzo ip del server influx è preimpostato su 192.168.1.23,
si può modificarlo con il comando "serverip" su External.

Comandi disponibili per Exernal:
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
oppure direttamente nel file "EXTERNALAPP.txt" e salvarlo.
Per verificare l'inserimento del valore aprire il file "VALUES.txt".
Potrebbero essere necessari diversi secondi per il file per aggiornarsi.