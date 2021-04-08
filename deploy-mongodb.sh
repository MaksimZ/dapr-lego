#!/bin/bash
source .env

aws eks --region $AWS_DEFAULT_REGION update-kubeconfig --name $CLUSTER_NAME

export MONGODB_PASSWORD=

kubectl create secret generic mongosecrets --from-literal=mongodb-password=$MONGODB_PASSWORD
