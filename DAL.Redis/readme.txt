Data structure

Each host has sorted set (with name messages:instance-{InstanceId})
Score: message dateTime's ticks
Value: Guid (unique for message)

Messages texts store in key-value; key is "message:{guid from sorted set}", value is text


Service factories:

DBFactory - DI lifetime singleton, without resources. 
DBFactory can create instance of DALServicesMaker

DALServiceMaker - DI lifetime scope; disposable, with resource (connection, RedisDB)
DALService can create services