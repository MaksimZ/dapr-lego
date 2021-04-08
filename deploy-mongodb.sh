#!/bin/bash
source .env

aws eks --region $AWS_DEFAULT_REGION update-kubeconfig --name $CLUSTER_NAME

helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install my-release bitnami/mongodb

export MONGODB_ROOT_PASSWORD=$(kubectl get secret --namespace default my-release-mongodb -o jsonpath="{.data.mongodb-root-password}" | base64 --decode)

kubectl create secret generic mongosecrets --from-literal=mongodb-password=$MONGODB_ROOT_PASSWORD
