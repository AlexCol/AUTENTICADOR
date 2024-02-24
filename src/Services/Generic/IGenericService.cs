using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTENTICADOR.src.Model.Entities.Base;

namespace AUTENTICADOR.src.Services.Generic;

public interface IGenericService<T> where T : BaseEntity {
	T FindById(Guid id);
	List<T> FindAll();
	T Create(T item);
	T Update(T item);
	void Delete(Guid id);
}
