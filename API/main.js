const fastify = require('fastify')({
  logger: true,                           //quando arriva una richiesta http riesce a loggare qualsiasi richiesta, senza che ogni volta lo facciamo noi (il log)
  ignoreTrailingSlash: true               // Se =True --> tratta la richiesta con "/" finale o senza, allo stesso modo ("products/"" o "product")
});

const bcrypt = require("bcrypt");
const fs = require('fs');

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


//list of servers
const addresses = ["127.0.0.10:3001", "127.0.0.20:3002", "127.0.0.30:3003"];

var counter=0;
var obj = JSON.parse(fs.readFileSync('config/LoadBalancer.json', 'utf8'));

//determina il server e lo comunica
fastify.post("/getip/", async (request, reply) => {
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
        counter+=1;
        reply.send(addresses[counter%3]);
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


const start = async () => {
  try {
    console.log(fastify.server.address());
    await fastify.listen(obj.api.port, obj.api.ip)                                                    //Creo web server e sto in ascolto sulla porta 3000
    fastify.log.info(`server listening on ${fastify.server.address().port}`)      // Ascolto tutte richiest http
  } catch (err) {
    fastify.log.error(err)
    process.exit(1)                                                               // Esci con un errore 
  }
}
start()
