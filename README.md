# projectv

ISTRUZIONI: 

•	Eseguire il codice di Data Reader e Data Sender su Visual Studio 2019. 
• Eseguiamo Redis da linea di commando: docker run --name pw-redis -p 6379:6379 -d redis
Avviamolo: docker start pw-redis
('docker ps' per controllare lo stato di redis.)

•	Avviare l’Influx DB attraverso gli eseguibili della cartella “influx”. 
•	Eseguire il main.js su Visual Studio Code aprendo una chiamata al server su porta 3000, e mediante la POST vengono quindi i dati all’influx db che li registra. 


	Scaricare package.json e digitare nel terminale "npm install" 
in modo da installare le seguenti librerie: fastify, fastifycors e influx.

	Programmi usati Visual Studio 2019 e Visual Studio Code.



