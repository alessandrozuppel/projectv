const fastify = require('fastify')({
  logger: true,                           //quando arriva una richiesta http riesce a loggare qualsiasi richiesta, senza che ogni volta lo facciamo noi (il log)
  ignoreTrailingSlash: true               // Se =True --> tratta la richiesta con "/" finale o senza, allo stesso modo ("products/"" o "product")
});

const xml = require('xmlhttprequest');
//list of servers
const addresses = ["127.0.0.10:3001", "127.0.0.20:3002", "127.0.0.30:3003"];

const http = new xml.XMLHttpRequest();
const path = '/api/busdati/';
var counter=0;

  //legge dati in entrata e invia a server estratto
  fastify.post(path, async (request, reply) => {
    console.log("COUNTER::"+counter);
    counter+=1;
    var url = "http://"+addresses[counter%3]+path;
    console.log(url);
    http.open("POST", url);
    var body = String(request.body);
    http.send(body);
  });


const start = async () => {
  try {
    await fastify.listen(3000, "0.0.0.0");                                //Creo web server e sto in ascolto sulla porta 3000
    fastify.log.info(`server listening on ${fastify.server.address().port}`)      // Ascolto tutte richiest http
  } catch (err) {
    fastify.log.error(err)
    process.exit(1)                                                               // Esci con un errore 
  }
}
start()
