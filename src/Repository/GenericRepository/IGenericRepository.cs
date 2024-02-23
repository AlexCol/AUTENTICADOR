using AUTENTICADOR.src.Model.Entities.Base;

namespace AUTENTICADOR.src.Repository.GenericRepository;

public interface IGenericRepository<T> where T : BaseEntity
{
	T FindById(long id);
	List<T> FindAll();
	T Create(T item);
	T Update(T item);
	void Delete(long id);
}
