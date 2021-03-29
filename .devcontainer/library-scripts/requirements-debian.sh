#!/usr/bin/env bash
set -e

echo "Install base" \
    && apt-get -y install git openssh-client less iproute2 procps apt-transport-https gnupg2 curl lsb-release unzip
    # Install AWS cli
echo "Install AWS CLI" \
    && apt-get install -y awscli \
    && curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" \
    && unzip awscliv2.zip \
    && rm awscliv2.zip \
    && ./aws/install
    #
    # KUBECTL
echo "Install Kubectl" \
    && curl -o kubectl https://amazon-eks.s3.us-west-2.amazonaws.com/1.16.8/2020-04-16/bin/linux/amd64/kubectl \
    && chmod +x ./kubectl \
    && mv ./kubectl /usr/local/bin/kubectl
    #  
    # AUTHENTICATOR
echo "Installing AWS IAM Auth"
curl -o aws-iam-authenticator https://amazon-eks.s3.us-west-2.amazonaws.com/1.16.8/2020-04-16/bin/linux/amd64/aws-iam-authenticator
chmod +x ./aws-iam-authenticator \
    && mv ./aws-iam-authenticator /usr/local/bin/aws-iam-authenticator
    #  
    # eksctl
echo "Install eksctl" \
    && curl --silent --location "https://github.com/weaveworks/eksctl/releases/latest/download/eksctl_$(uname -s)_amd64.tar.gz" | tar xz -C /tmp \
    && chmod +x /tmp/eksctl \
    && mv /tmp/eksctl /usr/local/bin/eksctl
    #  
    # HELM
echo "Install Helm" \
    && curl https://raw.githubusercontent.com/helm/helm/master/scripts/get-helm-3 | /bin/bash