using Serilog;

namespace AUTENTICADOR.src.Extensions.toFluntNotifications;

public static class ConvertToEnumerable {
	public static IEnumerable<string> convertToEnumerable(this IReadOnlyCollection<Flunt.Notifications.Notification> notifications) {
		var list = new List<string>();
		foreach (var notification in notifications) {
			list.Add(notification.Message);
		}
		return list;
	}
}
