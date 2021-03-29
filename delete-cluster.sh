#!/bin/bash
source .env
echo "Deleting cluster"

eksctl delete cluster --wait -f .kubernetes/cluster.yaml

echo "Done."