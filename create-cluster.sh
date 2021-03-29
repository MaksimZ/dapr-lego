#!/bin/bash
source .env
# eksctl create cluster -f .kubernetes/cluster.yaml
# exit 0


echo "Login to cluster"
aws eks --region $AWS_DEFAULT_REGION update-kubeconfig --name $CLUSTER_NAME
echo "Done"

# Add ALB
echo "Adding ingress"
kubectl apply -f https://raw.githubusercontent.com/kubernetes-sigs/aws-alb-ingress-controller/v1.1.8/docs/examples/rbac-role.yaml
kubectl apply -f .kubernetes/alb-ingress-controller.yaml
echo "Done"


# Add user dapr-lego-cli to Cluster
echo "Adding user $USER_NAME to cluster..."
eksctl create iamidentitymapping --cluster $CLUSTER_NAME-cluster --arn arn:aws:iam::IAM_USERID:user/$USER_NAME --username $USER_NAME --group system:masters
echo "Done."
