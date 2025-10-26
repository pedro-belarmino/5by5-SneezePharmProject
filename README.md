# 💊 SneezePharma

**Versão:** v1.1  
**Módulo:** C# Básico  
**Data de Início:** 21/10/2025  
**Entrega:** 24/10/2025  

---

## 🧠 Visão Geral

**SneezePharma** é uma aplicação desenvolvida em **C# (Console Application)** como parte do módulo **C# Básico**.  
O projeto tem como objetivo simular um sistema de gestão para uma rede de farmácias, controlando operações como cadastro de clientes e fornecedores, vendas, compras e manipulação de medicamentos.  

O sistema **não utiliza banco de dados** — todos os dados são armazenados em **arquivos texto (.data)** com campos de **tamanho fixo**, garantindo a integridade e a consistência das informações.

---

## 🎯 Objetivos

### Objetivo Geral
Desenvolver uma aplicação modular em C#, onde cada integrante do grupo é responsável por um módulo do sistema, e todos compartilham informações entre si.

### Objetivo Específico
Criar um sistema que gerencie as operações da rede **SneezePharma**, contemplando:

- Clientes e Fornecedores  
- Princípios Ativos e Medicamentos  
- Clientes com restrição de crédito  
- Fornecedores bloqueados  
- Vendas de medicamentos  
- Compras de princípios ativos  
- Manipulação de medicamentos  

---

## 🧩 Módulos do Sistema

### 1. **Cadastros Básicos**
- **Clientes** (`Customers.data`)
- **Fornecedores** (`Suppliers.data`)
- **Princípios Ativos** (`Ingredient.data`)
- **Medicamentos** (`Medicine.data`)
- **Clientes com Restrição de Crédito** (`RestrictedCustomers.data`)
- **Fornecedores Bloqueados** (`RestrictedSuppliers.data`)

### 2. **Vendas de Medicamentos**
- **Vendas** (`Sales.data`)
- **Itens da Venda** (`SaleItems.data`)

### 3. **Compras de Princípios Ativos**
- **Compras** (`Purchases.data`)
- **Itens da Compra** (`PurchaseItem.data`)

### 4. **Manipulação de Medicamentos**
- **Produção (Manipulação)** (`Produce.data`)
- **Itens da Produção** (`ProduceItem.data`)

---

## ⚙️ Requisitos Técnicos

- Linguagem: **C# (.NET Console Application)**  
- Armazenamento: **Arquivos de texto (.data)**  
- Cada linha representa um registro  
- Campos de **tamanho fixo** (preenchidos com espaços à direita)  
- Não utilizar delimitadores especiais (como vírgulas ou ponto e vírgula)  
- Datas no formato **DDMMAAAA**  
- Valores decimais devem usar ponto (`.`) como separador  

---

## 🧮 Validações e Regras de Negócio

- **CPF/CNPJ:** devem ser válidos (verificação de formato e dígitos)  
- **Idade mínima:**  
  - Clientes devem ter **pelo menos 18 anos**  
  - Fornecedores devem ter **no mínimo 2 anos de abertura**  
- **Exclusão lógica:** registros não são removidos fisicamente — apenas marcados como **Inativos (I)**  
- **Restrições:**  
  - Não permitir venda para cliente com restrição de crédito  
  - Não permitir compra de fornecedor bloqueado  
- **Limites de itens:**  
  - Vendas → máximo de **3 itens**  
  - Compras → máximo de **3 itens**  
  - Produção → **apenas 1 medicamento** por registro  
- **Atualizações automáticas:**  
  - Ao registrar venda → atualizar *Última Compra* do cliente e *Última Venda* do medicamento  
  - Ao registrar compra → atualizar *Último Fornecimento* do fornecedor e *Última Compra* do princípio ativo  

---

## 🖥️ Interface do Usuário

O sistema funciona inteiramente via **console** e possui um menu principal com as opções:

- Cadastros Básicos  
- Vendas de Medicamentos  
- Compras de Princípios Ativos  
- Manipulação de Medicamentos  
- Sair  

Cada módulo permite as seguintes operações:
- **Incluir**  
- **Localizar**  
- **Alterar**  
- **Imprimir**  

---

### Integrantes do projeto

-  [Pedro Belarmino](https://github.com/pedro-belarmino) (Líder)
-  [Henrique Rossin](https://github.com/henriquerossin)
-  [Érica Gonçalves](https://github.com/G-ERICA)
-  [Everton Silva](https://github.com/EvertonSilvaTps)
-  [Christian Kevelyn](https://github.com/EvertonSilvaTps)


  
