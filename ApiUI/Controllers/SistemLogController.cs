using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace ApiUI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SistemLogController : ControllerBase
	{
		private readonly ISistemLogService _sistemLogService;

		public SistemLogController(ISistemLogService sistemLogService)
		{
			_sistemLogService = sistemLogService;
		}

		[HttpGet]
		[Route("GetAllLog")]
		public List<SistemLogDto> GetAllLog() => _sistemLogService.GetLoglar();

		[HttpGet]
		[Route("GetLogById")]
		public SistemLogDto GetLogById(int logId) => _sistemLogService.GetLogById(logId);

		[HttpGet]
		[Route("GetLoglarBySeviye")]
		public List<SistemLogDto> GetLoglarBySeviye(string seviye) => _sistemLogService.GetLoglarBySeviye(seviye);

		[HttpGet]
		[Route("GetLoglarByModul")]
		public List<SistemLogDto> GetLoglarByModul(string modul) => _sistemLogService.GetLoglarByModul(modul);

		[HttpPost]
		[Route("InsertLog")]
		public SistemLogDto InsertLog(SistemLogDto model) => _sistemLogService.InsertLog(model);

		[HttpDelete]
		[Route("DeleteLog/{logId}")]
		public IActionResult DeleteLog(int logId)
		{
			var result = _sistemLogService.DeleteLog(logId);
			if (!result)
				return NotFound(new { message = "Log bulunamadi" });
			return Ok(new { message = "Log silindi" });
		}

		[HttpDelete]
		[Route("DeleteAllLogs")]
		public IActionResult DeleteAllLogs()
		{
			var result = _sistemLogService.DeleteAllLogs();
			return Ok(new { message = "Tum loglar silindi" });
		}
	}
}
