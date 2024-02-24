
using System.Net;
using System.Net.Mail;
using AUTENTICADOR.src.Model.EmailModel;
using AUTENTICADOR.src.Repository.UserRepository;
using AUTENTICADOR.src.ValueObjects;
using Serilog;

namespace AUTENTICADOR.src.Services.EmailService;

public class EmailService : IEmailService {
	private readonly IConfiguration _configuration;
	private readonly EmailModel _model;
	private readonly IUserRepository _userRepo;

	public EmailService(EmailModel model, IConfiguration configuration, IUserRepository userRepo) {
		_model = model;
		_configuration = configuration;
		_userRepo = userRepo;
	}

	public void sendRegisterEmail(string email, string origin) {
		var user = _userRepo.FindByEmail(email);

		var emailTo = user.Email;
		var subject = "Ativação de cadastro site - Projetos Alexandre";

		var linkAtivaConta = _configuration.GetValue<string>("PaginaDeAtivacao");
		linkAtivaConta = linkAtivaConta.Replace("<token>", user.ActivationToken);
		linkAtivaConta = linkAtivaConta.Replace("<origin>", origin);
		var body = "";
		body += "<h1>Bem vindo!</h1>";
		body += "<p>Seu cadastro foi realizado com sucesso. Para ativar sua conta, por favor clique no link abaixo.</p>";
		body += $"<a href='{linkAtivaConta}'>Ative agora mesmo.</a>";

		sendEmail(emailTo, subject, body);
	}

	public void sendRecoverPasswordEmail(string email) {
		var user = _userRepo.FindByEmail(email);

		var emailTo = user.Email;
		var subject = "Recuperação de senha - Projetos Alexandre";

		var linkRecuperaConta = _configuration.GetValue<string>("PaginaDeRecuperacao");
		linkRecuperaConta = linkRecuperaConta.Replace("<token>", user.ActivationToken);

		var body = "";
		body += "<h1>Recupeção de Senha!</h1>";
		body += "<p>Você está recendo esse email devido a uma solicitação no site para recuperação de senha.</p>";
		body += "<p>Se não mandou a requisição, pode igorar o email.</p>";
		body += $"<a href='{linkRecuperaConta}'>Link de recuperação de senha.</a>";

		sendEmail(emailTo, subject, body);
	}

	private void sendEmail(string emailTo, string subject, string body) {

		var smtpClient = new SmtpClient(_model.smtpServer) {
			Port = _model.port,
			Credentials = new NetworkCredential(_model.emailFrom, _model.password),
			EnableSsl = _model.enableSSL
		};

		var fromAddress = new MailAddress(_model.emailFrom, "Alexandre Coletti");
		var toAddress = new MailAddress(emailTo);
		var mailMessage = new MailMessage(fromAddress, toAddress) {
			Subject = subject,
			Body = body,
			IsBodyHtml = true
		};

		try {
			smtpClient.Send(mailMessage);
		} catch (Exception e) {
			throw new Exception("Erro ao enviar e-mail: " + e.Message);
		}
	}
}
