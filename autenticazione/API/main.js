const fastify = require('fastify')({ 
  logger: true,                           //quando arriva una richiesta http riesce a loggare qualsiasi richiesta, senza che ogni volta lo facciamo noi (il log)
  ignoreTrailingSlash: true               // Se =True --> tratta la richiesta con "/" finale o senza, allo stesso modo ("products/"" o "product")
});

fastify.register(require('fastify-cors'));

const Influx = require('influx');
const fs=require('fs');
fastify.register(require("fastify-jwt"), {
  secret: 'supersecret'
});
const bcrypt = require("bcrypt");

var obj= JSON.parse(fs.readFileSync('../config/configApi_Influxdb.json','utf8'));








fastify.post('/token', async(request,reply) => {
  try{
  let model= request.body;
  if(model.password== "passwordsicura"){
      var user= {
          id: 10,
          user: "pippo"
      };
      const token= fastify.jwt.sign({payload: user});
      reply.send(token);
      }
  else{
      reply.status(404).send({
          "Error": "Username o Password sbagliati"
      });
      }

  }
  catch(err){
      reply.send(err);
  }
  });





fastify.register(async function(fastify, opts){
  fastify.addHook('preHandler', async(request, reply) => {
      try{
          let model=request.body;
          await request.jwtVerify(model.Token);
      }
      catch(err){
          reply.send(err);
      }
  });

  fastify.post('/api/busdati/', async (request, reply) => {
    var dati=request.body;
    console.log(dati);
    reply.status(201).send("201");
  });

});





const start = async () => {
  try {                                                       
    await fastify.listen(obj.api.port,obj.api.ip)                                                    //Creo web server e sto in ascolto sulla porta 3000
    fastify.log.info(`server listening on ${fastify.server.address().port}`)      // Ascolto tutte richiest http
  } catch (err) {
    fastify.log.error(err)
    process.exit(1)                                                               // Esci con un errore 
  }
}
start()