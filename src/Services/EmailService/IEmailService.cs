namespace AUTENTICADOR.src.Services.EmailService;

public interface IEmailService {
	void sendRegisterEmail(string email, string origin);
	void sendRecoverPasswordEmail(string email);
}
