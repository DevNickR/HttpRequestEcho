# HTTP Echo Test Container

This repository contains a simple test container that can be used for HTTP echo purposes when testing environments, deployments, and HTTP requests. The container accepts requests on **any route** and responds with useful request details, making it ideal for debugging and testing HTTP clients.

## Features

- Listens on **all routes** (`GET`, `POST`, and `PUT`).
- Returns the request headers, method, body (if present), and full URL.
- Responds with either **JSON** or **HTML** based on the `Accept` header.
- Includes current **UTC time** in the response.
- Echoes back the request body for `POST` and `PUT` methods.

## Use Cases

- Testing environment setups (e.g., Kubernetes, Docker Swarm).
- Debugging HTTP requests in deployment pipelines.
- Verifying HTTP headers, request bodies, and routing behavior.
- Echo server to inspect and debug client-side requests.

## Running the Container

You can pull and run this container from Docker Hub:

```bash
docker pull nrivett/http-echo:latest
docker run -p 8080:80 nrivett/http-echo:latest
```

## Custom Tags
This container is automatically built and tagged with both:

The branch name.
The commit ID for the main and any release branches.
