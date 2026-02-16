using ApiAccess.Abstract;
using Shared.Dtos;
using Shared.Helpers.Abstract;

namespace ApiAccess.Base
{
	public class SistemLogApiRequest : ISistemLogApiRequest
	{
		private readonly IRequestService _requestService;

		public SistemLogApiRequest(IRequestService requestService)
		{
			_requestService = requestService;
		}

		public List<SistemLogDto> GetAllLog()
		{
			return _requestService.Get<List<SistemLogDto>>("SistemLog/GetAllLog");
		}

		public SistemLogDto GetLogById(int logId)
		{
			return _requestService.Get<SistemLogDto>("SistemLog/GetLogById?logId=" + logId);
		}

		public List<SistemLogDto> GetLoglarBySeviye(string seviye)
		{
			return _requestService.Get<List<SistemLogDto>>("SistemLog/GetLoglarBySeviye?seviye=" + seviye);
		}

		public List<SistemLogDto> GetLoglarByModul(string modul)
		{
			return _requestService.Get<List<SistemLogDto>>("SistemLog/GetLoglarByModul?modul=" + modul);
		}

		public SistemLogDto InsertLog(SistemLogDto model)
		{
			return _requestService.Post<SistemLogDto>("SistemLog/InsertLog", model);
		}

		public bool DeleteLog(int logId)
		{
			return _requestService.Delete<bool>("SistemLog/DeleteLog/" + logId);
		}

		public bool DeleteAllLogs()
		{
			return _requestService.Delete<bool>("SistemLog/DeleteAllLogs");
		}
	}
}
