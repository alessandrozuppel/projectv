const fastify = require('fastify')({ 
  logger: true,                           //quando arriva una richiesta http riesce a loggare qualsiasi richiesta, senza che ogni volta lo facciamo noi (il log)
  ignoreTrailingSlash: true               // Se =True --> tratta la richiesta con "/" finale o senza, allo stesso modo ("products/"" o "product")
});

fastify.register(require('fastify-cors'));

const Influx = require('influx');
const influx = new Influx.InfluxDB({
host: 'localhost',
database: 'busdati',
port:8086,
schema: [
 {
   measurement: 'Posizione',
   fields: {
     latitudine: Influx.FieldType.STRING,
     longitudine: Influx.FieldType.STRING
   },
   tags: [
     'idBus'
   ]
 }
]
})
influx.writePoints([
{
  measurement: 'Posizione',
  tags: { idBus: 2222 },
  fields: { latitudine:22.2222, longitudine:22.2222 },
}
]).then(() => {
return influx.query(`
  select * from Posizione
`)
})/*.then(rows => {
rows.forEach(row => console.log(`A request to ${row.path} took ${row.duration}ms`))
})
*/
fastify.post('/api/busdati/', async (request, reply) => {
  var dati=request.body;



  console.log("Dati: ",dati);

  reply.status(201).send(dati);

  
});


const start = async () => {
  try {                                                       
    await fastify.listen(3000,'192.168.101.81')                                                    //Creo web server e sto in ascolto sulla porta 3000
    fastify.log.info(`server listening on ${fastify.server.address().port}`)      // Ascolto tutte richiest http
  } catch (err) {
    fastify.log.error(err)
    process.exit(1)                                                               // Esci con un errore 
  }
}
start()