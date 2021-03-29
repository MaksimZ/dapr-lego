#!/bin/bash
source .env
echo "Deleting cluster"

eksctl delete cluster --wait -f cluster.yaml

echo "Done."