#!/bin/bash

kubectl delete component statestore
kubectl apply -f .kubernetes/components/statestore-mongo-template.yaml.template