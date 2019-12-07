In questa cartella sono presenti i file necessari al funzionamento del lato server.
Il main.js contiene l'API con cui si inetrfacceranno i mezzi e gestisce
-La comunicazione con il server MSSQL:
	-Autenticazione dei mezzi
		-Effettuata questa con successo fornisce jwt
	-Fornitura tratta

-La comunicazione con il server InfluxDB

La cartella tracks contiene i file delle tratte dei mezzi.
La cartella config contiene i file di configurazione di accesso ai server
(su github sono presenti solo i template)

AUTENTICAZIONE
L'api riceve da un mezzo il suo username e password criptata, richiede quindi l'hash
della password salvata sul database e lo paragona con quello inviato dal mezzo:
-Se coincidono l'api prosegue a fornire un jwt al mezzo
-Se non coincidono restituisce un messaggio di errore al mezzo

FORNITURA TRATTA
Quando un mezzo richiede la sua tratta, l'api si connette al database Routes_Bus e
riceve il path del file contenente la tratta, legge i dati dal file e li restituisce al mezzo.

SALVATAGGIO DATI
L'api riceve dei json dai mezzi e si connette all'influxDB per salvarli