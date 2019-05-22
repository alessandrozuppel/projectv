const fastify = require('fastify')({ 
  logger: true,                           //quando arriva una richiesta http riesce a loggare qualsiasi richiesta, senza che ogni volta lo facciamo noi (il log)
  ignoreTrailingSlash: true               // Se =True --> tratta la richiesta con "/" finale o senza, allo stesso modo ("products/"" o "product")
});

fastify.register(require('fastify-cors'));

const Influx = require('influx');
const fs=require('fs');

var obj= JSON.parse(fs.readFileSync('../config/configApi_Influxdb.json','utf8'));

const influx = new Influx.InfluxDB({
host: obj.influx.host,
database: obj.influx.database,
port:obj.influx.port
})
fastify.post('/api/busdati/', async (request, reply) => {
  var dati=request.body;
  //console.log("Dati: ",dati);
  if(dati.Apertura==''){
    influx.writePoints([
      {
        measurement: 'Posizione',
        tags: { idBus: dati.IdVeicolo },
        fields: { latitudine:dati.Latitudine, longitudine:dati.Longitudine },
        timestamp:dati.DataOraBus+"00",
      }
      ]);
  }
  else{
    var porta;
    if(dati.Apertura==1){
       porta='Aperta';
    }
    else{
      porta='Chiusa';
    }
    influx.writePoints([
      {
        measurement: 'Apertura',
        tags: { idBus: dati.IdVeicolo },
        fields: { Apertura:porta, latitudine:dati.Latitudine, longitudine:dati.Longitudine },
        timestamp:dati.DataOraPorte+"00",
      }
      ]);
  }

  reply.status(201).send("201");

  
});


const start = async () => {
  try {                                                       
    await fastify.listen(obj.api.port,obj.influx.ip)                                                    //Creo web server e sto in ascolto sulla porta 3000
    fastify.log.info(`server listening on ${fastify.server.address().port}`)      // Ascolto tutte richiest http
  } catch (err) {
    fastify.log.error(err)
    process.exit(1)                                                               // Esci con un errore 
  }
}
start()