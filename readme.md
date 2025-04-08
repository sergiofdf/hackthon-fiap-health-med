# 🏥 Health API - Sistema de Agendamento de Consultas Médicas

Este projeto consiste em uma **API** desenvolvida com **.NET 8**, como parte do **projeto final da pós-graduação em Arquitetura de Sistemas .NET com Azure da FIAP**.

A API permite o **agendamento de consultas médicas**, com funcionalidades de cadastro de usuários (médicos, pacientes e administradores), gerenciamento de agendas e autenticação via JWT.

---

## 📚 Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- XUnit
- Entity Framework Core
- PostgreSQL
- RabbitMQ
- Swagger (Swashbuckle)
- Docker
- GitHub Actions (CI/CD)
- FluentValidation
- Kubernetes
- Prometheus
- Grafana

---

## ⚙️ Funcionalidades Principais

- Registro e login de usuários com diferentes estratégias de login (CPF, Email, CRM)
- Autenticação com JWT
- Perfis de usuário: `Doctor`, `Patient`, `Admin`
- Autorização baseada em perfis
- Cadastro e edição de agendas médicas
- Consulta de Médicos com Filtro por especialidade médica
- Consulta de horários disponíveis
- Cadastro e cancelamento de consultas
- Validações avançadas com FluentValidation
- Documentação da API via Swagger UI

---

## 📦 Como executar o projeto localmente

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- [Kubernetes](https://kubernetes.io/)

### Clonando o repositório e executando via Kubernetes

```bash
#Clone do repositório
git clone https://github.com/pedropagotto/hackthon-fiap-health-med.git

#Entra na pasta dos arquivos de configuração do k8s
cd hackthon-fiap-health-med/k8s/

#Cria os pods e estrutura necessária para execução do projeto
cd Secrets && kubectl apply -f secrets.yaml && cd .. && cd Ingress && kubectl apply -f ingress.yaml && cd .. && cd App &&  kubectl apply -f deployment.yaml && cd .. && cd Prometheus &&  kubectl apply -f configmap.yaml && kubectl apply -f deployment.yaml && cd .. && cd Grafana && kubectl apply -f volume.yaml && kubectl apply -f deployment.yaml && cd ..
