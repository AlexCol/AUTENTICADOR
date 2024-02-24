using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AUTENTICADOR.src.Model;
using AUTENTICADOR.src.Model.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AUTENTICADOR.src.Repository.GenericRepository;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity {
	protected PostgreContext _context;
	private DbSet<T> dataset;

	public GenericRepository(PostgreContext context) {
		_context = context;
		dataset = context.Set<T>();
	}

	public T FindById(Guid id) {
		T item = dataset.SingleOrDefault(p => p.id.Equals(id));
		return item;
	}

	public List<T> FindAll() {
		return dataset.ToList();
	}

	public T Create(T item) {
		try {
			dataset.Add(item);
			_context.SaveChanges();
			return item;
		} catch (Exception e) {
			throw new InvalidDataException("Erro ao cadastrar. " + e.Message);
		}
	}

	public T Update(T item) {
		T itemAtual = dataset.SingleOrDefault(p => p.id.Equals(item.id));
		if (itemAtual == null) throw new InvalidDataException("Erro ao atualizar o registro.");

		_context.Entry(itemAtual).CurrentValues.SetValues(item);
		_context.SaveChanges();
		return item;
	}
	public void Delete(Guid id) {
		var user = FindById(id);
		if (user == null) throw new Exception("Usuário não encontrado ou já excluído.");

		dataset.Remove(user);
		_context.SaveChanges();
	}
}
