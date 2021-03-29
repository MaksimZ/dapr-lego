#!/bin/bash
source .env



echo "Adding ingress"
kubectl apply -f .kubernetes/alb-ingress-controller.yaml
kubectl apply -f .kubernetes/ingress-controller.yaml
echo "Done."

echo "Deploying application"

echo "Done."