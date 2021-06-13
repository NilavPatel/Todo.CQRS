## How to configure Eventstore?

1. Install [Chocolatey](https://docs.chocolatey.org/en-us/choco/setup)
2. Run below command in CMD with administrative rights  
   `choco install eventstore-oss`
3. Create below folders  
   C:\ESDB\Data  
   C:\ESDB\Index  
   C:\ESDB\Logs
4. Add config file `C:\ESDB\eventstore.conf` with below content

```javascript
# Paths
Db: C:\ESDB\Data
Index: C:\ESDB\Index
Log: C:\ESDB\Logs

# Run in insecure mode
Insecure: true

# Network configuration
IntIp: 127.0.0.1
ExtIp: 127.0.0.1
HttpPort: 2113
IntTcpPort: 1112
ExtTcpPort: 1113
EnableExternalTcp: true
EnableAtomPubOverHTTP: true

# Projections configuration
RunProjections: None
```

5. Run command `EventStore.ClusterNode.exe --config C:\ESDB\eventstore.conf` in CMD with administrative rights
6. Open URL http://localhost:2113/web/index.html#/dashboard
