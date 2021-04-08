#!/bin/bash
source .env
BUILD_NUMBER=latest

# docker build -f Actors/AllActors/Dockerfile -t $ECR_Url/actors.allactors:$BUILD_NUMBER .
docker run -p 3000:3000 -e ASPNETCORE_Kestrel__Endpoints__Http__Url="http://+:3000" $ECR_Url/actors.allactors:$BUILD_NUMBER
exit

aws ecr get-login-password --region $AWS_DEFAULT_REGION | docker login --username AWS --password-stdin $ECR_Url

docker build -f Actors/AllActors/Dockerfile -t $ECR_Url/actors.allactors:$BUILD_NUMBER . && docker push $ECR_Url/actors.allactors:$BUILD_NUMBER
docker build -f Api/CharacterApi/Dockerfile -t $ECR_Url/api.characterapi:$BUILD_NUMBER . && docker push $ECR_Url/api.characterapi:$BUILD_NUMBER
docker build -f Api/LocationApi/Dockerfile -t $ECR_Url/api.locationapi:$BUILD_NUMBER . && docker push $ECR_Url/api.locationapi:$BUILD_NUMBER
docker build -f Api/QuestApi/Dockerfile -t $ECR_Url/api.questapi:$BUILD_NUMBER . && docker push $ECR_Url/api.questapi:$BUILD_NUMBER


# kubectl set image deployment.apps/actors-actionbuilder actors-actionbuilder=$ECR_Url/actors.actionbuilder:$BUILD_NUMBER
# kubectl set image deployment.apps/flow-normalization flow-normalization=$ECR_Url/flow.normalization:$BUILD_NUMBER
# kubectl set image deployment.apps/flow-queuedispatcher flow-queuedispatcher=$ECR_Url/flow.queuedispatcher:$BUILD_NUMBER
# kubectl set image deployment.apps/flow-resultprocessing flow-resultprocessing=$ECR_Url/flow.resultprocessing:$BUILD_NUMBER
# kubectl set image deployment.apps/communication-queuepoller communication-queuepoller=$ECR_Url/communication.queuepoller:$BUILD_NUMBER


# kubectl -n default rollout restart deployment actors-actionbuilder
# kubectl -n default rollout restart deployment flow-normalization
# kubectl -n default rollout restart deployment flow-queuedispatcher
# kubectl -n default rollout restart deployment flow-resultprocessing
# kubectl -n default rollout restart deployment communication-queuepoller
# kubectl -n default rollout restart deployment communication-pnrread

# kubectl get pods -w