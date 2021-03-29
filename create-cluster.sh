#!/bin/bash
source .env

curl -o iam_policy.json https://raw.githubusercontent.com/kubernetes-sigs/aws-load-balancer-controller/v2.1.3/docs/install/iam_policy.json
aws iam create-policy \
    --policy-name AWSLoadBalancerControllerIAMPolicy \
    --policy-document file://iam_policy.json

rm iam_policy.json

eksctl create cluster -f .kubernetes/cluster.yaml
# exit 0


echo "Login to cluster"
aws eks --region $AWS_DEFAULT_REGION update-kubeconfig --name $CLUSTER_NAME
echo "Done"

# Add ALB
echo "Adding ingress"
kubectl apply -f https://raw.githubusercontent.com/kubernetes-sigs/aws-alb-ingress-controller/v1.1.8/docs/examples/rbac-role.yaml
kubectl apply -f .kubernetes/alb-ingress-controller.yaml
echo "Done"


echo "Adding autoscaler"
kubectl apply -f .kubernetes/cluster-autoscaler-autodiscover.yaml
echo "Done"

# Add user dapr-lego-cli to Cluster
# echo "Adding user $USER_NAME to cluster..."
# eksctl create iamidentitymapping --cluster $CLUSTER_NAME-cluster --arn arn:aws:iam::IAM_USERID:user/$USER_NAME --username $USER_NAME --group system:masters
# echo "Done."
