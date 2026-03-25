# DevOps Cheat Sheet

> Quick reference for Docker, Docker Compose, and Kubernetes commands used in real development workflows.

---

## Table of Contents

- [Docker](#docker)
  - [Image Commands](#image-commands)
  - [Container Commands](#container-commands)
  - [Disk Usage & Cleanup](#disk-usage--cleanup)
  - [Networking](#networking)
  - [Volumes](#volumes)
  - [Registry Commands](#registry-commands)
- [Docker Compose](#docker-compose)
- [Kubernetes (kubectl)](#kubernetes-kubectl)
  - [Basic Commands](#basic-commands)
  - [Resources](#resources)
  - [Apply & Delete](#apply--delete)
  - [Inspect & Debug](#inspect--debug)
  - [Exec & Port Forwarding](#exec--port-forwarding)
  - [Scaling](#scaling)
- [Development Workflow Example](#development-workflow-example)

---

## Docker

### Image Commands

```bash
# Build an image from a Dockerfile in the current directory
docker build -t my-app:latest .

# Build with a specific Dockerfile and build args
docker build -f Dockerfile.prod --build-arg NODE_ENV=production -t my-app:prod .

# Tag an existing image
docker tag my-app:latest myrepo/my-app:v1.0.0

# List all local images
docker images
docker image ls

# Remove a specific image
docker rmi my-app:latest

# Remove all dangling (untagged) images
docker image prune

# Remove all unused images (not just dangling)
docker image prune -a

# View image build history (layers)
docker history my-app:latest

# Inspect image metadata (JSON)
docker image inspect my-app:latest
```

---

### Container Commands

```bash
# Run a container (foreground)
docker run my-app:latest

# Run with a name, port mapping, and environment variable
docker run --name api-server -p 3000:3000 -e NODE_ENV=production my-app:latest

# Run in detached (background) mode
docker run -d --name api-server -p 3000:3000 my-app:latest

# Run interactively with a shell
docker run -it --rm ubuntu:22.04 bash

# Run with a volume mount
docker run -v $(pwd)/data:/app/data my-app:latest

# List running containers
docker ps

# List all containers (including stopped)
docker ps -a

# Stop a running container
docker stop api-server

# Start a stopped container
docker start api-server

# Restart a container
docker restart api-server

# Remove a stopped container
docker rm api-server

# Force remove a running container
docker rm -f api-server

# View container logs
docker logs api-server

# Follow (tail) live logs
docker logs -f api-server

# Show last 50 lines of logs
docker logs --tail 50 api-server

# Execute a command inside a running container
docker exec api-server ls /app

# Open an interactive shell in a running container
docker exec -it api-server bash

# Inspect container metadata (JSON)
docker inspect api-server

# Copy files between host and container
docker cp ./config.json api-server:/app/config.json
docker cp api-server:/app/logs ./local-logs
```

---

### Disk Usage & Cleanup

```bash
# Show Docker disk usage summary
docker system df

# Show detailed disk usage
docker system df -v

# Remove all stopped containers, unused networks, dangling images, and build cache
docker system prune

# Remove everything including unused images and volumes (use with caution)
docker system prune -a --volumes

# Remove all stopped containers
docker container prune

# Remove all unused volumes
docker volume prune

# Remove all unused networks
docker network prune
```

---

### Networking

```bash
# List all networks
docker network ls

# Create a custom bridge network
docker network create my-network

# Create a network with a specific subnet
docker network create --subnet=172.20.0.0/16 my-network

# Connect a running container to a network
docker network connect my-network api-server

# Disconnect a container from a network
docker network disconnect my-network api-server

# Inspect a network (shows connected containers)
docker network inspect my-network

# Remove a network
docker network rm my-network

# Run a container attached to a specific network
docker run -d --network my-network --name api-server my-app:latest
```

---

### Volumes

```bash
# List all volumes
docker volume ls

# Create a named volume
docker volume create app-data

# Inspect a volume (shows mountpoint and metadata)
docker volume inspect app-data

# Mount a named volume when running a container
docker run -d -v app-data:/app/data my-app:latest

# Mount a host directory (bind mount)
docker run -d -v $(pwd)/data:/app/data my-app:latest

# Mount as read-only
docker run -d -v app-data:/app/data:ro my-app:latest

# Remove a specific volume
docker volume rm app-data

# Remove all unused volumes
docker volume prune
```

---

### Registry Commands

```bash
# Log in to Docker Hub
docker login

# Log in to a private registry
docker login registry.example.com

# Push an image to a registry
docker push myrepo/my-app:v1.0.0

# Pull an image from a registry
docker pull myrepo/my-app:v1.0.0

# Pull a specific digest (immutable reference)
docker pull myrepo/my-app@sha256:abc123...

# Log out from a registry
docker logout

# Search Docker Hub for images
docker search nginx
```

---

## Docker Compose

```bash
# Start all services defined in docker-compose.yml (foreground)
docker compose up

# Start all services in detached (background) mode
docker compose up -d

# Stop and remove containers, networks created by up
docker compose down

# Stop and remove containers, networks, AND volumes
docker compose down -v

# Rebuild images before starting (useful after code changes)
docker compose up -d --build

# Rebuild without using cache
docker compose build --no-cache

# View logs for all services
docker compose logs

# Follow live logs for all services
docker compose logs -f

# View logs for a specific service
docker compose logs -f api

# List containers managed by Compose
docker compose ps

# Restart all services
docker compose restart

# Restart a specific service
docker compose restart api

# Stop all services (without removing containers)
docker compose stop

# Start previously stopped services
docker compose start

# Run a one-off command in a service container
docker compose run --rm api npm run migrate

# Open a shell in a running service container
docker compose exec api bash

# Scale a service to N replicas
docker compose up -d --scale worker=3

# Pull latest images for all services
docker compose pull

# Validate and view the resolved Compose config
docker compose config
```

---

## Kubernetes (kubectl)

### Basic Commands

```bash
# Check kubectl version
kubectl version --client

# View cluster info
kubectl cluster-info

# List all available API resources
kubectl api-resources

# Set the active context (cluster)
kubectl config use-context my-cluster

# View current context
kubectl config current-context

# List all contexts
kubectl config get-contexts

# Set a default namespace for the current context
kubectl config set-context --current --namespace=my-namespace
```

---

### Resources

```bash
# List all pods in the current namespace
kubectl get pods

# List pods in all namespaces
kubectl get pods -A

# List pods with extra info (node, IP)
kubectl get pods -o wide

# List pods with labels
kubectl get pods --show-labels

# List pods matching a label selector
kubectl get pods -l app=api

# Watch pods in real time
kubectl get pods -w

# List all services
kubectl get services
kubectl get svc

# List all deployments
kubectl get deployments
kubectl get deploy

# List all in one command (pods, services, deployments)
kubectl get pods,svc,deploy

# List nodes in the cluster
kubectl get nodes

# List all namespaces
kubectl get namespaces

# List persistent volume claims
kubectl get pvc

# List config maps
kubectl get configmaps

# List secrets
kubectl get secrets

# List ingress resources
kubectl get ingress

# Output resources in YAML format
kubectl get deployment api -o yaml

# Output resources in JSON format
kubectl get pod api-xyz -o json
```

---

### Apply & Delete

```bash
# Apply a YAML manifest (create or update)
kubectl apply -f deployment.yaml

# Apply all YAML files in a directory
kubectl apply -f ./k8s/

# Apply from a remote URL
kubectl apply -f https://example.com/manifests/app.yaml

# Delete a resource from a YAML file
kubectl delete -f deployment.yaml

# Delete a specific resource by type and name
kubectl delete deployment api
kubectl delete pod api-xyz
kubectl delete svc api-service

# Delete all pods matching a label
kubectl delete pods -l app=api

# Delete a namespace (and all its resources)
kubectl delete namespace staging

# Force delete a stuck pod
kubectl delete pod api-xyz --grace-period=0 --force
```

---

### Inspect & Debug

```bash
# Describe a pod (events, conditions, resource usage)
kubectl describe pod api-xyz

# Describe a deployment
kubectl describe deployment api

# Describe a service
kubectl describe svc api-service

# Describe a node
kubectl describe node worker-node-1

# View events in the current namespace
kubectl get events

# View events sorted by timestamp
kubectl get events --sort-by='.lastTimestamp'

# View resource usage (requires metrics-server)
kubectl top pods
kubectl top nodes
```

---

### Exec & Port Forwarding

```bash
# View logs for a pod
kubectl logs api-xyz

# Follow live logs
kubectl logs -f api-xyz

# View logs for a specific container in a multi-container pod
kubectl logs api-xyz -c sidecar

# View previous container logs (after a crash)
kubectl logs api-xyz --previous

# Tail the last 100 lines
kubectl logs api-xyz --tail=100

# Execute a command in a running pod
kubectl exec api-xyz -- ls /app

# Open an interactive shell in a pod
kubectl exec -it api-xyz -- bash

# Open a shell in a specific container
kubectl exec -it api-xyz -c app -- sh

# Forward a local port to a pod
kubectl port-forward pod/api-xyz 8080:3000

# Forward a local port to a service
kubectl port-forward svc/api-service 8080:80

# Forward a local port to a deployment
kubectl port-forward deployment/api 8080:3000
```

---

### Scaling

```bash
# Scale a deployment to 3 replicas
kubectl scale deployment api --replicas=3

# Scale down to zero (useful for stopping without deleting)
kubectl scale deployment api --replicas=0

# Autoscale based on CPU usage
kubectl autoscale deployment api --min=2 --max=10 --cpu-percent=70

# View horizontal pod autoscalers
kubectl get hpa

# Update the image of a deployment (rolling update)
kubectl set image deployment/api app=myrepo/my-app:v2.0.0

# Check rollout status
kubectl rollout status deployment/api

# View rollout history
kubectl rollout history deployment/api

# Roll back to the previous version
kubectl rollout undo deployment/api

# Roll back to a specific revision
kubectl rollout undo deployment/api --to-revision=2

# Pause a rolling update
kubectl rollout pause deployment/api

# Resume a paused rollout
kubectl rollout resume deployment/api
```

---

## Development Workflow Example

End-to-end example for building, testing, and deploying a containerized backend API.

### 1. Build & Test Locally with Docker

```bash
# Build the image
docker build -t my-api:latest .

# Run locally for testing
docker run -d \
  --name my-api \
  -p 3000:3000 \
  -e DATABASE_URL=postgres://localhost/mydb \
  my-api:latest

# Verify it's running
curl http://localhost:3000/health

# Check logs
docker logs -f my-api

# Clean up
docker stop my-api && docker rm my-api
```

---

### 2. Run Full Stack Locally with Docker Compose

```bash
# Start all services (API + DB + cache)
docker compose up -d --build

# Verify all services are healthy
docker compose ps

# Run database migrations
docker compose exec api npm run migrate

# Tail logs across all services
docker compose logs -f

# Tear down when done
docker compose down -v
```

---

### 3. Tag & Push to Registry

```bash
# Tag the image for your registry
docker tag my-api:latest myrepo/my-api:v1.2.0
docker tag my-api:latest myrepo/my-api:latest

# Push both tags
docker push myrepo/my-api:v1.2.0
docker push myrepo/my-api:latest
```

---

### 4. Deploy to Kubernetes

```bash
# Apply Kubernetes manifests
kubectl apply -f ./k8s/namespace.yaml
kubectl apply -f ./k8s/configmap.yaml
kubectl apply -f ./k8s/secret.yaml
kubectl apply -f ./k8s/deployment.yaml
kubectl apply -f ./k8s/service.yaml
kubectl apply -f ./k8s/ingress.yaml

# Watch pods come up
kubectl get pods -w -n production

# Verify deployment is healthy
kubectl rollout status deployment/my-api -n production

# Check service endpoints
kubectl get svc my-api -n production
```

---

### 5. Update to a New Version (Rolling Update)

```bash
# Push new image
docker build -t myrepo/my-api:v1.3.0 .
docker push myrepo/my-api:v1.3.0

# Trigger rolling update
kubectl set image deployment/my-api \
  app=myrepo/my-api:v1.3.0 \
  -n production

# Monitor the rollout
kubectl rollout status deployment/my-api -n production

# If something goes wrong, roll back instantly
kubectl rollout undo deployment/my-api -n production
```

---

### 6. Debug a Live Pod

```bash
# Check pod status and events
kubectl describe pod -l app=my-api -n production

# Tail live logs
kubectl logs -f -l app=my-api -n production

# Shell into a running pod
kubectl exec -it \
  $(kubectl get pod -l app=my-api -n production -o jsonpath='{.items[0].metadata.name}') \
  -n production -- bash

# Forward port for local debugging
kubectl port-forward \
  svc/my-api 8080:80 \
  -n production
```

---

*Keep this file handy as a daily driver. Commands are written for real workflows — not just documentation.*