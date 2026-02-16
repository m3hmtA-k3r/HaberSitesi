using Business.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiUI.Controllers
{
	/// <summary>
	/// API endpoints for Yazar to Kullanici migration
	/// Admin only - consolidates legacy Yazar accounts with unified user system
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class YazarMigrationController : ControllerBase
	{
		private readonly IYazarMigrationService _migrationService;

		public YazarMigrationController(IYazarMigrationService migrationService)
		{
			_migrationService = migrationService;
		}

		/// <summary>
		/// Get current migration status
		/// </summary>
		[HttpGet("status")]
		public IActionResult GetStatus()
		{
			var status = _migrationService.GetMigrationStatus();
			return Ok(new
			{
				Total = status.Total,
				Linked = status.Linked,
				Unlinked = status.Unlinked,
				PercentComplete = status.Total > 0
					? Math.Round((double)status.Linked / status.Total * 100, 1)
					: 100
			});
		}

		/// <summary>
		/// Migrate all unlinked Yazarlar to Kullanici accounts
		/// </summary>
		[HttpPost("migrate-all")]
		public async Task<IActionResult> MigrateAll()
		{
			var beforeStatus = _migrationService.GetMigrationStatus();
			var migratedCount = await _migrationService.MigrateAllYazarlarAsync();
			var afterStatus = _migrationService.GetMigrationStatus();

			return Ok(new
			{
				Success = true,
				Message = $"{migratedCount} yazar basariyla Kullanici sistemine baglandi.",
				Before = new { beforeStatus.Total, beforeStatus.Linked, beforeStatus.Unlinked },
				After = new { afterStatus.Total, afterStatus.Linked, afterStatus.Unlinked },
				Migrated = migratedCount
			});
		}

		/// <summary>
		/// Migrate a specific Yazar
		/// </summary>
		[HttpPost("migrate/{yazarId}")]
		public async Task<IActionResult> MigrateYazar(int yazarId)
		{
			var success = await _migrationService.MigrateYazarAsync(yazarId);

			if (success)
			{
				return Ok(new
				{
					Success = true,
					Message = $"Yazar (ID: {yazarId}) basariyla Kullanici sistemine baglandi."
				});
			}

			return NotFound(new
			{
				Success = false,
				Message = $"Yazar (ID: {yazarId}) bulunamadi veya zaten baglanmis."
			});
		}
	}
}
