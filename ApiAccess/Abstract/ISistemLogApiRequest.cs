using Shared.Dtos;

namespace ApiAccess.Abstract
{
	public interface ISistemLogApiRequest
	{
		List<SistemLogDto> GetAllLog();
		SistemLogDto GetLogById(int logId);
		List<SistemLogDto> GetLoglarBySeviye(string seviye);
		List<SistemLogDto> GetLoglarByModul(string modul);
		SistemLogDto InsertLog(SistemLogDto model);
		bool DeleteLog(int logId);
		bool DeleteAllLogs();
	}
}
