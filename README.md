# SME Cyber Exposure Dashboard

A lightweight, security-focused exposure monitoring platform designed for **Small and Medium Enterprises (SMEs)**.  
The system continuously scans an organisation’s public-facing digital assets and presents **actionable cyber risk insights** through a clean dashboard.

---

## Project Overview

SMEs are often exposed to cyber risks due to limited security resources and visibility.  
This project aims to provide **enterprise-grade exposure monitoring** with minimal operational overhead.

The platform:
- Identifies exposed services and misconfigurations
- Detects outdated software, CMS, and plugins
- Highlights SSL/TLS and domain security issues
- Tracks risk trends over time
- Provides prioritised remediation guidance

---

## Architecture (High Level)

- **Backend API**: ASP.NET Core (.NET 8)
- **Database**: PostgreSQL
- **Authentication**: JWT-based authentication
- **Scanner Engine**: Python microservices
- **Security Tools**:
  - Shodan API
  - Nmap
  - OpenVAS (Dockerised)
- **Frontend**: React (dashboard & visualisations)
- **Orchestration**: Docker & Docker Compose

---

## Core Features

- Secure authentication using JWT
- Asset discovery (domains, IPs, services)
- Vulnerability scanning & exposure detection
- Risk scoring and prioritisation
- Historical risk trends & reporting
- Suggested remediation actions
- Multi-tenant SME-friendly design

---
## Configuration

Sensitive values are stored using environment-based configuration.

---
### Required Environment Variables

- JWT__Key=your-secure-jwt-key
- JWT__Issuer=SmeCyberExposure
- JWT__Audience=SmeCyberExposureUsers
- Shodan__ApiKey=your-shodan-api-key

