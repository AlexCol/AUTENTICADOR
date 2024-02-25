# AUTENTICADOR

Sistema para controle de autenticações.
-Controla usuário (login com jwt, registro, edição, deleção);
-Envia email para ativação do cadastro; Permitindo reenvio da ativação (e invalidando as anteriores);
-Função de recuperação de senha;

### Instalação
1. Baixe o projeto;
2. Renomeie o arquivo appsettings.model.json para appsettings.json, preenchendo os campos conforme seus requisitos (banco de dados, serviço de email, paginas que receberão a ativação de usuário e recuperação de senha).
2.1. Caso não deseje um 'frontend' para a recuperação da senha e ativação da conta, pode mandar os links diretamente para os endpoints responsaveis por isso (já é feito isso nos links do projeto), mas para a ativação, deve ser mudado para 'Get', e das senhas somente via Postman ou similar, pois precisa ser enviada a nova senha e confirmação de senha.
3. Abra o projeto no Visual Studio Code e deixe a IDE instalar as dependencias necessárias.
4. Execute o comando:
> dotnet watch run

# Fluxo para testes 
#### utilizado {{baseUrl}} para subistituir o que for informado em launchSettings.json

**Registro - Post**
> {{baseUrl}}/user/register?origin=siteQueMandouARequest

Body:

> {
     "Email": "seu@email.com.br",
     "FirstName": "Nome",
     "LastName": "Sobrenome",
     "Password": "Senha",
     "ConfirmPassword": "Confirmação de senha"
 }

siteQueMandouARequest: para permitir controle de quem solicitou o registro, para controles e possíveis redirecionamentos para o site solicitante.

**GetProfile - Get**
> {{baseUrl}}/user/profile

Header:
> Authorization: Bearer {{accessToken}}

Com isso se consegue buscar os dados do usuário logado (do token enviado).

**UpdateUser - Put**
> {{baseUrl}}/user

Header:
> Authorization: Bearer {{accessToken}}

Body:

> { //omite-se dados que não se deseja atualizar (email não é atualizavel, mesmo que enviado, não será mudado)
     "FirstName": "Nome",
     "LastName": "Sobrenome",
     "Password": "Senha",
     "ConfirmPassword": "Confirmação de senha"
 }
 
 **Delete Usuário - Delete**
 >{{baseUrl}}/user
 Header:
> Authorization: Bearer {{accessToken}}

Realiza a exclusão do usuário logado (do token enviado).
 
 **Login - Post**
 >{{baseUrl}}/auth/signin
 
 Body:
 >{
    "email": "seu@email.com.br",
    "Password": "suaSenha"
}

Vai retornar um jwt e um refreshToken.
 
 **Refresh - Post**
 >{{baseUrl}}/auth/refresh
 
 Body:
 >{
    "accessToken": "{{accessToken}}",
    "refreshToken": "{{refreshToken}}"
}

Permite solicitar um JwtToken a partir do refreshtoken, caso o primeiro já tenha expirado e o segundo não, de modo que se consegue autorizar novamente (gerar novo jwt) sem a necessidade de trafegar novamente usuário e senha.
 
 **Activate User - Put**
 >{{baseUrl}}/auth/activate?t={{ActivateToken}}
 
 ActivateToken criado no momento do cadastro, enviado no email(já montado com o link) ao criar o usuário.
 
 **Revoke Token - Put**
 >{{baseUrl}}/auth/revoke
 
  Header:
> Authorization: Bearer {{accessToken}}

Limpa o refreshtoken, de modo que não se consiga mais recuperar o jwt com um token vencido, forçando novo login no caso de jwt expirado.


 **Resend Activation Token - Put**
>{{baseUrl}}/auth/resend_activation_token?origin=meuSite

Body:
>{
    "email": "seu@email.com.br"
}

Reenvia o token para ativação de conta (caso tenha dado algum problema e o usuário encontrou o email com o primeiro). Torna os tokens anteriores invalidos. Origem tem o mesmo proposito da criação do cadastro.

**RecoverPasswordRequest - Put**
>{{baseUrl}}/auth/recover_password

Body:
>{
    "email": "seu@email.com.br"
}

Envia um email com um link (e token) para atualização do email.

**ResetPassword - Put**
>{{baseUrl}}/auth/newpassword?t={{token}}

body:
>{
    "password": "suaSenha",
    "confirmpassword": "suaSenha"
}

Token é o Activationtoken (caso se busque no banco de dados), ele é enviado no email na requisição do RecoverPasswordRequest.