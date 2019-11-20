api 2,3 sono copie di ap1. Il main è il load balancer che fornisce l'indirizzo di una delle
api, a rotazione.
api 1,2,3 comunicano con lo stesso database.
Per avviare le tre api e il load balancer avviare starter.bat.