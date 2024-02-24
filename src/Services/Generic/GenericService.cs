using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTENTICADOR.src.Model.Entities.Base;
using AUTENTICADOR.src.Repository.GenericRepository;

namespace AUTENTICADOR.src.Services.Generic;

public class GenericService<T> : IGenericService<T> where T : BaseEntity {

	protected IGenericRepository<T> _repository;
	public GenericService(IGenericRepository<T> repository) {
		_repository = repository;
	}

	virtual public T Create(T item) {
		return _repository.Create(item);
	}

	virtual public void Delete(Guid id) {
		_repository.Delete(id);
	}

	virtual public List<T> FindAll() {
		return _repository.FindAll();
	}

	virtual public T FindById(Guid id) {
		return _repository.FindById(id);
	}

	virtual public T Update(T item) {
		return _repository.Update(item);
	}
}
