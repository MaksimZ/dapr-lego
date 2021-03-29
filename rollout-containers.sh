#!/bin/bash
source .env
# BUILD_NUMBER=dev_one_200806.11 - stable
BUILD_NUMBER=MAGIC

#       # Configure cluster
#       echo "Configure cluster..."
#       aws eks --region $AWS_DEFAULT_REGION update-kubeconfig --name $CLUSTER_NAME
#       echo "Done."

      # Configure redis store
    #   echo "Configure redis store..."
    #   REDIS_SERVER_URL=`aws elasticache describe-cache-clusters --cache-cluster-id "$REDIS_CLUSTER_NAME" --show-cache-node-info --query 'CacheClusters[*].CacheNodes[*].Endpoint[].Address' --output text`
    #   cat .kubernetes/templates/redis-base.yaml | sed "s/redis-master/$REDIS_SERVER_URL/" > .kubernetes/redis.yaml
    #   kubectl apply -f .kubernetes/redis.yaml
    #   echo "Done."

      # Deploy applications
    #   echo "Deploy applications..."
    #   kubectl apply -f .kubernetes/testbinding.yaml
    #   kubectl apply -f .kubernetes/test-ingress.yaml
    #   kubectl apply -f .kubernetes/actors.actionbuilder.yml
    #   kubectl apply -f .kubernetes/flow.normalization.yml
    #   kubectl apply -f .kubernetes/flow.queuedispatcher.yml
    #   echo "Done."

#       # Deploy ingress
#       echo "Deploy Ingress..."
#       kubectl apply -f .kubernetes/test-ingress.yaml
#       echo "Done."

aws ecr get-login-password --region $AWS_DEFAULT_REGION | docker login --username AWS --password-stdin $ECR_Url

# docker build -f Actors.ActionBuilder/Dockerfile -t $ECR_Url/actors.actionbuilder:$BUILD_NUMBER . && docker push $ECR_Url/actors.actionbuilder:$BUILD_NUMBER
# docker build -f Flow.Normalization/Dockerfile -t $ECR_Url/flow.normalization:$BUILD_NUMBER . && docker push $ECR_Url/flow.normalization:$BUILD_NUMBER
# docker build -f Flow.QueueDispatcher/Dockerfile -t $ECR_Url/flow.queuedispatcher:$BUILD_NUMBER . && docker push $ECR_Url/flow.queuedispatcher:$BUILD_NUMBER
# docker build -f Flow.ResultProcessing/Dockerfile -t $ECR_Url/flow.resultprocessing:$BUILD_NUMBER . && docker push $ECR_Url/flow.resultprocessing:$BUILD_NUMBER
# docker build -f Communication.QueuePoller/Dockerfile -t $ECR_Url/communication.queuepoller:$BUILD_NUMBER . && docker push $ECR_Url/communication.queuepoller:$BUILD_NUMBER


# kubectl set image deployment.apps/actors-actionbuilder actors-actionbuilder=$ECR_Url/actors.actionbuilder:$BUILD_NUMBER
# kubectl set image deployment.apps/flow-normalization flow-normalization=$ECR_Url/flow.normalization:$BUILD_NUMBER
# kubectl set image deployment.apps/flow-queuedispatcher flow-queuedispatcher=$ECR_Url/flow.queuedispatcher:$BUILD_NUMBER
# kubectl set image deployment.apps/flow-resultprocessing flow-resultprocessing=$ECR_Url/flow.resultprocessing:$BUILD_NUMBER
# kubectl set image deployment.apps/communication-queuepoller communication-queuepoller=$ECR_Url/communication.queuepoller:$BUILD_NUMBER


kubectl -n default rollout restart deployment actors-actionbuilder
kubectl -n default rollout restart deployment flow-normalization
kubectl -n default rollout restart deployment flow-queuedispatcher
kubectl -n default rollout restart deployment flow-resultprocessing
kubectl -n default rollout restart deployment communication-queuepoller
kubectl -n default rollout restart deployment communication-pnrread

kubectl get pods -w