#!/bin/bash
source .env

aws configure set aws_access_key_id $AWS_ACCESS_KEY_ID
aws configure set aws_secret_access_key $AWS_SECRET_ACCESS_KEY
aws configure set region $AWS_DEFAULT_REGION

aws eks --region $AWS_DEFAULT_REGION update-kubeconfig --name $CLUSTER_NAME

kubectl create secret generic credentialsecrets --from-literal=accesskey=$AWS_ACCESS_KEY_ID --from-literal=usersecretKey=$AWS_SECRET_ACCESS_KEY