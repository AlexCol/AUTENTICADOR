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

		userRequest.ValidateAll();
		if (!userRequest.IsValid) {
			var error = new ErrorModel(userRequest.Notifications.convertToEnumerable());
			throw new Exception(error.ToString());
		}

		var newUser = _mapper.Map<User>(userRequest);
		newUser.Password = SecutiryUtils.ComputeHash(newUser.Password, SHA256.Create());
		newUser.ActivationToken = _repository.RegenActivationToken();

		return _mapper.Map<UserResponseVO>(_repository.Create(newUser));
	}

	//!update
	public UserResponseVO Update(UserRequestVO userRequest) {
		var current = _repository.FindById(userRequest.id);
		if (current == null || !current.Activated) throw new Exception("Usuário não encontrado ou inativo, não pode ser alterado!");

		userRequest.ValidateFilledFields();
		if (!userRequest.IsValid) {
			var error = new ErrorModel(userRequest.Notifications.convertToEnumerable());
			throw new Exception(error.ToString());
		}

		current.FirstName = userRequest.FirstName != null ? userRequest.FirstName : current.FirstName;
		current.LastName = userRequest.LastName != null ? userRequest.LastName : current.LastName;
		current.Password = userRequest.Password != null ? SecutiryUtils.ComputeHash(userRequest.Password, SHA256.Create()) : current.Password;

		return _mapper.Map<UserResponseVO>(_repository.Update(current));
	}

	//!delete
	public void Delete(Guid id) {
		_repository.Delete(id);
	}
}
