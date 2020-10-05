docker build -t dbernat/dating-app-api:latest -t dbernat/dating-app-api:$SHA -f ./API/Dockerfile ./API
docker push dbernat/dating-app-api:latest
docker push dbernat/dating-app-api:$SHA