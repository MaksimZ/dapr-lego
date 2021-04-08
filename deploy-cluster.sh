#!/bin/bash
source .env


echo "Adding ingress"
# kubectl apply -f .kubernetes/alb-ingress-controller.yaml
kubectl apply -f .kubernetes/ingress-controller.yaml
echo "Done."

echo "Deploy redis"
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install redis bitnami/redis
export REDIS_PASSWORD=$(kubectl get secret --namespace default redis -o jsonpath="{.data.redis-password}" | base64 --decode)
echo "Done."

echo "deploy secrects"
kubectl create secret generic credentialsecrets --from-literal=redis-password=$REDIS_PASSWORD --from-literal=accesskey=$AWS_ACCESS_KEY_ID --from-literal=usersecretKey=$AWS_SECRET_ACCESS_KEY
echo "done."

echo "deploy dapr"
dapr init --runtime-version=1.0.0 -k
echo "done"

echo "Deploying components"
kubectl apply -f .kubernetes/components
echo "Done."

echo "Deploying applications"
kubectl apply -f .kubernetes/apps
echo "Done."