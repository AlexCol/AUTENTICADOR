using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flunt.Notifications;

namespace AUTENTICADOR.src.Model.Entities.Base;

public class BaseEntity {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public Guid id { get; set; }
}
