using System.Security.Cryptography;
using AUTENTICADOR.src.Extensions.toFluntNotifications;
using AUTENTICADOR.src.Extensions.toUserRepo;
using AUTENTICADOR.src.Model.Entities;
using AUTENTICADOR.src.Model.Error;
using AUTENTICADOR.src.Repository.UserRepository;
using AUTENTICADOR.src.Utils.Secutiry;
using AUTENTICADOR.src.ValueObjects;
using AutoMapper;
using Serilog;

namespace AUTENTICADOR.src.Services.UserService;

public class UserService : IUserService {
	private readonly IUserRepository _repository;
	private readonly IMapper _mapper;
	public UserService(IUserRepository repository, IMapper mapper) {
		_repository = repository;
		_mapper = mapper;
	}

	//!validate user
	private User ValidateUserData(UserRequestVO request) {
		var newUser = new User(
			request.Email,
			request.FirstName,
			request.LastName,
			request.Password,
			null,
			DateTime.Now
		);
		if (!newUser.IsValid) {
			var error = new ErrorModel(newUser.Notifications.convertToEnumerable());
			throw new Exception(error.ToString());
		}
		return newUser;
	}

	//!findby id
	public UserResponseVO FindById(Guid id) {
		return _mapper.Map<UserResponseVO>(_repository.FindById(id));
	}
	//!listall
	public List<UserResponseVO> FindAll() {
		return _mapper.Map<List<UserResponseVO>>(_repository.FindAll());
	}
	//!create
	public UserResponseVO Create(UserRequestVO userRequest) {
		var emailUser = _repository.FindByEmail(userRequest.Email);
		if (emailUser != null) throw new Exception("Email já cadastrado!");

		var newUser = ValidateUserData(userRequest);

		newUser.Password = SecutiryUtils.ComputeHash(newUser.Password, SHA256.Create());
		newUser.ActivationToken = _repository.RegenActivationToken();

		return _mapper.Map<UserResponseVO>(_repository.Create(newUser));
	}

	//!update
	public UserResponseVO Update(UserRequestVO userRequest) {
		var current = _repository.FindById(userRequest.id);
		if (current == null || !current.Activated) throw new Exception("Usuário não encontrado ou inativo, não pode ser alterado!");

		bool updatePassword = userRequest.Password != null;

		userRequest.Email = current.Email;
		userRequest.FirstName = userRequest.FirstName != null ? userRequest.FirstName : current.FirstName;
		userRequest.LastName = userRequest.LastName != null ? userRequest.LastName : current.LastName;
		userRequest.Password = updatePassword ? userRequest.Password : "1234@Aa"; //se não atualizar senha, colocar uma qualquer pra não dar erro ao validar

		var user = ValidateUserData(userRequest); //dar erro se algum dos novos dados não estiver correto
		user.id = userRequest.id;
		user.Activated = current.Activated;

		if (updatePassword) { //atualizando senha
			user.Password = SecutiryUtils.ComputeHash(user.Password, SHA256.Create());
		} else {
			user.Password = current.Password;
		}

		return _mapper.Map<UserResponseVO>(_repository.Update(user));
	}

	//!delete
	public void Delete(Guid id) {
		_repository.Delete(id);
	}
}
