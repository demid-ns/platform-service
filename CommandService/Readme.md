### Docker commands

Build docker:

```
docker build -t <put image name here> .
```

Run docker:

```
docker run -p 5001:80 <put image name here>
```

Push docker: 

```
docker push <put image name here>
```

Clean docker cache: 

```
docker system prune -a
```

Stop all docker containers

```
docker start ps -a
```

Start docker 

```
docker start <put container id here>
```

Stop docker 

```
docker stop <put container id here>
```


