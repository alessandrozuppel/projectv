const fastify = require('fastify')({
  logger: true,                           //quando arriva una richiesta http riesce a loggare qualsiasi richiesta, senza che ogni volta lo facciamo noi (il log)
  ignoreTrailingSlash: true               // Se =True --> tratta la richiesta con "/" finale o senza, allo stesso modo ("products/"" o "product")
});

var counter=0;
var obj = JSON.parse(fs.readFileSync('config/LoadBalancer.json', 'utf8'));

//list of servers
const addresses = obj.servers.addresses;

//determina il server e lo comunica
fastify.get("/getip/", async (request, reply) => {
  try
  {
    counter+=1;
    reply.send(addresse[counter%addresses.length]);
  }
  catch
  {
    reply.send("500");
  }
});


const start = async () => {
  try {
    console.log(fastify.server.address());
    await fastify.listen(obj.api.port, obj.api.ip)                                //Creo web server e sto in ascolto sulla porta 3000
    fastify.log.info(`server listening on ${fastify.server.address().port}`)      // Ascolto tutte richiest http
  } catch (err) {
    fastify.log.error(err)
    process.exit(1)                                                               // Esci con un errore 
  }
}
start()
