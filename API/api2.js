const sql = require("mssql");
const config = {
  user: 'sa',
  password: 'passwordpassword',
  server: 'localhost\\sqlexpress', // You can use 'localhost\\instance' to connect to named instance
  database: 'ITS',

  options: {
      encrypt: true // Use this if you're on Windows Azure
  }
};

const fastify = require('fastify')({
  logger: true,                           //quando arriva una richiesta http riesce a loggare qualsiasi richiesta, senza che ogni volta lo facciamo noi (il log)
  ignoreTrailingSlash: true               // Se =True --> tratta la richiesta con "/" finale o senza, allo stesso modo ("products/"" o "product")
});

fastify.register(require('fastify-cors'));

const Influx = require('influx');
const fs = require('fs');

fastify.register(require("fastify-jwt"), {
  secret: 'supersecret'
});
const bcrypt = require("bcrypt");

var obj = JSON.parse(fs.readFileSync('config/configApi_Influxdb2.json', 'utf8'));

const influx = new Influx.InfluxDB({
  host: obj.influx.host,
  database: obj.influx.database,
  port: obj.influx.port
})

fastify.post('/token', async (request, reply) => {
  try
  {
    let server = await sql.connect(config);
    let model = request.body;
    var check = await sql.query(`select Password from Users where Username='${model.username}';`);
   if(check.recordset.lenght>0)
    {
      var hash = bcrypt.hashSync(check.recordset[0], 10);
      if(hash==model.password)
      {
        var user =
        {
          id: 10,
          user: "pippo"
        };
        const token = fastify.jwt.sign({ payload: user });
        reply.send(token);
      }
      else
      {
        reply.send("400");
      }
    }
    else
    {
      reply.send("400");
    }
  }
  catch
  {
    reply.send("500");
  }
});


fastify.register(async function (fastify, opts) {
  fastify.addHook('preHandler', async (request, reply) => {
    try {
      let model = request.body;
      await request.jwtVerify(model.Token);
    }
    catch (err) {
      reply.send(err);
    }
  });

  // Lettura dati dal simulatore e salvataggio su influx DB
  fastify.post('/api/busdati/', async (request, reply) => {
    var dati = request.body;
    //console.log("Dati: ",dati);
    if (dati.Apertura == '') {
      influx.writePoints([
        {
          measurement: 'Posizione',
          tags: { idBus: dati.IdVeicolo },
          fields: { latitudine: dati.Latitudine, longitudine: dati.Longitudine },
          timestamp: dati.DataOraBus + "00",
        }
      ]);
    }
    else {
      var porta;
      if (dati.Apertura == 1) {
        porta = 'Aperta';
      }
      else {
        porta = 'Chiusa';
      }
      influx.writePoints([
        {
          measurement: 'Apertura',
          tags: { idBus: dati.IdVeicolo },
          fields: { Apertura: porta, nPersone: dati.Conta_Persone, latitudine: dati.Latitudine, longitudine: dati.Longitudine },
          timestamp: dati.DataOraPorte + "00",
        }
      ]);
    }

    reply.status(201).send("201");


  });
});

// visualizzazione dei dati 

/*
1) visualizzare posizione di un determinato autobus:
2) numero di persone di un determinato autobus
*/

fastify.get('/api/visbus/:id', async (request, reply) => {
  var id = request.params.id;
  console.log(id);
  var results;
  var query = 'select * from Posizione where idBus=\'' + id + '\' ';

  try{
    influx.query(query).then(results => {
      //console.log("sono qui");
      if (results==''){
       // console.log("sono qui?");
        reply.status(404).send("Autobus not found");
      }
      else{
      reply.send(results);
      }
    });
  }
  catch(error){
    reply.status(404).send(error);
  }
  
});
/*
3) numero di persone di un determinato autobus
*/
fastify.get('/api/numpers/:id', async (request, reply) => {
  var id = request.params.id;
  console.log(id);
  var results;
  var query = 'select nPersone from Apertura where idBus=\'' + id + '\' ';

  
    influx.query(query).then(results => {
      //console.log("sono qui");
      if (results==''){
        // console.log("sono qui?");
         reply.status(404).send("Autobus not found");
       }
       else{
       reply.send(results);
       }
    });
});
/*
2) visualizzare gli autobus online
*/
const start = async () => {
  try {
    await fastify.listen(obj.api.port, obj.api.ip)                                                    //Creo web server e sto in ascolto sulla porta 3000
    fastify.log.info(`server listening on ${fastify.server.address().port}`)      // Ascolto tutte richiest http
  } catch (err) {
    fastify.log.error(err)
    process.exit(1)                                                               // Esci con un errore 
  }
}
start()
