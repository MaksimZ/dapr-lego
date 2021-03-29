#!/bin/bash
source .env

echo "Adding autoscaler"
kubectl apply -f .kubernetes/cluster-autoscaler-autodiscover.yaml
echo "Done"

echo "Adding ingress"
kubectl apply -f .kubernetes/alb-ingress-controller.yaml
kubectl apply -f .kubernetes/ingress-controller.yaml
echo "Done."

echo "Deploying application"

echo "Done."