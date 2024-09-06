# Erros Comuns no Docker Compose

## 1. **Erro: TLS Handshake Timeout**
Quando executando o comando `docker-compose up --build`, você pode encontrar o seguinte erro:

```
Error response from daemon: Get "https://registry-1.docker.io/v2/": net/http: TLS handshake timeout
```

### **Por que esse erro acontece?**
Esse erro geralmente ocorre devido a um problema de conectividade com o Docker Registry (o repositório de imagens do Docker). O **TLS handshake** é uma etapa crítica para estabelecer uma conexão segura entre o cliente (seu Docker) e o servidor (Docker Registry). Se essa etapa falhar ou demorar muito, o erro de "timeout" aparece.

### **Possíveis causas:**
1. **Conexão de Internet Lenta ou Instável:** O Docker não consegue estabelecer uma conexão confiável com o Docker Registry devido a problemas com sua rede.
2. **VPN ou Proxy:** Se você está utilizando uma VPN ou proxy, isso pode interferir no processo de handshake TLS, bloqueando ou atrasando a comunicação.
3. **Firewall ou Regras de Segurança:** Algumas redes corporativas ou firewalls bloqueiam portas necessárias para que o Docker se comunique com o Docker Registry.
4. **Problemas no Docker Registry:** Às vezes, o próprio repositório do Docker pode estar passando por instabilidades ou estar fora do ar.

### **Como resolver?**
1. **Verificar a Conexão de Internet:**
   - Certifique-se de que sua conexão está estável e rápida.
   - Tente pingar `registry-1.docker.io` para ver se consegue se comunicar com o Docker Registry.

2. **Desativar VPN/Proxy:**
   - Se você estiver utilizando uma VPN, tente desativá-la e executar o comando novamente.
   - Verifique se há um proxy configurado e, se possível, desative-o temporariamente.

3. **Verificar Configurações de Firewall:**
   - Caso esteja em uma rede corporativa, peça ao administrador de rede para verificar se as portas usadas pelo Docker (geralmente a porta 443 para conexões seguras) estão bloqueadas.

4. **Repetir a Operação:**
   - Às vezes, o problema pode ser intermitente. Tente rodar `docker-compose up --build` novamente após alguns minutos.

---

## 2. **Erro: Portas Indisponíveis**
Outro erro comum ao executar o `docker-compose up` envolve portas já sendo usadas:

```
Error response from daemon: Ports are not available: exposing port TCP 0.0.0.0:3306 -> 0.0.0.0:0: listen tcp 0.0.0.0:3306: bind: Only one usage of each socket address (protocol/network address/port) is normally permitted.
```

### **Por que esse erro acontece?**
Esse erro ocorre quando o Docker tenta expor uma porta que já está sendo usada por outro processo. No exemplo acima, a porta **3306** (usada pelo MySQL) está ocupada, o que impede o Docker de alocar essa porta para o container.

### **Possíveis causas:**
1. **Outro Serviço Usando a Mesma Porta:** Pode haver um serviço, como uma instância do MySQL, já rodando na máquina e usando a porta 3306.
2. **Container Docker Anterior:** Um container anterior pode não ter sido finalizado corretamente, e a porta ainda está sendo usada.

### **Como resolver?**
1. **Verificar Processos na Porta:**
   - No Windows, use o comando abaixo para verificar qual processo está utilizando a porta:
     ```
     netstat -ano | findstr :3306
     ```
   - No Linux/macOS, use:
     ```
     sudo lsof -i :3306
     ```

2. **Matar o Processo em Execução:**
   - Depois de identificar o processo que está usando a porta, mate-o. No Windows, use:
     ```
     taskkill /PID <ID do processo> /F
     ```

3. **Alterar a Porta no `docker-compose.yml`:**
   - Se não puder parar o serviço existente, altere a porta mapeada no seu `docker-compose.yml` para evitar conflito. Exemplo:
     ```yaml
     ports:
       - "3307:3306"
     ```

4. **Reiniciar o Docker:**
   - Em alguns casos, reiniciar o Docker pode liberar portas ocupadas por containers anteriores.

5. **Finalizar Containers em Execução:**
   - Use o seguinte comando para parar e remover containers em execução que possam estar ocupando as portas:
     ```
     docker-compose down
     ```

### **Links úteis:**
- [Solução de problemas de porta no Docker](https://docs.docker.com/config/containers/container-networking/)
- [Mapeamento de portas no Docker Compose](https://docs.docker.com/compose/networking/)

