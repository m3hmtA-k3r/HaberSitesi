using Application.DTOs;

namespace Business.Abstract
{
	public interface ISistemLogService
	{
		List<SistemLogDto> GetLoglar();
		List<SistemLogDto> GetLoglarBySeviye(string seviye);
		List<SistemLogDto> GetLoglarByModul(string modul);
		SistemLogDto GetLogById(int id);
		SistemLogDto InsertLog(SistemLogDto model);
		bool DeleteLog(int id);
		bool DeleteAllLogs();
	}
}
