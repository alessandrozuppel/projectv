In questa cartella sono presenti i file utilizzati dal mezzo.
DataSender si occupa della comunicazione con il server, quindi:
-Esegue autenticazione, se effettuata con successo
	-Legge eventuali comandi scritti in EXTERNALAPP.txt
	-Ottiene token jwt
	-Ottiene la tratta giornaliera
	-Invia i dati dei sensori

DataReader si occupa della gestione dei sensori:
-Legge tratta giornaliera da DataSender e successivamente
	-Legge eventuali comandi scritti in EXTERNALTIME.txt
	-Produce dati (nella simulazione abbiamo sfruttato i dati della tratta)
	-Salva i dati su Redis sotto forma di JSON


La cartella External contiene un progetto per modificare dei settings usati dal 
DataReader e DataSender mentre questi sono in esecuzione. Maggiori informazioni nel
README.txt all'interno della cartella.

La cartella exe contiene gli eseguibili di tutti i progetti e relative dipendenze,
da poter fa correre su Arduino o RaspBerry. Maggiori informazioni nel README.txt
all'interno della cartella.