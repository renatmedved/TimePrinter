Data structure

Each host has sorted set (with name messages:host-{InstanceId})
Score: message dateTime's ticks
Value: Guid (unique for message)

Messages texts store in key-value; key is "message:{guid from sorted set}", value is text